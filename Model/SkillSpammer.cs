using BruteGamingMacros.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.Core.Model
{
    public class KeyConfig
    {
        public Key Key { get; set; }
        public bool ClickActive { get; set; }

        public KeyConfig(Key key, bool clickAtive)
        {
            this.Key = key;
            this.ClickActive = clickAtive;
        }
    }

    public class SkillSpammer : IAction
    {
        [DllImport("user32.dll")] public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        [DllImport("user32.dll")] public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", SetLastError = true)] public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();

        public bool IsGameWindowActive()
        {
            IntPtr foreground = GetForegroundWindow();
            return
                foreground == FindWindow(AppConfig.WindowClassLR, null) ||
                foreground == FindWindow(AppConfig.WindowClassMR, null) ||
                foreground == FindWindow(AppConfig.WindowClassHR, null);
        }

        private const string ACTION_NAME = "AHK";
        private ThreadRunner thread;
        private AmmoSwapHandler ammoSwapHandler = new AmmoSwapHandler();
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

                this.thread = new ThreadRunner(_ => SkillSpammerThreadExecution(roClient));
                ThreadRunner.Start(this.thread);

                ammoSwapHandler.Start();
            }
        }

        private int SkillSpammerThreadExecution(Client roClient)
        {
            if (AHKMode.Equals(COMPATIBILITY))
            {
                foreach (KeyConfig config in AhkEntries.Values)
                {
                    Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.Key.ToString());
                    if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt) && this.IsGameWindowActive())
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
                if (this.IsGameWindowActive())
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
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                Thread.Sleep(1);
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);
                return 0;
            };

            while (Keyboard.IsKeyDown(config.Key))
            {
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
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
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
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
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
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

            ammoSwapHandler.Stop();
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
