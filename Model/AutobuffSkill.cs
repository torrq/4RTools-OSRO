using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Forms;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{

    public class AutoBuffSkill : IAction
    {
        public static string ACTION_NAME_AUTOBUFFSKILL = "AutobuffSkill";
        public string ActionName { get; set; }
        private _4RThread thread;
        private int _delay = AppConfig.AutoBuffSkillsDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoBuffSkillsDefaultDelay : _delay;
            set => _delay = value;
        }

        public List<string> CityList { get; set; }

        public Dictionary<EffectStatusIDs, Key> buffMapping = new Dictionary<EffectStatusIDs, Key>();

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public AutoBuffSkill(string actionName)
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

        private static readonly Dictionary<Key, string> _sendKeysMap = new Dictionary<Key, string>()
         {
             { Key.D0, "0" },
             { Key.D1, "1" },
             { Key.D2, "2" },
             { Key.D3, "3" },
             { Key.D4, "4" },
             { Key.D5, "5" },
             { Key.D6, "6" },
             { Key.D7, "7" },
             { Key.D8, "8" },
             { Key.D9, "9" }
         };

        private string ToSendKeysFormat(Key key)
        {
            if (_sendKeysMap.TryGetValue(key, out string value))
            {
                return value;
            }
            return key.ToString().ToLower();
        }

        public _4RThread AutoBuffThread(Client c)
        {
            _4RThread autobuffItemThread = new _4RThread(_ =>
            {
                bool foundQuag = false;
                bool foundDecreaseAgi = false;
                string currentMap = c.ReadCurrentMap();
                ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;

                if (!prefs.StopBuffsCity || !this.CityList.Contains(currentMap))
                {
                    List<EffectStatusIDs> currentBuffs = new List<EffectStatusIDs>();
                    Dictionary<EffectStatusIDs, Key> buffsToApply = new Dictionary<EffectStatusIDs, Key>(this.buffMapping);

                    for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                    {
                        uint currentStatusValue = c.CurrentBuffStatusCode(i);

                        if (currentStatusValue == uint.MaxValue) { continue; }

                        EffectStatusIDs status = (EffectStatusIDs)currentStatusValue;
                        currentBuffs.Add(status);

                        HandleOverweightStatus(c, status);

                        if (status == EffectStatusIDs.OVERTHRUSTMAX && buffsToApply.ContainsKey(EffectStatusIDs.OVERTHRUST))
                        {
                            buffsToApply.Remove(EffectStatusIDs.OVERTHRUST);
                        }

                        if (buffMapping.ContainsKey(status)) //CHECK IF STATUS EXISTS IN STATUS LIST AND DO ACTION
                        {
                            buffsToApply.Remove(status);
                        }

                        if (status == EffectStatusIDs.QUAGMIRE) foundQuag = true;
                        if (status == EffectStatusIDs.DECREASE_AGI) foundDecreaseAgi = true;
                    }

                    if (!currentBuffs.Contains(EffectStatusIDs.RIDDING))
                    {
                        foreach (var buffToApply in buffsToApply)
                        {
                            if (ShouldSkipBuffDueToQuag(foundQuag, buffToApply.Key))
                            {
                                continue; // Use continue instead of break to check other buffs
                            }

                            if (ShouldSkipBuffDueToDecreaseAgi(foundDecreaseAgi, buffToApply.Key))
                            {
                                continue; // Use continue instead of break to check other buffs
                            }

                            if (c.ReadCurrentHp() >= Constants.MINIMUM_HP_TO_RECOVER)
                            {
                                this.UseAutobuff(buffToApply.Value);
                                Thread.Sleep(Delay);
                            }
                        }
                    }
                    currentBuffs.Clear();
                }
                Thread.Sleep(300);
                return 0;
            });

            return autobuffItemThread;
        }
        private void HandleOverweightStatus(Client c, EffectStatusIDs status)
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (status == EffectStatusIDs.WEIGHT50 && prefs.OverweightMode == "overweight50")
            {
                var frmToggleApplication = (ToggleApplicationStateForm)Application.OpenForms["ToggleApplicationStateForm"];
                frmToggleApplication.toggleStatus();
                DebugLogger.Info("Overweight 50%, disable now");
                SendOverweightMacro(c, "50");
            }
            else if (status == EffectStatusIDs.WEIGHT90 && prefs.OverweightMode == "overweight90")
            {
                DebugLogger.Info("Overweight 90%, disable now");
                var frmToggleApplication = (ToggleApplicationStateForm)Application.OpenForms["ToggleApplicationStateForm"];
                frmToggleApplication.toggleStatus();
                SendOverweightMacro(c, "90");
            }
        }

        private void SendOverweightMacro(Client c, string percentage, int times = 2, int intervalMs = 5000)
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (!string.IsNullOrEmpty(prefs.OverweightKey.ToString()) && prefs.OverweightKey.ToString() != "None")
            {
                // Set focus to the RO window
                IntPtr handle = ClientSingleton.GetClient().Process.MainWindowHandle;
                SetForegroundWindow(handle);

                Thread.Sleep(1000);

                string keyToSend = "%" + ToSendKeysFormat(prefs.OverweightKey);
                for (int i = 0; i < times; i++)
                {
                    // Send the key combination
                    System.Windows.Forms.SendKeys.SendWait(keyToSend);
                    DebugLogger.Info($"Sent macro {i + 1}/{times}: Alt + {prefs.OverweightKey} (Overweight {percentage}%)");

                    // Don't sleep after the last iteration
                    if (i < times - 1)
                    {
                        Thread.Sleep(intervalMs);
                    }
                }
            }
        }

        private bool ShouldSkipBuffDueToQuag(bool foundQuag, EffectStatusIDs buffKey)
        {
            return foundQuag && (buffKey == EffectStatusIDs.CONCENTRATION || buffKey == EffectStatusIDs.INC_AGI || buffKey == EffectStatusIDs.TRUESIGHT || buffKey == EffectStatusIDs.ADRENALINE || buffKey == EffectStatusIDs.SPEARQUICKEN || buffKey == EffectStatusIDs.ONEHANDQUICKEN || buffKey == EffectStatusIDs.WINDWALK || buffKey == EffectStatusIDs.TWOHANDQUICKEN);
        }

        private bool ShouldSkipBuffDueToDecreaseAgi(bool foundDecreaseAgi, EffectStatusIDs buffKey)
        {
            return foundDecreaseAgi && (buffKey == EffectStatusIDs.TWOHANDQUICKEN || buffKey == EffectStatusIDs.ADRENALINE || buffKey == EffectStatusIDs.ADRENALINE2 || buffKey == EffectStatusIDs.ONEHANDQUICKEN || buffKey == EffectStatusIDs.SPEARQUICKEN);
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

        public void SetBuffMapping(Dictionary<EffectStatusIDs, Key> buffs)
        {
            this.buffMapping = new Dictionary<EffectStatusIDs, Key>(buffs);
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
