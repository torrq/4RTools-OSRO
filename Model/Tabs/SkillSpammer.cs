using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Model
{
    public class KeyConfig
    {
        public Key Key { get; set; }
        public bool ClickActive { get; set; }
        public bool IsIndeterminate { get; set; }

        public KeyConfig(Key key, bool clickActive, bool isIndeterminate = false)
        {
            Key = key;
            ClickActive = clickActive;
            IsIndeterminate = isIndeterminate;
        }
    }

    public class SkillSpammer : IAction
    {
        [DllImport("user32.dll")] public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        [DllImport("user32.dll")] public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", SetLastError = true)] public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public static bool IsGameWindowActive()
        {
            try
            {
                Client currentClient = ClientSingleton.GetClient();
                if (currentClient?.Process == null || currentClient.Process.HasExited)
                {
                    return false;
                }

                IntPtr activeWindowHandle = GetForegroundWindow();
                if (activeWindowHandle == IntPtr.Zero)
                {
                    return false;
                }

                // Get the process ID of the active window
                uint activeProcessId;
                GetWindowThreadProcessId(activeWindowHandle, out activeProcessId);

                // Compare with our client's process ID
                return activeProcessId == currentClient.Process.Id;
            }
            catch (Exception ex)
            {
                DebugLogger.Debug($"Error checking if game window is active: {ex.Message}");
                return false;
            }
        }

        private const string ACTION_NAME = "AHK";
        private ThreadRunner thread;
        public const string COMPATIBILITY = "ahkCompatibility";
        public const string SPEED_BOOST = "ahkSpeedBoost";
        public Dictionary<string, KeyConfig> AhkEntries { get; set; } = new Dictionary<string, KeyConfig>();

        private int _delay = AppConfig.SkillSpammerDefaultDelay;
        public int AhkDelay
        {
            get => _delay <= 0 ? AppConfig.SkillSpammerDefaultDelay : _delay;
            set => _delay = value;
        }

        public bool MouseFlick { get; set; } = false;
        public bool NoShift { get; set; } = false;
        public string AHKMode { get; set; } = COMPATIBILITY;

        public SkillSpammer() { }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    ThreadRunner.Stop(this.thread);
                }

                this.thread = new ThreadRunner(_ => SkillSpammerThread(roClient), "SkillSpammer");

                ThreadRunner.Start(this.thread);
            }
        }

        private int SkillSpammerThread(Client roClient)
        {
            if (AHKMode.Equals(COMPATIBILITY))
            {
                foreach (KeyConfig config in AhkEntries.Values)
                {
                    Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.Key.ToString());
                    if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt) && SkillSpammer.IsGameWindowActive())
                    {
                        if (config.ClickActive && Keyboard.IsKeyDown(config.Key))
                        {
                            if (NoShift) keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY, 0);
                            SkillSpammerCompatibility(roClient, config, thisk);
                            if (NoShift) keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY | Constants.KEYEVENTF_KEYUP, 0);
                        }
                        else
                        {
                            this.SkillSpammerNoClick(roClient, config, thisk);
                        }
                    }
                }
            }
            else
            {
                if (SkillSpammer.IsGameWindowActive())
                {
                    foreach (KeyConfig config in AhkEntries.Values)
                    {
                        Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.Key.ToString());
                        this.SkillSpammerSpeedBoost(roClient, config, thisk);
                    }
                }
            }
            return 0;
        }

        private void SkillSpammerCompatibility(Client roClient, KeyConfig config, Keys thisk)
        {
            Func<int, int> send_click = (evt) =>
            {
                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                Thread.Sleep(1);
                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);
                return 0;
            };

            while (Keyboard.IsKeyDown(config.Key))
            {
                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                if (this.MouseFlick)
                {
                    System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK, System.Windows.Forms.Cursor.Position.Y - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK);
                    send_click(0);
                    System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X + Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK, System.Windows.Forms.Cursor.Position.Y + Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK);
                }
                else
                {
                    send_click(0);
                }
                Thread.Sleep(this.AhkDelay);
            }
        }

        private void SkillSpammerSpeedBoost(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.Key))
            {
                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                if (config.ClickActive)
                {
                    Point cursorPos = System.Windows.Forms.Cursor.Position;
                    mouse_event(Constants.MOUSEEVENTF_LEFTDOWN, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                    Thread.Sleep(1);
                    mouse_event(Constants.MOUSEEVENTF_LEFTUP, (uint)cursorPos.X, (uint)cursorPos.Y, 0, 0);
                }
                Thread.Sleep(this.AhkDelay);
            }
        }

        private void SkillSpammerNoClick(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.Key))
            {
                Win32Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                Thread.Sleep(this.AhkDelay);
            }
        }

        public void AddSkillSpammerEntry(string chkboxName, KeyConfig value)
        {
            if (this.AhkEntries.ContainsKey(chkboxName))
            {
                RemoveSkillSpammerEntry(chkboxName);
            }
            this.AhkEntries.Add(chkboxName, value);
        }

        public void RemoveSkillSpammerEntry(string chkboxName)
        {
            this.AhkEntries.Remove(chkboxName);
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
