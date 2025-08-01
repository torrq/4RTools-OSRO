﻿using _ORTools.Utils;
using System;
using System.Windows.Forms;

namespace _ORTools
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
                DebugLogger.Error("Unhandled exception:\n" + ex.Message);
                MessageBox.Show("An unexpected error occurred. Please check the logs.", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DebugLogger.Info("Application exiting...");
                Application.Exit();
            }
        }
    }
}
