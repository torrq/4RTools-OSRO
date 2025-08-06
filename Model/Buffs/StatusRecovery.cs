using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
{
    public class StatusRecovery : IAction
    {
        public static string ACTION_NAME_PANACEA_AUTOBUFF = "StatusRecovery";

        private ThreadRunner thread;

        // Dictionary to store multiple status lists with their associated keys
        public Dictionary<string, StatusRecoveryList> statusLists = new Dictionary<string, StatusRecoveryList>();

        // Legacy property for backward compatibility with old JSON format
        [JsonIgnore]
        public Dictionary<EffectStatusIDs, Keys> buffMapping
        {
            get
            {
                // Return the Panacea mapping for backward compatibility
                var mapping = new Dictionary<EffectStatusIDs, Keys>();
                if (statusLists.ContainsKey("Panacea") && statusLists["Panacea"].Key != Keys.None)
                {
                    var panaceaKey = statusLists["Panacea"].Key;
                    foreach (var status in statusLists["Panacea"].Statuses)
                    {
                        mapping[status] = panaceaKey;
                    }
                }
                return mapping;
            }
            set
            {
                // Handle setting from old JSON format
                if (value != null && value.Count > 0)
                {
                    var firstKey = value.Values.FirstOrDefault();
                    if (firstKey != Keys.None)
                    {
                        SetKeyForList("Panacea", firstKey);
                    }
                }
            }
        }

        public int Delay { get; set; } = 50;

        // Add error tracking
        private int consecutiveErrors = 0;
        private const int maxConsecutiveErrors = 5;
        private DateTime lastSuccessfulRead = DateTime.Now;

        public StatusRecovery()
        {
            InitializeDefaultLists();
        }

        private void InitializeDefaultLists()
        {
            // Panacea list - cures many major debuffs
            var panaceaStatuses = new List<EffectStatusIDs>
            {
                EffectStatusIDs.POISON,
                EffectStatusIDs.SILENCE,
                EffectStatusIDs.BLIND,
                EffectStatusIDs.CURSE,
                EffectStatusIDs.CONFUSION,
                EffectStatusIDs.HALLUCINATION,
                EffectStatusIDs.BLEEDING
            };
            statusLists["Panacea"] = new StatusRecoveryList("Panacea", panaceaStatuses, Keys.None);

            // Royal Jelly list - cures many major debuffs
            var royalJellyStatuses = new List<EffectStatusIDs>
            {
                EffectStatusIDs.POISON,
                EffectStatusIDs.SILENCE,
                EffectStatusIDs.BLIND,
                EffectStatusIDs.CURSE,
                EffectStatusIDs.CONFUSION,
                EffectStatusIDs.HALLUCINATION,
                EffectStatusIDs.BLEEDING
            };
            statusLists["RoyalJelly"] = new StatusRecoveryList("RoyalJelly", royalJellyStatuses, Keys.None);

            // Green Potion list - cures Poison and Silence
            var greenPotionStatuses = new List<EffectStatusIDs>
            {
                EffectStatusIDs.POISON,
                EffectStatusIDs.SILENCE,
            };
            statusLists["GreenPotion"] = new StatusRecoveryList("GreenPotion", greenPotionStatuses, Keys.None);
        }

        public string GetActionName()
        {
            return ACTION_NAME_PANACEA_AUTOBUFF;
        }

        public ThreadRunner StatusRecoveryThread(Client c)
        {
            Client roClient = ClientSingleton.GetClient();
            ThreadRunner statusEffectsThread = new ThreadRunner(_ =>
            {
                try
                {
                    // Check if client is still valid before proceeding
                    if (c?.Process == null || c.Process.HasExited)
                    {
                        DebugLogger.Debug("StatusRecovery: Client process is null or has exited, stopping thread.");
                        return -1; // Signal thread to stop
                    }

                    // Check if we've had too many consecutive errors
                    if (consecutiveErrors >= maxConsecutiveErrors)
                    {
                        var timeSinceLastSuccess = DateTime.Now - lastSuccessfulRead;
                        if (timeSinceLastSuccess.TotalSeconds > 30) // Wait 30 seconds before retrying
                        {
                            DebugLogger.Debug($"StatusRecovery: Too many consecutive errors ({consecutiveErrors}), waiting before retry...");
                            Thread.Sleep(5000); // Wait 5 seconds
                            consecutiveErrors = 0; // Reset error count
                            c.RefreshLoginStatus(); // Force refresh process status
                        }
                        else
                        {
                            Thread.Sleep(this.Delay);
                            return 0;
                        }
                    }

                    bool hadError = false;
                    bool foundAnyStatus = false;

                    for (int i = 0; i <= Constants.MAX_BUFF_LIST_INDEX_SIZE - 1; i++)
                    {
                        try
                        {
                            uint currentStatus = c.CurrentBuffStatusCode(i);

                            if (currentStatus == uint.MaxValue)
                            {
                                continue;
                            }

                            foundAnyStatus = true;
                            EffectStatusIDs status = (EffectStatusIDs)currentStatus;

                            // Check each status list to see if any contains the current status
                            foreach (var statusList in statusLists.Values)
                            {
                                if (statusList.ContainsStatus(status) && statusList.Key != Keys.None)
                                {
                                    this.UseStatusRecovery(statusList.Key);
                                    DebugLogger.Debug($"StatusRecovery: Used {statusList.Name} for status {status}");
                                    break; // Use first matching list only
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Debug($"StatusRecovery: Error reading status at index {i}: {ex.Message}");
                            hadError = true;
                            break; // Break the loop on error
                        }
                    }

                    // Update error tracking
                    if (hadError)
                    {
                        consecutiveErrors++;
                        DebugLogger.Debug($"StatusRecovery: Consecutive errors: {consecutiveErrors}");
                    }
                    else
                    {
                        consecutiveErrors = 0;
                        lastSuccessfulRead = DateTime.Now;
                    }

                    // If we couldn't read any status and had an error, the process might be invalid
                    if (hadError && !foundAnyStatus)
                    {
                        DebugLogger.Debug("StatusRecovery: Could not read any status data, process may be invalid.");
                    }
                }
                catch (Exception ex)
                {
                    consecutiveErrors++;
                    DebugLogger.Debug($"StatusRecovery: Thread exception: {ex.Message}");
                }

                Thread.Sleep(this.Delay);
                return 0;
            }, "StatusRecovery");

            return statusEffectsThread;
        }

        public string GetConfiguration()
        {
            // Only serialize the keys, not the predefined status lists
            var configData = new Dictionary<string, object>
            {
                ["Keys"] = statusLists.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Key),
                ["Delay"] = this.Delay
            };
            return JsonConvert.SerializeObject(configData);
        }

        public void LoadConfiguration(string config)
        {
            try
            {
                // Try to deserialize as the new format with Delay
                var configData = JsonConvert.DeserializeObject<Dictionary<string, object>>(config);
                if (configData != null)
                {
                    // Load keys
                    if (configData.ContainsKey("Keys"))
                    {
                        var keysData = JsonConvert.DeserializeObject<Dictionary<string, Keys>>(configData["Keys"].ToString());
                        if (keysData != null)
                        {
                            foreach (var kvp in keysData)
                            {
                                if (statusLists.ContainsKey(kvp.Key))
                                {
                                    statusLists[kvp.Key].Key = kvp.Value;
                                }
                            }
                        }
                    }

                    // Load delay
                    if (configData.ContainsKey("Delay"))
                    {
                        if (int.TryParse(configData["Delay"].ToString(), out int delay))
                        {
                            this.Delay = Math.Max(100, delay); // Minimum 100ms delay
                        }
                    }
                    return;
                }
            }
            catch
            {
                // Fall back to old formats
            }

            try
            {
                // Try to deserialize as simple key mapping
                var configData = JsonConvert.DeserializeObject<Dictionary<string, Keys>>(config);
                if (configData != null)
                {
                    foreach (var kvp in configData)
                    {
                        if (statusLists.ContainsKey(kvp.Key))
                        {
                            statusLists[kvp.Key].Key = kvp.Value;
                        }
                    }
                    return;
                }
            }
            catch
            {
                // Fall back to oldest format
            }

            try
            {
                // Try to deserialize as the oldest format (full StatusRecovery object)
                var oldStatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(config);
                if (oldStatusRecovery != null)
                {
                    // Migrate old buffMapping to new format
                    if (oldStatusRecovery.buffMapping != null && oldStatusRecovery.buffMapping.Count > 0)
                    {
                        var firstKey = oldStatusRecovery.buffMapping.Values.FirstOrDefault();
                        if (firstKey != Keys.None)
                        {
                            SetKeyForList("Panacea", firstKey);
                        }
                    }

                    // Copy other properties if they exist
                    if (oldStatusRecovery.Delay > 0)
                    {
                        this.Delay = Math.Max(100, oldStatusRecovery.Delay);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error loading StatusRecovery configuration: {ex.Message}");
            }
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
                this.thread = StatusRecoveryThread(roClient);
                ThreadRunner.Start(this.thread);
            }
        }

        public void SetKeyForList(string listName, Keys key)
        {
            if (statusLists.ContainsKey(listName))
            {
                statusLists[listName].Key = FormHelper.IsValidKey(key) ? key : Keys.None;
            }
        }

        public Keys GetKeyForList(string listName)
        {
            return statusLists.ContainsKey(listName) ? statusLists[listName].Key : Keys.None;
        }

        public List<string> GetAvailableLists()
        {
            return statusLists.Keys.ToList();
        }

        public StatusRecoveryList GetList(string listName)
        {
            return statusLists.ContainsKey(listName) ? statusLists[listName] : null;
        }

        // Legacy method for backward compatibility
        public void AddKeyToBuff(EffectStatusIDs status, Keys key)
        {
            // For backward compatibility, assume this is for Panacea
            SetKeyForList("Panacea", key);
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

        private void UseStatusRecovery(Keys key)
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
                DebugLogger.Debug($"StatusRecovery: Error using status recovery key {key}: {ex.Message}");
            }
        }
    }

    // Helper class to represent a status recovery list
    public class StatusRecoveryList
    {
        public string Name { get; set; }
        public List<EffectStatusIDs> Statuses { get; set; }
        public Keys Key { get; set; }

        public StatusRecoveryList(string name, List<EffectStatusIDs> statuses, Keys key)
        {
            Name = name;
            Statuses = statuses ?? new List<EffectStatusIDs>();
            Key = key;
        }

        public bool ContainsStatus(EffectStatusIDs status)
        {
            return Statuses.Contains(status);
        }
    }
}