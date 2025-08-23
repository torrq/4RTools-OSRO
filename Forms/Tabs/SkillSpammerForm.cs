using _ORTools.Controls;
using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class SkillSpammerForm : Form, IObserver
    {
        private SkillSpammer skillSpammer;

        public SkillSpammerForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();
            FormHelper.SetNumericUpDownMinimumDelays(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_ON:
                    this.skillSpammer.Start();
                    break;
                case MessageCode.TURN_OFF:
                    this.skillSpammer.Stop();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            RemoveHandlers();
            FormHelper.ResetCheckboxForm(this);
            SetLegendDefaultValues();

            this.skillSpammer = ProfileSingleton.GetCurrent().SkillSpammer;
            InitializeCheckAsThreeState();

            this.txtSpammerDelay.Text = ProfileSingleton.GetCurrent().SkillSpammer.SpammerDelay.ToString();

            Dictionary<string, KeyConfig> spammerClones = new Dictionary<string, KeyConfig>(ProfileSingleton.GetCurrent().SkillSpammer.SpammerEntries);

            foreach (KeyValuePair<string, KeyConfig> config in spammerClones)
            {
                BorderedCheckBox checkBox = this.Controls.Find(config.Key, true).OfType<BorderedCheckBox>().FirstOrDefault();
                if (checkBox != null)
                {
                    checkBox.CheckState = config.Value.IsIndeterminate ? CheckState.Indeterminate : (config.Value.ClickActive ? CheckState.Checked : CheckState.Unchecked);
                }
            }
        }

        private void OnCheckChange(object sender, EventArgs e)
        {
            BorderedCheckBox checkbox = sender as BorderedCheckBox;
            if (checkbox == null) return;

            bool haveMouseClick = checkbox.CheckState == CheckState.Checked;
            bool isIndeterminate = checkbox.CheckState == CheckState.Indeterminate;

            Keys key;
            if (checkbox.Tag != null)
            {
                key = (Keys)Enum.Parse(typeof(Keys), checkbox.Tag.ToString(), true);
            }
            else
            {
                key = (Keys)Enum.Parse(typeof(Keys), checkbox.Text, true);
            }

            if (checkbox.CheckState == CheckState.Checked || checkbox.CheckState == CheckState.Indeterminate)
            {
                this.skillSpammer.AddSkillSpammerEntry(checkbox.Name, new KeyConfig(key, haveMouseClick, isIndeterminate));
            }
            else
            {
                this.skillSpammer.RemoveSkillSpammerEntry(checkbox.Name);
            }

            ProfileSingleton.SetConfiguration(this.skillSpammer);
        }

        private void TxtSpammerDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.skillSpammer == null)
                {
                    return;
                }

                this.skillSpammer.SpammerDelay = Convert.ToInt16(this.txtSpammerDelay.Value);
                ProfileSingleton.SetConfiguration(this.skillSpammer);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in TxtSpammerDelay_TextChanged: {ex}");
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

                    if (check.Enabled)
                        check.CheckStateChanged += OnCheckChange;
                }
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

        // Event handlers for individual checkboxes (can be removed if not needed)
        private void SkillSpammerForm_Load(object sender, EventArgs e) { }

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