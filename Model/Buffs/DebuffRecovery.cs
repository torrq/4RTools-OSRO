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
    public class DebuffRecovery : IAction
    {
        public static string ACTION_NAME_DEBUFF_RECOVERY = "DebuffsRecovery";

        private ThreadRunner thread;
        public Dictionary<EffectStatusIDs, Keys> buffMapping = new Dictionary<EffectStatusIDs, Keys>();
        public int Delay { get; set; } = 50;

        private readonly string ActionName;

        // Add error tracking
        private int consecutiveErrors = 0;
        private const int maxConsecutiveErrors = 5;
        private DateTime lastSuccessfulRead = DateTime.Now;

        // Default constructor
        public DebuffRecovery() : this(ACTION_NAME_DEBUFF_RECOVERY)
        {
        }

        // Constructor with custom action name
        public DebuffRecovery(string actionName)
        {
            this.ActionName = actionName;
        }

        public string GetActionName()
        {
            return this.ActionName;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public ThreadRunner DebuffRecoveryThread(Client c)
        {
            Client roClient = ClientSingleton.GetClient();
            return new ThreadRunner(_ =>
            {
                try
                {
                    // Check if client is still valid before proceeding
                    if (c?.Process == null || c.Process.HasExited)
                    {
                        DebugLogger.Debug("DebuffRecovery: Client process is null or has exited, stopping thread.");
                        FormHelper.ToggleStateOff("DebuffRecovery");
                        return -1; // Signal thread to stop
                    }

                    // Check if we've had too many consecutive errors
                    if (consecutiveErrors >= maxConsecutiveErrors)
                    {
                        var timeSinceLastSuccess = DateTime.Now - lastSuccessfulRead;
                        if (timeSinceLastSuccess.TotalSeconds > 30) // Wait 30 seconds before retrying
                        {
                            DebugLogger.Debug($"DebuffRecovery: Too many consecutive errors ({consecutiveErrors}), waiting before retry...");
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

                            // Check if we have a mapping for this status
                            if (buffMapping.ContainsKey(status))
                            {
                                Keys key = buffMapping[status];
                                if (Enum.IsDefined(typeof(EffectStatusIDs), currentStatus))
                                {
                                    this.UseStatusRecovery(key);
                                    DebugLogger.Debug($"DebuffRecovery: Used key {key} for status {status}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Debug($"DebuffRecovery: Error reading status at index {i}: {ex.Message}");
                            hadError = true;
                            break; // Break the loop on error
                        }
                    }

                    // Update error tracking
                    if (hadError)
                    {
                        consecutiveErrors++;
                        DebugLogger.Debug($"DebuffRecovery: Consecutive errors: {consecutiveErrors}");
                    }
                    else
                    {
                        consecutiveErrors = 0;
                        lastSuccessfulRead = DateTime.Now;
                    }

                    // If we couldn't read any status and had an error, the process might be invalid
                    if (hadError && !foundAnyStatus)
                    {
                        DebugLogger.Debug("DebuffRecovery: Could not read any status data, process may be invalid.");
                    }
                }
                catch (Exception ex)
                {
                    consecutiveErrors++;
                    DebugLogger.Debug($"DebuffRecovery: Thread exception: {ex.Message}");
                }

                Thread.Sleep(this.Delay);
                return 0;
            }, "DebuffRecovery");
        }

        public string GetConfiguration()
        {
            // Create a configuration object that includes both mapping and delay
            var configData = new Dictionary<string, object>
            {
                ["BuffMapping"] = this.buffMapping,
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
                            this.Delay = Math.Max(100, delay); // Minimum 100ms delay
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
                // Try to deserialize as the old format (full DebuffRecovery object)
                var oldDebuffRecovery = JsonConvert.DeserializeObject<DebuffRecovery>(config);
                if (oldDebuffRecovery != null)
                {
                    if (oldDebuffRecovery.buffMapping != null)
                    {
                        this.buffMapping = oldDebuffRecovery.buffMapping;
                    }

                    if (oldDebuffRecovery.Delay > 0)
                    {
                        this.Delay = Math.Max(100, oldDebuffRecovery.Delay);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error loading DebuffRecovery configuration: {ex.Message}");
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
                this.thread = DebuffRecoveryThread(roClient);
                ThreadRunner.Start(this.thread);
            }
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
                DebugLogger.Debug($"DebuffRecovery: Removed mapping for status {status}");
            }
        }

        public void ClearAllMappings()
        {
            buffMapping.Clear();
            //DebugLogger.Debug("DebuffRecovery: Cleared all status mappings");
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
                DebugLogger.Debug($"DebuffRecovery: Error using status recovery key {key}: {ex.Message}");
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