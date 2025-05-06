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

        public DebugLogWindow(Icon containerIcon)
        {
            InitializeComponents();
            this.Icon = containerIcon;
        }

        private void InitializeComponents()
        {
            this.Text = "4RTools Debug Log";
            this.Size = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.BackColor = Color.White;
            this.DoubleBuffered = true;

            debugConsole = new RichTextBox
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Consolas", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
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
                debugConsole.Invoke((MethodInvoker)delegate {
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
                    defaultLineColor = AppConfig.LogColor_STATUS;
                    break;
                default:
                    defaultLineColor = debugConsole.ForeColor;
                    break;
            }

            if (level == DebugLogger.LogLevel.STATUS)
            {
                var match = Regex.Match(message, @"^(\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\]) (\[STATUS\]) (.*)$");

                if (match.Success)
                {
                    debugConsole.SelectionColor = debugConsole.ForeColor;
                    debugConsole.AppendText(match.Groups[1].Value + " ");

                    debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    debugConsole.AppendText(match.Groups[2].Value + " ");

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

                            debugConsole.SelectionColor = AppConfig.LogColor_StatusName;
                            debugConsole.AppendText(parts[1]);
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
                debugConsole.SelectionColor = defaultLineColor;
                debugConsole.AppendText(message);
            }

            debugConsole.SelectionColor = debugConsole.ForeColor;
            debugConsole.AppendText(Environment.NewLine);

            debugConsole.ScrollToCaret();
            debugConsole.ResumeLayout();
        }
    }
}