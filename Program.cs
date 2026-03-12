using _ORTools.Utils;
using System;
using System.Windows.Forms;

namespace _ORTools
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Set Windows timer resolution to 1ms so Thread.Sleep(1/5/etc.) is accurate.
            // Default is ~15.6ms which makes every short sleep wildly over-sleep.
            Win32Interop.timeBeginPeriod(1);

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
                DebugLogger.Error("Unhandled exception:\n" + ex.Message);
                MessageBox.Show("An unexpected error occurred. Please check the logs.", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Win32Interop.timeEndPeriod(1);
                DebugLogger.Info("Application exiting...");
                Application.Exit();
            }
        }
    }
}
