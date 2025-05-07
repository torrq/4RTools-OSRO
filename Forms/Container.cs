using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class Container : Form, IObserver
    {
        private Subject subject = new Subject();
        private string currentProfile;
        List<ClientDTO> clients = new List<ClientDTO>();
        private ToggleStateForm frmToggleApplication = new ToggleStateForm();
        private DebugLogWindow debugLogWindow;
        private bool isShuttingDown = false;
        private DebugLogger.LogMessageHandler debugLogHandler;

        public Container()
        {
            ConfigGlobal.Initialize();
            ConfigGlobal.SaveConfig();

            DebugLogger.Info($"Container constructor: DebugMode is {ConfigGlobal.GetConfig().DebugMode} after ConfigGlobal.Initialize");

            this.subject.Attach(this);

            InitializeComponent();

            this.Text = AppConfig.WindowTitle;

            Server.Initialize();
            clients.AddRange(Server.GetLocalClients());

            LoadServers(clients);

            this.IsMdiContainer = true;
            SetBackGroundColorOfMDIForm();

            if (ConfigGlobal.GetConfig().DebugMode)
            {
                DebugLogger.Info("DebugMode is true: Creating and showing DebugLogWindow");
                debugLogWindow = new DebugLogWindow(this.Icon);
                debugLogWindow.Owner = this; // Owner relationship for focus sync
                debugLogWindow.Show();
                SubscribeToDebugLogger();

                this.LocationChanged += Container_LocationOrSizeChanged;
                this.SizeChanged += Container_LocationOrSizeChanged;
            }
            else
            {
                DebugLogger.Info("DebugMode is false: No debug log window created");
            }

            frmToggleApplication = SetToggleApplicationStateWindow();
            SetAutopotWindow();
            SetAutopotYggWindow();
            SetSkillTimerWindow();
            SetCustomButtonsWindow();
            SetAHKWindow();
            SetAutoBuffStatusWindow();
            SetProfileWindow();
            SetAutobuffItemWindow();
            SetAutobuffSkillWindow();
            SetSongMacroWindow();
            SetATKDEFWindow();
            SetMacroSwitchWindow();
            SetConfigWindow();
        }

        private void PositionDebugLogWindow()
        {
            if (debugLogWindow != null && !debugLogWindow.IsDisposed)
            {
                // Position the DebugLogWindow 8 pixels above the bottom of the Container
                int x = this.Location.X + this.DisplayRectangle.X + 8;
                int y = this.Location.Y + this.Height - 8;
                debugLogWindow.Location = new Point(x, y);

                // Match the width of the DebugLogWindow to the Container's drawable area width
                debugLogWindow.Width = this.DisplayRectangle.Width;
            }
        }

        private void Container_LocationOrSizeChanged(object sender, EventArgs e)
        {
            if (!isShuttingDown)
            {
                PositionDebugLogWindow();
            }
        }

        private void SubscribeToDebugLogger()
        {
            debugLogHandler = (message, level) =>
            {
                if (debugLogWindow != null && !debugLogWindow.IsDisposed)
                {
                    debugLogWindow.DebugLogger_OnLogMessage(message, level);
                }
            };
            DebugLogger.OnLogMessage += debugLogHandler;
        }

        private void UnsubscribeFromDebugLogger()
        {
            if (debugLogHandler != null)
            {
                DebugLogger.OnLogMessage -= debugLogHandler;
                debugLogHandler = null;
            }
        }

        public void Addform(TabPage tp, Form f)
        {
            if (!tp.Controls.Contains(f))
            {
                tp.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
                Refresh();
            }
            Refresh();
        }

        private void SetBackGroundColorOfMDIForm()
        {
            foreach (Control ctl in this.Controls)
            {
                if ((ctl) is MdiClient)
                {
                    ctl.BackColor = System.Drawing.Color.White;
                }
            }
        }

        private void ProcessCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProcessString = this.processCB.SelectedItem.ToString();
            Client client = new Client(selectedProcessString);
            ClientSingleton.Instance(client);

            if (client.Process != null)
            {
                DebugLogger.Info($"Process selected: {client.Process.ProcessName} - {client.Process.Id}");
            }
            else
            {
                DebugLogger.Warning($"Process selected: {selectedProcessString} - Process instance not available in Client object.");
            }

            characterName.Text = client.ReadCharacterName();
            characterMap.Text = client.ReadCurrentMap();
            subject.Notify(new Utils.Message(Utils.MessageCode.PROCESS_CHANGED, null));
        }

        private void TabControlAutopot_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tabControl)) return;

            e.Graphics.FillRectangle(new SolidBrush(AppConfig.AccentBackColor), e.Bounds);

            bool isActiveTab = (e.Index == tabControl.SelectedIndex);

            Font tabFont = isActiveTab ? new Font(e.Font, FontStyle.Bold) : e.Font;

            Color textColor = Color.Black;

            string text = tabControl.TabPages[e.Index].Text;
            using (Brush textBrush = new SolidBrush(textColor))
            {
                SizeF textSize = e.Graphics.MeasureString(text, tabFont);
                float textX = e.Bounds.X + (e.Bounds.Width - textSize.Width) / 2;
                float textY = e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2;
                e.Graphics.DrawString(text, tabFont, textBrush, textX, textY);
            }
        }

        private void Container_Load(object sender, EventArgs e)
        {
            ProfileSingleton.Create("Default");
            this.RefreshProcessList();
            this.RefreshProfileList();
            this.profileCB.SelectedItem = "Default";

            ConfigureTabControl(tabControlAutopot);

            // Position DebugLogWindow after the Container is fully loaded
            if (debugLogWindow != null && !debugLogWindow.IsDisposed)
            {
                PositionDebugLogWindow();
            }
        }

        private void ConfigureTabControl(TabControl tabControl)
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControlAutopot_DrawItem;
            tabControl.BackColor = AppConfig.AccentBackColor;
            tabControl.ForeColor = Color.Black;

            tabControl.Appearance = TabAppearance.Normal;
        }

        public void RefreshProfileList()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.profileCB.Items.Clear();
            });
            foreach (string p in Profile.ListAll())
            {
                this.profileCB.Items.Add(p);
            }
        }

        private void RefreshProcessList()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.processCB.Items.Clear();
            });
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle != "" && ClientListSingleton.ExistsByProcessName(p.ProcessName))
                {
                    this.processCB.Items.Add(string.Format("{0}.exe - {1}", p.ProcessName, p.Id));
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshProcessList();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!isShuttingDown)
            {
                ShutdownApplication();
            }
            base.OnClosed(e);
        }

        private void ShutdownApplication()
        {
            if (isShuttingDown)
            {
                DebugLogger.Info("Shutdown already in progress, skipping redundant call...");
                return;
            }

            isShuttingDown = true;

            try
            {
                DebugLogger.Info("Shutting down application...");

                KeyboardHook.Disable();

                DebugLogger.Debug("Subject: Notifying observers...");
                subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));

                UnsubscribeFromDebugLogger();

                if (debugLogWindow != null && !debugLogWindow.IsDisposed)
                {
                    DebugLogger.Info("Closing DebugLogWindow...");
                    debugLogWindow.Close();
                    debugLogWindow.Dispose();
                    debugLogWindow = null;
                }

                foreach (Form childForm in this.MdiChildren)
                {
                    if (!childForm.IsDisposed)
                    {
                        childForm.Close();
                    }
                }

                this.Close();

                DebugLogger.Info("Shutting down logger...");
                DebugLogger.Shutdown();

                DebugLogger.Info("Forcibly exiting application...");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to shutdown application cleanly");
                Environment.Exit(1);
            }
        }

        private void LblLinkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(AppConfig.GithubLink);
        }

        private void LblLinkDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(AppConfig.DiscordLink);
        }

        private void WebsiteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(AppConfig.Website);
        }

        private void ProfileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.profileCB.Text != currentProfile)
            {
                try
                {
                    if (currentProfile != null)
                    {
                        this.frmToggleApplication.TurnOFF();
                    }
                    DebugLogger.Info($"Loading profile: {this.profileCB.Text}");
                    ProfileSingleton.ClearProfile(this.profileCB.Text);
                    ProfileSingleton.Load(this.profileCB.Text);
                    subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, null));
                    currentProfile = this.profileCB.Text.ToString();
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, $"Failed to load profile: {this.profileCB.Text}");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void Update(ISubject subject)
        {
            if (isShuttingDown)
            {
                return;
            }

            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.TURN_ON:
                case MessageCode.PROFILE_CHANGED:
                    Client client = ClientSingleton.GetClient();
                    if (client != null)
                    {
                        characterName.Text = ClientSingleton.GetClient().ReadCharacterName();
                        characterMap.Text = ClientSingleton.GetClient().ReadCurrentMap();
                    }
                    break;
                case MessageCode.SERVER_LIST_CHANGED:
                    this.RefreshProcessList();
                    DebugLogger.Info("Server list refreshed.");
                    break;
                case MessageCode.CLICK_ICON_TRAY:
                    //DebugLogger.Info("Tray icon clicked: Showing main window.");
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    break;
                case MessageCode.DEBUG_MODE_CHANGED:
                    bool newDebugMode = (bool)((subject as Subject).Message.Data);
                    DebugLogger.Info($"Received DEBUG_MODE_CHANGED notification. New DebugMode: {newDebugMode}");
                    if (newDebugMode && debugLogWindow == null)
                    {
                        DebugLogger.Info("DebugMode set to true: Creating and showing DebugLogWindow...");
                        debugLogWindow = new DebugLogWindow(this.Icon);
                        debugLogWindow.Owner = this; // Set Owner when re-enabling DebugMode
                        PositionDebugLogWindow();
                        debugLogWindow.Show();
                        SubscribeToDebugLogger();
                    }
                    else if (!newDebugMode && debugLogWindow != null && !debugLogWindow.IsDisposed)
                    {
                        DebugLogger.Info("DebugMode set to false: Closing DebugLogWindow...");
                        UnsubscribeFromDebugLogger();
                        debugLogWindow.Close();
                        debugLogWindow = null;
                    }
                    else if (newDebugMode && debugLogWindow != null && debugLogWindow.Visible == false)
                    {
                        DebugLogger.Info("DebugMode set to true: Showing existing DebugLogWindow...");
                        debugLogWindow.Owner = this; // Ensure Owner is set when showing again
                        PositionDebugLogWindow();
                        debugLogWindow.Show();
                    }
                    break;
                case MessageCode.SHUTDOWN_APPLICATION:
                    if (!isShuttingDown)
                    {
                        ShutdownApplication();
                    }
                    break;
            }
        }

        private void ContainerResize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) { this.Hide(); }
        }

        private void LoadServers(List<ClientDTO> clients)
        {
            foreach (ClientDTO clientDTO in clients)
            {
                try
                {
                    ClientListSingleton.AddClient(new Client(clientDTO));
                }
                catch { }
            }
        }

        #region Frames

        public ToggleStateForm SetToggleApplicationStateWindow()
        {
            ToggleStateForm frm = new ToggleStateForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(360, 80),
                MdiParent = this
            };
            frm.Show();
            return frm;
        }

        public void SetAutopotWindow()
        {
            AutopotForm frm = new AutopotForm(subject, false)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageAutopot, frm);
        }

        private void SetAutoBuffStatusWindow()
        {
            AutoBuffStatusForm frm = new AutoBuffStatusForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageDebuffs, frm);
        }

        public void SetAutopotYggWindow()
        {
            AutopotForm frm = new AutopotForm(subject, true)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageYggAutopot, frm);
        }

        public void SetSkillTimerWindow()
        {
            SkillTimerForm frm = new SkillTimerForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageSkillTimer, frm);
        }

        public void SetCustomButtonsWindow()
        {
            TransferButtonForm form = new TransferButtonForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(360, 220),
                MdiParent = this
            };
            form.Show();
        }

        public void SetAHKWindow()
        {
            AHKForm frm = new AHKForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageSpammer, frm);
        }

        public void SetProfileWindow()
        {
            ProfileForm frm = new ProfileForm(this)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageProfiles, frm);
        }

        public void SetAutobuffItemWindow()
        {
            AutobuffItemForm frm = new AutobuffItemForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageAutobuffItem, frm);
        }

        public void SetAutobuffSkillWindow()
        {
            AutobuffSkillForm frm = new AutobuffSkillForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            Addform(this.tabPageAutobuffSkill, frm);
            frm.Show();
        }

        public void SetSongMacroWindow()
        {
            MacroSongForm frm = new MacroSongForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            Addform(this.tabPageMacroSongs, frm);
            frm.Show();
        }

        public void SetATKDEFWindow()
        {
            ATKDEFForm frm = new ATKDEFForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            Addform(this.atkDef, frm);
            frm.Show();
        }

        public void SetMacroSwitchWindow()
        {
            MacroSwitchForm frm = new MacroSwitchForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            Addform(this.tabMacroSwitch, frm);
            frm.Show();
        }

        public void SetConfigWindow()
        {
            ConfigForm frm = new ConfigForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            Addform(this.tabConfig, frm);
            frm.Show();
        }

        #endregion
    }
}