using System;
using System.Drawing;
using System.Windows.Forms;
using _4RTools.Utils;
using System.Text.RegularExpressions;

namespace _4RTools.Forms
{
    public partial class DebugLogWindow : Form
    {
        private RichTextBox debugConsole;
        // components is automatically generated in Designer.cs, no need to declare here
        // private System.ComponentModel.IContainer components = null;


        public DebugLogWindow()
        {
            InitializeComponents();

            // The subscription is now managed solely by the Container class
        }

        private void InitializeComponents()
        {
            // Designer code handles this in DebugLogWindow.Designer.cs
            // But your provided file seems to do it manually here. Let's keep it as you had it for now.
            this.Text = "4RTools Debug Log";
            this.Size = new Size(300, 600);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.Manual; // Set StartPosition to Manual since Location is set externally
            this.Location = new Point(0, 0); // Default location before Container positions it
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

            // Call the designer-generated InitializeComponent if it exists and is needed
            // InitializeComponent();
        }

        // Changed protection level from private to internal (needed by Container's lambda)
        internal void DebugLogger_OnLogMessage(string message, DebugLogger.LogLevel level) // <-- Method takes 2 arguments
        {
            // Use InvokeRequired to ensure this runs on the UI thread
            if (debugConsole.InvokeRequired)
            {
                // Use MethodInvoker or Action for invoking
                debugConsole.Invoke((MethodInvoker)delegate {
                    DebugLogger_OnLogMessage(message, level);
                });
                // Or using Action:
                // debugConsole.Invoke(new Action(() => DebugLogger_OnLogMessage(message, level)));
                return;
            }

            // ### ADDED CHECK: Ignore messages that are null, empty, or only whitespace ###
            if (string.IsNullOrWhiteSpace(message))
            {
                return; // Do not append anything if the message is just whitespace or empty
            }


            // Append text to the RichTextBox
            debugConsole.SuspendLayout(); // Suspend layout for performance during updates
            debugConsole.SelectionStart = debugConsole.TextLength; // Move caret to the end
            debugConsole.SelectionLength = 0;

            // Apply color based on log level or customize for STATUS
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

            // Assuming the message string received from DebugLogger.Log is already formatted
            // with timestamp and level, we just need to append it with the correct color.
            // The STATUS parsing logic below is an exception where we break down the formatted string.

            if (level == DebugLogger.LogLevel.STATUS)
            {
                // Detailed coloring for STATUS messages if they match the expected format
                // This logic *assumes* the input `message` string includes the timestamp and [STATUS] tag
                var match = Regex.Match(message, @"^(\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\]) (\[STATUS\]) (.*)$");

                if (match.Success)
                {
                    // Append timestamp with default color
                    debugConsole.SelectionColor = debugConsole.ForeColor;
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
                            debugConsole.SelectionColor = AppConfig.LogColor_StatusId;
                            debugConsole.AppendText(parts[0]);

                            debugConsole.SelectionColor = debugConsole.ForeColor; // Default for separator
                            debugConsole.AppendText(":");

                            debugConsole.SelectionColor = AppConfig.LogColor_StatusName;
                            debugConsole.AppendText(parts[1]);
                        }
                        else
                        {
                            // Fallback
                            debugConsole.SelectionColor = debugConsole.ForeColor;
                            debugConsole.AppendText(statusEntry);
                        }

                        if (i < statuses.Length - 1)
                        {
                            debugConsole.SelectionColor = debugConsole.ForeColor; // Default for space
                            debugConsole.AppendText(" ");
                        }
                    }
                }
                else
                {
                    // Fallback if regex doesn't match, just append the whole message with STATUS color
                    debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    debugConsole.AppendText(message); // Append the original message here
                }
            }
            else // For INFO, WARNING, ERROR, DEBUG (most common cases)
            {
                // Append the whole message string as received from the logger
                // The logger (DebugLogger.Log) formats it with timestamp and level before sending the event.
                debugConsole.SelectionColor = defaultLineColor; // Apply color based on level
                debugConsole.AppendText(message); // Append the original message here
            }


            // Always append exactly one newline after processing a valid message
            debugConsole.SelectionColor = debugConsole.ForeColor; // Reset color for the newline
            debugConsole.AppendText(Environment.NewLine); // Add a newline

            // Auto-scroll to the bottom
            debugConsole.ScrollToCaret();
            debugConsole.ResumeLayout(); // Resume layout after appending
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Prevent the window from being closed; hide it instead
            e.Cancel = true;
            this.Hide();

            // The unsubscription is managed solely by the Container class
        }

    }
}