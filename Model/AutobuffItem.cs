using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public class AutoBuffItem : IAction
    {
        public static string ACTION_NAME_AUTOBUFFITEM = "AutobuffItem";
        public string ActionName { get; set; }
        private ThreadRunner thread;
        private int _delay = AppConfig.AutoBuffItemsDefaultDelay;

        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoBuffItemsDefaultDelay : _delay;
            set => _delay = value;
        }

        public Dictionary<EffectStatusIDs, Key> buffMapping = new Dictionary<EffectStatusIDs, Key>();

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
                    ThreadRunner.Stop(this.thread);
                }
                this.thread = AutoBuffThread(roClient);
                ThreadRunner.Start(this.thread);
            }
        }

        public ThreadRunner AutoBuffThread(Client c)
        {
            ThreadRunner autobuffItemThread = new ThreadRunner(_ =>
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

                    if (StatusUtils.IsValidStatus(currentStatus))
                    {
                        statusList.Add((i, currentStatus));
                        //DebugLogger.Debug("currentStatus: " + i + ":" + currentStatus);
                    }
                }

                StatusIdLogger.LogAllStatuses(statusList);

                if (!prefs.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
                {
                    // Process buffs
                    List<EffectStatusIDs> buffs = new List<EffectStatusIDs>();
                    Dictionary<EffectStatusIDs, Key> bmClone = new Dictionary<EffectStatusIDs, Key>(this.buffMapping);

                    foreach (var (i, currentStatus) in statusList)
                    {
                        if (!StatusUtils.IsValidStatus(currentStatus)) { continue; }

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
                        // BUG FIX: Changed break to continue to skip individual buffs instead of exiting loop
                        if (foundQuag && (item.Key == EffectStatusIDs.CONCENTRATION || item.Key == EffectStatusIDs.INC_AGI || item.Key == EffectStatusIDs.TRUESIGHT || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.SPEARQUICKEN || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.WINDWALK))
                        {
                            continue; // Skip this buff but continue processing others
                        }
                        else if (foundDecreaseAgi && (item.Key == EffectStatusIDs.TWOHANDQUICKEN || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.ADRENALINE2 || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.SPEARQUICKEN))
                        {
                            continue; // Skip this buff but continue processing others
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
                        if (StatusUtils.IsValidStatus(currentStatus))
                        {
                            statusList.Add((i, currentStatus));
                        }
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