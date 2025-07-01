using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Utils
{
    public static class OverweightMacro
    {
        private static readonly Dictionary<Key, string> _sendKeysMap = new Dictionary<Key, string>()
        {
             { Key.D0, "0" },
             { Key.D1, "1" },
             { Key.D2, "2" },
             { Key.D3, "3" },
             { Key.D4, "4" },
             { Key.D5, "5" },
             { Key.D6, "6" },
             { Key.D7, "7" },
             { Key.D8, "8" },
             { Key.D9, "9" }
        };

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static string ToSendKeysFormat(Key key)
        {
            if (_sendKeysMap.TryGetValue(key, out string value))
            {
                return value;
            }
            return key.ToString().ToLower();
        }

        public static void SendOverweightMacro(string percentage, int times = 2, int intervalMs = 5000)
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (!string.IsNullOrEmpty(prefs.OverweightKey.ToString()) && prefs.OverweightKey.ToString() != "None")
            {
                IntPtr handle = ClientSingleton.GetClient().Process.MainWindowHandle;
                SetForegroundWindow(handle);

                Thread.Sleep(1000);

                string keyToSend = "%" + ToSendKeysFormat(prefs.OverweightKey);
                for (int i = 0; i < times; i++)
                {
                    SendKeys.SendWait(keyToSend);
                    DebugLogger.Info($"Sent macro {i + 1}/{times}: Alt + {prefs.OverweightKey} (Overweight {percentage}%)");

                    if (i < times - 1)
                    {
                        Thread.Sleep(intervalMs);
                    }
                }
            }
        }
    }
}