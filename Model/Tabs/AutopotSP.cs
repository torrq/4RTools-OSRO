using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Model
{
    public class AutopotSP : IAction
    {
        public class SPSlot
        {
            public int Id { get; set; }
            public System.Windows.Input.Key Key { get; set; } = System.Windows.Input.Key.None;
            public int SPPercent { get; set; } = 0;
            public bool Enabled { get; set; } = false;
        }

        public static string ACTION_NAME_AUTOPOT_SP = "AutopotSP";
        private static readonly int AUTOPOT_SP_ROWS = 5;
        private static readonly int MIN_CYCLE_DELAY = 1; // Minimum 1ms between cycles

        public List<SPSlot> SPSlots { get; set; } = new List<SPSlot>();

        private int _delay = AppConfig.AutoPotDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoPotDefaultDelay : _delay;
            set => _delay = value;
        }

        public string ActionName { get; set; }
        private ThreadRunner thread;

        // Track last used slot globally for proper cycling across all available slots
        private int _lastUsedSlotIndex = -1;

        public AutopotSP() { }

        public AutopotSP(string actionName)
        {
            this.ActionName = actionName;
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            if (this.SPSlots == null || this.SPSlots.Count == 0)
            {
                this.SPSlots = new List<SPSlot>();
                for (int i = 1; i <= AUTOPOT_SP_ROWS; i++)
                {
                    this.SPSlots.Add(new SPSlot { Id = i });
                }
            }
        }

        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            if (SPSlots == null || SPSlots.Count == 0)
            {
                InitializeSlots();
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
                this.thread = new ThreadRunner(_ => AutopotSPThread(roClient), "AutopotSP");
                ThreadRunner.Start(this.thread);
            }
        }

        private int AutopotSPThread(Client roClient)
        {
            if (roClient?.Process == null || roClient.Process.HasExited)
            {
                DebugLogger.Debug("AutopotSP: Client process is null or has exited, stopping thread.");
                FormHelper.ToggleStateOff("AutopotSP");
                return -1;
            }

            bool potUsed = false;

            try
            {
                string currentMap = roClient.ReadCurrentMap();
                bool isInCity = ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity &&
                               Server.GetCityList().Contains(currentMap);

                if (!isInCity)
                {
                    potUsed = ProcessSPHealing(roClient);
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed, but don't crash the thread
                System.Diagnostics.Debug.WriteLine($"Autopot SP error: {ex.Message}");
            }

            // Use minimal delay for fast response, user-configured delay if pot was used
            int sleepTime = potUsed ? this.Delay : MIN_CYCLE_DELAY;
            Thread.Sleep(sleepTime);

            return 0;
        }

        private bool ProcessSPHealing(Client roClient)
        {
            // Check the global pot cooldown before attempting to use a pot
            if (!PotionManager.CanUsePot())
                return false;

            // Find all enabled slots that meet the SP threshold, grouped by SP percentage
            var slotsBySPPercent = new Dictionary<int, List<int>>();
            for (int i = 0; i < SPSlots.Count; i++)
            {
                var slot = SPSlots[i];
                if (slot.Enabled && slot.SPPercent > 0 && roClient.IsSpBelow(slot.SPPercent))
                {
                    if (!slotsBySPPercent.ContainsKey(slot.SPPercent))
                        slotsBySPPercent[slot.SPPercent] = new List<int>();

                    slotsBySPPercent[slot.SPPercent].Add(i);
                }
            }

            if (slotsBySPPercent.Count == 0)
                return false;

            // Get ALL SP percentages that we're below, sorted by priority (highest first)
            var sortedSPPercentages = slotsBySPPercent.Keys.OrderByDescending(x => x).ToList();

            // Collect all available slots from all applicable SP percentages
            var allAvailableSlots = new List<int>();
            foreach (var spPercent in sortedSPPercentages)
            {
                allAvailableSlots.AddRange(slotsBySPPercent[spPercent]);
            }

            // Sort the slots by their original slot index to maintain priority order
            allAvailableSlots.Sort();

            // Use the next slot in the cycling order across all available slots
            int nextSlotIndex = GetNextSlotIndex(allAvailableSlots);
            if (nextSlotIndex != -1 && UsePot(SPSlots[nextSlotIndex].Key))
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

        private bool UsePot(Key key)
        {
            if (key == Key.None) return false;

            try
            {
                // Only send if Alt is not pressed (prevents conflicts with alt+key combinations)
                if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
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

        public string GetActionName() => ActionName ?? ACTION_NAME_AUTOPOT_SP;
    }
}