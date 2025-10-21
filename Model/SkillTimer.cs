using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public class SkillTimer : IAction
    {
        private readonly string ACTION_NAME = "SkillTimer";

        public Dictionary<int, MacroKey> skillTimer = new Dictionary<int, MacroKey>();

        private ThreadRunner thread1;
        private ThreadRunner thread2;
        private ThreadRunner thread3;
        private ThreadRunner thread4;

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                ValidadeThreads(this.thread1);
                ValidadeThreads(this.thread2);
                ValidadeThreads(this.thread3);
                ValidadeThreads(this.thread4);

                this.thread1 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[1].Delay, skillTimer[1].Key));
                this.thread2 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[2].Delay, skillTimer[2].Key));
                this.thread3 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[3].Delay, skillTimer[3].Key));
                this.thread4 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[4].Delay, skillTimer[4].Key));

                ThreadRunner.Start(this.thread1);
                ThreadRunner.Start(this.thread2);
                ThreadRunner.Start(this.thread3);
                ThreadRunner.Start(this.thread4);
            }
        }

        private void ValidadeThreads(ThreadRunner _4RThread)
        {
            if (_4RThread != null)
            {
                ThreadRunner.Stop(_4RThread);
            }
        }

        private int AutoRefreshThreadExecution(Client roClient, int delay, Key rKey)
        {
            string currentMap = roClient.ReadCurrentMap();
            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
            {
                if (rKey != Key.None)
                {
                    Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), rKey.ToString()), 0);
                }
            }
            Thread.Sleep(delay);
            return 0;
        }

        public void Stop()
        {
            if (this.thread1 != null)
            {
                ThreadRunner.Stop(this.thread1);
                this.thread1.Terminate();
                this.thread1 = null;
            }
            if (this.thread2 != null)
            {
                ThreadRunner.Stop(this.thread2);
                this.thread2.Terminate();
                this.thread2 = null;
            }
            if (this.thread3 != null)
            {
                ThreadRunner.Stop(this.thread3);
                this.thread3.Terminate();
                this.thread3 = null;
            }
            if (this.thread4 != null)
            {
                ThreadRunner.Stop(this.thread4);
                this.thread4.Terminate();
                this.thread4 = null;
            }
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return ACTION_NAME;
        }
    }
}