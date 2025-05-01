using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class ConfigForm : Form, IObserver
    {
        private readonly Subject _subject;
        private bool isInitializing = true; // Flag to prevent event firing during initialization

        public ConfigForm(Subject subject)
        {
            Config cfg = ConfigGlobal.GetConfig();

            InitializeComponent();

            // Detach event handler before setting initial state
            this.chkDebugMode.CheckedChanged -= new System.EventHandler(this.chkDebugMode_CheckedChanged);
            this.chkDebugMode.Checked = cfg.DebugMode;
            // Reattach event handler after setting initial state
            this.chkDebugMode.CheckedChanged += new System.EventHandler(this.chkDebugMode_CheckedChanged);

            isInitializing = false; // Initialization complete

            this.ammo1textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.ammo1textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.ammo1textBox.TextChanged += new EventHandler(this.TextAmmo1_TextChanged);
            this.ammo2textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.ammo2textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.ammo2textBox.TextChanged += new EventHandler(this.TextAmmo2_TextChanged);
            this.overweightKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.overweightKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.overweightKey.TextChanged += new EventHandler(this.OverweightKey_TextChanged);

            var newListBuff = ProfileSingleton.GetCurrent().UserPreferences.AutoBuffOrder;
            this.skillsListBox.MouseLeave += new System.EventHandler(this.SkillsListBox_MouseLeave);
            this.skillsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SkillsListBox_MouseDown);
            this.skillsListBox.DragOver += new DragEventHandler(this.SkillsListBox_DragOver);
            this.skillsListBox.DragDrop += new DragEventHandler(this.SkillsListBox_DragDrop);

            string cityName = _4RTools.Model.Server.GetCitiesFile();

            toolTipchkStopBuffsOnCity.SetToolTip(chkStopBuffsOnCity, "Pause when in a city (cities defined in " + cityName + ")");

            _subject = subject;
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                case MessageCode.ADDED_NEW_AUTOBUFF_SKILL:
                case MessageCode.ADDED_NEW_AUTOSWITCH_PETS:
                    UpdateUI(subject);
                    break;
            }
        }

        public void UpdateUI(ISubject subject)
        {
            ConfigProfile prefs = ProfileSingleton.GetCurrent().UserPreferences;
            try
            {
                AutoBuffSkill currentBuffs = (AutoBuffSkill)(subject as Subject).Message.Data ?? ProfileSingleton.GetCurrent().AutobuffSkill;
                var buffsList = currentBuffs.buffMapping.Keys.ToList();
                skillsListBox.Items.Clear();

                foreach (var buff in buffsList)
                {
                    skillsListBox.Items.Add(buff.ToDescriptionString());
                }

                // Temporarily detach to avoid triggering logic during UI update
                this.chkStopBuffsOnCity.CheckedChanged -= ChkStopBuffsOnCity_CheckedChanged;
                this.chkSoundEnabled.CheckedChanged -= ChkSoundEnabled_CheckedChanged;
                this.switchAmmoCheckBox.CheckedChanged -= SwitchAmmoCheckBox_CheckedChanged;
                this.chkDebugMode.CheckedChanged -= chkDebugMode_CheckedChanged; // Detach DebugMode handler as well

                this.chkStopBuffsOnCity.Checked = prefs.StopBuffsCity;
                this.chkSoundEnabled.Checked = prefs.SoundEnabled;
                this.switchAmmoCheckBox.Checked = prefs.SwitchAmmo;
                this.ammo1textBox.Text = prefs.Ammo1Key.ToString();
                this.ammo2textBox.Text = prefs.Ammo2Key.ToString();
                this.overweightKey.Text = prefs.OverweightKey.ToString();

                RadioButton rdOverweightMode = (RadioButton)this.groupOverweight.Controls[ProfileSingleton.GetCurrent().UserPreferences.OverweightMode.ToString()];
                if (rdOverweightMode != null)
                {
                    rdOverweightMode.Checked = true;
                }
                ;

                // Reattach event handlers
                this.chkStopBuffsOnCity.CheckedChanged += ChkStopBuffsOnCity_CheckedChanged;
                this.chkSoundEnabled.CheckedChanged += ChkSoundEnabled_CheckedChanged;
                this.switchAmmoCheckBox.CheckedChanged += SwitchAmmoCheckBox_CheckedChanged;
                this.chkDebugMode.CheckedChanged += chkDebugMode_CheckedChanged; // Reattach DebugMode handler

            }
            catch (Exception)
            {
            }
        }

        private void SkillsListBox_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var autoBuffSkill = ProfileSingleton.GetCurrent().AutobuffSkill;
                var newOrderList = new List<EffectStatusIDs>();
                var orderedBuffList = skillsListBox.Items;
                Dictionary<EffectStatusIDs, Key> currentList = new Dictionary<EffectStatusIDs, Key>(autoBuffSkill.buffMapping);
                Dictionary<EffectStatusIDs, Key> newOrderedBuffList = new Dictionary<EffectStatusIDs, Key>();
                if (currentList.Count > 0)
                {

                    foreach (var buff in orderedBuffList)
                    {
                        var buffId = buff.ToString().ToEffectStatusId();
                        newOrderList.Add(buffId);
                        var findBuff = currentList.FirstOrDefault(t => t.Key == buffId);
                        newOrderedBuffList.Add(findBuff.Key, findBuff.Value);
                    }
                    ProfileSingleton.GetCurrent().UserPreferences.SetAutoBuffOrder(newOrderList);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);

                    ProfileSingleton.GetCurrent().AutobuffSkill.ClearKeyMapping();
                    ProfileSingleton.GetCurrent().AutobuffSkill.SetBuffMapping(newOrderedBuffList);
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffSkill);

                    newOrderedBuffList.Clear();
                }
            }
            catch { }
        }
        private void SkillsListBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.skillsListBox.SelectedItem == null) return;
            this.skillsListBox.DoDragDrop(this.skillsListBox.SelectedItem, DragDropEffects.Move);
        }

        private void SkillsListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void SkillsListBox_DragDrop(object sender, DragEventArgs e)
        {
            Point point = skillsListBox.PointToClient(new Point(e.X, e.Y));
            int index = this.skillsListBox.IndexFromPoint(point);
            if (index < 0) index = this.skillsListBox.Items.Count - 1;
            object data = skillsListBox.SelectedItem;
            this.skillsListBox.Items.Remove(data);
            this.skillsListBox.Items.Insert(index, data);
        }
        private void ChkStopBuffsOnCity_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.StopBuffsCity = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void ChkSoundEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.SoundEnabled = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void SwitchAmmoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.GetCurrent().UserPreferences.SwitchAmmo = chk.Checked;
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
        }

        private void TextAmmo1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.Ammo1Key = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch { }
        }

        private void TextAmmo2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.Ammo2Key = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch { }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void OverweightKey_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.ToString() != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text.ToString());
                    ProfileSingleton.GetCurrent().UserPreferences.OverweightKey = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch { }
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void OverweightMode_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton rb && rb.Checked)
            {
                if (!string.IsNullOrWhiteSpace(rb.Name))
                {
                    ProfileSingleton.GetCurrent().UserPreferences.OverweightMode = rb.Name;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
        }

        private void groupSettings_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing)
                return;

            Config cfg = ConfigGlobal.GetConfig();
            bool newValue = chkDebugMode.Checked;
            bool currentValue = cfg.DebugMode;

            // Only proceed if the checkbox state actually changed from the saved config
            if (newValue != currentValue)
            {
                string action = newValue ? "enable" : "disable";
                string message = $"Restart to {action} debug mode now?";

                // Prompt for restart confirmation
                bool confirmRestart = DialogConfirm.ShowDialog(message, "App Restart Required");

                // Check if the user confirmed the restart
                if (confirmRestart)
                {
                    cfg.DebugMode = newValue; // Update the setting
                    ConfigGlobal.SaveConfig(); // Save the updated config

                    DebugLogger.Info($"DebugMode changed to {newValue}. Initiating application restart...");
                    _subject.Notify(new Utils.Message(MessageCode.DEBUG_MODE_CHANGED, newValue));
                    DebugLogger.Info("Attempting Application.Restart()...");
                    Application.Restart();
                    Environment.Exit(0);
                }
                else // User cancelled restart
                {
                    DebugLogger.Info("User cancelled application restart.");
                    // Revert the checkbox to the original value if the user cancels
                    // Detach temporarily to prevent triggering the event again
                    this.chkDebugMode.CheckedChanged -= chkDebugMode_CheckedChanged;
                    chkDebugMode.Checked = currentValue;
                    this.chkDebugMode.CheckedChanged += chkDebugMode_CheckedChanged;
                }
            }
            else
            {
                // If the checkbox state matches the saved config, do nothing.
                // This handles cases where the event might fire without a logical change.
                // DebugLogger.Info($"DebugMode checkbox checked changed, but value is already {newValue}. No action needed.");
            }
        }


        private void groupGlobalSettings_Enter(object sender, EventArgs e)
        {

        }

        private void toolTip5_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolTip3_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolTipWeight90_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}