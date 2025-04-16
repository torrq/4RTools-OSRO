using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
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

    public class AHK : IAction
    {
        // Import the mouse_event function from the Windows API
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const string ACTION_NAME = "AHK20";
        private _4RThread thread;
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

        public AHK()
        {
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    _4RThread.Stop(this.thread);
                }

                this.thread = new _4RThread(_ => AHKThreadExecution(roClient));
                _4RThread.Start(this.thread);
            }
        }

        private int AHKThreadExecution(Client roClient)
        {
            if (AHKMode.Equals(COMPATIBILITY))
            {
                foreach (KeyConfig config in AhkEntries.Values)
                {
                    Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.Key.ToString());
                    if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                    {
                        if (config.ClickActive && Keyboard.IsKeyDown(config.Key))
                        {
                            if (NoShift) keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY, 0);
                            AHKCompatibility(roClient, config, thisk);
                            if (NoShift) keybd_event(Constants.VK_SHIFT, 0x45, Constants.KEYEVENTF_EXTENDEDKEY | Constants.KEYEVENTF_KEYUP, 0);

                        }
                        else
                        {
                            this.AHKNoClick(roClient, config, thisk);
                        }
                    }
                }
            }
            else
            {
                foreach (KeyConfig config in AhkEntries.Values)
                {
                    Keys thisk = (Keys)Enum.Parse(typeof(Keys), config.Key.ToString());
                    this.AHKSpeedBoost(roClient, config, thisk);
                }
            }
            return 0;
        }

        private void AHKCompatibility(Client roClient, KeyConfig config, Keys thisk)
        {
            Func<int, int> send_click;

            send_click = (evt) =>
            {
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONDOWN, 0, 0);
                Thread.Sleep(1);
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONUP, 0, 0);
                return 0;
            };

            if (this.MouseFlick)
            {
                bool ammo = false;
                while (Keyboard.IsKeyDown(config.Key))
                {
                    AutoSwitchAmmo(roClient, ref ammo);
                    Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                    System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK, System.Windows.Forms.Cursor.Position.Y - Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK);
                    send_click(0);
                    System.Windows.Forms.Cursor.Position = new Point(System.Windows.Forms.Cursor.Position.X + Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK, System.Windows.Forms.Cursor.Position.Y + Constants.MOUSE_DIAGONAL_MOVIMENTATION_PIXELS_AHK);
                    Thread.Sleep(this.AhkDelay);
                }
            }
            else
            {
                bool ammo = false;
                while (Keyboard.IsKeyDown(config.Key))
                {
                    AutoSwitchAmmo(roClient, ref ammo);
                    Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                    send_click(0);
                    Thread.Sleep(this.AhkDelay);
                }
            }
        }

        private void AHKSynchronous(Client roClient, KeyConfig config, Keys thisk)
        {

            Func<int, int> send_click;
            //bool ammo = false;
            send_click = (evt) =>
            {
                SendMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
                Thread.Sleep(1);
                SendMessage(roClient.Process.MainWindowHandle, Constants.WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                return 0;
            };

        }

        private void AHKSpeedBoost(Client roClient, KeyConfig config, Keys thisk)
        {
            bool ammo = false;
            while (Keyboard.IsKeyDown(config.Key))
            {
                AutoSwitchAmmo(roClient, ref ammo);

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

        private void AutoSwitchAmmo(Client roClient, ref bool ammo)
        {
            UserPreferences prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (prefs.SwitchAmmo)
            {
                if (prefs.Ammo1Key.ToString() != String.Empty && prefs.Ammo2Key.ToString() != String.Empty)
                {
                    if (ammo == false)
                    {
                        Key key = prefs.Ammo1Key;
                        Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, ToKeys(key), 0);
                        ammo = true;
                    }
                    else
                    {
                        Key key = prefs.Ammo2Key;
                        Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, ToKeys(key), 0);
                        ammo = false;
                    }

                }
            }
        }

        private Keys ToKeys(Key k)
        {
            // Explicit mapping for D0-D9 and other special keys
            switch (k)
            {
                case Key.D0: return Keys.D0;
                case Key.D1: return Keys.D1;
                case Key.D2: return Keys.D2;
                case Key.D3: return Keys.D3;
                case Key.D4: return Keys.D4;
                case Key.D5: return Keys.D5;
                case Key.D6: return Keys.D6;
                case Key.D7: return Keys.D7;
                case Key.D8: return Keys.D8;
                case Key.D9: return Keys.D9;
                case Key.OemPlus: return Keys.Oemplus;
                case Key.OemTilde: return Keys.Oemtilde;
                case Key.OemComma: return Keys.Oemcomma;
                default:
                    // Use Enum.Parse for other keys
                    return (Keys)Enum.Parse(typeof(Keys), k.ToString());
            }
        }

        private void AHKNoClick(Client roClient, KeyConfig config, Keys thisk)
        {
            while (Keyboard.IsKeyDown(config.Key))
            {
                Interop.PostMessage(roClient.Process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, thisk, 0);
                Thread.Sleep(this.AhkDelay);
            }
        }

        public void AddAHKEntry(string chkboxName, KeyConfig value)
        {
            if (this.AhkEntries.ContainsKey(chkboxName))
            {
                RemoveAHKEntry(chkboxName);
            }

            this.AhkEntries.Add(chkboxName, value);

        }

        public void RemoveAHKEntry(string chkboxName)
        {
            this.AhkEntries.Remove(chkboxName);
        }

        public void Stop()
        {
            _4RThread.Stop(this.thread);
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
