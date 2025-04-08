using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using _4RTools.Model;
using _4RTools.Utils;
using System.Collections.Generic;

namespace _4RTools.Forms
{
    public partial class Container : Form, IObserver
    {

        private Subject subject = new Subject();
        private string currentProfile;
        List<ClientDTO> clients = new List<ClientDTO>();
        private ToggleApplicationStateForm frmToggleApplication = new ToggleApplicationStateForm();

        public Container()
        {
            this.subject.Attach(this);

            InitializeComponent();

            this.Text = AppConfig.WindowTitle;

            LocalServerManager.Initialize(); // Will log errors if they occur
            clients.AddRange(LocalServerManager.GetLocalClients()); //Load Local Servers First

            LoadServers(clients);

            //Container Configuration
            this.IsMdiContainer = true;
            SetBackGroundColorOfMDIForm();

            //Paint Children Forms 
            frmToggleApplication = SetToggleApplicationStateWindow();
            SetAutopotWindow();
            SetAutopotYggWindow();
            SetSkillTimerWindow();
            SetCustomButtonsWindow();
            SetAHKWindow();
            SetAutoBuffStatusWindow();
            SetProfileWindow();
            SetAutobuffStuffWindow();
            SetAutobuffSkillWindow();
            SetSongMacroWindow();
            SetATKDEFWindow();
            SetMacroSwitchWindow();
            SetConfigWindow();
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
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(238, 248, 255)), e.Bounds); // #EEF8FF

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
            tabControl.BackColor = Color.FromArgb(238, 248, 255);
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
            ShutdownApplication();
            base.OnClosed(e);
        }

        private void ShutdownApplication()
        {
            KeyboardHook.Disable();
            subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));
            Environment.Exit(0);
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
                    if (currentProfile != null) {
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
                case MessageCode.SHUTDOWN_APPLICATION:
                    this.ShutdownApplication();
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
            CustomButtonForm form = new CustomButtonForm(subject)
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

        public void SetAutobuffStuffWindow()
        {
            StuffAutoBuffForm frm = new StuffAutoBuffForm(subject)
            {
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 65),
                MdiParent = this
            };
            frm.Show();
            Addform(this.tabPageAutobuffStuff, frm);
        }

        public void SetAutobuffSkillWindow()
        {
            SkillAutoBuffForm frm = new SkillAutoBuffForm(subject)
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
