using BruteGamingMacros.Core.Utils;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BruteGamingMacros.UI.Forms
{
    public partial class DebugLogWindow : Form
    {
        // Helper method to get a brighter version of a color
        private Color GetBrighterColor(Color baseColor)
        {
            // Make the color brighter by increasing its RGB values
            // but keeping them within the valid range (0-255)
            int r = Math.Min(255, (int)(baseColor.R * 1.3));
            int g = Math.Min(255, (int)(baseColor.G * 1.3));
            int b = Math.Min(255, (int)(baseColor.B * 1.3));

            return Color.FromArgb(baseColor.A, r, g, b);
        }

        private RichTextBox debugConsole;

        public DebugLogWindow(Icon containerIcon)
        {
            InitializeComponents();
            this.Icon = containerIcon;
        }

        private void InitializeComponents()
        {
            this.Text = AppConfig.Name + " Debug Log";
            this.Size = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            debugConsole = new RichTextBox
            {
                BackColor = AppConfig.DebugConsoleBackColor,
                ForeColor = AppConfig.DebugConsoleForeColor,
                Font = AppConfig.DebugConsoleFont,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            this.Controls.Add(debugConsole);
        }

        internal void DebugLogger_OnLogMessage(string message, DebugLogger.LogLevel level)
        {
            if (debugConsole.InvokeRequired)
            {
                debugConsole.Invoke((MethodInvoker)delegate
                {
                    DebugLogger_OnLogMessage(message, level);
                });
                return;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            debugConsole.SuspendLayout();
            debugConsole.SelectionStart = debugConsole.TextLength;
            debugConsole.SelectionLength = 0;

            Color defaultLineColor;
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
                    defaultLineColor = AppConfig.LogColor_STATUS;
                    break;
                default:
                    defaultLineColor = debugConsole.ForeColor;
                    break;
            }

            if (level == DebugLogger.LogLevel.STATUS)
            {
                var match = Regex.Match(message, $@"^(\d{{2}}:\d{{2}}:\d{{2}}\.\d{{3}}) \[({AppConfig.STATUS})\] (.*)$");

                if (match.Success)
                {
                    // Timestamp
                    debugConsole.SelectionColor = AppConfig.LogColor_Timestamp;
                    debugConsole.AppendText(match.Groups[1].Value + " ");

                    // [STATUS]
                    debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    debugConsole.AppendText("[" + match.Groups[2].Value + "] ");

                    // Status data
                    string statusDetails = match.Groups[3].Value;
                    string[] statuses = statusDetails.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < statuses.Length; i++)
                    {
                        string statusEntry = statuses[i];
                        string[] parts = statusEntry.Split(':');

                        if (parts.Length == 2)
                        {
                            debugConsole.SelectionColor = AppConfig.LogColor_StatusId;
                            debugConsole.AppendText(parts[0]);

                            debugConsole.SelectionColor = debugConsole.ForeColor;
                            debugConsole.AppendText(":");

                            // Check for unknown status
                            if (parts[1] == "**UNKNOWN**")
                            {
                                debugConsole.SelectionColor = AppConfig.LogColor_StatusUnknown;
                                debugConsole.AppendText(parts[1]);
                            }
                            else
                            {
                                debugConsole.SelectionColor = AppConfig.LogColor_StatusName;
                                debugConsole.AppendText(parts[1]);
                            }
                        }
                        else
                        {
                            debugConsole.SelectionColor = debugConsole.ForeColor;
                            debugConsole.AppendText(statusEntry);
                        }

                        if (i < statuses.Length - 1)
                        {
                            debugConsole.SelectionColor = debugConsole.ForeColor;
                            debugConsole.AppendText(" ");
                        }
                    }
                }
                else
                {
                    debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    debugConsole.AppendText(message);
                }
            }
            else
            {
                // Create a pattern that will match any of the log levels from AppConfig
                string logLevelPattern = $"({AppConfig.INFO}|{AppConfig.WARNING}|{AppConfig.ERROR}|{AppConfig.DEBUG}|{AppConfig.STATUS})";
                var match = Regex.Match(message, $@"^(\d{{2}}:\d{{2}}:\d{{2}}\.\d{{3}}) \[{logLevelPattern}\] (.*)$");

                if (match.Success)
                {
                    // Timestamp
                    debugConsole.SelectionColor = AppConfig.LogColor_Timestamp;
                    debugConsole.AppendText(match.Groups[1].Value + " ");

                    // Log level tag
                    debugConsole.SelectionColor = defaultLineColor;
                    debugConsole.AppendText("[" + match.Groups[2].Value + "] ");

                    // Log message content with brighter color based on log level
                    Color contentColor = GetBrighterColor(defaultLineColor);
                    debugConsole.SelectionColor = contentColor;
                    debugConsole.AppendText(match.Groups[3].Value);
                }
                else
                {
                    debugConsole.SelectionColor = defaultLineColor;
                    debugConsole.AppendText(message);
                }
            }

            debugConsole.SelectionColor = debugConsole.ForeColor;
            debugConsole.AppendText(Environment.NewLine);
            debugConsole.ScrollToCaret();
            debugConsole.ResumeLayout();
        }
    }
}