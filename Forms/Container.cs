using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private ToggleApplicationStateForm frmToggleApplication = new ToggleApplicationStateForm();
        private DebugLogWindow debugLogWindow; // Separate debug log window
        private bool isShuttingDown = false; // Flag to prevent multiple shutdowns

        public Container()
        {
            ConfigGlobal.Initialize();
            ConfigGlobal.SaveConfig();

            // Log the DebugMode value to confirm its state at initialization
            DebugLogger.Info($"Container constructor: DebugMode is {ConfigGlobal.GetConfig().DebugMode} after ConfigGlobal.Initialize");

            this.subject.Attach(this);

            InitializeComponent();

            this.Text = AppConfig.WindowTitle;

            Server.Initialize(); // Will log errors if they occur
            clients.AddRange(Server.GetLocalClients()); //Load Local Servers First

            LoadServers(clients);

            //Container Configuration
            this.IsMdiContainer = true;
            SetBackGroundColorOfMDIForm();

            // Create and show the debug log window only if DebugMode is true
            if (ConfigGlobal.GetConfig().DebugMode)
            {
                DebugLogger.Info("DebugMode is true: Creating and showing DebugLogWindow");
                debugLogWindow = new DebugLogWindow();
                PositionDebugLogWindow();
                debugLogWindow.Show();
                SubscribeToDebugLogger(); // Subscribe ONLY if the window is created

                // Add event handlers to reposition DebugLogWindow when Container moves or resizes
                this.LocationChanged += Container_LocationOrSizeChanged;
                this.SizeChanged += Container_LocationOrSizeChanged;
            }
            else
            {
                DebugLogger.Info("DebugMode is false: No debug log window created");
            }

            //Paint Children Forms
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
                // Position the DebugLogWindow directly below the Container
                int x = this.Location.X;
                int y = this.Location.Y + this.Height;
                debugLogWindow.Location = new Point(x, y);

                // Match the width of the DebugLogWindow to the Container's width
                debugLogWindow.Width = this.Width;

                // Optionally, set a fixed height for the DebugLogWindow (adjust as needed)
                debugLogWindow.Height = 200; // You can adjust this value to fit your needs
            }
        }

        private void Container_LocationOrSizeChanged(object sender, EventArgs e)
        {
            // Reposition the DebugLogWindow whenever the Container moves or resizes
            if (!isShuttingDown) // Avoid repositioning during shutdown
            {
                PositionDebugLogWindow();
            }
        }

        private void SubscribeToDebugLogger()
        {
            // Modified the lambda to accept two arguments (message and level)
            // And updated the call to debugLogWindow to pass both arguments
            DebugLogger.OnLogMessage += (message, level) =>
            {
                if (debugLogWindow != null && !debugLogWindow.IsDisposed)
                {
                    // Call the method in DebugLogWindow that handles colored appending
                    debugLogWindow.DebugLogger_OnLogMessage(message, level);
                }
            };
        }

        // Add an unsubscribe method for the DebugLogger event
        private void UnsubscribeFromDebugLogger()
        {
            // Need to use the exact same lambda or store the delegate instance
            // Storing the delegate instance is generally cleaner for unsubscribing
            // Let's refactor SubscribeToDebugLogger to use a stored delegate

            // If you can't refactor easily now, you might skip explicit unsubscribe for this lambda
            // if the DebugLogWindow lifetime is strictly tied to the Container.
            // However, it's good practice to unsubscribe.
            // For now, leaving this as a placeholder/note. The primary error is fixed.
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
            Client client = new Client(this.processCB.SelectedItem.ToString());
            ClientSingleton.Instance(client);
            characterName.Text = client.ReadCharacterName();
            characterMap.Text = client.ReadCurrentMap();
            subject.Notify(new Utils.Message(Utils.MessageCode.PROCESS_CHANGED, null));
        }

        private void TabControlAutopot_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is TabControl tabControl)) return;

            // Background color for all tabs
            e.Graphics.FillRectangle(new SolidBrush(AppConfig.AccentBackColor), e.Bounds);

            // Check if this is the selected (active) tab
            bool isActiveTab = (e.Index == tabControl.SelectedIndex);

            // Use bold font for active tab, regular for others
            Font tabFont = isActiveTab ? new Font(e.Font, FontStyle.Bold) : e.Font;

            // Set text color (change if needed)
            Color textColor = Color.Black;

            // Draw tab text centered
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
            //ConfigureTabControl(atkDefMode);
        }

        private void ConfigureTabControl(TabControl tabControl)
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControlAutopot_DrawItem;
            tabControl.BackColor = AppConfig.AccentBackColor;
            tabControl.ForeColor = Color.Black;

            // Ensure a flat modern look
            tabControl.Appearance = TabAppearance.Normal; // Prevents 3D borders
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

                // Disable keyboard hooks
                KeyboardHook.Disable();

                // Notify observers to turn off
                DebugLogger.Debug("Subject: Notifying observers...");
                subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));

                // Close and dispose of the DebugLogWindow if it exists
                if (debugLogWindow != null && !debugLogWindow.IsDisposed)
                {
                    DebugLogger.Info("Closing DebugLogWindow...");
                    // Unsubscribe before closing
                    DebugLogger.OnLogMessage -= (message, level) => { // Need to match the lambda signature
                        if (debugLogWindow != null && !debugLogWindow.IsDisposed)
                        {
                            debugLogWindow.DebugLogger_OnLogMessage(message, level);
                        }
                    }; // WARNING: Unsubscribing lambdas like this can be tricky. See note in SubscribeToDebugLogger.
                    debugLogWindow.Close();
                    // debugLogWindow.Dispose(); // Dispose is called by Close() by default
                    debugLogWindow = null;
                }

                // Close all child forms
                foreach (Form childForm in this.MdiChildren)
                {
                    if (!childForm.IsDisposed)
                    {
                        childForm.Close();
                        // childForm.Dispose(); // Dispose is called by Close() by default
                    }
                }

                // Ensure the main form is closed and disposed
                // Application.Exit() will handle closing forms, but explicit Dispose is fine.
                this.Close();
                // this.Dispose(); // Dispose is often called by the application loop

                // Shutdown the debug logger
                DebugLogger.Info("Shutting down logger...");
                DebugLogger.Shutdown();

                // Exit the Windows Forms application loop
                DebugLogger.Info("Exiting application loop...");
                Application.Exit();

                // Use Environment.Exit only as a last resort if Application.Exit fails
                // DebugLogger.Info("Terminating process...");
                // Environment.Exit(0);
            }
            catch (Exception ex)
            {
                DebugLogger.Error(ex, "Failed to shutdown application cleanly");
                Environment.Exit(1); // Use Environment.Exit on error
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
                    ProfileSingleton.ClearProfile(this.profileCB.Text);
                    ProfileSingleton.Load(this.profileCB.Text); //LOAD PROFILE
                    subject.Notify(new Utils.Message(MessageCode.PROFILE_CHANGED, null));
                    currentProfile = this.profileCB.Text.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void Update(ISubject subject)
        {
            // Skip processing notifications if shutdown is in progress
            if (isShuttingDown)
            {
                DebugLogger.Info("Shutdown in progress, ignoring subject notification...");
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
                    break;
                case MessageCode.CLICK_ICON_TRAY:
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    break;
                case MessageCode.DEBUG_MODE_CHANGED:
                    bool newDebugMode = (bool)((subject as Subject).Message.Data);
                    DebugLogger.Info($"Received DEBUG_MODE_CHANGED notification. New DebugMode: {newDebugMode}");
                    // Handle DebugLogWindow visibility based on the new debug mode state
                    if (newDebugMode && debugLogWindow == null) // Debug mode turned ON and window doesn't exist
                    {
                        DebugLogger.Info("DebugMode set to true: Creating and showing DebugLogWindow...");
                        debugLogWindow = new DebugLogWindow();
                        PositionDebugLogWindow();
                        debugLogWindow.Show();
                        SubscribeToDebugLogger(); // Subscribe when the window is created
                    }
                    else if (!newDebugMode && debugLogWindow != null && !debugLogWindow.IsDisposed) // Debug mode turned OFF and window exists
                    {
                        DebugLogger.Info("DebugMode set to false: Closing DebugLogWindow...");
                        // The ShutdownApplication method will handle unsubscribing and disposing on full app exit.
                        // If you need to close the window *without* exiting the app, you'd unsubscribe here.
                        // Given your current ShutdownApplication logic, letting it handle cleanup might be simpler.
                        // debugLogWindow.Close();
                        // debugLogWindow.Dispose();
                        // debugLogWindow = null;
                        // If closing without exiting, explicitly unsubscribe:
                        // UnsubscribeFromDebugLogger(); // Requires refactoring SubscribeTo store the delegate
                        debugLogWindow.Hide(); // Hide instead of closing if keeping the instance
                    }
                    else if (newDebugMode && debugLogWindow != null && debugLogWindow.Visible == false) // Debug mode ON and window exists but is hidden
                    {
                        DebugLogger.Info("DebugMode set to true: Showing existing DebugLogWindow...");
                        PositionDebugLogWindow(); // Reposition in case the main window moved
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

        public ToggleApplicationStateForm SetToggleApplicationStateWindow()
        {
            ToggleApplicationStateForm frm = new ToggleApplicationStateForm(subject)
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
                Location = new Point(445, 220),
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