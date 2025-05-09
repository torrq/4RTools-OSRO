using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _4RTools.Forms
{
    public partial class Container : Form, IObserver
    {
        private Subject subject = new Subject();
        private string currentProfile;
        List<ClientDTO> clients = new List<ClientDTO>();
        private ToggleStateForm frmToggleApplication = new ToggleStateForm();
        private NotificationTrayManager trayManager;
        private DebugLogWindow debugLogWindow;
        private bool isShuttingDown = false;
        private DebugLogger.LogMessageHandler debugLogHandler;
        private ProfileForm profileForm;
        private Font italicFont;
        private Font regularFont;

        public Container()
        {
            ConfigGlobal.Initialize();
            ConfigGlobal.SaveConfig();

            DebugLogger.Info($"Container constructor: DebugMode is {ConfigGlobal.GetConfig().DebugMode} after ConfigGlobal.Initialize");

            this.subject.Attach(this);

            InitializeComponent();

            this.regularFont = this.profileCB.Font;
            this.italicFont = new Font(this.regularFont, FontStyle.Italic);

            this.profileCB.DrawMode = DrawMode.OwnerDrawFixed;
            this.profileCB.DrawItem += new DrawItemEventHandler(this.profileCB_DrawItem);

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
                debugLogWindow.Owner = this;
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
            trayManager = frmToggleApplication.GetTrayManager();
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

        private void profileCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            string itemText = this.profileCB.Items[e.Index].ToString();
            Font font = itemText == "Default" ? this.italicFont : this.regularFont;

            Brush backgroundBrush;
            Brush foregroundBrush;

            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            {
                backgroundBrush = SystemBrushes.Window;
                foregroundBrush = SystemBrushes.WindowText;
            }
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backgroundBrush = SystemBrushes.Highlight;
                foregroundBrush = SystemBrushes.HighlightText;
            }
            else
            {
                backgroundBrush = SystemBrushes.Window;
                foregroundBrush = SystemBrushes.WindowText;
            }

            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            e.Graphics.DrawString(itemText, font, foregroundBrush, e.Bounds.Left + 2, e.Bounds.Top);

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus &&
                (e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)
            {
                e.DrawFocusRectangle();
            }
        }

        private void PositionDebugLogWindow()
        {
            if (debugLogWindow != null && !debugLogWindow.IsDisposed)
            {
                int x = this.Location.X + this.DisplayRectangle.X + 8;
                int y = this.Location.Y + this.Height - 8;
                debugLogWindow.Location = new Point(x, y);
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

            // Load the last used profile or Default
            string lastUsedProfile = ConfigGlobal.GetConfig().LastUsedProfile;
            string profileToLoad = "Default"; // Fallback to Default
            if (!string.IsNullOrWhiteSpace(lastUsedProfile) && Profile.ListAll().Contains(lastUsedProfile))
            {
                profileToLoad = lastUsedProfile;
            }

            DebugLogger.Info($"Container_Load: Attempting to load profile '{profileToLoad}'");
            LoadProfile(profileToLoad);
            this.profileCB.SelectedItem = profileToLoad;

            ConfigureTabControl(tabControlAutopot);

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
                string currentSelection = profileCB.SelectedItem?.ToString();

                profileCB.Items.Clear();

                var profiles = Profile.ListAll().Select(FormUtils.RestoreInvalidCharacters).ToList();
                if (profiles.Contains("Default"))
                {
                    profileCB.Items.Add("Default");
                    profiles.Remove("Default");
                }
                foreach (string profile in profiles.OrderBy(profile => profile, StringComparer.OrdinalIgnoreCase))
                {
                    profileCB.Items.Add(profile);
                }

                this.profileCB.SelectedIndexChanged -= ProfileCB_SelectedIndexChanged;

                if (currentSelection != null && profileCB.Items.Contains(currentSelection))
                {
                    profileCB.SelectedItem = currentSelection;
                }
                else if (profileCB.Items.Count > 0)
                {
                    profileCB.SelectedItem = ConfigGlobal.GetConfig().LastUsedProfile ?? "Default";
                }

                this.profileCB.SelectedIndexChanged += ProfileCB_SelectedIndexChanged;

                if (profileForm != null && !profileForm.IsDisposed)
                {
                    profileForm.RefreshProfileList();
                }

                if (trayManager != null)
                {
                    trayManager.RefreshProfileMenu();
                }
            });
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

        private string EncodeInvalidCharacters(string profileName)
        {
            var substitutions = new (char InvalidChar, string Replacement)[]
            {
                (':', "&#58;"),
                ('"', "&#34;"),
                ('|', "&#124;"),
                ('?', "&#63;"),
            };

            string result = profileName;
            foreach (var (invalidChar, replacement) in substitutions)
            {
                result = result.Replace(invalidChar.ToString(), replacement);
            }

            return result;
        }

        public void LoadProfile(string profileName)
        {
            if (profileName != currentProfile)
            {
                try
                {
                    if (currentProfile != null)
                    {
                        this.frmToggleApplication.TurnOFF();
                    }

                    string encodedProfileName = EncodeInvalidCharacters(profileName);

                    DebugLogger.Info($"Loading profile: {profileName}" + (profileName != encodedProfileName ? $" ({encodedProfileName})" : ""));
                    ProfileSingleton.ClearProfile(encodedProfileName);
                    ProfileSingleton.Load(encodedProfileName);
                    subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, null));
                    currentProfile = profileName;
                    profileCB.SelectedItem = profileName;

                    // Save the profile as LastUsedProfile
                    ConfigGlobal.GetConfig().LastUsedProfile = encodedProfileName;
                    ConfigGlobal.SaveConfig();
                    DebugLogger.Info($"Saved profile '{encodedProfileName}' as LastUsedProfile");

                    if (profileForm != null && !profileForm.IsDisposed)
                    {
                        profileForm.UpdateProfileIcon(profileName);
                    }
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, $"Failed to load profile: {profileName}");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Fallback to Default profile if loading fails
                    if (profileName != "Default")
                    {
                        DebugLogger.Info("Falling back to Default profile due to load failure");
                        LoadProfile("Default");
                    }
                }
            }
        }

        private void ProfileCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.profileCB.Text != currentProfile)
            {
                LoadProfile(this.profileCB.Text);
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
                    if ((subject as Subject).Message.Data is string newProfileName && newProfileName != currentProfile)
                    {
                        LoadProfile(newProfileName);
                    }
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
                        debugLogWindow.Owner = this;
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
                        debugLogWindow.Owner = this;
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
                Location = new Point(450, 220),
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
            profileForm = new ProfileForm(this)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            profileForm.Show();
            Addform(this.tabPageProfiles, profileForm);
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