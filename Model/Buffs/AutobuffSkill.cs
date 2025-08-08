using _ORTools.Forms;
using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
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

        public Dictionary<EffectStatusIDs, Keys> buffMapping = new Dictionary<EffectStatusIDs, Keys>();

        // Add error tracking
        private int consecutiveErrors = 0;
        private const int maxConsecutiveErrors = 5;
        private DateTime lastSuccessfulRead = DateTime.Now;

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
                // Reset error tracking
                consecutiveErrors = 0;
                lastSuccessfulRead = DateTime.Now;

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
                try
                {
                    // Check if client is still valid before proceeding
                    if (c?.Process == null || c.Process.HasExited)
                    {
                        DebugLogger.Debug("AutoBuffSkill: Client process is null or has exited, stopping thread.");
                        FormHelper.ToggleStateOff("AutobuffSkill");
                        return -1; // Signal thread to stop
                    }

                    // Check if we've had too many consecutive errors
                    if (consecutiveErrors >= maxConsecutiveErrors)
                    {
                        var timeSinceLastSuccess = DateTime.Now - lastSuccessfulRead;
                        if (timeSinceLastSuccess.TotalSeconds > 10) // Wait 10 seconds before retrying
                        {
                            DebugLogger.Debug($"AutoBuffSkill: Too many consecutive errors ({consecutiveErrors}), waiting before retry...");
                            Thread.Sleep(5000); // Wait 5 seconds
                            consecutiveErrors = 0; // Reset error count
                            c.RefreshLoginStatus(); // Force refresh process status
                        }
                        else
                        {
                            Thread.Sleep(300);
                            return 0;
                        }
                    }

                    bool hadError = false;
                    bool foundQuag = false;
                    bool foundDecreaseAgi = false;
                    string currentMap = string.Empty;
                    ConfigProfile prefs = null;

                    try
                    {
                        currentMap = c.ReadCurrentMap();
                        prefs = ProfileSingleton.GetCurrent().UserPreferences;
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Debug($"AutoBuffSkill: Error reading map or preferences: {ex.Message}");
                        hadError = true;
                    }

                    if (!hadError && prefs != null)
                    {
                        if (!prefs.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
                        {
                            List<EffectStatusIDs> currentBuffs = new List<EffectStatusIDs>();
                            Dictionary<EffectStatusIDs, Keys> buffsToApply = new Dictionary<EffectStatusIDs, Keys>(this.buffMapping);
                            bool statusReadError = false;

                            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                            {
                                try
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
                                catch (Exception ex)
                                {
                                    DebugLogger.Debug($"AutoBuffSkill: Error reading status at index {i}: {ex.Message}");
                                    statusReadError = true;
                                    break;
                                }
                            }

                            if (!statusReadError && !currentBuffs.Contains(EffectStatusIDs.RIDDING))
                            {
                                foreach (var buffToApply in buffsToApply)
                                {
                                    try
                                    {
                                        if (ShouldSkipBuffDueToQuag(foundQuag, buffToApply.Key))
                                        {
                                            continue; // Use continue instead of break to check other buffs
                                        }

                                        if (ShouldSkipBuffDueToDecreaseAgi(foundDecreaseAgi, buffToApply.Key))
                                        {
                                            continue; // Use continue instead of break to check other buffs
                                        }

                                        uint currentHp = c.ReadCurrentHp();
                                        if (currentHp >= Constants.MINIMUM_HP_TO_RECOVER)
                                        {
                                            this.UseAutobuff(buffToApply.Value);
                                            Thread.Sleep(Delay);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugLogger.Debug($"AutoBuffSkill: Error applying buff {buffToApply.Key}: {ex.Message}");
                                        hadError = true;
                                        break;
                                    }
                                }
                            }
                            else if (statusReadError)
                            {
                                hadError = true;
                            }

                            currentBuffs.Clear();
                        }
                    }

                    // Update error tracking
                    if (hadError)
                    {
                        consecutiveErrors++;
                        DebugLogger.Debug($"AutoBuffSkill: Consecutive errors: {consecutiveErrors}");
                    }
                    else
                    {
                        consecutiveErrors = 0;
                        lastSuccessfulRead = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    consecutiveErrors++;
                    DebugLogger.Debug($"AutoBuffSkill: Thread exception: {ex.Message}");
                }

                Thread.Sleep(300);
                return 0;
            }, "AutobuffSkill");

            return autobuffItemThread;
        }

        private void HandleOverweightStatus(Client c, EffectStatusIDs status)
        {
            try
            {
                ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
                if (status == EffectStatusIDs.WEIGHT90 && prefs.AutoOffOverweight)
                {
                    DebugLogger.Info("Overweight 90%, disable now");
                    var frmStateSwitch = (StateSwitchForm)Application.OpenForms["StateSwitchForm"];
                    if (frmStateSwitch != null)
                    {
                        frmStateSwitch.toggleStatus();
                        WeightLimitMacro.SendOverweightMacro();
                    }
                    else
                    {
                        DebugLogger.Error("HandleOverweightStatus: Could not find 'StateSwitchForm' to toggle status.");
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"AutoBuffSkill: Error handling overweight status: {ex.Message}");
            }
        }

        private bool ShouldSkipBuffDueToQuag(bool foundQuag, EffectStatusIDs buffKey)
        {
            return foundQuag && (buffKey == EffectStatusIDs.CONCENTRATION ||
                                buffKey == EffectStatusIDs.INC_AGI ||
                                buffKey == EffectStatusIDs.TRUESIGHT ||
                                buffKey == EffectStatusIDs.ADRENALINE ||
                                buffKey == EffectStatusIDs.SPEARQUICKEN ||
                                buffKey == EffectStatusIDs.ONEHANDQUICKEN ||
                                buffKey == EffectStatusIDs.WINDWALK ||
                                buffKey == EffectStatusIDs.TWOHANDQUICKEN);
        }

        private bool ShouldSkipBuffDueToDecreaseAgi(bool foundDecreaseAgi, EffectStatusIDs buffKey)
        {
            return foundDecreaseAgi && (buffKey == EffectStatusIDs.TWOHANDQUICKEN ||
                                       buffKey == EffectStatusIDs.ADRENALINE ||
                                       buffKey == EffectStatusIDs.ADRENALINE2 ||
                                       buffKey == EffectStatusIDs.ONEHANDQUICKEN ||
                                       buffKey == EffectStatusIDs.SPEARQUICKEN);
        }

        public void AddKeyToBuff(EffectStatusIDs status, Keys key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }

            if (FormHelper.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }

        public void RemoveKeyFromBuff(EffectStatusIDs status)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
                DebugLogger.Debug($"AutoBuffSkill: Removed mapping for status {status}");
            }
        }

        public void SetBuffMapping(Dictionary<EffectStatusIDs, Keys> buffs)
        {
            this.buffMapping = new Dictionary<EffectStatusIDs, Keys>(buffs);
        }

        public void ClearKeyMapping()
        {
            buffMapping.Clear();
            //DebugLogger.Debug("AutoBuffSkill: Cleared all key mappings");
        }

        public bool HasMappingForStatus(EffectStatusIDs status)
        {
            return buffMapping.ContainsKey(status);
        }

        public Keys GetKeyForStatus(EffectStatusIDs status)
        {
            return buffMapping.ContainsKey(status) ? buffMapping[status] : Keys.None;
        }

        public Dictionary<EffectStatusIDs, Keys> GetAllMappings()
        {
            return new Dictionary<EffectStatusIDs, Keys>(buffMapping);
        }

        public int GetMappingCount()
        {
            return buffMapping.Count;
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
            // Create a configuration object that includes both mapping and delay
            var configData = new Dictionary<string, object>
            {
                ["ActionName"] = this.ActionName,
                ["BuffMapping"] = this.buffMapping,
                ["Delay"] = this._delay
            };
            return JsonConvert.SerializeObject(configData);
        }

        public void LoadConfiguration(string config)
        {
            try
            {
                // Try to deserialize as the new format
                var configData = JsonConvert.DeserializeObject<Dictionary<string, object>>(config);
                if (configData != null)
                {
                    // Load action name
                    if (configData.ContainsKey("ActionName") && configData["ActionName"] != null)
                    {
                        this.ActionName = configData["ActionName"].ToString();
                    }

                    // Load buff mapping
                    if (configData.ContainsKey("BuffMapping"))
                    {
                        var mappingData = JsonConvert.DeserializeObject<Dictionary<EffectStatusIDs, Keys>>(configData["BuffMapping"].ToString());
                        if (mappingData != null)
                        {
                            this.buffMapping = mappingData;
                        }
                    }

                    // Load delay
                    if (configData.ContainsKey("Delay"))
                    {
                        if (int.TryParse(configData["Delay"].ToString(), out int delay))
                        {
                            this._delay = delay;
                        }
                    }
                    return;
                }
            }
            catch
            {
                // Fall back to old format
            }

            try
            {
                // Try to deserialize as the old format (full AutoBuffSkill object)
                var oldAutoBuffSkill = JsonConvert.DeserializeObject<AutoBuffSkill>(config);
                if (oldAutoBuffSkill != null)
                {
                    if (!string.IsNullOrEmpty(oldAutoBuffSkill.ActionName))
                    {
                        this.ActionName = oldAutoBuffSkill.ActionName;
                    }

                    if (oldAutoBuffSkill.buffMapping != null)
                    {
                        this.buffMapping = oldAutoBuffSkill.buffMapping;
                    }

                    this._delay = oldAutoBuffSkill._delay;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error loading AutoBuffSkill configuration: {ex.Message}");
            }
        }

        public string GetActionName()
        {
            return this.ActionName;
        }

        private void UseAutobuff(Keys key)
        {
            try
            {
                if ((key != Keys.None) && !Win32Interop.IsKeyPressed(Keys.LMenu) && !Win32Interop.IsKeyPressed(Keys.RMenu))
                {
                    var client = ClientSingleton.GetClient();
                    if (client?.Process != null && !client.Process.HasExited)
                    {
                        Win32Interop.PostMessage(client.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"AutoBuffSkill: Error using autobuff key {key}: {ex.Message}");
            }
        }

        // Properties for monitoring and diagnostics
        public int ConsecutiveErrorCount => consecutiveErrors;
        public DateTime LastSuccessfulRead => lastSuccessfulRead;

        // Method to manually reset error tracking
        public void ResetErrorTracking()
        {
            consecutiveErrors = 0;
            lastSuccessfulRead = DateTime.Now;
        }
    }
}