using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Resources.BruteGaming;
using BruteGamingMacros.Core.Utils;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace BruteGamingMacros.UI.Forms
{
    public partial class ToggleStateForm : Form, IObserver
    {
        private Subject subject;
        private NotificationTrayManager trayManager;
        private Keys lastKey;
        private bool isApplicationOn = false;

        public ToggleStateForm() { }

        public ToggleStateForm(Subject subject)
        {
            InitializeComponent();

            this.subject = subject;
            subject.Attach(this);
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

            trayManager = new NotificationTrayManager(subject, isApplicationOn);
        }

        public NotificationTrayManager GetTrayManager()
        {
            return trayManager;
        }

        public void Update(ISubject subject)
        {
            switch (subject.Message.Code)
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
                        trayManager.UpdateIcon(isApplicationOn);
                    }
                    catch
                    {
                        lastKey = Keys.None;
                        this.txtStatusToggleKey.Text = string.Empty;
                        isApplicationOn = false;
                        SetVisualState(isApplicationOn);
                        trayManager.UpdateIcon(isApplicationOn);
                    }
                    break;

                case MessageCode.TURN_ON:
                case MessageCode.TURN_OFF:
                    if (isApplicationOn != (subject.Message.Code == MessageCode.TURN_ON))
                    {
                        toggleStatus();
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
                    trayManager.UpdateIcon(isApplicationOn);
                }
                else
                {
                    this.txtStatusToggleKey.Text = lastKey.ToString();
                    isApplicationOn = false;
                    SetVisualState(isApplicationOn);
                    trayManager.UpdateIcon(isApplicationOn);
                }
            }
            catch
            {
                this.txtStatusToggleKey.Text = lastKey.ToString();
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                trayManager.UpdateIcon(isApplicationOn);
            }
        }

        public bool toggleStatus()
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;

            if (isApplicationOn)
            {
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                trayManager.UpdateIcon(isApplicationOn);

                this.subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));

                this.lblStatusToggle.Text = "Press the key to start!";
                this.lblStatusToggle.ForeColor = Color.FromArgb(148, 155, 164);

                if (prefs.SoundEnabled)
                {
                    new SoundPlayer(Sounds.toggle_off).Play();
                }
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client != null)
                {
                    isApplicationOn = true;
                    SetVisualState(isApplicationOn);
                    trayManager.UpdateIcon(isApplicationOn);

                    this.subject.Notify(new Utils.Message(MessageCode.TURN_ON, null));

                    this.lblStatusToggle.Text = "Press the key to stop!";
                    this.lblStatusToggle.ForeColor = Color.FromArgb(120, 120, 120);

                    if (prefs.SoundEnabled)
                    {
                        new SoundPlayer(Sounds.toggle_on).Play();
                    }
                }
                else
                {
                    this.lblStatusToggle.Text = "Please select a valid Ragnarok Client!";
                    this.lblStatusToggle.ForeColor = Color.Red;
                    isApplicationOn = false;
                    SetVisualState(isApplicationOn);
                    trayManager.UpdateIcon(isApplicationOn);
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
                trayManager.UpdateIcon(isApplicationOn);

                this.subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));
                this.lblStatusToggle.Text = "Press the key to start!";
                this.lblStatusToggle.ForeColor = Color.FromArgb(148, 155, 164);

                if (prefs.SoundEnabled)
                {
                    new SoundPlayer(Sounds.toggle_off).Play();
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
                this.lblStatusToggle.ForeColor = Color.FromArgb(120, 120, 120);
                this.toolTipStatusToggle.SetToolTip(this.btnStatusToggle, "Click or press hotkey to turn OFF");
            }
            else
            {
                this.btnStatusToggle.BackColor = Color.Transparent;
                this.btnStatusToggle.Image = Icons.toggle_off;
                this.lblStatusToggle.ForeColor = Color.FromArgb(148, 155, 164);
                this.toolTipStatusToggle.SetToolTip(this.btnStatusToggle, "Click or press hotkey to turn ON");
            }
        }

        private void lblStatusToggle_Click(object sender, EventArgs e)
        {
        }
    }
}