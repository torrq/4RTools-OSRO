using System;
using System.Windows.Forms;
using _4RTools.Utils;

namespace _4RTools
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Forms.Container app = new Forms.Container()) // Ensure proper disposal
            {
                try
                {
                    Application.Run(app);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message,
                                    "Application Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            DebugLogger.Shutdown();
            // Ensure proper cleanup after exiting
            Application.Exit();
        }
    }
}
