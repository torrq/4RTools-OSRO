using _ORTools.Forms;
using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
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

        public Dictionary<EffectStatusIDs, Keys> buffMapping = new Dictionary<EffectStatusIDs, Keys>();

        // Add error tracking
        private int consecutiveErrors = 0;
        private const int maxConsecutiveErrors = 5;
        private DateTime lastSuccessfulRead = DateTime.Now;

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
                        DebugLogger.Debug("AutoBuffItem: Client process is null or has exited, stopping thread.");
                        FormHelper.ToggleStateOff("AutobuffItem");
                        return -1; // Signal thread to stop
                    }

                    // Check if we've had too many consecutive errors
                    if (consecutiveErrors >= maxConsecutiveErrors)
                    {
                        var timeSinceLastSuccess = DateTime.Now - lastSuccessfulRead;
                        if (timeSinceLastSuccess.TotalSeconds > 10) // Wait 10 seconds before retrying
                        {
                            DebugLogger.Debug($"AutoBuffItem: Too many consecutive errors ({consecutiveErrors}), waiting before retry...");
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
                        DebugLogger.Debug($"AutoBuffItem: Error reading map or preferences: {ex.Message}");
                        hadError = true;
                    }

                    if (!hadError && prefs != null)
                    {
                        // Collect and log statuses always
                        var statusList = new List<(int index, uint statusId)>();
                        bool statusReadError = false;

                        for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                        {
                            try
                            {
                                uint currentStatus = c.CurrentBuffStatusCode(i);

                                if (StatusUtils.IsValidStatus(currentStatus))
                                {
                                    statusList.Add((i, currentStatus));
                                    //DebugLogger.Debug("currentStatus: " + i + ":" + currentStatus);
                                }
                            }
                            catch (Exception ex)
                            {
                                DebugLogger.Debug($"AutoBuffItem: Error reading status at index {i}: {ex.Message}");
                                statusReadError = true;
                                break;
                            }
                        }

                        if (!statusReadError)
                        {
                            try
                            {
                                StatusEffectLogger.LogAllStatuses(statusList);
                            }
                            catch (Exception ex)
                            {
                                DebugLogger.Debug($"AutoBuffItem: Error logging statuses: {ex.Message}");
                            }

                            if (!prefs.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
                            {
                                // Process buffs
                                List<EffectStatusIDs> buffs = new List<EffectStatusIDs>();
                                Dictionary<EffectStatusIDs, Keys> bmClone = new Dictionary<EffectStatusIDs, Keys>(this.buffMapping);

                                foreach (var (i, currentStatus) in statusList)
                                {
                                    if (!StatusUtils.IsValidStatus(currentStatus)) { continue; }

                                    buffs.Add((EffectStatusIDs)currentStatus);
                                    EffectStatusIDs status = (EffectStatusIDs)currentStatus;

                                    if (status == EffectStatusIDs.WS_OVERTHRUSTMAX)
                                    {
                                        if (buffMapping.ContainsKey(EffectStatusIDs.BS_OVERTHRUST))
                                        {
                                            bmClone.Remove(EffectStatusIDs.BS_OVERTHRUST);
                                        }
                                    }

                                    if (buffMapping.ContainsKey(status))
                                    {
                                        bmClone.Remove(status);
                                    }

                                    if (status == EffectStatusIDs.WZ_QUAGMIRE) foundQuag = true;
                                    if (status == EffectStatusIDs.AL_DECAGI) foundDecreaseAgi = true;
                                }

                                buffs.Clear();

                                // Apply buffs with error handling
                                foreach (var item in bmClone)
                                {
                                    try
                                    {
                                        if (foundQuag && (item.Key == EffectStatusIDs.AC_CONCENTRATION || item.Key == EffectStatusIDs.AL_INCAGI || item.Key == EffectStatusIDs.SN_SIGHT || item.Key == EffectStatusIDs.BS_ADRENALINE || item.Key == EffectStatusIDs.CR_SPEARQUICKEN || item.Key == EffectStatusIDs.KN_ONEHAND || item.Key == EffectStatusIDs.SN_WINDWALK))
                                        {
                                            break;
                                        }
                                        else if (foundDecreaseAgi && (item.Key == EffectStatusIDs.KN_TWOHANDQUICKEN || item.Key == EffectStatusIDs.BS_ADRENALINE || item.Key == EffectStatusIDs.BS_ADRENALINE2 || item.Key == EffectStatusIDs.KN_ONEHAND || item.Key == EffectStatusIDs.CR_SPEARQUICKEN))
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            uint currentHp = c.ReadCurrentHp();
                                            if (currentHp >= Constants.MINIMUM_HP_TO_RECOVER)
                                            {
                                                this.UseAutobuff(item.Value);
                                                Thread.Sleep(Delay);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugLogger.Debug($"AutoBuffItem: Error applying buff {item.Key}: {ex.Message}");
                                        hadError = true;
                                        break;
                                    }
                                }

                                // Collect and log statuses again after autobuff actions
                                if (!hadError)
                                {
                                    try
                                    {
                                        statusList.Clear();
                                        for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                                        {
                                            uint currentStatus = c.CurrentBuffStatusCode(i);
                                            if (StatusUtils.IsValidStatus(currentStatus))
                                            {
                                                statusList.Add((i, currentStatus));
                                            }
                                        }

                                        StatusEffectLogger.LogAllStatuses(statusList);
                                    }
                                    catch (Exception ex)
                                    {
                                        DebugLogger.Debug($"AutoBuffItem: Error re-reading statuses: {ex.Message}");
                                        hadError = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            hadError = true;
                        }
                    }

                    // Update error tracking
                    if (hadError)
                    {
                        consecutiveErrors++;
                        DebugLogger.Debug($"AutoBuffItem: Consecutive errors: {consecutiveErrors}");
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
                    DebugLogger.Debug($"AutoBuffItem: Thread exception: {ex.Message}");
                }

                Thread.Sleep(300);
                return 0;
            }, "AutobuffItem");

            return autobuffItemThread;
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
                DebugLogger.Debug($"AutoBuffItem: Removed mapping for status {status}");
            }
        }

        public void ClearKeyMapping()
        {
            buffMapping.Clear();
            //DebugLogger.Debug("AutoBuffItem: Cleared all key mappings");
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
                // Try to deserialize as the old format (full AutoBuffItem object)
                var oldAutoBuffItem = JsonConvert.DeserializeObject<AutoBuffItem>(config);
                if (oldAutoBuffItem != null)
                {
                    if (!string.IsNullOrEmpty(oldAutoBuffItem.ActionName))
                    {
                        this.ActionName = oldAutoBuffItem.ActionName;
                    }

                    if (oldAutoBuffItem.buffMapping != null)
                    {
                        this.buffMapping = oldAutoBuffItem.buffMapping;
                    }

                    this._delay = oldAutoBuffItem._delay;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error loading AutoBuffItem configuration: {ex.Message}");
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
                DebugLogger.Debug($"AutoBuffItem: Error using autobuff key {key}: {ex.Message}");
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