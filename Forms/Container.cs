using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BruteGamingMacros.UI.Forms
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
        private Font boldFont;
        private Font smallItalicFont;
        private Font smallBoldFont;
        private Font smallRegularFont;
        private int maxDropDownWidth = 150;
        private const string OFFLINE_TEXT = "OFFLINE";

        public Container()
        {
            ConfigGlobal.Initialize();
            ConfigGlobal.SaveConfig();

            DebugLogger.Info($"Container constructor: DebugMode is {ConfigGlobal.GetConfig().DebugMode} after ConfigGlobal.Initialize");

            this.subject.Attach(this);

            InitializeComponent();

            this.regularFont = this.profileCB.Font;
            this.italicFont = new Font(this.regularFont, FontStyle.Italic);
            this.boldFont = new Font(this.regularFont, FontStyle.Bold);
            float smallerFontSize = this.regularFont.Size - 1;
            this.smallItalicFont = new Font(this.regularFont.FontFamily, smallerFontSize, FontStyle.Italic);
            this.smallBoldFont = new Font(this.regularFont.FontFamily, smallerFontSize, FontStyle.Bold);
            this.smallRegularFont = new Font(this.regularFont.FontFamily, smallerFontSize, FontStyle.Regular);

            this.profileCB.DrawMode = DrawMode.OwnerDrawFixed;
            this.profileCB.DrawItem += new DrawItemEventHandler(this.profileCB_DrawItem);

            this.processCB.DrawMode = DrawMode.OwnerDrawVariable;
            this.processCB.ItemHeight = this.profileCB.ItemHeight;
            this.processCB.DropDownWidth = this.maxDropDownWidth;
            this.processCB.DropDownHeight = 150;
            this.processCB.MeasureItem += new MeasureItemEventHandler(this.processCB_MeasureItem);
            this.processCB.DrawItem += new DrawItemEventHandler(this.processCB_DrawItem);
            this.processCB.DropDown += new EventHandler(this.ProcessCB_DropDown); // Added for refresh on click

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

        private void ProcessCB_DropDown(object sender, EventArgs e)
        {
            RefreshProcessList();
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

        private void processCB_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = processCB.Items[e.Index] as ProcessDisplayItem;
            if (item == null) return;

            int lineHeight = processCB.Font.Height;
            e.ItemHeight = lineHeight * 2;

            using (Graphics g = processCB.CreateGraphics())
            {
                float processWidth = g.MeasureString(item.ProcessText, processCB.Font).Width + 2;

                float contextWidth;
                bool isNotLoggedIn = !item.IsOnline ||
                                     (string.IsNullOrEmpty(item.CharacterName) || item.CharacterName == "- -") &&
                                     (string.IsNullOrEmpty(item.CurrentMap) || item.CurrentMap == "- -");

                if (isNotLoggedIn)
                {
                    float indent = 10;
                    contextWidth = indent + g.MeasureString(OFFLINE_TEXT, this.smallItalicFont).Width;
                }
                else
                {
                    float indent = 10;
                    float characterWidth = g.MeasureString(item.CharacterName, this.smallBoldFont).Width;
                    float atWidth = g.MeasureString(" @ ", this.smallRegularFont).Width;
                    float mapWidth = g.MeasureString(item.CurrentMap, this.smallRegularFont).Width;
                    contextWidth = indent + characterWidth + atWidth + mapWidth;
                }

                float itemWidth = Math.Max(processWidth, contextWidth) + 2;
                this.maxDropDownWidth = Math.Max(this.maxDropDownWidth, (int)itemWidth);
                processCB.DropDownWidth = this.maxDropDownWidth;
            }
        }

        private void processCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = processCB.Items[e.Index] as ProcessDisplayItem;
            if (item == null) return;

            Brush backgroundBrush = null;
            Brush foregroundBrush = null;
            bool disposeCustomBrushes = false;

            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
            {
                backgroundBrush = SystemBrushes.Window;
                foregroundBrush = SystemBrushes.WindowText;
            }
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backgroundBrush = new SolidBrush(Color.FromArgb(220, 220, 220));
                foregroundBrush = new SolidBrush(Color.Black);
                disposeCustomBrushes = true;
            }
            else
            {
                backgroundBrush = SystemBrushes.Window;
                foregroundBrush = SystemBrushes.WindowText;
            }

            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            e.Graphics.DrawString(item.ProcessText, e.Font, foregroundBrush, e.Bounds.Left + 2, e.Bounds.Top + 2);

            bool isNotLoggedIn = !item.IsOnline ||
                                 (string.IsNullOrEmpty(item.CharacterName) || item.CharacterName == "- -") &&
                                 (string.IsNullOrEmpty(item.CurrentMap) || item.CurrentMap == "- -");

            int lineHeight = e.Font.Height;
            float xOffset = e.Bounds.Left + 10;
            float yOffset = e.Bounds.Top + lineHeight + 2;

            if (isNotLoggedIn)
            {
                using (Brush offlineBrush = new SolidBrush(Color.Red))
                {
                    e.Graphics.DrawString(OFFLINE_TEXT, this.smallItalicFont, offlineBrush, xOffset, yOffset);
                }
            }
            else
            {
                string characterText = item.CharacterName;
                using (Brush characterBrush = new SolidBrush(AppConfig.CharacterColor))
                {
                    e.Graphics.DrawString(characterText, this.smallBoldFont, characterBrush, xOffset, yOffset);
                    xOffset += e.Graphics.MeasureString(characterText, this.smallBoldFont).Width;
                }

                using (Brush atBrush = new SolidBrush(Color.Black))
                {
                    e.Graphics.DrawString(" @ ", this.smallRegularFont, atBrush, xOffset, yOffset);
                    xOffset += e.Graphics.MeasureString(" @ ", this.smallRegularFont).Width;
                }

                using (Brush mapBrush = new SolidBrush(AppConfig.MapColor))
                {
                    e.Graphics.DrawString(item.CurrentMap, this.smallRegularFont, mapBrush, xOffset, yOffset);
                }
            }

            if (disposeCustomBrushes)
            {
                backgroundBrush?.Dispose();
                foregroundBrush?.Dispose();
            }

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus &&
                (e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)
            {
                using (Pen focusPen = new Pen(Color.LightGray, 1))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    Rectangle focusBounds = e.Bounds;
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }
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
            if (processCB.SelectedIndex < 0) return;

            string selectedProcessString = (processCB.SelectedItem as ProcessDisplayItem)?.ProcessText;
            if (string.IsNullOrEmpty(selectedProcessString)) return;

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

            if (client != null)
            {
                DebugLogger.Info($"Client online status: {(client.IsOnline() ? "Online" : "Offline")}");
            }

            characterName.Text = client.ReadCharacterName() ?? "- -";
            characterMap.Text = client.ReadCurrentMap() ?? "- -";
            subject.Notify(new Utils.Message(Utils.MessageCode.PROCESS_CHANGED, null));
        }

        private void Container_Load(object sender, EventArgs e)
        {
            ProfileSingleton.Create("Default");
            this.RefreshProcessList();
            this.RefreshProfileList();

            string lastUsedProfile = ConfigGlobal.GetConfig().LastUsedProfile;
            string profileToLoad = "Default";
            if (!string.IsNullOrWhiteSpace(lastUsedProfile) && Profile.ListAll().Contains(lastUsedProfile))
            {
                profileToLoad = lastUsedProfile;
            }

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

                var profiles = Profile.ListAll().ToList();
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

                trayManager?.RefreshProfileMenu();
            });
        }

        private void RefreshProcessList()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.processCB.Items.Clear();
                this.maxDropDownWidth = 150;

                var processItems = new List<ProcessDisplayItem>();
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.MainWindowTitle != "" && ClientListSingleton.ExistsByProcessName(p.ProcessName))
                    {
                        Client client = new Client($"{p.ProcessName}.exe - {p.Id}");
                        string processText = $"{p.ProcessName}.exe - {p.Id}";
                        string characterName = client.ReadCharacterName() ?? "- -";
                        string currentMap = client.ReadCurrentMap() ?? "- -";
                        bool isOnline = client.IsOnline();
                        processItems.Add(new ProcessDisplayItem(processText, characterName, currentMap, isOnline));
                    }
                }

                var sortedItems = processItems.OrderBy(item =>
                    !item.IsOnline ||
                    (string.IsNullOrEmpty(item.CharacterName) || item.CharacterName == "- -") &&
                    (string.IsNullOrEmpty(item.CurrentMap) || item.CurrentMap == "- -") ? 1 : 0)
                    .ThenBy(item => item.CharacterName == "- -" ? "" : item.CharacterName)
                    .ThenBy(item =>
                    {
                        string idPart = item.ProcessText.Split('-').Last().Trim();
                        return int.TryParse(idPart, out int id) ? id : int.MaxValue;
                    });

                foreach (var item in sortedItems)
                {
                    this.processCB.Items.Add(item);
                }
            });
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

                trayManager?.Dispose();

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

                    DebugLogger.Info($"Loading profile: {profileName}");
                    Client client = ClientSingleton.GetClient();
                    if (client != null)
                    {
                        DebugLogger.Info($"Client online status: {(client.IsOnline() ? "Online" : "Offline")}");
                    }
                    ProfileSingleton.ClearProfile(profileName);
                    ProfileSingleton.Load(profileName);
                    subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, null));
                    currentProfile = profileName;
                    profileCB.SelectedItem = profileName;

                    ConfigGlobal.GetConfig().LastUsedProfile = profileName;
                    ConfigGlobal.SaveConfig();

                    if (profileForm != null && !profileForm.IsDisposed)
                    {
                        profileForm.UpdateProfileIcon(profileName);
                    }
                }
                catch (Exception ex)
                {
                    DebugLogger.Error(ex, $"Failed to load profile: {profileName}");
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        debugLogWindow = new DebugLogWindow(this.Icon)
                        {
                            Owner = this
                        };
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
                catch (Exception ex)
                {
                    // Log error loading client but continue with others
                    Console.WriteLine($"Error loading client '{clientDTO.Name}': {ex.Message}");
                }
            }
        }

        private class ProcessDisplayItem
        {
            public string ProcessText { get; }
            public string CharacterName { get; }
            public string CurrentMap { get; }
            public bool IsOnline { get; }

            public ProcessDisplayItem(string processText, string characterName, string currentMap, bool isOnline)
            {
                ProcessText = processText;
                CharacterName = characterName;
                CurrentMap = currentMap;
                IsOnline = isOnline;
            }

            public override string ToString()
            {
                return ProcessText;
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
            TransferHelperForm form = new TransferHelperForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(450, 220),
                MdiParent = this
            };
            form.Show();
        }

        public void SetAHKWindow()
        {
            SkillSpammerForm frm = new SkillSpammerForm(subject)
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