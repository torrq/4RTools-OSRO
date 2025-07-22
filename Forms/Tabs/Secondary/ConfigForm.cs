using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Forms
{
    public partial class ConfigForm : Form, IObserver
    {
        private readonly Subject _subject;
        private bool isInitializing = true;

        public ConfigForm(Subject subject)
        {
            Config cfg = ConfigGlobal.GetConfig();

            InitializeComponent();

            // Detach event handler before setting initial state
            this.chkDebugMode.CheckedChanged -= this.chkDebugMode_CheckedChanged;
            this.chkDebugMode.Checked = cfg.DebugMode;
            // Reattach event handler after setting initial state
            this.chkDebugMode.CheckedChanged += this.chkDebugMode_CheckedChanged;

            isInitializing = false; // Initialization complete

            this.ammo1textBox.KeyDown += FormHelper.OnKeyDown;
            this.ammo1textBox.KeyPress += FormHelper.OnKeyPress;
            this.ammo1textBox.TextChanged += this.TextAmmo1_TextChanged;
            this.ammo2textBox.KeyDown += FormHelper.OnKeyDown;
            this.ammo2textBox.KeyPress += FormHelper.OnKeyPress;
            this.ammo2textBox.TextChanged += this.TextAmmo2_TextChanged;
            this.ammoTrigger.KeyDown += FormHelper.OnKeyDown;
            this.ammoTrigger.KeyPress += FormHelper.OnKeyPress;
            this.ammoTrigger.TextChanged += this.TextAmmoTrigger_TextChanged;

            var newListBuff = ProfileSingleton.GetCurrent().UserPreferences.AutoBuffOrder;
            this.skillsListBox.MouseLeave += this.SkillsListBox_MouseLeave;
            this.skillsListBox.MouseDown += this.SkillsListBox_MouseDown;
            this.skillsListBox.DragOver += this.SkillsListBox_DragOver;
            this.skillsListBox.DragDrop += this.SkillsListBox_DragDrop;

            string cityName = _ORTools.Model.Server.GetCitiesFile();

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
                this.ammoTrigger.Text = prefs.AmmoTriggerKey.ToString();

                // Reattach event handlers
                this.chkStopBuffsOnCity.CheckedChanged += ChkStopBuffsOnCity_CheckedChanged;
                this.chkSoundEnabled.CheckedChanged += ChkSoundEnabled_CheckedChanged;
                this.switchAmmoCheckBox.CheckedChanged += SwitchAmmoCheckBox_CheckedChanged;
                this.chkDebugMode.CheckedChanged += chkDebugMode_CheckedChanged; // Reattach DebugMode handler

            }
            catch (Exception ex)
            {
                DebugLogger.Error("Error in UpdateUI: " + ex.Message);
            }
        }

        private void SkillsListBox_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                var profile = ProfileSingleton.GetCurrent();
                var autoBuffSkill = profile.AutobuffSkill;
                var currentBuffMapping = autoBuffSkill.buffMapping;

                if (currentBuffMapping == null || currentBuffMapping.Count == 0)
                {
                    return; // Nothing to process
                }

                var newOrderList = new List<EffectStatusIDs>();
                var newOrderedBuffList = new Dictionary<EffectStatusIDs, Key>();
                var processedBuffIds = new HashSet<EffectStatusIDs>(); // Track processed IDs to avoid duplicates

                foreach (var item in skillsListBox.Items)
                {
                    var buffId = item.ToString().ToEffectStatusId();

                    // Skip if we've already processed this buff ID
                    if (processedBuffIds.Contains(buffId))
                    {
                        continue;
                    }

                    // Check if this buff exists in the current mapping
                    if (currentBuffMapping.TryGetValue(buffId, out Key keyValue))
                    {
                        newOrderList.Add(buffId);
                        newOrderedBuffList.Add(buffId, keyValue);
                        processedBuffIds.Add(buffId);
                    }
                }

                // Only update if we have items to process
                if (newOrderList.Count > 0)
                {
                    // Update configurations
                    profile.UserPreferences.SetAutoBuffOrder(newOrderList);
                    ProfileSingleton.SetConfiguration(profile.UserPreferences);

                    autoBuffSkill.ClearKeyMapping();
                    autoBuffSkill.SetBuffMapping(newOrderedBuffList);
                    ProfileSingleton.SetConfiguration(autoBuffSkill);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in SkillsListBox_MouseLeave: {ex.Message}");
            }
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
                if (txtBox.Text != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text);
                    ProfileSingleton.GetCurrent().UserPreferences.Ammo1Key = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch (ArgumentException ex)
            {
                DebugLogger.Error("Invalid key entered for Ammo1: " + ex.Message);
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Unexpected error in TextAmmo1_TextChanged: " + ex.Message);
            }
        }

        private void TextAmmo2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text);
                    ProfileSingleton.GetCurrent().UserPreferences.Ammo2Key = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch (ArgumentException ex)
            {
                DebugLogger.Error("Invalid key entered for Ammo2: " + ex.Message);
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Unexpected error in TextAmmo2_TextChanged: " + ex.Message);
            }
        }

        private void TextAmmoTrigger_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text != string.Empty)
                {
                    Key key = (Key)Enum.Parse(typeof(Key), txtBox.Text);
                    ProfileSingleton.GetCurrent().UserPreferences.AmmoTriggerKey = key;
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                }
            }
            catch (ArgumentException ex)
            {
                DebugLogger.Error("Invalid key entered for AmmoTrigger: " + ex.Message);
            }
            catch (Exception ex)
            {
                DebugLogger.Error("Unexpected error in TextAmmoTrigger_TextChanged: " + ex.Message);
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void groupSettings_Enter(object sender, EventArgs e)
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
                    if (cfg.DebugMode != newValue)
                    {
                        cfg.DebugMode = newValue; // Update the setting
                        ConfigGlobal.SaveConfig(); // Save the updated config

                        // Notify the DebugLogger about the change in debug mode
                        DebugLogger.UpdateDebugMode(newValue);

                        DebugLogger.Info($"DebugMode changed to {newValue}. Initiating application restart...");
                        _subject.Notify(new Utils.Message(MessageCode.DEBUG_MODE_CHANGED, newValue));
                        DebugLogger.Info("Attempting Application.Restart()...");
                        Application.Restart();
                        Environment.Exit(0);
                    }
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

    }
}