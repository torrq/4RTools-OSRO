using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{
    public class AutoBuffItem : IAction
    {
        public static string ACTION_NAME_AUTOBUFFITEM = "AutobuffItem";
        public string ActionName { get; set; }
        private _4RThread thread;
        private int _delay = AppConfig.AutoBuffItemsDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoBuffItemsDefaultDelay : _delay;
            set => _delay = value;
        }
        public Dictionary<EffectStatusIDs, Key> buffMapping = new Dictionary<EffectStatusIDs, Key>();
        public List<string> CityList { get; set; }

        public AutoBuffItem(string actionName)
        {
            this.ActionName = actionName;
        }

        public void Start()
        {
            Stop();
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    _4RThread.Stop(this.thread);
                }
                if (this.CityList == null || this.CityList.Count == 0) this.CityList = Server.GetCityList();
                this.thread = AutoBuffThread(roClient);
                _4RThread.Start(this.thread);
            }
        }

        public _4RThread AutoBuffThread(Client c)
        {
            _4RThread autobuffItemThread = new _4RThread(_ =>
            {
                bool foundQuag = false;
                bool foundDecreaseAgi = false;
                string currentMap = c.ReadCurrentMap();
                ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;

                // Collect and log statuses always
                var statusList = new List<(int index, uint statusId)>();
                for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                {
                    uint currentStatus = c.CurrentBuffStatusCode(i);
                    statusList.Add((i, currentStatus));
                }
                StatusIdLogger.LogAllStatuses(statusList);

                if (!prefs.StopBuffsCity || this.CityList.Contains(currentMap) == false)
                {
                    // Process buffs
                    List<EffectStatusIDs> buffs = new List<EffectStatusIDs>();
                    Dictionary<EffectStatusIDs, Key> bmClone = new Dictionary<EffectStatusIDs, Key>(this.buffMapping);

                    foreach (var (i, currentStatus) in statusList)
                    {
                        if (currentStatus == uint.MaxValue) { continue; }

                        buffs.Add((EffectStatusIDs)currentStatus);
                        EffectStatusIDs status = (EffectStatusIDs)currentStatus;

                        if (status == EffectStatusIDs.OVERTHRUSTMAX)
                        {
                            if (buffMapping.ContainsKey(EffectStatusIDs.OVERTHRUST))
                            {
                                bmClone.Remove(EffectStatusIDs.OVERTHRUST);
                            }
                        }

                        if (buffMapping.ContainsKey(status))
                        {
                            bmClone.Remove(status);
                        }

                        if (status == EffectStatusIDs.QUAGMIRE) foundQuag = true;
                        if (status == EffectStatusIDs.DECREASE_AGI) foundDecreaseAgi = true;
                    }

                    buffs.Clear();
                    foreach (var item in bmClone)
                    {
                        if (foundQuag && (item.Key == EffectStatusIDs.CONCENTRATION || item.Key == EffectStatusIDs.INC_AGI || item.Key == EffectStatusIDs.TRUESIGHT || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.SPEARQUICKEN || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.WINDWALK))
                        {
                            break;
                        }
                        else if (foundDecreaseAgi && (item.Key == EffectStatusIDs.TWOHANDQUICKEN || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.ADRENALINE2 || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.SPEARQUICKEN))
                        {
                            break;
                        }
                        else if (c.ReadCurrentHp() >= Constants.MINIMUM_HP_TO_RECOVER)
                        {
                            this.UseAutobuff(item.Value);
                            Thread.Sleep(Delay);
                        }
                    }

                    // Collect and log statuses again after autobuff actions
                    statusList.Clear();
                    for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                    {
                        uint currentStatus = c.CurrentBuffStatusCode(i);
                        statusList.Add((i, currentStatus));
                    }
                    StatusIdLogger.LogAllStatuses(statusList);
                }

                Thread.Sleep(300);
                return 0;
            });

            return autobuffItemThread;
        }

        public void AddKeyToBuff(EffectStatusIDs status, Key key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }

            if (FormUtils.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }

        public void ClearKeyMapping()
        {
            buffMapping.Clear();
        }

        public void Stop()
        {
            _4RThread.Stop(this.thread);
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return this.ActionName;
        }

        private void UseAutobuff(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                Interop.PostMessage(ClientSingleton.GetClient().Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
        }
    }
}