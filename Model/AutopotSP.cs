using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Model
{
    public class AutopotSP : IAction
    {
        public static string ACTION_NAME_AUTOPOT_SP = "AutopotSP";

        public Key SPKey1 { get; set; }
        public Key SPKey2 { get; set; }
        public Key SPKey3 { get; set; }
        public Key SPKey4 { get; set; }
        public Key SPKey5 { get; set; }


        public int SPPercent1 { get; set; }
        public int SPPercent2 { get; set; }
        public int SPPercent3 { get; set; }
        public int SPPercent4 { get; set; }
        public int SPPercent5 { get; set; }

        private int _delay = AppConfig.AutoPotDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoPotDefaultDelay : _delay;
            set => _delay = value;
        }


        public string ActionName { get; set; }
        private ThreadRunner thread;

        public AutopotSP() { }
        public AutopotSP(string actionName)
        {
            this.ActionName = actionName;
        }

        public AutopotSP(
            Key spKey1, Key spKey2, Key spKey3, Key spKey4, Key spKey5,
            int spPercent1, int spPercent2, int spPercent3, int spPercent4, int spPercent5,
            int delay)
        {
            this.Delay = delay;

            this.SPKey1 = spKey1;
            this.SPPercent1 = spPercent1;

            this.SPKey2 = spKey2;
            this.SPPercent2 = spPercent2;

            this.SPKey3 = spKey3;
            this.SPPercent3 = spPercent3;

            this.SPKey4 = spKey4;
            this.SPPercent4 = spPercent4;

            this.SPKey5 = spKey5;
            this.SPPercent5 = spPercent5;
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
                int spPotCount = 0;
                this.thread = new ThreadRunner(_ => AutopotThreadExecution(roClient, spPotCount));
                ThreadRunner.Start(this.thread);
            }
        }

        private int AutopotThreadExecution(Client roClient, int spPotCount)
        {
            string currentMap = roClient.ReadCurrentMap();
            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
            {
                healSPFirst(roClient, spPotCount);
            }
            Thread.Sleep(this.Delay);
            return 0;
        }

        private void healSPFirst(Client roClient, int spPotCount)
        {
            if (roClient.IsSpBelow(SPPercent1))
            {
                Pot(this.SPKey1);
                spPotCount++;
            }

        }

        private void Pot(Key key)
        {
            Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
            if ((k != Keys.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                Interop.PostMessage(ClientSingleton.GetClient().Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, k, 0); // keydown
                Interop.PostMessage(ClientSingleton.GetClient().Process.MainWindowHandle, Constants.WM_KEYUP_MSG_ID, k, 0); // keyup
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

        public string GetActionName() => ActionName ?? ACTION_NAME_AUTOPOT_SP;

    }
}