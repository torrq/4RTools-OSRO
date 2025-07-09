using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Forms
{
    public partial class AutopotHPForm : Form, IObserver
    {
        private AutopotHP autopotHP;

        public AutopotHPForm(Subject subject)
        {
            InitializeComponent();
            subject.Attach(this);

            AttachKeyEvents(txtHPKey1, OnHPKey1Changed);
            AttachKeyEvents(txtHPKey2, OnHPKey2Changed);
            AttachKeyEvents(txtHPKey3, OnHPKey3Changed);
            AttachKeyEvents(txtHPKey4, OnHPKey4Changed);
            AttachKeyEvents(txtHPKey5, OnHPKey5Changed);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopotHP = ProfileSingleton.GetCurrent().AutopotHP;
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    this.autopotHP.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.autopotHP.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            if (autopotHP == null) { return; }

            // HP Keys
            this.txtHPKey1.Text = this.autopotHP.HPKey1.ToString();
            this.txtHPKey2.Text = this.autopotHP.HPKey2.ToString();
            this.txtHPKey3.Text = this.autopotHP.HPKey3.ToString();
            this.txtHPKey4.Text = this.autopotHP.HPKey4.ToString();
            this.txtHPKey5.Text = this.autopotHP.HPKey5.ToString();

            // HP Percentages - using Value property to avoid casting issues
            hpPct1.Value = autopotHP.HPPercent1;
            hpPct2.Value = autopotHP.HPPercent2;
            hpPct3.Value = autopotHP.HPPercent3;
            hpPct4.Value = autopotHP.HPPercent4;
            hpPct5.Value = autopotHP.HPPercent5;

            FormUtils.AttachBlankFix(hpPct1);
            FormUtils.AttachBlankFix(hpPct2);
            FormUtils.AttachBlankFix(hpPct3);
            FormUtils.AttachBlankFix(hpPct4);
            FormUtils.AttachBlankFix(hpPct5);

            // HP Enabled checkboxes
            HPEnabled1.Checked = autopotHP.HPEnabled1;
            HPEnabled2.Checked = autopotHP.HPEnabled2;
            HPEnabled3.Checked = autopotHP.HPEnabled3;
            HPEnabled4.Checked = autopotHP.HPEnabled4;
            HPEnabled5.Checked = autopotHP.HPEnabled5;


            // Other settings
            this.numAutopotDelay.Text = this.autopotHP.Delay.ToString();
            this.chkStopOnCriticalInjury.Checked = this.autopotHP.StopOnCriticalInjury;
        }

        private void AttachKeyEvents(TextBox textBox, EventHandler changeHandler)
        {
            textBox.KeyDown += FormUtils.OnKeyDown;
            textBox.KeyPress += FormUtils.OnKeyPress;
            textBox.TextChanged += changeHandler;
        }

        // HP Key change handlers
        private void OnHPKey1Changed(object sender, EventArgs e)
        {
            UpdateHPKey(txtHPKey1.Text, key => autopotHP.HPKey1 = key);
        }

        private void OnHPKey2Changed(object sender, EventArgs e)
        {
            UpdateHPKey(txtHPKey2.Text, key => autopotHP.HPKey2 = key);
        }

        private void OnHPKey3Changed(object sender, EventArgs e)
        {
            UpdateHPKey(txtHPKey3.Text, key => autopotHP.HPKey3 = key);
        }

        private void OnHPKey4Changed(object sender, EventArgs e)
        {
            UpdateHPKey(txtHPKey4.Text, key => autopotHP.HPKey4 = key);
        }

        private void OnHPKey5Changed(object sender, EventArgs e)
        {
            UpdateHPKey(txtHPKey5.Text, key => autopotHP.HPKey5 = key);
        }

        private void UpdateHPKey(string keyText, Action<Key> setKey)
        {
            try
            {
                Key key = (Key)Enum.Parse(typeof(Key), keyText);
                setKey(key);
                ProfileSingleton.SetConfiguration(autopotHP);
                this.ActiveControl = null;
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error updating HP key: {ex.Message}");
            }
        }

        // HP Percentage change handlers
        private void NumHPPercent1_ValueChanged(object sender, EventArgs e)
        {
            UpdateHPPercent((int)hpPct1.Value, percent => autopotHP.HPPercent1 = percent);
        }

        private void NumHPPercent2_ValueChanged(object sender, EventArgs e)
        {
            UpdateHPPercent((int)hpPct2.Value, percent => autopotHP.HPPercent2 = percent);
        }

        private void NumHPPercent3_ValueChanged(object sender, EventArgs e)
        {
            UpdateHPPercent((int)hpPct3.Value, percent => autopotHP.HPPercent3 = percent);
        }

        private void NumHPPercent4_ValueChanged(object sender, EventArgs e)
        {
            UpdateHPPercent((int)hpPct4.Value, percent => autopotHP.HPPercent4 = percent);
        }

        private void NumHPPercent5_ValueChanged(object sender, EventArgs e)
        {
            UpdateHPPercent((int)hpPct5.Value, percent => autopotHP.HPPercent5 = percent);
        }

        private void UpdateHPPercent(int value, Action<int> setPercent)
        {
            try
            {
                if (autopotHP != null)
                {
                    setPercent(value);
                    ProfileSingleton.SetConfiguration(autopotHP);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error updating HP percentage: {ex.Message}");
            }
        }

        // HP Enabled checkbox handlers
        private void ChkHPEnabled1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHPEnabled(HPEnabled1.Checked, enabled => autopotHP.HPEnabled1 = enabled);
        }

        private void ChkHPEnabled2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHPEnabled(HPEnabled2.Checked, enabled => autopotHP.HPEnabled2 = enabled);
        }

        private void ChkHPEnabled3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHPEnabled(HPEnabled3.Checked, enabled => autopotHP.HPEnabled3 = enabled);
        }

        private void ChkHPEnabled4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHPEnabled(HPEnabled4.Checked, enabled => autopotHP.HPEnabled4 = enabled);
        }

        private void ChkHPEnabled5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHPEnabled(HPEnabled5.Checked, enabled => autopotHP.HPEnabled5 = enabled);
        }

        private void UpdateHPEnabled(bool enabled, Action<bool> setEnabled)
        {
            try
            {
                if (autopotHP != null)
                {
                    setEnabled(enabled);
                    ProfileSingleton.SetConfiguration(autopotHP);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error updating HP enabled state: {ex.Message}");
            }
        }

        // Other control handlers
        private void NumDelay_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (autopotHP != null)
                {
                    autopotHP.Delay = (int)numAutopotDelay.Value;
                    ProfileSingleton.SetConfiguration(autopotHP);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error updating delay: {ex.Message}");
            }
        }

        private void ChkStopOnCriticalInjury_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (autopotHP != null)
                {
                    autopotHP.StopOnCriticalInjury = chkStopOnCriticalInjury.Checked;
                    ProfileSingleton.SetConfiguration(autopotHP);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error updating critical injury setting: {ex.Message}");
            }
        }

        private void AutopotHPForm_Load(object sender, EventArgs e)
        {
            // Form load event - can be used for additional initialization if needed
        }
    }
}