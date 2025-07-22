using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

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
        public Dictionary<EffectStatusIDs, Key> buffMapping
        {
            get
            {
                // Return the Panacea mapping for backward compatibility
                var mapping = new Dictionary<EffectStatusIDs, Key>();
                if (statusLists.ContainsKey("Panacea") && statusLists["Panacea"].Key != Key.None)
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
                    if (firstKey != Key.None)
                    {
                        SetKeyForList("Panacea", firstKey);
                    }
                }
            }
        }

        public int Delay { get; set; } = 1;

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
            statusLists["Panacea"] = new StatusRecoveryList("Panacea", panaceaStatuses, Key.None);

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
            statusLists["RoyalJelly"] = new StatusRecoveryList("RoyalJelly", royalJellyStatuses, Key.None);

            // Green Potion list - cures Poison and Silence
            var greenPotionStatuses = new List<EffectStatusIDs>
            {
                EffectStatusIDs.POISON,
                EffectStatusIDs.SILENCE,
            };
            statusLists["GreenPotion"] = new StatusRecoveryList("GreenPotion", greenPotionStatuses, Key.None);
        }

        public string GetActionName()
        {
            return ACTION_NAME_PANACEA_AUTOBUFF;
        }

        public ThreadRunner RestoreStatusThread(Client c)
        {
            Client roClient = ClientSingleton.GetClient();
            ThreadRunner statusEffectsThread = new ThreadRunner(_ =>
            {
                for (int i = 0; i <= Constants.MAX_BUFF_LIST_INDEX_SIZE - 1; i++)
                {
                    uint currentStatus = c.CurrentBuffStatusCode(i);

                    if (currentStatus == uint.MaxValue) { continue; }

                    EffectStatusIDs status = (EffectStatusIDs)currentStatus;

                    // Check each status list to see if any contains the current status
                    foreach (var statusList in statusLists.Values)
                    {
                        if (statusList.ContainsStatus(status) && statusList.Key != Key.None)
                        {
                            this.UseStatusRecovery(statusList.Key);
                            break; // Use first matching list only
                        }
                    }
                }
                Thread.Sleep(this.Delay);
                return 0;
            });

            return statusEffectsThread;
        }

        public string GetConfiguration()
        {
            // Only serialize the keys, not the predefined status lists
            var configData = new Dictionary<string, Key>();
            foreach (var kvp in statusLists)
            {
                configData[kvp.Key] = kvp.Value.Key;
            }
            return JsonConvert.SerializeObject(configData);
        }

        public void LoadConfiguration(string config)
        {
            try
            {
                // First try to deserialize as the new format (Dictionary<string, Key>)
                var configData = JsonConvert.DeserializeObject<Dictionary<string, Key>>(config);
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
                // If that fails, try to deserialize as the old format (full StatusRecovery object)
                try
                {
                    var oldStatusRecovery = JsonConvert.DeserializeObject<StatusRecovery>(config);
                    if (oldStatusRecovery != null)
                    {
                        // Migrate old buffMapping to new format
                        if (oldStatusRecovery.buffMapping != null && oldStatusRecovery.buffMapping.Count > 0)
                        {
                            // Check if this looks like the old Panacea mapping (multiple statuses with same key)
                            var firstKey = oldStatusRecovery.buffMapping.Values.FirstOrDefault();
                            if (firstKey != Key.None)
                            {
                                // Set the Panacea key
                                SetKeyForList("Panacea", firstKey);
                            }
                        }

                        // Copy other properties if they exist
                        if (oldStatusRecovery.Delay > 0)
                        {
                            this.Delay = oldStatusRecovery.Delay;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle deserialization errors
                    Console.WriteLine($"Error loading StatusRecovery configuration: {ex.Message}");
                }
            }
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
                this.thread = RestoreStatusThread(roClient);
                ThreadRunner.Start(this.thread);
            }
        }

        public void SetKeyForList(string listName, Key key)
        {
            if (statusLists.ContainsKey(listName))
            {
                statusLists[listName].Key = FormHelper.IsValidKey(key) ? key : Key.None;
            }
        }

        public Key GetKeyForList(string listName)
        {
            return statusLists.ContainsKey(listName) ? statusLists[listName].Key : Key.None;
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
        public void AddKeyToBuff(EffectStatusIDs status, Key key)
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

        private void UseStatusRecovery(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                Win32Interop.PostMessage(ClientSingleton.GetClient().Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
            }
        }
    }

    // Helper class to represent a status recovery list
    public class StatusRecoveryList
    {
        public string Name { get; set; }
        public List<EffectStatusIDs> Statuses { get; set; }
        public Key Key { get; set; }

        public StatusRecoveryList(string name, List<EffectStatusIDs> statuses, Key key)
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