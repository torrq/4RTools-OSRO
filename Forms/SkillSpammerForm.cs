using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.UI.Forms
{
    public partial class SkillSpammerForm : Form, IObserver
    {
        private SkillSpammer ahk;
        public SkillSpammerForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();
            FormUtils.SetNumericUpDownMinimumDelays(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_ON:
                    this.ahk.Start();
                    break;
                case MessageCode.TURN_OFF:
                    this.ahk.Stop();

                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            RemoveHandlers();
            FormUtils.ResetCheckboxForm(this);
            SetLegendDefaultValues();
            this.ahk = ProfileSingleton.GetCurrent().SkillSpammer;
            InitializeCheckAsThreeState();

            RadioButton rdAhkMode = (RadioButton)this.skillSpammerModeGroup.Controls[ProfileSingleton.GetCurrent().SkillSpammer.AHKMode];
            if (rdAhkMode != null) { rdAhkMode.Checked = true; }
            ;
            this.txtSpammerDelay.Text = ProfileSingleton.GetCurrent().SkillSpammer.AhkDelay.ToString();
            this.chkNoShift.Checked = ProfileSingleton.GetCurrent().SkillSpammer.NoShift;
            this.chkMouseFlick.Checked = ProfileSingleton.GetCurrent().SkillSpammer.MouseFlick;
            this.DisableControlsIfSpeedBoost();

            Dictionary<string, KeyConfig> ahkClones = new Dictionary<string, KeyConfig>(ProfileSingleton.GetCurrent().SkillSpammer.AhkEntries);

            foreach (KeyValuePair<string, KeyConfig> config in ahkClones)
            {
                ToggleCheckboxByName(config.Key, config.Value.ClickActive);
            }
        }

        private void OnCheckChange(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            bool haveMouseClick = checkbox.CheckState == CheckState.Checked;

            if (checkbox.CheckState == CheckState.Checked || checkbox.CheckState == CheckState.Indeterminate)
            {
                Key key;
                if (checkbox.Tag != null)
                {
                    key = (Key)new KeyConverter().ConvertFromString(checkbox.Tag.ToString());
                }
                else
                {
                    key = (Key)new KeyConverter().ConvertFromString(checkbox.Text);
                }

                this.ahk.AddSkillSpammerEntry(checkbox.Name, new KeyConfig(key, haveMouseClick));
            }
            else
                this.ahk.RemoveSkillSpammerEntry(checkbox.Name);

            ProfileSingleton.SetConfiguration(this.ahk);
        }

        private void TxtSpammerDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // At start, a null value is set, so we avoid it
                if (this.ahk == null)
                {
                    return;
                }

                this.ahk.AhkDelay = Convert.ToInt16(this.txtSpammerDelay.Value);
                ProfileSingleton.SetConfiguration(this.ahk);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in TxtSpammerDelay_TextChanged: {ex}");
            }

        }

        private void ToggleCheckboxByName(string Name, bool state)
        {
            try
            {
                CheckBox checkBox = (CheckBox)this.Controls.Find(Name, true)[0];
                checkBox.CheckState = state ? CheckState.Checked : CheckState.Indeterminate;
                ProfileSingleton.SetConfiguration(this.ahk);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in ToggleCheckboxByName: {ex}");
            }
        }

        private void RemoveHandlers()
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    check.CheckStateChanged -= OnCheckChange;
                }
            this.chkNoShift.CheckedChanged -= new System.EventHandler(this.ChkNoShift_CheckedChanged);
        }


        private void InitializeCheckAsThreeState()
        {
            foreach (Control c in this.Controls)
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    if ((check.Name.Split(new[] { "chk" }, StringSplitOptions.None).Length == 2))
                    {
                        check.ThreeState = true;
                    }
                    ;

                    if (check.Enabled)
                        check.CheckStateChanged += OnCheckChange;
                }
            this.chkNoShift.CheckedChanged += new System.EventHandler(this.ChkNoShift_CheckedChanged);
        }

        private void SetLegendDefaultValues()
        {
            this.cbWithNoClick.ThreeState = true;
            this.cbWithNoClick.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cbWithNoClick.AutoCheck = false;
            this.cbWithClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWithClick.ThreeState = true;
            this.cbWithClick.AutoCheck = false;
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                this.ahk.AHKMode = rb.Name;
                ProfileSingleton.SetConfiguration(this.ahk);
                this.DisableControlsIfSpeedBoost();
            }
        }

        private void ChkMouseFlick_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            this.ahk.MouseFlick = chk.Checked;
            ProfileSingleton.SetConfiguration(this.ahk);
        }

        private void ChkNoShift_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            this.ahk.NoShift = chk.Checked;
            ProfileSingleton.SetConfiguration(this.ahk);
        }

        private void DisableControlsIfSpeedBoost()
        {
            if (this.ahk.AHKMode == SkillSpammer.SPEED_BOOST)
            {
                this.chkMouseFlick.Enabled = false;
                this.chkNoShift.Enabled = false;
            }
            else
            {
                this.chkMouseFlick.Enabled = true;
                this.chkNoShift.Enabled = true;
            }
        }

        private void AHKForm_Load(object sender, EventArgs e) { }

        private void ChkF1_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF2_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF3_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF4_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF5_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF6_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF7_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF8_CheckedChanged(object sender, EventArgs e) { }

        private void ChkF9_CheckedChanged(object sender, EventArgs e) { }

        private void Chk1_CheckedChanged(object sender, EventArgs e) { }

        private void Chk2_CheckedChanged(object sender, EventArgs e) { }

        private void Chk3_CheckedChanged(object sender, EventArgs e) { }

        private void Chk4_CheckedChanged(object sender, EventArgs e) { }

        private void Chk5_CheckedChanged(object sender, EventArgs e) { }

        private void Chk6_CheckedChanged(object sender, EventArgs e) { }

        private void Chk7_CheckedChanged(object sender, EventArgs e) { }

        private void Chk8_CheckedChanged(object sender, EventArgs e) { }

        private void ChkL_CheckedChanged(object sender, EventArgs e) { }

        private void PictureBox1_Click(object sender, EventArgs e) { }

        private void MRWebsiteButton_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.WebsiteMR);
        }

        private void HRWebsiteButton_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.WebsiteHR);
        }

        private void GitHubButton_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.GithubLink);
        }

        private void MRDiscordButton_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.DiscordLinkMR);
        }

        private void HRDiscordButton_Click(object sender, EventArgs e)
        {
            Process.Start(AppConfig.DiscordLinkHR);
        }

    }
}
