using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using _ORTools.Utils;
using Newtonsoft.Json;

namespace _ORTools.Model
{
    public class AutopotHP : IAction
    {
        public class HPSlot
        {
            public int Id { get; set; }
            public Keys Key { get; set; } = Keys.None;
            public int HPPercent { get; set; } = 0;
            public bool Enabled { get; set; } = false;
        }

        public static string ACTION_NAME_AUTOPOT_HP = "AutopotHP";
        private static readonly int AUTOPOT_HP_ROWS = 5;
        private static readonly int MIN_CYCLE_DELAY = 1; // Minimum 1ms between cycles

        public List<HPSlot> HPSlots { get; set; } = new List<HPSlot>();

        private int _delay = AppConfig.AutoPotDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoPotDefaultDelay : _delay;
            set => _delay = value;
        }

        public bool StopOnCriticalInjury { get; set; } = false;
        public string ActionName { get; set; }
        private ThreadRunner thread;
        private volatile bool _hasCriticalWound = false;
        private long _lastCriticalWoundCheck = 0;
        private readonly long _criticalWoundCheckInterval = TimeSpan.FromMilliseconds(200).Ticks; // Check every 200ms

        // Track last used slot globally for proper cycling across all available slots
        private int _lastUsedSlotIndex = -1;

        public AutopotHP() { }

        public AutopotHP(string actionName)
        {
            this.ActionName = actionName;
            InitializeSlots();
        }

        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            if (HPSlots == null || HPSlots.Count == 0)
            {
                InitializeSlots();
            }
        }

        private void InitializeSlots()
        {
            if (this.HPSlots == null || this.HPSlots.Count == 0)
            {
                this.HPSlots = new List<HPSlot>();
                for (int i = 1; i <= AUTOPOT_HP_ROWS; i++)
                {
                    this.HPSlots.Add(new HPSlot { Id = i });
                }
            }
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    ThreadRunner.Stop(this.thread);
                }
                this.thread = new ThreadRunner(_ => AutopotHPThread(roClient), "AutopotHP");
                ThreadRunner.Start(this.thread);
            }
        }

        private int AutopotHPThread(Client roClient)
        {
            if (roClient?.Process == null || roClient.Process.HasExited)
            {
                DebugLogger.Debug(
                    "AutopotHP: Client process is null or has exited, stopping thread."
                );
                FormHelper.ToggleStateOff("AutopotHP");
                return -1;
            }

            bool potUsed = false;

            try
            {
                string currentMap = roClient.ReadCurrentMap();
                bool isInCity =
                    ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity
                    && Server.GetCityList().Contains(currentMap);

                if (!isInCity)
                {
                    // Only check critical wound if the setting is enabled and enough time has passed
                    if (StopOnCriticalInjury)
                    {
                        long currentTicks = DateTime.UtcNow.Ticks;
                        if (currentTicks - _lastCriticalWoundCheck >= _criticalWoundCheckInterval)
                        {
                            _hasCriticalWound = HasCriticalWound(roClient);
                            _lastCriticalWoundCheck = currentTicks;
                        }
                    }

                    potUsed = ProcessHPHealing(roClient);
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed, but don't crash the thread
                System.Diagnostics.Debug.WriteLine($"Autopot HP error: {ex.Message}");
            }

            // Use minimal delay for fast response, user-configured delay if pot was used
            int sleepTime = potUsed ? this.Delay : MIN_CYCLE_DELAY;
            Thread.Sleep(sleepTime);

            return 0;
        }

        private bool ProcessHPHealing(Client roClient)
        {
            // Early exit if we should stop on critical injury and have one
            if (StopOnCriticalInjury && _hasCriticalWound)
                return false;

            // Check the global pot cooldown before attempting to use a pot
            if (!PotionManager.CanUsePot())
                return false;

            // Find all enabled slots that meet the HP threshold, grouped by HP percentage
            var slotsByHPPercent = new Dictionary<int, List<int>>();
            for (int i = 0; i < HPSlots.Count; i++)
            {
                var slot = HPSlots[i];
                if (slot.Enabled && slot.HPPercent > 0 && roClient.IsHpBelow(slot.HPPercent))
                {
                    if (!slotsByHPPercent.ContainsKey(slot.HPPercent))
                        slotsByHPPercent[slot.HPPercent] = new List<int>();

                    slotsByHPPercent[slot.HPPercent].Add(i);
                }
            }

            if (slotsByHPPercent.Count == 0)
                return false;

            // Get ALL HP percentages that we're below, sorted by priority (highest first)
            var sortedHPPercentages = slotsByHPPercent.Keys.OrderByDescending(x => x).ToList();

            // Collect all available slots from all applicable HP percentages
            var allAvailableSlots = new List<int>();
            foreach (var hpPercent in sortedHPPercentages)
            {
                allAvailableSlots.AddRange(slotsByHPPercent[hpPercent]);
            }

            // Sort the slots by their original slot index to maintain priority order
            allAvailableSlots.Sort();

            // Find the highest HP percentage that we have slots for (for cycling tracking)
            int primaryHPPercent = sortedHPPercentages[0];

            // Use the next slot in the cycling order across all available slots
            int nextSlotIndex = GetNextSlotIndex(allAvailableSlots);
            if (nextSlotIndex != -1 && UsePot(HPSlots[nextSlotIndex].Key))
            {
                _lastUsedSlotIndex = nextSlotIndex;
                PotionManager.RecordPotUsage();
                return true;
            }

            return false; // No pot was used
        }

        /// <summary>
        /// Gets the next slot index to use based on global cycling logic.
        /// </summary>
        private int GetNextSlotIndex(List<int> availableSlots)
        {
            // If no previous slot was used or it's not in the current available slots, start with the first available
            if (_lastUsedSlotIndex == -1 || !availableSlots.Contains(_lastUsedSlotIndex))
            {
                return availableSlots[0];
            }

            // Find the current slot in the available list and get the next one
            int currentPosition = availableSlots.IndexOf(_lastUsedSlotIndex);
            int nextPosition = (currentPosition + 1) % availableSlots.Count;

            return availableSlots[nextPosition];
        }

        private bool UsePot(Keys key)
        {
            if (key == Keys.None)
                return false;

            try
            {
                // Only send if Alt is not pressed (prevents conflicts with alt+key combinations)
                if (
                    !Win32Interop.IsKeyPressed(Keys.LMenu) && !Win32Interop.IsKeyPressed(Keys.RMenu)
                )
                {
                    Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
                    var handle = ClientSingleton.GetClient().Process.MainWindowHandle;

                    // Send key press and release
                    Win32Interop.PostMessage(handle, Constants.WM_KEYDOWN_MSG_ID, k, 0);
                    Win32Interop.PostMessage(handle, Constants.WM_KEYUP_MSG_ID, k, 0);

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log parse errors but don't crash
                System.Diagnostics.Debug.WriteLine($"Failed to use pot key {key}: {ex.Message}");
            }

            return false;
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                ThreadRunner.Stop(this.thread);
                this.thread.Terminate();
                this.thread = null;
            }
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName() => ActionName ?? ACTION_NAME_AUTOPOT_HP;

        // Optimized critical wound check - only called when needed
        private bool HasCriticalWound(Client c)
        {
            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatus = c.CurrentBuffStatusCode(i);
                if (currentStatus == uint.MaxValue)
                    continue;

                if ((EffectStatusIDs)currentStatus == EffectStatusIDs.CRITICALWOUND)
                    return true;
            }
            return false;
        }
    }
}
