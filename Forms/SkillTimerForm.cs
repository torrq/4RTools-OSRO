using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;
namespace _4RTools.Forms
{
    public partial class SkillTimerForm : Form, IObserver
    {
        private readonly ToggleStateForm frmToggleApplication;

        public SkillTimerForm(Subject subject, ToggleStateForm toggleStateForm)
        {
            InitializeComponent();
            subject.Attach(this);
            ConfigureTimerLanes();
            this.frmToggleApplication = toggleStateForm;
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    FormValidate();
                    UpdateUI();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().SkillTimer.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().SkillTimer.Stop();
                    break;
            }
        }

        private void FormValidate()
        {
            for (int i = 1; i <= SkillTimer.MAX_SKILL_TIMERS; i++)
            {
                ValidateAllSkillTimer(i);
            }
        }

        private void UpdateUI()
        {
            for (int i = 1; i <= SkillTimer.MAX_SKILL_TIMERS; i++)
            {
                UpdatePanelData(i);
            }
        }

        private void ConfigureTimerLanes()
        {
            for (int i = 1; i <= SkillTimer.MAX_SKILL_TIMERS; i++)
            {
                InitializeLane(i);
            }
        }

        private void ValidateAllSkillTimer(int id)
        {
            try
            {
                SkillTimer spammers = ProfileSingleton.GetCurrent().SkillTimer;

                if (!spammers.skillTimer.ContainsKey(id))
                {
                    spammers.skillTimer.Add(id, new MacroKey(Key.None, AppConfig.SkillTimerDefaultDelay));
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in ValidateAllSkillTimer: {ex}");
            }
        }


        private void InitializeLane(int id)
        {
            try
            {
                TextBox textBox = (TextBox)Controls.Find("txtSkillTimerKey" + id, true)[0];
                textBox.KeyDown += FormUtils.OnKeyDown;
                textBox.KeyPress += FormUtils.OnKeyPress;
                textBox.TextChanged += OnTextChange;

                NumericUpDown txtAutoRefreshDelay = (NumericUpDown)Controls.Find("txtAutoRefreshDelay" + id, true)[0];
                txtAutoRefreshDelay.ValueChanged += TxtAutoRefreshDelayTextChanged;

                CheckBox enabledCheckbox = (CheckBox)Controls.Find($"SkillTimerEnabled{id}", true)[0];
                enabledCheckbox.CheckedChanged += SkillTimerEnabled_CheckedChanged;

                // Wire up the new radio buttons to a single handler
                for (int i = 1; i <= 3; i++)
                {
                    RadioButton rb = (RadioButton)Controls.Find($"SkillTimerClick{id}_{i}", true)[0];
                    rb.CheckedChanged += SkillTimerClickMode_CheckedChanged;
                }

                // Wire up the AltKey checkbox
                CheckBox altKeyCheckbox = (CheckBox)Controls.Find($"SkillTimerAltKey{id}", true)[0];
                altKeyCheckbox.CheckedChanged += SkillTimerAltKey_CheckedChanged;

                // Set up custom appearance for AltKey checkbox
                altKeyCheckbox.Appearance = Appearance.Button;
                altKeyCheckbox.FlatStyle = FlatStyle.Flat;
                altKeyCheckbox.FlatAppearance.BorderSize = 0;
                altKeyCheckbox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                altKeyCheckbox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                altKeyCheckbox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                altKeyCheckbox.BackColor = System.Drawing.Color.Transparent;
                altKeyCheckbox.Image = global::_4RTools.Resources._4RTools.Icons.key_alt_off;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Failed to initialize lane {id}: {ex.Message}");
            }
        }

        private void UpdatePanelData(int id)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;
                if (!Spammers.skillTimer.ContainsKey(id)) return;
                MacroKey skillTimer = Spammers.skillTimer[id];

                Control[] enabledControls = Controls.Find($"SkillTimerEnabled{id}", true);
                if (enabledControls.Length > 0)
                {
                    CheckBox enabledCheckbox = (CheckBox)enabledControls[0];
                    enabledCheckbox.CheckedChanged -= SkillTimerEnabled_CheckedChanged;
                    enabledCheckbox.Checked = skillTimer.Enabled;
                    enabledCheckbox.CheckedChanged += SkillTimerEnabled_CheckedChanged;
                }

                //Update Trigger Macro Value
                Control[] c = Controls.Find("txtSkillTimerKey" + id, true);
                if (c.Length > 0)
                {
                    ((TextBox)c[0]).Text = skillTimer.Key.ToString();
                }

                //Update Delay Macro Value
                Control[] d = Controls.Find("txtAutoRefreshDelay" + id, true);
                if (d.Length > 0)
                {
                    ((NumericUpDown)d[0]).Value = skillTimer.Delay;
                }

                // Update Click Mode RadioButtons by detaching handlers, setting value, and re-attaching
                for (int i = 1; i <= 3; i++)
                {
                    RadioButton rb = (RadioButton)Controls.Find($"SkillTimerClick{id}_{i}", true)[0];
                    rb.CheckedChanged -= SkillTimerClickMode_CheckedChanged;
                }

                // ClickMode is 0, 1, 2. Radio buttons are _1, _2, _3.
                int radioIndex = skillTimer.ClickMode + 1;
                RadioButton selectedRb = (RadioButton)Controls.Find($"SkillTimerClick{id}_{radioIndex}", true)[0];
                selectedRb.Checked = true;

                for (int i = 1; i <= 3; i++)
                {
                    RadioButton rb = (RadioButton)Controls.Find($"SkillTimerClick{id}_{i}", true)[0];
                    rb.CheckedChanged += SkillTimerClickMode_CheckedChanged;
                }

                // Update AltKey checkbox
                Control[] altKeyControls = Controls.Find($"SkillTimerAltKey{id}", true);
                if (altKeyControls.Length > 0)
                {
                    CheckBox altKeyCheckbox = (CheckBox)altKeyControls[0];
                    altKeyCheckbox.CheckedChanged -= SkillTimerAltKey_CheckedChanged;
                    altKeyCheckbox.Checked = skillTimer.AltKey;
                    altKeyCheckbox.Image = skillTimer.AltKey ?
                        global::_4RTools.Resources._4RTools.Icons.key_alt :
                        global::_4RTools.Resources._4RTools.Icons.key_alt_off;
                    altKeyCheckbox.CheckedChanged += SkillTimerAltKey_CheckedChanged;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Failed to update panel data for SkillTimer {id}: {ex.Message}");
            }
        }

        private void SkillTimerAltKey_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                try
                {
                    // Parse the control name to get the lane ID
                    // e.g., "SkillTimerAltKey1" -> ID=1
                    string controlName = checkBox.Name;
                    int id = ExtractIdFromControlName(controlName, "SkillTimerAltKey", SkillTimer.MAX_SKILL_TIMERS);
                    if (id == -1) return;

                    SkillTimer skillTimer = ProfileSingleton.GetCurrent().SkillTimer;

                    if (skillTimer.skillTimer.ContainsKey(id))
                    {
                        skillTimer.skillTimer[id].AltKey = checkBox.Checked;
                        ProfileSingleton.SetConfiguration(skillTimer);
                    }

                    // Update the image based on checked state
                    checkBox.Image = checkBox.Checked ?
                        global::_4RTools.Resources._4RTools.Icons.key_alt :
                        global::_4RTools.Resources._4RTools.Icons.key_alt_off;
                }
                catch (Exception ex)
                {
                    DebugLogger.Error($"SkillTimerAltKey_CheckedChanged failed: {ex}");
                }
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;
                TextBox textBox = (TextBox)sender;
                Key key = (Key)Enum.Parse(typeof(Key), textBox.Text.ToString());
                var id = ExtractIdFromControlName(textBox.Name, "txtSkillTimerKey", SkillTimer.MAX_SKILL_TIMERS);
                if (id == -1) return;

                if (Spammers.skillTimer.ContainsKey(id))
                {
                    MacroKey skillTimer = Spammers.skillTimer[id];
                    skillTimer.Key = key;
                }
                else
                {
                    Spammers.skillTimer.Add(id, new MacroKey(Key.None, AppConfig.SkillTimerDefaultDelay));
                }

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().SkillTimer);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"OnTextChange failed: {ex}");
            }
        }

        public static int ExtractIdFromControlName(string controlName, string prefix, int maxId)
        {
            if (controlName.StartsWith(prefix))
            {
                string idStr = controlName.Substring(prefix.Length);
                if (int.TryParse(idStr, out int id) && id >= 1 && id <= maxId)
                {
                    return id;
                }
            }

            return -1; // invalid or out of range
        }

        private void SkillTimerEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                try
                {
                    string controlName = checkBox.Name; // e.g., "SkillTimerEnabled3"
                    int id = ExtractIdFromControlName(controlName, "SkillTimerEnabled", SkillTimer.MAX_SKILL_TIMERS);
                    if (id == -1) return;

                    SkillTimer skillTimer = ProfileSingleton.GetCurrent().SkillTimer;
                    if (skillTimer.skillTimer.ContainsKey(id))
                    {
                        skillTimer.skillTimer[id].Enabled = checkBox.Checked;
                        ProfileSingleton.SetConfiguration(skillTimer);

                        // Start or stop the individual timer based on checkbox state
                        if (checkBox.Checked && frmToggleApplication.IsApplicationOn())
                        {
                            skillTimer.StartTimer(id);
                        }
                        else
                        {
                            skillTimer.StopTimer(id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugLogger.Error($"SkillTimerEnabled_CheckedChanged failed: {ex}");
                }
            }
        }

        private void SkillTimerClickMode_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked) // Process only when a radio button becomes checked
            {
                try
                {
                    // Parse the control name to get the lane ID and the mode
                    // e.g., "SkillTimerClick1_2" -> ID=1, Mode=2
                    string[] nameParts = radioButton.Name.Split('_');
                    int id = ExtractIdFromControlName(nameParts[0], "SkillTimerClick", SkillTimer.MAX_SKILL_TIMERS);
                    if (id == -1) return;

                    int mode = int.Parse(nameParts[1]);

                    // Convert radio button index (1, 2, 3) to data value (0, 1, 2)
                    int clickModeValue = mode - 1;

                    SkillTimer skillTimer = ProfileSingleton.GetCurrent().SkillTimer;

                    if (skillTimer.skillTimer.ContainsKey(id))
                    {
                        skillTimer.skillTimer[id].ClickMode = clickModeValue;
                        ProfileSingleton.SetConfiguration(skillTimer);
                    }
                }
                catch (Exception ex)
                {
                    DebugLogger.Error($"SkillTimerClickMode_CheckedChanged failed: {ex}");
                }
            }
        }

        private void TxtAutoRefreshDelayTextChanged(object sender, EventArgs e)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;
                NumericUpDown numericUpDown = (NumericUpDown)sender;
                int delay = (int)numericUpDown.Value;
                int id = ExtractIdFromControlName(numericUpDown.Name, "txtAutoRefreshDelay", SkillTimer.MAX_SKILL_TIMERS);
                if (id == -1) return;

                if (Spammers.skillTimer.ContainsKey(id))
                {
                    var skillTimer = Spammers.skillTimer[id];
                    skillTimer.Delay = delay;
                }
                else
                {
                    Spammers.skillTimer.Add(id, new MacroKey(Key.None, AppConfig.SkillTimerDefaultDelay));
                }

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().SkillTimer);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"TxtAutoRefreshDelayTextChanged failed: {ex}");
            }
        }

        private void SkillTimerForm_Load(object sender, EventArgs e)
        {

        }


    }
}