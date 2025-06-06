﻿using _4RTools.Utils;
using System;
using System.Windows.Forms;

namespace _4RTools
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
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
                Application.Exit();
            }
        }
    }
}
