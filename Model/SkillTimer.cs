using _4RTools.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Model
{
    public class SkillTimer : IAction
    {
        // P/Invoke definitions for center-screen clicking
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int X, Y; }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;
        private const int SW_RESTORE = 9;

        private static IntPtr MakeLParam(int low, int high) => (IntPtr)((high << 16) | (low & 0xFFFF));

        private readonly string ACTION_NAME = "SkillTimer";
        public Dictionary<int, MacroKey> skillTimer = new Dictionary<int, MacroKey>();

        private ThreadRunner thread1;
        private ThreadRunner thread2;
        private ThreadRunner thread3;
        private ThreadRunner thread4;
        private ThreadRunner thread5;
        private ThreadRunner thread6;
        private ThreadRunner thread7;
        private ThreadRunner thread8;
        private ThreadRunner thread9;
        private ThreadRunner thread10;

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                ValidadeThreads(this.thread1);
                ValidadeThreads(this.thread2);
                ValidadeThreads(this.thread3);
                ValidadeThreads(this.thread4);
                ValidadeThreads(this.thread5);
                ValidadeThreads(this.thread6);
                ValidadeThreads(this.thread7);
                ValidadeThreads(this.thread8);
                ValidadeThreads(this.thread9);
                ValidadeThreads(this.thread10);

                this.thread1 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[1]));
                this.thread2 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[2]));
                this.thread3 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[3]));
                this.thread4 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[4]));
                this.thread5 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[5]));
                this.thread6 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[6]));
                this.thread7 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[7]));
                this.thread8 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[8]));
                this.thread9 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[9]));
                this.thread10 = new ThreadRunner((_) => AutoRefreshThreadExecution(roClient, skillTimer[10]));

                ThreadRunner.Start(this.thread1);
                ThreadRunner.Start(this.thread2);
                ThreadRunner.Start(this.thread3);
                ThreadRunner.Start(this.thread4);
                ThreadRunner.Start(this.thread5);
                ThreadRunner.Start(this.thread6);
                ThreadRunner.Start(this.thread7);
                ThreadRunner.Start(this.thread8);
                ThreadRunner.Start(this.thread9);
                ThreadRunner.Start(this.thread10);
            }
        }

        private void ValidadeThreads(ThreadRunner _4RThread)
        {
            if (_4RThread != null)
            {
                ThreadRunner.Stop(_4RThread);
            }
        }

        private int AutoRefreshThreadExecution(Client roClient, MacroKey macro)
        {
            string currentMap = roClient.ReadCurrentMap();

            if (!ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity || !Server.GetCityList().Contains(currentMap))
            {
                IntPtr hWnd = roClient.Process.MainWindowHandle;

                if (macro.Key != Key.None)
                {
                    // FIX: Use KeyInterop for robust conversion from Input.Key to Forms.Keys
                    Interop.PostMessage(hWnd, Constants.WM_KEYDOWN_MSG_ID, (Keys)KeyInterop.VirtualKeyFromKey(macro.Key), 0);
                }

                // Handle clicking based on the ClickMode
                switch (macro.ClickMode)
                {
                    case 1: // Click at current cursor position
                        // Try multiple approaches for better reliability
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

        public void Stop()
        {
            if (this.thread1 != null)
            {
                ThreadRunner.Stop(this.thread1);
                this.thread1.Terminate();
                this.thread1 = null;
            }
            if (this.thread2 != null)
            {
                ThreadRunner.Stop(this.thread2);
                this.thread2.Terminate();
                this.thread2 = null;
            }
            if (this.thread3 != null)
            {
                ThreadRunner.Stop(this.thread3);
                this.thread3.Terminate();
                this.thread3 = null;
            }
            if (this.thread4 != null)
            {
                ThreadRunner.Stop(this.thread4);
                this.thread4.Terminate();
                this.thread4 = null;
            }
            if (this.thread5 != null)
            {
                ThreadRunner.Stop(this.thread5);
                this.thread5.Terminate();
                this.thread5 = null;
            }
            if (this.thread6 != null)
            {
                ThreadRunner.Stop(this.thread1);
                this.thread6.Terminate();
                this.thread6 = null;
            }
            if (this.thread7 != null)
            {
                ThreadRunner.Stop(this.thread2);
                this.thread7.Terminate();
                this.thread7 = null;
            }
            if (this.thread8 != null)
            {
                ThreadRunner.Stop(this.thread3);
                this.thread8.Terminate();
                this.thread8 = null;
            }
            if (this.thread9 != null)
            {
                ThreadRunner.Stop(this.thread4);
                this.thread9.Terminate();
                this.thread9 = null;
            }
            if (this.thread10 != null)
            {
                ThreadRunner.Stop(this.thread5);
                this.thread10.Terminate();
                this.thread10 = null;
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

        private void TryClickAtCurrentPosition(IntPtr hWnd)
        {
            // Method 1: Try SendMessage first (works for some games when unfocused)
            SendMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, IntPtr.Zero);
            Thread.Sleep(25);
            SendMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

            // Method 2: If that doesn't work, try PostMessage as backup
            // PostMessage(hWnd, WM_LBUTTONDOWN, (IntPtr)1, IntPtr.Zero);
            // Thread.Sleep(25);
            // PostMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        private void TryClickAtCenter(IntPtr hWnd)
        {
            if (!GetClientRect(hWnd, out RECT clientRect))
                return;

            // Calculate the center of the client area
            int centerX = clientRect.Right / 2;
            int centerY = clientRect.Bottom / 2;

            // Convert client coordinates to screen coordinates
            POINT centerPoint = new POINT { X = centerX, Y = centerY };
            ClientToScreen(hWnd, ref centerPoint);

            // Save current cursor position
            GetCursorPos(out POINT originalPos);

            // Move cursor to center, click, then restore position
            SetCursorPos(centerPoint.X, centerPoint.Y);
            Thread.Sleep(25); // Keep the original timing that was working

            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(50); // Slightly longer delay between down and up
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);

            // Restore original cursor position
            SetCursorPos(originalPos.X, originalPos.Y);
        }
    }
}