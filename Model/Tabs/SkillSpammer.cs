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
        public Keys Key { get; set; }
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
        private Dictionary<Keys, bool> toggledKeys = new Dictionary<Keys, bool>();
        private Dictionary<Keys, bool> keyPressedLastFrame = new Dictionary<Keys, bool>();

        public event EventHandler<bool> ToggleModeChanged;

        public Keys ToggleModeKey { get; set; } = Keys.None;

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

        public bool MouseFlick { get; set; } = false;

        public bool NoShift { get; set; } = false;

        public bool ToggleMode { get; set; } = false;

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

            // Cache expensive lookups once per iteration
            IntPtr windowHandle = roClient.Process.MainWindowHandle;
            bool noShift = this.NoShift;
            bool mouseFlick = this.MouseFlick;

            // Handle toggle mode key press
            if (this.ToggleModeKey != Keys.None)
            {
                bool isToggleKeyPressed = Win32Interop.IsKeyPressed(this.ToggleModeKey);
                bool wasToggleKeyPressed = keyPressedLastFrame.ContainsKey(this.ToggleModeKey) && keyPressedLastFrame[this.ToggleModeKey];

                if (isToggleKeyPressed && !wasToggleKeyPressed)
                {
                    this.ToggleMode = !this.ToggleMode;
                    ToggleModeChanged?.Invoke(this, this.ToggleMode);
                    ProfileSingleton.SetConfiguration(this);

                    if (!this.ToggleMode)
                    {
                        toggledKeys.Clear();
                    }
                }

                keyPressedLastFrame[this.ToggleModeKey] = isToggleKeyPressed;
            }

            foreach (var kvp in SpammerEntries)
            {
                var config = kvp.Value;
                if (config.ClickActive || config.IsIndeterminate)
                {
                    SkillSpammerSpeedBoost(config, windowHandle, noShift, mouseFlick);
                }
            }

            return 0;
        }

        private void SkillSpammerSpeedBoost(KeyConfig config, IntPtr windowHandle, bool noShift, bool mouseFlick)
        {
            bool isKeyPressed = Win32Interop.IsKeyPressed(config.Key);
            bool wasKeyPressed = keyPressedLastFrame.ContainsKey(config.Key) && keyPressedLastFrame[config.Key];

            if (this.ToggleMode)
            {
                if (isKeyPressed && !wasKeyPressed)
                {
                    if (!toggledKeys.ContainsKey(config.Key))
                        toggledKeys[config.Key] = false;

                    toggledKeys[config.Key] = !toggledKeys[config.Key];
                }

                keyPressedLastFrame[config.Key] = isKeyPressed;

                if (toggledKeys.ContainsKey(config.Key) && toggledKeys[config.Key])
                {
                    ExecuteSkillSpam(config, windowHandle, noShift, mouseFlick);
                }
            }
            else
            {
                if (isKeyPressed)
                {
                    ExecuteSkillSpam(config, windowHandle, noShift, mouseFlick);
                }
            }
        }

        private void ExecuteSkillSpam(KeyConfig config, IntPtr windowHandle, bool noShift, bool mouseFlick)
        {
            if (noShift)
            {
                Win32Interop.keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY, 0);
            }

            Win32Interop.PostMessage(windowHandle, Constants.WM_KEYDOWN_MSG_ID, config.Key, 0);

            if (config.ClickActive && !config.IsIndeterminate)
            {
                Point cursorPos = System.Windows.Forms.Cursor.Position;

                if (mouseFlick)
                {
                    Point flickPos = new Point(
                        cursorPos.X - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK,
                        cursorPos.Y - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK
                    );

                    System.Windows.Forms.Cursor.Position = flickPos;
                    Win32Interop.mouse_event(Constants.MOUSEEVENTF_LEFTDOWN, (uint)flickPos.X, (uint)flickPos.Y, 0, 0);
                    Thread.Sleep(1);
                    Win32Interop.mouse_event(Constants.MOUSEEVENTF_LEFTUP, (uint)flickPos.X, (uint)flickPos.Y, 0, 0);
                    System.Windows.Forms.Cursor.Position = cursorPos;
                }
                else
                {
                    Win32Interop.mouse_event(Constants.MOUSEEVENTF_LEFTDOWN, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                    Thread.Sleep(1);
                    Win32Interop.mouse_event(Constants.MOUSEEVENTF_LEFTUP, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                }
            }

            if (noShift)
            {
                Win32Interop.keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY | Constants.KEYEVENTF_KEYUP, 0);
            }

            Thread.Sleep(this.SpammerDelay);
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