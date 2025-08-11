using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
{
    public class KeyConfig
    {
        public Keys Key { get; set; } // <- System.Windows.Forms.Keys
        public bool ClickActive { get; set; }
        public bool IsIndeterminate { get; set; }

        public KeyConfig(Keys key, bool clickActive, bool isIndeterminate = false)
        {
            Key = key;
            ClickActive = clickActive;
            IsIndeterminate = isIndeterminate;
        }
    }

    public class SkillSpammer : IAction
    {
        public static bool IsGameWindowActive()
        {
            try
            {
                Client currentClient = ClientSingleton.GetClient();
                if (currentClient?.Process == null || currentClient.Process.HasExited)
                {
                    return false;
                }

                IntPtr activeWindowHandle = Win32Interop.GetForegroundWindow();
                if (activeWindowHandle == IntPtr.Zero)
                {
                    return false;
                }

                uint activeProcessId;
                Win32Interop.GetWindowThreadProcessId(activeWindowHandle, out activeProcessId);

                return activeProcessId == currentClient.Process.Id;
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error checking if game window is active: {ex.Message}");
                return false;
            }
        }

        private const string ACTION_NAME = "SkillSpammer";
        private ThreadRunner thread;
        public Dictionary<string, KeyConfig> SpammerEntries { get; set; } = new Dictionary<string, KeyConfig>();

        private int _delay = AppConfig.SkillSpammerDefaultDelay;
        public int SpammerDelay
        {
            get => _delay <= 0 ? AppConfig.SkillSpammerDefaultDelay : _delay;
            set => _delay = value;
        }

        public SkillSpammer() { }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    ThreadRunner.Stop(this.thread);
                    this.thread.Terminate();
                }

                this.thread = new ThreadRunner(_ => SkillSpammerThread(roClient), "SkillSpammerThread");
                ThreadRunner.Start(this.thread);
            }
        }

        private int SkillSpammerThread(Client roClient)
        {
            if (!SkillSpammer.IsGameWindowActive())
                return 0;

            foreach (var kvp in SpammerEntries)
            {
                var config = kvp.Value;
                var keyToSpam = config.Key;

                // Only process if the entry is either checked (ClickActive=true) or indeterminate (IsIndeterminate=true)
                // Unchecked entries (ClickActive=false, IsIndeterminate=false) should be ignored
                bool shouldProcess = config.ClickActive || config.IsIndeterminate;

                if (shouldProcess)
                {
                    // Use the simple, proven approach from the older code
                    SkillSpammerSpeedBoost(roClient, config, keyToSpam);
                }
            }

            return 0;
        }

        private void SkillSpammerSpeedBoost(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Win32Interop.IsKeyPressed(config.Key))
            {
                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);

                // Handle clicking based on config state
                // Checked (ClickActive=true, IsIndeterminate=false) = Key + Click
                // Indeterminate (IsIndeterminate=true) = Key only, no click
                if (config.ClickActive && !config.IsIndeterminate)
                {
                    Point cursorPos = System.Windows.Forms.Cursor.Position;
                    Win32Interop.mouse_event(Constants.MOUSEEVENTF_LEFTDOWN, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                    Thread.Sleep(1);
                    Win32Interop.mouse_event(Constants.MOUSEEVENTF_LEFTUP, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                }

                Thread.Sleep(this.SpammerDelay);
            }
        }

        public void AddSkillSpammerEntry(string entryName, KeyConfig value)
        {
            if (this.SpammerEntries.ContainsKey(entryName))
            {
                RemoveSkillSpammerEntry(entryName);
            }
            this.SpammerEntries.Add(entryName, value);
        }

        public void RemoveSkillSpammerEntry(string entryName)
        {
            this.SpammerEntries.Remove(entryName);
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
            return ACTION_NAME;
        }
    }
}