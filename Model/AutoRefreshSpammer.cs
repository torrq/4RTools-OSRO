using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{
    public class AutoRefreshSpammer : IAction
    {
        private string ACTION_NAME = "AutoRefreshSpammer";

        public Dictionary<int, MacroKey> skillTimer = new Dictionary<int, MacroKey>();
        public List<string> CityList { get; set; }

        private _4RThread thread1;
        private _4RThread thread2;
        private _4RThread thread3;
        private _4RThread thread4;

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                ValidadeThreads(this.thread1);
                ValidadeThreads(this.thread2);
                ValidadeThreads(this.thread3);
                ValidadeThreads(this.thread4);

                if (this.CityList == null || this.CityList.Count == 0) this.CityList = LocalServerManager.GetCityList();

                this.thread1 = new _4RThread((_) => AutoRefreshThreadExecution(roClient, skillTimer[1].Delay, skillTimer[1].Key));
                this.thread2 = new _4RThread((_) => AutoRefreshThreadExecution(roClient, skillTimer[2].Delay, skillTimer[2].Key));
                this.thread3 = new _4RThread((_) => AutoRefreshThreadExecution(roClient, skillTimer[3].Delay, skillTimer[3].Key));
                this.thread4 = new _4RThread((_) => AutoRefreshThreadExecution(roClient, skillTimer[4].Delay, skillTimer[4].Key));

                _4RThread.Start(this.thread1);
                _4RThread.Start(this.thread2);
                _4RThread.Start(this.thread3);
                _4RThread.Start(this.thread4);
            }
        }

        private void ValidadeThreads(_4RThread _4RThread)
        {
            if (_4RThread != null)
            {
                _4RThread.Stop(_4RThread);
            }
        }

        private int AutoRefreshThreadExecution(Client roClient, int delay, Key rKey)
        {
            string currentMap = roClient.ReadCurrentMap();
            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || this.CityList.Contains(currentMap) == false)
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
            _4RThread.Stop(this.thread1);
            _4RThread.Stop(this.thread2);
            _4RThread.Stop(this.thread3);
            _4RThread.Stop(this.thread4);
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
