using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Model
{
    public class AutopotHP : IAction
    {
        public static string ACTION_NAME_AUTOPOT_HP = "AutopotHP";

        // HP Keys
        public Key HPKey1 { get; set; } = Key.None;
        public Key HPKey2 { get; set; } = Key.None;
        public Key HPKey3 { get; set; } = Key.None;
        public Key HPKey4 { get; set; } = Key.None;
        public Key HPKey5 { get; set; } = Key.None;

        // HP Percentages - consistent int type
        public int HPPercent1 { get; set; } = 0;
        public int HPPercent2 { get; set; } = 0;
        public int HPPercent3 { get; set; } = 0;
        public int HPPercent4 { get; set; } = 0;
        public int HPPercent5 { get; set; } = 0;

        // HP Enabled flags
        public bool HPEnabled1 { get; set; } = false;
        public bool HPEnabled2 { get; set; } = false;
        public bool HPEnabled3 { get; set; } = false;
        public bool HPEnabled4 { get; set; } = false;
        public bool HPEnabled5 { get; set; } = false;

        private int _delay = AppConfig.AutoPotDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoPotDefaultDelay : _delay;
            set => _delay = value;
        }

        public bool StopOnCriticalInjury { get; set; } = false;
        public string ActionName { get; set; }
        private ThreadRunner thread;

        public AutopotHP() { }

        public AutopotHP(string actionName)
        {
            this.ActionName = actionName;
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
                this.thread = new ThreadRunner(_ => AutopotThreadExecution(roClient));
                ThreadRunner.Start(this.thread);
            }
        }

        private int AutopotThreadExecution(Client roClient)
        {
            string currentMap = roClient.ReadCurrentMap();
            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
            {
                bool hasCriticalWound = HasCriticalWound(roClient);
                ProcessHPHealing(roClient, hasCriticalWound);
            }
            Thread.Sleep(this.Delay);
            return 0;
        }

        private void ProcessHPHealing(Client roClient, bool hasCriticalWound)
        {
            // Check if we should stop due to critical injury
            if (this.StopOnCriticalInjury && hasCriticalWound)
                return;

            // Process HP healing in order of priority (1-5)
            var hpSlots = new[]
            {
                new { Key = HPKey1, Percent = HPPercent1, Enabled = HPEnabled1 },
                new { Key = HPKey2, Percent = HPPercent2, Enabled = HPEnabled2 },
                new { Key = HPKey3, Percent = HPPercent3, Enabled = HPEnabled3 },
                new { Key = HPKey4, Percent = HPPercent4, Enabled = HPEnabled4 },
                new { Key = HPKey5, Percent = HPPercent5, Enabled = HPEnabled5 }
            };

            foreach (var slot in hpSlots)
            {
                if (slot.Enabled && slot.Percent > 0 && roClient.IsHpBelow(slot.Percent))
                {
                    UsePot(slot.Key);
                    break; // Only use one pot per cycle
                }
            }
        }

        private void UsePot(Key key)
        {
            if (key == Key.None) return;

            Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
            if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                var handle = ClientSingleton.GetClient().Process.MainWindowHandle;
                Interop.PostMessage(handle, Constants.WM_KEYDOWN_MSG_ID, k, 0);
                Interop.PostMessage(handle, Constants.WM_KEYUP_MSG_ID, k, 0);
            }
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

        public bool HasCriticalWound(Client c)
        {
            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatus = c.CurrentBuffStatusCode(i);
                if (currentStatus == uint.MaxValue) continue;

                if ((EffectStatusIDs)currentStatus == EffectStatusIDs.CRITICALWOUND)
                    return true;
            }
            return false;
        }
    }
}