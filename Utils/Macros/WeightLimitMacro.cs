using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using _ORTools.Model;

namespace _ORTools.Utils
{
    public static class WeightLimitMacro
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        private static readonly Dictionary<Keys, string> _sendKeysMap = new Dictionary<Keys, string>()
        {
             { Keys.D0, "0" },
             { Keys.D1, "1" },
             { Keys.D2, "2" },
             { Keys.D3, "3" },
             { Keys.D4, "4" },
             { Keys.D5, "5" },
             { Keys.D6, "6" },
             { Keys.D7, "7" },
             { Keys.D8, "8" },
             { Keys.D9, "9" }
        };

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static string ToSendKeysFormat(Keys key)
        {
            if (_sendKeysMap.TryGetValue(key, out string value))
            {
                return value;
            }
            return key.ToString().ToLower();
        }

        public static void SendOverweightMacro()
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            int timesToSend = 2;
            int intervalMs = 5000;
            string keyToSend;
            bool ShouldSendKey1 = (!string.IsNullOrEmpty(prefs.AutoOffKey1.ToString()) && prefs.AutoOffKey1.ToString() != AppConfig.TEXT_NONE);
            bool ShouldSendKey2 = (!string.IsNullOrEmpty(prefs.AutoOffKey2.ToString()) && prefs.AutoOffKey2.ToString() != AppConfig.TEXT_NONE);
            bool ShouldKillClient = prefs.AutoOffKillClient;

            DebugLogger.Debug($"OverweightMacro: ShouldSendKey1={ShouldSendKey1}, ShouldSendKey2={ShouldSendKey2}, ShouldKillClient={ShouldKillClient}");

            if (ShouldSendKey1 || ShouldSendKey2)
            {
                IntPtr hWnd = ClientSingleton.GetClient().Process.MainWindowHandle;

                // Only focus the window if it's not already focused
                if (GetForegroundWindow() != hWnd) { SetForegroundWindow(hWnd); }

                Thread.Sleep(1000);

                if (ShouldSendKey1)
                {
                    keyToSend = "%" + ToSendKeysFormat(prefs.AutoOffKey1);
                    for (int i = 0; i < timesToSend; i++)
                    {
                        SendKeys.SendWait(keyToSend);
                        DebugLogger.Info($"Sent macro {i + 1}/{timesToSend}: Alt + {prefs.AutoOffKey1} (Auto-off, key 1)");

                        if (i < timesToSend - 1)
                        {
                            Thread.Sleep(intervalMs);
                        }
                    }
                }

                if(ShouldSendKey1 && ShouldSendKey2)
                {
                    // Add a small delay between the two keys if both are sent
                    Thread.Sleep(1000);
                }

                if (ShouldSendKey2)
                {
                    keyToSend = "%" + ToSendKeysFormat(prefs.AutoOffKey2);
                    for (int i = 0; i < timesToSend; i++)
                    {
                        SendKeys.SendWait(keyToSend);
                        DebugLogger.Info($"Sent macro {i + 1}/{timesToSend}: Alt + {prefs.AutoOffKey2} (Auto-off, key 2)");

                        if (i < timesToSend - 1)
                        {
                            Thread.Sleep(intervalMs);
                        }
                    }
                }

                if(ShouldKillClient)
                {
                    // Add a small delay before killing the client
                    Thread.Sleep(1000);
                    DebugLogger.Info($"Killing the client (Auto-off)");
                    ClientSingleton.GetClient().Kill();
                }
            }
        }
    }
}