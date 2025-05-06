using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using _4RTools.Model;
using _4RTools.Resources._4RTools;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class ToggleApplicationStateForm : Form, IObserver
    {
        private Subject subject;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem menuItemClose;
        private ToolStripMenuItem menuItemToggle;

        private Keys lastKey;

        private bool isApplicationOn = false;

        public ToggleApplicationStateForm() { }

        public ToggleApplicationStateForm(Subject subject)
        {
            InitializeComponent();

            subject.Attach(this);
            this.subject = subject;
            KeyboardHook.Enable();

            Keys initialToggleKey = Keys.None;
            try
            {
                initialToggleKey = (Keys)Enum.Parse(typeof(Keys), ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey);
            }
            catch
            {
            }
            lastKey = initialToggleKey;

            this.txtStatusToggleKey.Text = ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey;
            this.txtStatusToggleKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            this.txtStatusToggleKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtStatusToggleKey.TextChanged += new EventHandler(this.onStatusToggleKeyChange);

            if (lastKey != Keys.None)
            {
                KeyboardHook.AddKeyDown(lastKey, new KeyboardHook.KeyPressed(this.toggleStatus));
            }

            SetVisualState(isApplicationOn);

            InitializeContextualMenu();
        }

        private void InitializeContextualMenu()
        {
            this.contextMenu = new ContextMenuStrip();
            this.menuItemToggle = new ToolStripMenuItem();
            this.menuItemClose = new ToolStripMenuItem();

            // Configure Toggle menu item (dynamic Start/Stop)
            UpdateToggleMenuItem();
            this.menuItemToggle.Click += new EventHandler(this.toggleStatusFromMenu);

            // Configure Close menu item
            this.menuItemClose.Text = "Exit " + AppConfig.Name;
            this.menuItemClose.Image = SystemIcons.Application.ToBitmap(); // Stock close-like icon
            this.menuItemClose.Click += new EventHandler(this.notifyShutdownApplication);

            this.contextMenu.Items.AddRange(new ToolStripItem[] { this.menuItemToggle, this.menuItemClose });

            this.notifyIconTray.ContextMenuStrip = this.contextMenu;
        }

        private void UpdateToggleMenuItem()
        {
            if (isApplicationOn)
            {
                this.menuItemToggle.Text = "Turn Off";
                this.menuItemToggle.Image = SystemIcons.Error.ToBitmap(); // Stock stop-like icon
            }
            else
            {
                this.menuItemToggle.Text = "Turn On";
                this.menuItemToggle.Image = SystemIcons.Information.ToBitmap(); // Stock play-like icon
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    Keys currentToggleKey = Keys.None;
                    try
                    {
                        currentToggleKey = (Keys)Enum.Parse(typeof(Keys), ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey);

                        if (lastKey != Keys.None)
                        {
                            KeyboardHook.RemoveDown(lastKey);
                        }

                        this.txtStatusToggleKey.Text = currentToggleKey.ToString();

                        if (currentToggleKey != Keys.None)
                        {
                            KeyboardHook.AddKeyDown(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));
                        }

                        lastKey = currentToggleKey;

                        isApplicationOn = false;
                        SetVisualState(isApplicationOn);
                        UpdateToggleMenuItem(); // Update menu item after profile change
                    }
                    catch
                    {
                        lastKey = Keys.None;
                        this.txtStatusToggleKey.Text = string.Empty;
                        isApplicationOn = false;
                        SetVisualState(isApplicationOn);
                        UpdateToggleMenuItem(); // Update menu item on error
                    }
                    break;
            }
        }

        private void btnToggleStatusHandler(object sender, EventArgs e) { this.toggleStatus(); }

        private void onStatusToggleKeyChange(object sender, EventArgs e)
        {
            try
            {
                Keys newToggleKey = (Keys)Enum.Parse(typeof(Keys), this.txtStatusToggleKey.Text);

                if (lastKey != Keys.None)
                {
                    KeyboardHook.RemoveDown(lastKey);
                }

                if (newToggleKey != Keys.None)
                {
                    KeyboardHook.AddKeyDown(newToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));

                    ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey = newToggleKey.ToString();
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);

                    lastKey = newToggleKey;

                    isApplicationOn = false;
                    SetVisualState(isApplicationOn);
                    UpdateToggleMenuItem(); // Update menu item after key change
                }
                else
                {
                    this.txtStatusToggleKey.Text = lastKey.ToString();
                    isApplicationOn = false;
                    SetVisualState(isApplicationOn);
                    UpdateToggleMenuItem(); // Update menu item on invalid key
                }
            }
            catch
            {
                this.txtStatusToggleKey.Text = lastKey.ToString();
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                UpdateToggleMenuItem(); // Update menu item on exception
            }
        }

        public bool toggleStatus()
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;

            if (isApplicationOn)
            {
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                UpdateToggleMenuItem(); // Update menu item after toggle

                this.subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));

                this.lblStatusToggle.Text = "Press the key to start!";
                this.lblStatusToggle.ForeColor = Color.FromArgb(148, 155, 164);

                if (prefs.SoundEnabled)
                {
                    new SoundPlayer(ETCResource.Speech_Off).Play();
                }
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client != null)
                {
                    isApplicationOn = true;
                    SetVisualState(isApplicationOn);
                    UpdateToggleMenuItem(); // Update menu item after toggle

                    this.subject.Notify(new Utils.Message(MessageCode.TURN_ON, null));

                    this.lblStatusToggle.Text = "Press the key to stop!";
                    this.lblStatusToggle.ForeColor = Color.FromArgb(120, 120, 120);

                    if (prefs.SoundEnabled)
                    {
                        new SoundPlayer(ETCResource.Speech_On).Play();
                    }
                }
                else
                {
                    this.lblStatusToggle.Text = "Please select a valid Ragnarok Client!";
                    this.lblStatusToggle.ForeColor = Color.Red;
                    isApplicationOn = false;
                    SetVisualState(isApplicationOn);
                    UpdateToggleMenuItem(); // Update menu item on client error
                    return false;
                }
            }

            return true;
        }

        public bool TurnOFF()
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;

            if (isApplicationOn)
            {
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                UpdateToggleMenuItem(); // Update menu item after turning off

                this.subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));
                this.lblStatusToggle.Text = "Press the key to start!";
                this.lblStatusToggle.ForeColor = Color.FromArgb(148, 155, 164);

                if (prefs.SoundEnabled)
                {
                    new SoundPlayer(ETCResource.Speech_Off).Play();
                }
            }

            return true;
        }

        private void SetVisualState(bool on)
        {
            if (on)
            {
                this.btnStatusToggle.BackColor = Color.Transparent;
                this.btnStatusToggle.Image = Icons.toggle_on;
                this.notifyIconTray.Icon = ETCResource.icon4rtools_on;
                this.lblStatusToggle.ForeColor = Color.FromArgb(120, 120, 120);
                this.toolTipStatusToggle.SetToolTip(this.btnStatusToggle, "Click or press hotkey to turn OFF");
            }
            else
            {
                this.btnStatusToggle.BackColor = Color.Transparent;
                this.btnStatusToggle.Image = Icons.toggle_off;
                this.notifyIconTray.Icon = ETCResource.icon4rtools_off;
                this.lblStatusToggle.ForeColor = Color.FromArgb(148, 155, 164);
                this.toolTipStatusToggle.SetToolTip(this.btnStatusToggle, "Click or press hotkey to turn ON");
            }
        }

        private void notifyIconDoubleClick(object sender, MouseEventArgs e)
        {
            this.subject.Notify(new Utils.Message(MessageCode.CLICK_ICON_TRAY, null));
        }

        private void notifyShutdownApplication(object sender, EventArgs e)
        {
            this.subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));
        }

        private void lblStatusToggle_Click(object sender, EventArgs e)
        {
        }

        private void toggleStatusFromMenu(object sender, EventArgs e)
        {
            toggleStatus();
        }
    }
}