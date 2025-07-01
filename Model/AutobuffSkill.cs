using _4RTools.Forms;
using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Model
{
    public class AutoBuffSkill : IAction
    {
        public static string ACTION_NAME_AUTOBUFFSKILL = "AutobuffSkill";
        public string ActionName { get; set; }
        private ThreadRunner thread;
        private int _delay = AppConfig.AutoBuffSkillsDefaultDelay;
        public int Delay
        {
            get => _delay <= 0 ? AppConfig.AutoBuffSkillsDefaultDelay : _delay;
            set => _delay = value;
        }

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

                if (!prefs.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
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
            if (status == EffectStatusIDs.WEIGHT90 && prefs.AutoOffOverweight)
            {
                DebugLogger.Info("Overweight 90%, disable now");
                // Corrected type cast to ToggleApplicationStateForm
                var frmToggleApplication = (ToggleStateForm)Application.OpenForms["ToggleApplicationStateForm"];
                if (frmToggleApplication != null)
                {
                    frmToggleApplication.toggleStatus();
                    OverweightMacro.SendOverweightMacro("90", 2, 5000);
                }
                else
                {
                    DebugLogger.Error("HandleOverweightStatus: Could not find 'ToggleApplicationStateForm' to toggle status.");
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