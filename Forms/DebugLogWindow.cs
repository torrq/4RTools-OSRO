using System;
using System.Drawing;
using System.Windows.Forms;
using _4RTools.Utils;
using System.Text.RegularExpressions; // Add this using directive

namespace _4RTools.Forms
{
    public partial class DebugLogWindow : Form
    {
        private RichTextBox debugConsole;

        public DebugLogWindow()
        {
            InitializeComponents();
            // Subscribe to the DebugLogger event when the window is created
            // Ensure the handler signature matches DebugLogger.LogMessageHandler (string, LogLevel)
            DebugLogger.OnLogMessage += DebugLogger_OnLogMessage;
        }

        private void InitializeComponents()
        {
            this.Text = "4RTools Debug Log";
            this.Size = new Size(300, 600);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.BackColor = Color.White;

            debugConsole = new RichTextBox
            {
                BackColor = Color.Black,
                ForeColor = Color.White, // Default text color
                Font = new Font("Consolas", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            this.Controls.Add(debugConsole);
        }

        // Changed protection level from private to internal
        internal void DebugLogger_OnLogMessage(string message, DebugLogger.LogLevel level)
        {
            if (debugConsole.InvokeRequired)
            {
                debugConsole.Invoke(new Action(() => DebugLogger_OnLogMessage(message, level)));
                return;
            }

            debugConsole.SuspendLayout();
            debugConsole.SelectionStart = debugConsole.TextLength;
            debugConsole.SelectionLength = 0;

            // Apply color based on log level for the entire line by default
            System.Drawing.Color defaultLineColor;
            switch (level)
            {
                case DebugLogger.LogLevel.INFO:
                    defaultLineColor = AppConfig.LogColor_INFO;
                    break;
                case DebugLogger.LogLevel.WARNING:
                    defaultLineColor = AppConfig.LogColor_WARNING;
                    break;
                case DebugLogger.LogLevel.ERROR:
                    defaultLineColor = AppConfig.LogColor_ERROR;
                    break;
                case DebugLogger.LogLevel.DEBUG:
                    defaultLineColor = AppConfig.LogColor_DEBUG;
                    break;
                case DebugLogger.LogLevel.STATUS:
                    defaultLineColor = AppConfig.LogColor_STATUS; // Initial color for STATUS line
                    break;
                default:
                    defaultLineColor = debugConsole.ForeColor; // Default to the RichTextBox's default
                    break;
            }

            debugConsole.SelectionColor = defaultLineColor;

            if (level == DebugLogger.LogLevel.STATUS)
            {
                // Detailed coloring for STATUS messages

                // Use regex to find the timestamp and the [STATUS] part
                var match = Regex.Match(message, @"^(\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\]) (\[STATUS\]) (.*)$");

                if (match.Success)
                {
                    // Append timestamp with default color (or customize)
                    debugConsole.SelectionColor = debugConsole.ForeColor; // Use default white for timestamp
                    debugConsole.AppendText(match.Groups[1].Value + " ");

                    // Append "[STATUS]" tag with AppConfig.LogColor_STATUS
                    debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    debugConsole.AppendText(match.Groups[2].Value + " ");

                    // Process the rest of the message (status IDs and names)
                    string statusDetails = match.Groups[3].Value;
                    string[] statuses = statusDetails.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < statuses.Length; i++)
                    {
                        string statusEntry = statuses[i]; // e.g., "1429:ELEMENT_HOLY"
                        string[] parts = statusEntry.Split(':'); // e.g., ["1429", "ELEMENT_HOLY"]

                        if (parts.Length == 2)
                        {
                            // Append Status ID
                            debugConsole.SelectionColor = AppConfig.LogColor_StatusId;
                            debugConsole.AppendText(parts[0]);

                            // Append ":" separator
                            debugConsole.SelectionColor = debugConsole.ForeColor; // Use default for separator
                            debugConsole.AppendText(":");

                            // Append Status Name
                            debugConsole.SelectionColor = AppConfig.LogColor_StatusName;
                            debugConsole.AppendText(parts[1]);
                        }
                        else
                        {
                            // If parsing fails, just append the original part with default color
                            debugConsole.SelectionColor = debugConsole.ForeColor;
                            debugConsole.AppendText(statusEntry);
                        }

                        // Add space between status entries, except for the last one
                        if (i < statuses.Length - 1)
                        {
                            debugConsole.SelectionColor = debugConsole.ForeColor; // Use default for space
                            debugConsole.AppendText(" ");
                        }
                    }
                }
                else
                {
                    // Fallback if regex doesn't match the expected STATUS format
                    debugConsole.SelectionColor = AppConfig.LogColor_STATUS; // Use general STATUS color
                    debugConsole.AppendText(message);
                }
            }
            else
            {
                // For other log levels, append the whole message with the determined defaultLineColor
                debugConsole.AppendText(message);
            }


            debugConsole.SelectionColor = debugConsole.ForeColor; // Reset color to default for the newline
            debugConsole.AppendText(Environment.NewLine); // Add a newline

            debugConsole.ScrollToCaret();
            debugConsole.ResumeLayout();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Unsubscribe from the DebugLogger event when the form is closing (or hiding)
            // This is important because the form is hidden, not truly disposed of, until the app exits.
            DebugLogger.OnLogMessage -= DebugLogger_OnLogMessage;

            // Prevent the window from being closed; hide it instead
            e.Cancel = true;
            this.Hide();
        }

        // Removed the manual Dispose method. It is handled by the designer.
        // protected override void Dispose(bool disposing)
        // {
        //     if (disposing)
        //     {
        //          DebugLogger.OnLogMessage -= DebugLogger_OnLogMessage;
        //         if (components != null)
        //         {
        //             components.Dispose();
        //         }
        //     }
        //     base.Dispose(disposing);
        // }
    }
}