using _ORTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace _ORTools.Model
{
    public class SkillTimer : IAction
    {
        #region Constants
        public const int MAX_SKILL_TIMERS = 10;
        #endregion

        private readonly string ACTION_NAME = "SkillTimer";
        public Dictionary<int, SkillTimerKey> skillTimer = new Dictionary<int, SkillTimerKey>();

        private readonly Dictionary<int, ThreadRunner> threads = new Dictionary<int, ThreadRunner>();

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient == null) return;

            StopAllThreads();

            // Create and start threads only for enabled skill timers
            for (int i = 1; i <= MAX_SKILL_TIMERS; i++)
            {
                if (skillTimer.TryGetValue(i, out var macro) && macro.Enabled)
                {
                    int skillIndex = i; // Capture loop variable
                    threads[i] = new ThreadRunner((_) => SkillTimerThread(roClient, skillTimer[skillIndex]), $"SkillTimer-{i}");
                    ThreadRunner.Start(threads[i]);
                }
            }
        }

        public void Stop()
        {
            StopAllThreads();
        }

        public void StartTimer(int timerId)
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient == null) return;

            // Stop existing thread for this timer if it exists
            StopTimer(timerId);

            // Start new thread if the timer exists and is enabled
            if (skillTimer.TryGetValue(timerId, out var macro) && macro.Enabled)
            {
                // Respect the StopBuffsCity setting - if enabled and we're in a city, don't start the timer
                string currentMap = roClient.ReadCurrentMap();
                if (ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity && Server.GetCityList().Contains(currentMap))
                {
                    // Don't start timer if we're in a city and StopBuffsCity is enabled
                    return;
                }

                threads[timerId] = new ThreadRunner((_) => SkillTimerThread(roClient, skillTimer[timerId]), $"SkillTimer-{timerId}");
                ThreadRunner.Start(threads[timerId]);
            }
        }

        public void StopTimer(int timerId)
        {
            if (threads.TryGetValue(timerId, out var thread))
            {
                ThreadRunner.Stop(thread);
                thread.Terminate();
                threads.Remove(timerId);
            }
        }

        private void StopAllThreads()
        {
            foreach (var thread in threads.Values.ToList())
            {
                if (thread != null)
                {
                    ThreadRunner.Stop(thread);
                    thread.Terminate();
                }
            }
            threads.Clear();
        }

        private int SkillTimerThread(Client roClient, SkillTimerKey macro)
        {
            string currentMap = roClient.ReadCurrentMap();
            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
            {
                IntPtr hWnd = roClient.Process.MainWindowHandle;
                if (macro.Key != Keys.None)
                {
                    if (macro.AltKey)
                    {
                        // Only focus the window if it's not already focused
                        if (Win32Interop.GetForegroundWindow() != hWnd)
                        {
                            Win32Interop.SetForegroundWindow(hWnd);
                        }
                        SendKeys.SendWait("%" + ToSendKeysFormat(macro.Key));
                        //DebugLogger.Info($"Sent ALT Skilltimer key: " + macro.Key);
                    }
                    else
                    {
                        // Remove the KeyInterop conversion since macro.Key is already Keys enum
                        Win32Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, macro.Key, 0);
                    }
                }
                // Handle clicking based on the ClickMode
                switch (macro.ClickMode)
                {
                    case 1: // Click at current cursor position
                        TryClickAtCurrentPosition(hWnd);
                        break;
                    case 2: // Click at the center of the game window
                        TryClickAtCenter(hWnd);
                        break;
                        // case 0: No click, do nothing.
                }
            }
            Thread.Sleep(macro.Delay);
            return 0;
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return ACTION_NAME;
        }

        private void TryClickAtCurrentPosition(IntPtr hWnd)
        {
            Win32Interop.SendMessage(hWnd, Win32Interop.WM_LBUTTONDOWN, (IntPtr)1, IntPtr.Zero);
            Thread.Sleep(25);
            Win32Interop.SendMessage(hWnd, Win32Interop.WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        private void TryClickAtCenter(IntPtr hWnd)
        {
            if (!Win32Interop.GetClientRect(hWnd, out Win32Interop.RECT clientRect))
                return;

            // Calculate the center of the client area
            int centerX = clientRect.Right / 2;
            int centerY = clientRect.Bottom / 2;

            // Convert client coordinates to screen coordinates
            Win32Interop.POINT centerPoint = new Win32Interop.POINT { X = centerX, Y = centerY };
            Win32Interop.ClientToScreen(hWnd, ref centerPoint);

            // Save current cursor position
            Win32Interop.GetCursorPos(out Win32Interop.POINT originalPos);

            // Move cursor to center, click, then restore position
            Win32Interop.SetCursorPos(centerPoint.X, centerPoint.Y);
            Thread.Sleep(25); // Keep the original timing that was working

            Win32Interop.mouse_event(Win32Interop.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(50); // Slightly longer delay between down and up
            Win32Interop.mouse_event(Win32Interop.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

            // Restore original cursor position
            Win32Interop.SetCursorPos(originalPos.X, originalPos.Y);
        }

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

        public static string ToSendKeysFormat(Keys key)
        {
            if (_sendKeysMap.TryGetValue(key, out string value))
            {
                return value;
            }
            return key.ToString().ToLower();
        }
    }
}