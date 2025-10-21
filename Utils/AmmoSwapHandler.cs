using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using BruteGamingMacros.Core.Model;

namespace BruteGamingMacros.Core.Utils
{
    public class AmmoSwapHandler
    {
        private ThreadRunner thread;
        private bool ammoToggle = false; // false = next press should send Ammo1Key, true = next press should send Ammo2Key
        private bool wasDown = false;
        private bool isSendingKey = false; // Flag to prevent hook recursion

        // Low-level keyboard hook
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc;
        private static IntPtr _hookID = IntPtr.Zero;
        private static AmmoSwapHandler _instance;

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, Keys wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public void Start()
        {
            if (thread != null) Stop();

            _instance = this;
            _proc = HookCallback; // Initialize the delegate here
            _hookID = SetHook(_proc);

            thread = new ThreadRunner(_ => Run());
            ThreadRunner.Start(thread);
        }

        public void Stop()
        {
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }

            if (thread != null)
            {
                ThreadRunner.Stop(thread);
                thread.Terminate();
                thread = null;
            }

            _instance = null;
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // If the hook is busy sending a key, or if conditions are not met, pass the event along.
            if (nCode < 0 || _instance == null || _instance.isSendingKey)
            {
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            var prefs = ProfileSingleton.GetCurrent().UserPreferences;
            if (prefs.SwitchAmmo && _instance.IsGameWindowActive())
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Key wpfKey = KeyInterop.KeyFromVirtualKey(vkCode);

                // Check if this is our trigger key
                if (wpfKey == prefs.AmmoTriggerKey)
                {
                    if (wParam == (IntPtr)WM_KEYDOWN && !_instance.wasDown)
                    {
                        //DebugLogger.Debug($"[AmmoSwapHandler] Intercepted {wpfKey} press, blocking and swapping");
                        _instance.SwapAmmoKey();
                        _instance.wasDown = true;
                        return (IntPtr)1; // Block the key
                    }
                    else if (wParam == (IntPtr)WM_KEYUP && _instance.wasDown)
                    {
                        //DebugLogger.Debug($"[AmmoSwapHandler] Intercepted {wpfKey} release");
                        _instance.wasDown = false;
                        return (IntPtr)1; // Block the key
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private int Run()
        {
            // Keep the thread alive for the hook
            while (true)
            {
                Thread.Sleep(100);
            }
        }

        private void SwapAmmoKey()
        {
            var prefs = ProfileSingleton.GetCurrent().UserPreferences;
            Client client = ClientSingleton.GetClient();
            if (client == null)
            {
                //DebugLogger.Debug("[AmmoSwapHandler] Client is null, skipping key press");
                return;
            }

            // Select the key based on current toggle state
            Key keyToPress = ammoToggle ? prefs.Ammo2Key : prefs.Ammo1Key;
            //DebugLogger.Debug($"[AmmoSwapHandler] Selected key: {keyToPress} (Ammo1Key={prefs.Ammo1Key}, Ammo2Key={prefs.Ammo2Key}, trigger={prefs.AmmoTriggerKey}, toggle={ammoToggle})");

            Keys winFormsKey = (Keys)KeyInterop.VirtualKeyFromKey(keyToPress);
            byte vkCode = (byte)winFormsKey;
            byte scanCode = (byte)MapVirtualKey(vkCode, 0);

            // Toggle for the *next* press
            ammoToggle = !ammoToggle;

            this.isSendingKey = true; // Set flag to ignore our own key events
            try
            {
                // Send the key using keybd_event
                keybd_event(vkCode, scanCode, 0, 0); // Key down
                Thread.Sleep(10);
                keybd_event(vkCode, scanCode, Constants.KEYEVENTF_KEYUP, 0); // Key up
            }
            finally
            {
                this.isSendingKey = false; // Always reset the flag
            }

            //DebugLogger.Debug($"[AmmoSwapHandler] Sent {winFormsKey} using keybd_event (vkCode={vkCode}, scanCode={scanCode}, new toggle={ammoToggle})");
        }

        private bool IsGameWindowActive()
        {
            IntPtr foreground = GetForegroundWindow();
            bool isActive =
                foreground == FindWindow(AppConfig.WindowClassLR, null) ||
                foreground == FindWindow(AppConfig.WindowClassMR, null) ||
                foreground == FindWindow(AppConfig.WindowClassHR, null);
            return isActive;
        }
    }
}