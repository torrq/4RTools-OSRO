using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using _4RTools.Utils;

namespace _4RTools
{
    internal static class Program
    {
        // Mutex to ensure single instance
        private static Mutex mutex = null;
        private const string MutexName = "4RTools-OSRO-Mutex";

        // Import Windows API functions to bring the existing window to the foreground
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        [STAThread]
        static void Main()
        {
            bool createdNew;
            mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                // Find the existing window and bring it to the foreground
                IntPtr hWnd = FindWindowByCaption();
                if (hWnd != IntPtr.Zero)
                {
                    ShowWindow(hWnd, SW_RESTORE);
                    SetForegroundWindow(hWnd);
                }
                return; // Exit the new instance
            }

            // This is the first instance, proceed with startup
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                using (Forms.Container app = new Forms.Container())
                {
                    Application.Run(app);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Unhandled exception:\n" + ex.ToString());
                MessageBox.Show("An unexpected error occurred. Please check the logs.", "Application Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DebugLogger.Info("Application exiting...");

                // Release the mutex
                DebugLogger.Info("Releasing Mutex...");
                mutex?.ReleaseMutex();
                mutex?.Dispose();
                mutex = null;

                Application.Exit();
            }
        }

        private static IntPtr FindWindowByCaption()
        {
            // Find the window with the title "4RTools-OSRO"
            return FindWindow(null, "4RTools-OSRO");
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}