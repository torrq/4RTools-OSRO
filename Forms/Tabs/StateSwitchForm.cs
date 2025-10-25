using _ORTools.Model;
using _ORTools.Resources.Media;
using _ORTools.Utils;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class StateSwitchForm : Form, IObserver
    {
        private Subject subject;
        private TrayManager trayManager;
        private Keys lastKey;
        private bool isApplicationOn = false;

        public StateSwitchForm() { }

        public StateSwitchForm(Subject subject)
        {
            InitializeComponent();
            this.Name = "StateSwitchForm";
            this.subject = subject;
            subject.Attach(this);
            KeyboardHook.Enable();

            Config GlobalConfig = ConfigGlobal.GetConfig();

            // Replace the initialization section in your constructor:
            Keys initialToggleKey = Keys.None;
            string toggleKeyString = ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey;

            try
            {
                if (!string.IsNullOrWhiteSpace(toggleKeyString) && toggleKeyString != "None")
                {
                    if (Enum.TryParse<Keys>(toggleKeyString, true, out Keys parsedKey))
                    {
                        initialToggleKey = parsedKey;
                    }
                }
            }
            catch
            {
                initialToggleKey = Keys.None;
            }

            lastKey = initialToggleKey;

            // Show empty text when None, otherwise show the key name
            if (initialToggleKey == Keys.None)
            {
                this.txtStatusToggleKey.Text = string.Empty;
            }
            else
            {
                this.txtStatusToggleKey.Text = initialToggleKey.ToString();
            }

            this.txtStatusToggleKey.KeyDown += new KeyEventHandler(FormHelper.OnKeyDown);
            this.txtStatusToggleKey.KeyPress += new KeyPressEventHandler(FormHelper.OnKeyPress);
            this.txtStatusToggleKey.TextChanged += new EventHandler(this.onStateSwitchKeyChange);

            // Apply appropriate style
            FormHelper.ApplyInputKeyStyle(this.txtStatusToggleKey, initialToggleKey != Keys.None);

            if (lastKey != Keys.None)
            {
                KeyboardHook.AddKeyDown(lastKey, new KeyboardHook.KeyPressed(this.toggleStatus));
            }

            SetVisualState(isApplicationOn);

            if (!GlobalConfig.DisableSystray) {
                trayManager = new TrayManager(subject, isApplicationOn);
            }
        }

        public TrayManager GetTrayManager()
        {
            return trayManager;
        }

        public bool IsApplicationOn()
        {
            return isApplicationOn;
        }

        public void Update(ISubject subject)
        {
            Config GlobalConfig = ConfigGlobal.GetConfig();

            switch (subject.Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    Keys currentToggleKey = Keys.None;
                    string currentToggleKeyString = ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey;

                    try
                    {
                        TextBox textBoxKey = this.txtStatusToggleKey;
                        if (!string.IsNullOrWhiteSpace(currentToggleKeyString) && currentToggleKeyString != "None")
                        {
                            currentToggleKey = (Keys)Enum.Parse(typeof(Keys), currentToggleKeyString);
                            FormHelper.ApplyInputKeyStyle(textBoxKey, true);
                        } else
                        {
                            FormHelper.ApplyInputKeyStyle(textBoxKey, false);
                        }

                        if (lastKey != Keys.None)
                        {
                            KeyboardHook.RemoveDown(lastKey);
                        }

                        // Set TextBox text - empty if None
                        this.txtStatusToggleKey.Text = (currentToggleKey == Keys.None) ? AppConfig.TEXT_NONE : currentToggleKey.ToString();

                        if (currentToggleKey != Keys.None)
                        {
                            KeyboardHook.AddKeyDown(currentToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));
                        }

                        lastKey = currentToggleKey;

                        isApplicationOn = false;
                        SetVisualState(isApplicationOn);
                        if (!GlobalConfig.DisableSystray)
                        {
                            trayManager.UpdateIcon(isApplicationOn);
                        }
                    }
                    catch
                    {
                        lastKey = Keys.None;
                        this.txtStatusToggleKey.Text = string.Empty;
                        isApplicationOn = false;
                        SetVisualState(isApplicationOn);
                        if (!GlobalConfig.DisableSystray)
                        {
                            trayManager.UpdateIcon(isApplicationOn);
                        }
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

        private void btnStateSwitchHandler(object sender, EventArgs e) { this.toggleStatus(); }

        private void onStateSwitchKeyChange(object sender, EventArgs e)
        {
            Config GlobalConfig = ConfigGlobal.GetConfig();

            try
            {
                // Validate input before parsing (same as TransferHelperForm)
                if (string.IsNullOrWhiteSpace(this.txtStatusToggleKey?.Text))
                {
                    // Handle empty case - set to None but don't revert text
                    if (lastKey != Keys.None)
                    {
                        KeyboardHook.RemoveDown(lastKey);
                    }

                    lastKey = Keys.None;
                    ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey = AppConfig.TEXT_NONE;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);

                    FormHelper.ApplyInputKeyStyle(this.txtStatusToggleKey, false);

                    isApplicationOn = false;
                    SetVisualState(isApplicationOn);
                    if (!GlobalConfig.DisableSystray)
                    {
                        trayManager.UpdateIcon(isApplicationOn);
                    }
                    return;
                }

                string keyText = this.txtStatusToggleKey.Text.Trim();

                // Attempt to parse the key (same as TransferHelperForm)
                if (!Enum.TryParse<Keys>(keyText, true, out Keys newToggleKey))
                {
                    // Failed to parse - just return, don't revert text (like TransferHelperForm)
                    return;
                }

                // Remove old key hook
                if (lastKey != Keys.None)
                {
                    KeyboardHook.RemoveDown(lastKey);
                }

                // Apply styling based on key value
                FormHelper.ApplyInputKeyStyle(this.txtStatusToggleKey, newToggleKey != Keys.None);

                // Update the configuration
                if (newToggleKey != Keys.None)
                {
                    KeyboardHook.AddKeyDown(newToggleKey, new KeyboardHook.KeyPressed(this.toggleStatus));
                    ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey = newToggleKey.ToString();
                }
                else
                {
                    ProfileSingleton.GetCurrent().UserPreferences.ToggleStateKey = "None";
                }

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                lastKey = newToggleKey;

                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                if (!GlobalConfig.DisableSystray)
                {
                    trayManager.UpdateIcon(isApplicationOn);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in onStatusToggleKeyChange: {ex.Message}");
                // Don't revert text on error - let user continue editing (like TransferHelperForm)
            }
            finally
            {
                // Always clear focus regardless of success or failure (like TransferHelperForm)
                this.ActiveControl = null;
            }
        }

        public bool toggleStatus()
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            Config GlobalConfig = ConfigGlobal.GetConfig();

            if (isApplicationOn)
            {
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                if (!GlobalConfig.DisableSystray)
                {
                    trayManager.UpdateIcon(isApplicationOn);
                }

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
                    if (!GlobalConfig.DisableSystray)
                    {
                        trayManager.UpdateIcon(isApplicationOn);
                    }

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
                    if (!GlobalConfig.DisableSystray)
                    {
                        trayManager.UpdateIcon(isApplicationOn);
                    }
                    return false;
                }
            }

            return true;
        }

        public bool TurnOFF()
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            Config GlobalConfig = ConfigGlobal.GetConfig();

            if (isApplicationOn)
            {
                isApplicationOn = false;
                SetVisualState(isApplicationOn);
                if (!GlobalConfig.DisableSystray)
                {
                    trayManager.UpdateIcon(isApplicationOn);
                }

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
    }
}