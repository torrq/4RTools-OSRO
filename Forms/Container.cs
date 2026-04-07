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
        private bool isShuttingDown;
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

        // Embedded debug log panel
        private RichTextBox _debugConsole;
        private bool _debugPanelVisible = false;
        private const int DEBUG_PANEL_HEIGHT = 200;
        public const int DEBUG_MAX_LINES = 2000;

        // Off-screen compositing: Windows draws the entire control tree to a back-buffer
        // before blitting to screen. Eliminates flicker/lag when dragging with many child controls.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        // Freeze all painting while the user is dragging/resizing the window.
        // WM_SETREDRAW(false) tells Windows to skip ALL paint messages for this HWND tree.
        // On release, WM_SETREDRAW(true) + Refresh() does a single
        //
        private const int WM_ENTERSIZEMOVE = 0x0231;
        private const int WM_EXITSIZEMOVE  = 0x0232;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_ENTERSIZEMOVE)
            {
                Win32Interop.SendMessage(this.Handle, 0x000B /* WM_SETREDRAW */, IntPtr.Zero, IntPtr.Zero);
            }
            else if (m.Msg == WM_EXITSIZEMOVE)
            {
                Win32Interop.SendMessage(this.Handle, 0x000B /* WM_SETREDRAW */, new IntPtr(1), IntPtr.Zero);
                this.Refresh();
            }
            base.WndProc(ref m);
        }

        public Container()
        {
            ConfigGlobal.Initialize();
            ConfigGlobal.SaveConfig();

            DebugLogger.Info($"Container constructor: DebugMode is {ConfigGlobal.GetConfig().DebugMode} after ConfigGlobal.Initialize");

            this.subject.Attach(this);

            InitializeComponent();
            this.DoubleBuffered = true;
            EnableDoubleBufferingRecursive(this);

            characterInfoForm = new CharacterInfo
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(395, 4)
            };
            Controls.Add(characterInfoForm);
            characterInfoForm.Show();

            // Setup for Mini-Mode Toggle
            fullModeClientSize = ClientSize;
            miniModeClientSize = new Size(ClientSize.Width, btnToggleMiniMode.Bottom);

            // Create embedded debug log panel (hidden until debug mode enabled)
            _debugConsole = new RichTextBox
            {
                BackColor = AppConfig.DebugConsoleBackColor,
                ForeColor = AppConfig.DebugConsoleForeColor,
                Font = AppConfig.DebugConsoleFont,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Visible = false
            };
            Controls.Add(_debugConsole);
            _debugConsole.BringToFront();

            regularFont = profileCB.Font;
            italicFont = new Font(regularFont, FontStyle.Italic);
            boldFont = new Font(regularFont, FontStyle.Bold);
            float smallerFontSize = regularFont.Size;
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
                // Debug panel shown in Container_Load after the form is visible
            }

            frmStateSwitch = SetStateSwitchWindow();
            if(!GlobalConfig.DisableSystray)
            {
                trayManager = frmStateSwitch.GetTrayManager();
            }
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

            // Enable double buffering on all child controls (including tab forms)
            // Must be called after all Set*Window() calls have added their forms
            EnableDoubleBufferingRecursive(this);

            SetMiniMode(ConfigGlobal.GetConfig().MiniMode);
        }

        public void SetMiniMode(bool isMiniMode)
        {
            if (this.isMiniMode != isMiniMode)
            {
                this.isMiniMode = isMiniMode;

                SuspendLayout();

                Size baseSize = isMiniMode ? miniModeClientSize : fullModeClientSize;
                ClientSize = _debugPanelVisible
                    ? new Size(baseSize.Width, baseSize.Height + DEBUG_PANEL_HEIGHT)
                    : baseSize;

                btnToggleMiniMode.Image = isMiniMode
                    ? global::_ORTools.Resources.Media.Icons.minimode_more
                    : global::_ORTools.Resources.Media.Icons.minimode_less;

                ResumeLayout(true);
                ConfigGlobal.GetConfig().MiniMode = isMiniMode;
                ConfigGlobal.SaveConfig();
            }
        }

        private Size CurrentBaseSize => isMiniMode ? miniModeClientSize : fullModeClientSize;

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
            if (e.Index < 0 || e.Index >= processCB.Items.Count) return;

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


        private static readonly SolidBrush _tabBgBrush = new SolidBrush(AppConfig.AccentBackColor);
        private static readonly SolidBrush _tabTextBrush = new SolidBrush(Color.Black);
        private Font _cachedTabBoldFont;

        private void TabControlTop_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tabControl)) return;

            e.Graphics.FillRectangle(_tabBgBrush, e.Bounds);

            bool isActiveTab = (e.Index == tabControl.SelectedIndex);
            if (isActiveTab && (_cachedTabBoldFont == null || _cachedTabBoldFont.FontFamily != e.Font.FontFamily || _cachedTabBoldFont.Size != e.Font.Size))
            {
                _cachedTabBoldFont?.Dispose();
                _cachedTabBoldFont = new Font(e.Font, FontStyle.Bold);
            }
            Font tabFont = isActiveTab ? _cachedTabBoldFont : e.Font;

            string text = tabControl.TabPages[e.Index].Text;
            Image icon = tabControl.ImageList?.Images[tabControl.TabPages[e.Index].ImageIndex];

            float textX = e.Bounds.X;

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

            e.Graphics.DrawString(text, tabFont, _tabTextBrush, textX, adjustedTextY);
        }

        private void ShowDebugPanel()
        {
            if (_debugPanelVisible) return;
            _debugPanelVisible = true;
            SuspendLayout();
            ClientSize = new Size(CurrentBaseSize.Width, CurrentBaseSize.Height + DEBUG_PANEL_HEIGHT);
            Padding = new Padding(0, 0, 0, DEBUG_PANEL_HEIGHT);
            PositionDebugConsole();
            _debugConsole.Visible = true;
            _debugConsole.BringToFront();
            ResumeLayout(true);
            DebugLogger.OnLogMessage += AppendDebugLog;
        }

        private void HideDebugPanel()
        {
            if (!_debugPanelVisible) return;
            DebugLogger.OnLogMessage -= AppendDebugLog;
            _debugPanelVisible = false;
            SuspendLayout();
            _debugConsole.Visible = false;
            Padding = new Padding(0);
            ClientSize = CurrentBaseSize;
            ResumeLayout(true);
        }

        private void PositionDebugConsole()
        {
            _debugConsole.SetBounds(0, ClientSize.Height - DEBUG_PANEL_HEIGHT,
                ClientSize.Width, DEBUG_PANEL_HEIGHT);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            if (_debugPanelVisible && _debugConsole != null)
                PositionDebugConsole();
        }

        private void AppendDebugLog(string message, DebugLogger.LogLevel level)
        {
            if (_debugConsole.InvokeRequired)
            {
                _debugConsole.BeginInvoke((MethodInvoker)(() => AppendDebugLog(message, level)));
                return;
            }

            if (string.IsNullOrWhiteSpace(message)) return;

            _debugConsole.SuspendLayout();

            // Cap line count to avoid unbounded memory growth
            if (_debugConsole.Lines.Length >= DEBUG_MAX_LINES)
            {
                int trimTo = DEBUG_MAX_LINES / 2;
                int removeUpTo = _debugConsole.GetFirstCharIndexFromLine(trimTo);
                _debugConsole.Select(0, removeUpTo);
                _debugConsole.SelectedText = string.Empty;
            }

            _debugConsole.SelectionStart = _debugConsole.TextLength;
            _debugConsole.SelectionLength = 0;

            Color defaultLineColor;
            switch (level)
            {
                case DebugLogger.LogLevel.INFO:    defaultLineColor = AppConfig.LogColor_INFO;    break;
                case DebugLogger.LogLevel.WARNING: defaultLineColor = AppConfig.LogColor_WARNING; break;
                case DebugLogger.LogLevel.ERROR:   defaultLineColor = AppConfig.LogColor_ERROR;   break;
                case DebugLogger.LogLevel.DEBUG:   defaultLineColor = AppConfig.LogColor_DEBUG;   break;
                case DebugLogger.LogLevel.STATUS:  defaultLineColor = AppConfig.LogColor_STATUS;  break;
                default: defaultLineColor = _debugConsole.ForeColor; break;
            }

            if (level == DebugLogger.LogLevel.STATUS)
            {
                var match = System.Text.RegularExpressions.Regex.Match(message,
                    $@"^(\d{{2}}:\d{{2}}:\d{{2}}\.\d{{3}}) \[({AppConfig.STATUS})\] (.*)$");

                if (match.Success)
                {
                    _debugConsole.SelectionColor = AppConfig.LogColor_Timestamp;
                    _debugConsole.AppendText(match.Groups[1].Value + " ");

                    _debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    _debugConsole.AppendText("[" + match.Groups[2].Value + "] ");

                    string[] statuses = match.Groups[3].Value
                        .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < statuses.Length; i++)
                    {
                        string[] parts = statuses[i].Split(':');
                        if (parts.Length == 2)
                        {
                            _debugConsole.SelectionColor = AppConfig.LogColor_StatusId;
                            _debugConsole.AppendText(parts[0]);
                            _debugConsole.SelectionColor = _debugConsole.ForeColor;
                            _debugConsole.AppendText(":");
                            _debugConsole.SelectionColor = parts[1] == "**UNKNOWN**"
                                ? AppConfig.LogColor_StatusUnknown
                                : AppConfig.LogColor_StatusName;
                            _debugConsole.AppendText(parts[1]);
                        }
                        else
                        {
                            _debugConsole.SelectionColor = _debugConsole.ForeColor;
                            _debugConsole.AppendText(statuses[i]);
                        }
                        if (i < statuses.Length - 1)
                        {
                            _debugConsole.SelectionColor = _debugConsole.ForeColor;
                            _debugConsole.AppendText(" ");
                        }
                    }
                }
                else
                {
                    _debugConsole.SelectionColor = AppConfig.LogColor_STATUS;
                    _debugConsole.AppendText(message);
                }
            }
            else
            {
                string logLevelPattern = $"({AppConfig.INFO}|{AppConfig.WARNING}|{AppConfig.ERROR}|{AppConfig.DEBUG}|{AppConfig.STATUS})";
                var match = System.Text.RegularExpressions.Regex.Match(message,
                    $@"^(\d{{2}}:\d{{2}}:\d{{2}}\.\d{{3}}) \[{logLevelPattern}\] (.*)$");

                if (match.Success)
                {
                    _debugConsole.SelectionColor = AppConfig.LogColor_Timestamp;
                    _debugConsole.AppendText(match.Groups[1].Value + " ");

                    _debugConsole.SelectionColor = defaultLineColor;
                    _debugConsole.AppendText("[" + match.Groups[2].Value + "] ");

                    int r = Math.Min(255, (int)(defaultLineColor.R * 1.3));
                    int g = Math.Min(255, (int)(defaultLineColor.G * 1.3));
                    int b = Math.Min(255, (int)(defaultLineColor.B * 1.3));
                    _debugConsole.SelectionColor = Color.FromArgb(defaultLineColor.A, r, g, b);
                    _debugConsole.AppendText(match.Groups[3].Value);
                }
                else
                {
                    _debugConsole.SelectionColor = defaultLineColor;
                    _debugConsole.AppendText(message);
                }
            }

            _debugConsole.SelectionColor = _debugConsole.ForeColor;
            _debugConsole.AppendText(Environment.NewLine);
            _debugConsole.ScrollToCaret();
            _debugConsole.ResumeLayout();
        }

        public void Addform(TabPage tp, Form f)
        {
            if (!tp.Controls.Contains(f))
            {
                tp.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
            }
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
            var info = processCB.SelectedItem as GameProcessInfo;
            if (info == null || string.IsNullOrEmpty(info.ProcessText)) return;

            // Reuse the Client already opened during list refresh; fall back to constructing a new one only if needed
            Client client = info.CachedClient ?? new Client(info.ProcessText);

            // Check if the client is logged in
            if (!client.IsLoggedIn)
            {
                DebugLogger.Warning($"Process selected: {info.ProcessText} - No logged-in memory found.");
                processCB.SelectedIndex = -1; // Deselect the combo box
                return;
            }

            // Proceed with client selection
            Client previousClient = ClientSingleton.GetClient();
            bool switchingClient = previousClient != null &&
                                   previousClient.Process?.Id != client.Process?.Id;

            ClientSingleton.Instance(client);

            if (client.Process != null)
            {
                DebugLogger.Info($"Process selected: {client.Process.ProcessName} - {client.Process.Id}");
            }
            else
            {
                DebugLogger.Warning($"Process selected: {info.ProcessText} - Process instance not available in Client object.");
            }

            // Update character info with formatting
            characterInfoForm.UpdateCharacterInfo(client);

            if (switchingClient && frmStateSwitch.IsApplicationOn())
            {
                DebugLogger.Info("Client switched while state ON — toggling off");
                frmStateSwitch.TurnOFF();
            }

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

            Config loadConfig = ConfigGlobal.GetConfig();
            if (loadConfig.DebugMode && loadConfig.DebugModeShowLog)
            {
                DebugLogger.Info("DebugModeShowLog is ON: Showing debug panel");
                ShowDebugPanel();
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
            Config GlobalConfig = ConfigGlobal.GetConfig();

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

                if (!GlobalConfig.DisableSystray)
                {
                    trayManager?.RefreshProfileMenu();
                }
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

            // Snapshot known process names on the UI thread (cheap — it's just a List lookup)
            var knownNames = ClientListSingleton.GetAll()
                .Select(c => c.ProcessName)
                .Distinct()
                .ToList();

            // Do all the slow work (GetProcessesByName, OpenProcess, memory reads) on a thread-pool thread
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                var processItems = new List<GameProcessInfo>();

                try
                {
                    foreach (string name in knownNames)
                    {
                        // GetProcessesByName is much faster than GetProcesses() for known names
                        foreach (Process p in Process.GetProcessesByName(name))
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(p.MainWindowTitle)) continue;

                                string processText = $"{p.ProcessName}.exe - {p.Id}";
                                Client client = new Client(processText);

                                if (client.IsLoggedIn)
                                {
                                    string characterName = client.ReadCharacterName();
                                    string currentMap    = client.ReadCurrentMap();
                                    processItems.Add(new GameProcessInfo(processText, characterName, currentMap, client));
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
                }
                catch (Exception ex)
                {
                    DebugLogger.Error($"Failed to enumerate processes: {ex.Message}");
                }

                var sortedItems = processItems
                    .OrderBy(item =>
                        (string.IsNullOrEmpty(item.CharacterName) && string.IsNullOrEmpty(item.CurrentMap)) ? 1 : 0)
                    .ThenBy(item =>
                    {
                        string[] parts = item.ProcessText.Split('-');
                        if (parts.Length >= 2 && int.TryParse(parts.Last().Trim(), out int id))
                            return id;
                        return int.MaxValue;
                    })
                    .ToList();

                // Marshal the final UI update back onto the UI thread
                if (this.IsDisposed) return;
                this.BeginInvoke((System.Windows.Forms.MethodInvoker)(() =>
                {
                    try
                    {
                        processCB.BeginUpdate();
                        processCB.Items.Clear();
                        this.maxDropDownWidth = 150;
                        foreach (var item in sortedItems)
                            processCB.Items.Add(item);
                        DebugLogger.Info($"Process list refreshed with {processCB.Items.Count} items.");
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.Error($"Failed to update process list UI: {ex.Message}");
                    }
                    finally
                    {
                        processCB.EndUpdate();
                    }
                }));
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

                HideDebugPanel();

                Config GlobalConfig = ConfigGlobal.GetConfig();

                if (!GlobalConfig.DisableSystray) {
                    trayManager?.Dispose();
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
                    if (newDebugMode)
                    {
                        DebugLogger.Info("DebugMode set to true: Showing debug panel");
                        ShowDebugPanel();
                    }
                    else
                    {
                        DebugLogger.Info("DebugMode set to false: Hiding debug panel");
                        HideDebugPanel();
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
            Config GlobalConfig = ConfigGlobal.GetConfig();
            if (!GlobalConfig.DisableSystray && this.WindowState == FormWindowState.Minimized) { this.Hide(); }
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
                    DebugLogger.Warning($"LoadServers: failed to load client '{clientDTO?.Name}': {ex.Message}");
                }
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
            FormHelper.StateSwitchFormInstance = frm;
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

        /// <summary>
        /// Recursively enables DoubleBuffered on every control in the tree.
        /// DoubleBuffered is protected, so we use reflection to set it on controls we don't own.
        /// </summary>
        private static void EnableDoubleBufferingRecursive(Control control)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop?.SetValue(control, true, null);

            foreach (Control child in control.Controls)
            {
                EnableDoubleBufferingRecursive(child);
            }
        }
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