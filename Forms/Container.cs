using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static _ORTools.Utils.FormHelper;

namespace _ORTools.Forms
{
    public partial class Container : Form, IObserver
    {
        private Subject subject = new Subject();
        private string currentProfile;
        private List<ClientDTO> clients = new List<ClientDTO>();
        private StateSwitchForm frmStateSwitch = new StateSwitchForm();
        private TrayManager trayManager;
        private DebugLogWindow debugLogWindow;
        private bool isShuttingDown;
        private DebugLogger.LogMessageHandler debugLogHandler;
        private ProfilesForm profileForm;
        private Font italicFont;
        private Font regularFont;
        private Font boldFont;
        private Font smallItalicFont;
        private Font smallBoldFont;
        private Font smallRegularFont;
        private int maxDropDownWidth = 150;
        private const string OFFLINE_TEXT = "OFFLINE";
        private CharacterInfo characterInfoForm;
        private bool isMiniMode;
        private Size fullModeClientSize;
        private Size miniModeClientSize;

        public Container()
        {
            ConfigGlobal.Initialize();
            ConfigGlobal.SaveConfig();

            DebugLogger.Info($"Container constructor: DebugMode is {ConfigGlobal.GetConfig().DebugMode} after ConfigGlobal.Initialize");

            this.subject.Attach(this);

            InitializeComponent();

            // Initialize CharacterInfo form
            characterInfoForm = new CharacterInfo
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(385, 6)
            };
            Controls.Add(characterInfoForm);
            characterInfoForm.Show();

            // Setup for Mini-Mode Toggle
            fullModeClientSize = ClientSize;
            miniModeClientSize = new Size(ClientSize.Width, btnToggleMiniMode.Bottom);

            regularFont = profileCB.Font;
            italicFont = new Font(regularFont, FontStyle.Italic);
            boldFont = new Font(regularFont, FontStyle.Bold);
            float smallerFontSize = regularFont.Size - 1;
            smallItalicFont = new Font(regularFont.FontFamily, smallerFontSize, FontStyle.Italic);
            smallBoldFont = new Font(regularFont.FontFamily, smallerFontSize, FontStyle.Bold);
            smallRegularFont = new Font(regularFont.FontFamily, smallerFontSize, FontStyle.Regular);

            profileCB.DrawMode = DrawMode.OwnerDrawFixed;
            profileCB.DrawItem += ProfileCB_DrawItem;

            processCB.DrawMode = DrawMode.OwnerDrawVariable;
            processCB.ItemHeight = profileCB.ItemHeight;
            processCB.DropDownWidth = maxDropDownWidth;
            processCB.DropDownHeight = 150;
            processCB.MeasureItem += ProcessCB_MeasureItem;
            processCB.DrawItem += processCB_DrawItem;
            processCB.DropDown += ProcessCB_DropDown;

            Text = AppConfig.WindowTitle;

            Server.Initialize();
            clients.AddRange(Server.GetLocalClients());

            LoadServers(clients);

            IsMdiContainer = true;
            SetBackGroundColorOfMDIForm();

            Config GlobalConfig = ConfigGlobal.GetConfig();

            if (GlobalConfig.DebugMode)
            {
                DebugLogger.Info("DebugMode is ON");

                if (GlobalConfig.DebugModeShowLog)
                {
                    DebugLogger.Info("DebugModeShowLog is ON: Creating and showing DebugLogWindow");
                    debugLogWindow = new DebugLogWindow(Icon)
                    {
                        Owner = this
                    };
                    debugLogWindow.Show();
                    SubscribeToDebugLogger();
                    this.LocationChanged += Container_LocationOrSizeChanged;
                    this.SizeChanged += Container_LocationOrSizeChanged;
                }
            }

            frmStateSwitch = SetStateSwitchWindow();
            trayManager = frmStateSwitch.GetTrayManager();
            SetAutopotHPWindow();
            SetAutopotSPWindow();
            SetSkillTimerWindow();
            SetAutoOffWindow();
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
            SetTopTabIcons();

            SetMiniMode(ConfigGlobal.GetConfig().MiniMode);
        }

        public void SetMiniMode(bool isMiniMode)
        {
            if (this.isMiniMode != isMiniMode)
            {
                this.isMiniMode = isMiniMode;

                SuspendLayout();

                if (isMiniMode)
                {
                    btnToggleMiniMode.Image = global::_ORTools.Resources.Media.Icons.minimode_more;
                    ClientSize = miniModeClientSize;
                }
                else
                {
                    btnToggleMiniMode.Image = global::_ORTools.Resources.Media.Icons.minimode_less;
                    if (isMiniMode)
                    {
                        btnToggleMiniMode.Image = global::_ORTools.Resources.Media.Icons.minimode_more;
                        ClientSize = miniModeClientSize;
                    }
                    else
                    {
                        btnToggleMiniMode.Image = global::_ORTools.Resources.Media.Icons.minimode_less;
                        ClientSize = fullModeClientSize;
                    }
                }

                ResumeLayout(true);
                ConfigGlobal.GetConfig().MiniMode = isMiniMode;
                ConfigGlobal.SaveConfig();
            }
        }

        private void SetTopTabIcons()
        {
            var icons = new List<Image>
            {
                global::_ORTools.Resources.Media.Icons.tab_autopot_hp,
                global::_ORTools.Resources.Media.Icons.tab_autopot_sp,
                global::_ORTools.Resources.Media.Icons.tab_skill_timer,
                global::_ORTools.Resources.Media.Icons.tab_auto_off,
            };

            TabIconHelper.SetTabIcons(tabControlTop, icons);
        }

        private void BtnToggleMiniMode_Click(object sender, EventArgs e)
        {
            SetMiniMode(!isMiniMode);
        }

        private void ProcessCB_DropDown(object sender, EventArgs e)
        {
            RefreshProcessList();
        }

        private void ProfileCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            string itemText = profileCB.Items[e.Index].ToString();
            Font font = itemText == "Default" ? italicFont : regularFont;

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

        private void ProcessCB_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            if (!(processCB.Items[e.Index] is GameProcessInfo item)) return;

            int lineHeight = processCB.Font.Height;
            e.ItemHeight = lineHeight * 2;

            using (Graphics g = processCB.CreateGraphics())
            {
                float processWidth = g.MeasureString(item.ProcessText, processCB.Font).Width + 2;

                float contextWidth;
                bool isNotLoggedIn = (string.IsNullOrEmpty(item.CharacterName)) &&
                                     (string.IsNullOrEmpty(item.CurrentMap));

                if (isNotLoggedIn)
                {
                    const float indent = 10;
                    contextWidth = indent + g.MeasureString(OFFLINE_TEXT, smallItalicFont).Width;
                }
                else
                {
                    const float indent = 10;
                    float characterWidth = g.MeasureString(item.CharacterName, smallBoldFont).Width;
                    float atWidth = g.MeasureString(" @ ", smallRegularFont).Width;
                    float mapWidth = g.MeasureString(item.CurrentMap, smallRegularFont).Width;
                    contextWidth = indent + characterWidth + atWidth + mapWidth;
                }

                float itemWidth = Math.Max(processWidth, contextWidth) + 2;
                maxDropDownWidth = Math.Max(maxDropDownWidth, (int)itemWidth);
                processCB.DropDownWidth = maxDropDownWidth;
            }
        }

        private void processCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = processCB.Items[e.Index] as GameProcessInfo;
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

            bool isNotLoggedIn = (string.IsNullOrEmpty(item.CharacterName)) &&
                                 (string.IsNullOrEmpty(item.CurrentMap));

            int lineHeight = e.Font.Height;
            float xOffset = e.Bounds.Left + 10;
            float yOffset = e.Bounds.Top + lineHeight + 2;

            if (isNotLoggedIn)
            {
                using (Brush offlineBrush = new SolidBrush(Color.Red))
                {
                    e.Graphics.DrawString(OFFLINE_TEXT, smallItalicFont, offlineBrush, xOffset, yOffset);
                }
            }
            else
            {
                string characterText = item.CharacterName;
                using (Brush characterBrush = new SolidBrush(AppConfig.CharacterColor))
                {
                    e.Graphics.DrawString(characterText, smallBoldFont, characterBrush, xOffset, yOffset);
                    xOffset += e.Graphics.MeasureString(characterText, smallBoldFont).Width;
                }

                using (Brush atBrush = new SolidBrush(Color.Black))
                {
                    e.Graphics.DrawString(" @ ", smallRegularFont, atBrush, xOffset, yOffset);
                    xOffset += e.Graphics.MeasureString(" @ ", smallRegularFont).Width;
                }

                using (Brush mapBrush = new SolidBrush(AppConfig.MapColor))
                {
                    e.Graphics.DrawString(item.CurrentMap, smallRegularFont, mapBrush, xOffset, yOffset);
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

        private void TabControlTop_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tabControl)) return;

            e.Graphics.FillRectangle(new SolidBrush(AppConfig.AccentBackColor), e.Bounds);

            bool isActiveTab = (e.Index == tabControl.SelectedIndex);
            Font tabFont = isActiveTab ? new Font(e.Font, FontStyle.Bold) : e.Font;
            Color textColor = Color.Black;

            string text = tabControl.TabPages[e.Index].Text;
            Image icon = tabControl.ImageList?.Images[tabControl.TabPages[e.Index].ImageIndex];

            float textX = e.Bounds.X;
            float textY = e.Bounds.Y;

            // Draw icon (if any)
            int spacing = 7;
            if (icon != null)
            {
                int iconX = e.Bounds.X + spacing;
                int iconY = e.Bounds.Y + (e.Bounds.Height - icon.Height - 1) / 2;
                e.Graphics.DrawImage(icon, iconX, iconY);
                textX += icon.Width + 9; // shift text to the right of the icon
            }

            SizeF textSize = e.Graphics.MeasureString(text, tabFont);
            float adjustedTextY = e.Bounds.Y + (e.Bounds.Height - textSize.Height) / 2;

            using (Brush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(text, tabFont, textBrush, textX, adjustedTextY);
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
            string selectedProcessString = (processCB.SelectedItem as GameProcessInfo)?.ProcessText;
            if (string.IsNullOrEmpty(selectedProcessString)) return;

            // Create Client to check login status
            Client client = new Client(selectedProcessString);

            // Check if the client is logged in
            if (!client.IsLoggedIn)
            {
                DebugLogger.Warning($"Process selected: {selectedProcessString} - No logged-in memory found.");
                processCB.SelectedIndex = -1; // Deselect the combo box
                return;
            }

            // Proceed with client selection
            ClientSingleton.Instance(client);

            if (client.Process != null)
            {
                DebugLogger.Info($"Process selected: {client.Process.ProcessName} - {client.Process.Id}");
            }
            else
            {
                DebugLogger.Warning($"Process selected: {selectedProcessString} - Process instance not available in Client object.");
            }

            // Update character info with formatting
            characterInfoForm.UpdateCharacterInfo(client);

            subject.Notify(new Utils.Message(MessageCode.PROCESS_CHANGED, null));
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

            ConfigureTabControl(tabControlTop);

            if (debugLogWindow != null && !debugLogWindow.IsDisposed)
            {
                PositionDebugLogWindow();
            }
        }

        private void ConfigureTabControl(TabControl tabControl)
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControlTop_DrawItem;
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
            // Avoid modifying ComboBox while it is open
            if (processCB.DroppedDown)
            {
                DebugLogger.Warning("ComboBox is dropped down. Skipping RefreshProcessList to avoid crash.");
                return;
            }

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)RefreshProcessList);
                return;
            }

            try
            {
                processCB.BeginUpdate();
                processCB.Items.Clear();
                this.maxDropDownWidth = 150;

                var processItems = new List<GameProcessInfo>();

                foreach (Process p in Process.GetProcesses())
                {
                    if (p.MainWindowTitle != "" && ClientListSingleton.ExistsByProcessName(p.ProcessName))
                    {
                        try
                        {
                            string processText = $"{p.ProcessName}.exe - {p.Id}";
                            Client client = new Client(processText);

                            // Only add processes with valid logged-in memory
                            if (client.IsLoggedIn)
                            {
                                string characterName = client.ReadCharacterName();
                                string currentMap = client.ReadCurrentMap();
                                processItems.Add(new GameProcessInfo(processText, characterName, currentMap));
                                DebugLogger.Info($"Added process to list: {processText} (Character: {characterName}, Map: {currentMap})");
                            }
                            else
                            {
                                DebugLogger.Warning($"Skipped process: {processText} - No logged-in memory found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Warning($"Skipped process due to error: {p.ProcessName} ({p.Id}) - {ex.Message}");
                        }
                    }
                }

                var sortedItems = processItems
                    .OrderBy(item =>
                        (string.IsNullOrEmpty(item.CharacterName) && string.IsNullOrEmpty(item.CurrentMap)) ? 1 : 0)
                    .ThenBy(item =>
                    {
                        try
                        {
                            string[] parts = item.ProcessText.Split('-');
                            if (parts.Length >= 2)
                            {
                                string idPart = parts.Last().Trim();
                                if (int.TryParse(idPart, out int id))
                                    return id;
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugLogger.Warning($"Failed to parse process ID from: {item.ProcessText}. Error: {ex.Message}");
                        }
                        return int.MaxValue;
                    });

                foreach (var item in sortedItems)
                {
                    processCB.Items.Add(item);
                }

                DebugLogger.Info($"Process list refreshed with {processCB.Items.Count} items.");
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Failed to refresh process list: {ex.Message}");
            }
            finally
            {
                processCB.EndUpdate();
            }
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
                DebugLogger.Error($"Failed to shutdown application cleanly: {ex.Message}");
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
                        this.frmStateSwitch.TurnOFF();
                    }

                    DebugLogger.Info($"Loading profile: {profileName}");
                    Client client = ClientSingleton.GetClient();
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
                catch { }
            }
        }

        #region Frames

        public StateSwitchForm SetStateSwitchWindow()
        {
            StateSwitchForm frm = new StateSwitchForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(400, 80),
                MdiParent = this
            };
            frm.Show();
            return frm;
        }

        private void SetAutoBuffStatusWindow()
        {
            DebuffsForm frm = new DebuffsForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageDebuffs, frm);
        }

        public void SetAutopotHPWindow()
        {
            AutopotHPForm frm = new AutopotHPForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageAutopotHP, frm);
        }

        public void SetAutopotSPWindow()
        {
            AutopotSPForm frm = new AutopotSPForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageAutopotSP, frm);
        }

        public void SetSkillTimerWindow()
        {
            SkillTimerForm frm = new SkillTimerForm(subject, frmStateSwitch)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageSkillTimer, frm);
        }

        public void SetAutoOffWindow()
        {
            AutoOffForm frm = new AutoOffForm(subject, frmStateSwitch)
            {
                FormBorderStyle = FormBorderStyle.None,
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageAutoOff, frm);
        }

        public void SetCustomButtonsWindow()
        {
            TransferHelperForm form = new TransferHelperForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(420, 230),
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
            profileForm = new ProfilesForm(this)
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
            SongsForm frm = new SongsForm(subject)
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
            SettingsForm frm = new SettingsForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            Addform(this.tabConfig, frm);
            frm.Show();
        }

        #endregion Frames
    }

    /// <summary>
    /// Custom button class that prevents the focus rectangle from being drawn.
    /// </summary>
    public class NoFocusButton : Button
    {
        public NoFocusButton()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }
}