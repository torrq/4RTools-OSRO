using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _ORTools.Utils
{
    internal static class Win32Interop
    {
        #region Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        #region Window Management
        /// <summary>Gets the handle to the currently active window.</summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>Gets the process and thread ID that created the specified window.</summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>Checks if the specified window handle is valid.</summary>
        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        /// <summary>Checks if the specified window is visible.</summary>
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>Gets the client area coordinates of the specified window.</summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>Brings the specified window to the foreground and activates it.</summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region Mouse Operations
        /// <summary>Synthesizes mouse motion and button clicks.</summary>
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        /// <summary>Gets the current cursor position in screen coordinates.</summary>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        /// <summary>Sets the cursor position to the specified screen coordinates.</summary>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        /// <summary>Converts screen coordinates to client area coordinates.</summary>
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref System.Drawing.Point lpPoint);

        /// <summary>Converts client area coordinates to screen coordinates.</summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
        #endregion

        #region Keyboard Operations
        /// <summary>Synthesizes a keystroke.</summary>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        /// <summary>Checks if a key is currently pressed down.</summary>
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        /// <summary>Returns true if the specified key is currently pressed.</summary>
        public static bool IsKeyPressed(Keys key)
        {
            return (GetAsyncKeyState(key) & 0x8000) != 0;
        }
        #endregion

        #region Message Posting
        /// <summary>Posts a message to the window's message queue (asynchronous).</summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, Keys wParam, int lParam);

        /// <summary>Sends a message directly to the window procedure (synchronous).</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>Sends a message directly to the window procedure (synchronous, uint variant).</summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Hook Operations
        /// <summary>Installs a hook procedure to monitor system events.</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHook.HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>Passes hook information to the next hook procedure in the chain.</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>Removes a hook procedure from the hook chain.</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(IntPtr idHook);

        /// <summary>Gets a handle to the specified module.</summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }
}