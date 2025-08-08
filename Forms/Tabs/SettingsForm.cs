using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class SettingsForm : Form, IObserver
    {
        private readonly Subject _subject;
        private bool isInitializing = true;

        public SettingsForm(Subject subject)
        {
            Config cfg = ConfigGlobal.GetConfig();

            InitializeComponent();

            // Detach event handler before setting initial state
            this.DebugMode.CheckedChanged -= this.DebugMode_CheckedChanged;
            this.DebugMode.Checked = cfg.DebugMode;
            this.DebugModeShowLog.CheckedChanged -= this.DebugModeShowLog_CheckedChanged;
            this.DebugModeShowLog.Checked = cfg.DebugModeShowLog;
            // Reattach event handler after setting initial state
            this.DebugMode.CheckedChanged += this.DebugMode_CheckedChanged;
            this.DebugModeShowLog.CheckedChanged += this.DebugModeShowLog_CheckedChanged;

            isInitializing = false; // Initialization complete

            var newListBuff = ProfileSingleton.GetCurrent().UserPreferences.AutoBuffOrder;
            this.skillsListBox.MouseDown += this.SkillsListBox_MouseDown;
            this.skillsListBox.DragOver += this.SkillsListBox_DragOver;
            this.skillsListBox.DragDrop += this.SkillsListBox_DragDrop;

            string cityName = _ORTools.Model.Server.GetCitiesFile();

            toolTipchkStopBuffsOnCity.SetToolTip(chkStopBuffsOnCity, "Pause when in a city (cities defined in " + cityName + ")");

            DebugModeShowLog.Enabled = DebugMode.Checked;

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

        public class BuffListItem
        {
            public EffectStatusIDs BuffId { get; set; }
            public string DisplayText { get; set; }

            public BuffListItem(EffectStatusIDs buffId)
            {
                BuffId = buffId;
                DisplayText = buffId.ToDescriptionString();
            }

            public override string ToString()
            {
                return DisplayText;
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
                    skillsListBox.Items.Add(new BuffListItem(buff));
                }

                // Temporarily detach to avoid triggering logic during UI update
                this.chkStopBuffsOnCity.CheckedChanged -= ChkStopBuffsOnCity_CheckedChanged;
                this.chkSoundEnabled.CheckedChanged -= ChkSoundEnabled_CheckedChanged;
                this.DebugMode.CheckedChanged -= DebugMode_CheckedChanged;
                this.DebugModeShowLog.CheckedChanged -= DebugModeShowLog_CheckedChanged;

                this.chkStopBuffsOnCity.Checked = prefs.StopBuffsCity;
                this.chkSoundEnabled.Checked = prefs.SoundEnabled;

                // Reattach event handlers
                this.chkStopBuffsOnCity.CheckedChanged += ChkStopBuffsOnCity_CheckedChanged;
                this.chkSoundEnabled.CheckedChanged += ChkSoundEnabled_CheckedChanged;
                this.DebugMode.CheckedChanged += DebugMode_CheckedChanged;
                this.DebugModeShowLog.CheckedChanged += DebugModeShowLog_CheckedChanged;

            }
            catch (Exception ex)
            {
                DebugLogger.Error("Error in UpdateUI: " + ex.Message);
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

            // Save immediately after successful drag-drop
            SaveBuffOrder();
        }

        private void SaveBuffOrder()
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
                var newOrderedBuffList = new Dictionary<EffectStatusIDs, Keys>();

                foreach (BuffListItem item in skillsListBox.Items)
                {
                    EffectStatusIDs buffId = item.BuffId;

                    // Check if this buff exists in the current mapping and we haven't processed it yet
                    if (currentBuffMapping.TryGetValue(buffId, out Keys keyValue) && !newOrderList.Contains(buffId))
                    {
                        newOrderList.Add(buffId);
                        newOrderedBuffList.Add(buffId, keyValue);
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

                    DebugLogger.Info($"Buff order saved after drag-drop. New order: {string.Join(", ", newOrderList)}");
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error saving buff order: {ex.Message}");
            }
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

        private void DebugMode_CheckedChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing) return;

            DebugModeShowLog.Enabled = DebugMode.Checked;

            Config cfg = ConfigGlobal.GetConfig();
            bool newValue = DebugMode.Checked;
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
                    this.DebugMode.CheckedChanged -= DebugMode_CheckedChanged;
                    DebugMode.Checked = currentValue;
                    this.DebugMode.CheckedChanged += DebugMode_CheckedChanged;
                    DebugModeShowLog.Enabled = DebugMode.Checked;
                }
            }
            else
            {
                // If the checkbox state matches the saved config, do nothing.
                // This handles cases where the event might fire without a logical change.
                // DebugLogger.Info($"DebugMode checkbox checked changed, but value is already {newValue}. No action needed.");
            }

        }

        private void DebugModeShowLog_CheckedChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing) return;

            Config cfg = ConfigGlobal.GetConfig();
            bool newValue = DebugModeShowLog.Checked;
            bool currentValue = cfg.DebugModeShowLog;

            // Only proceed if the checkbox state actually changed from the saved config
            if (newValue != currentValue)
            {
                string action = newValue ? "display" : "hide";
                string message = $"Restart to {action} debug mode log now?";

                // Prompt for restart confirmation
                bool confirmRestart = DialogConfirm.ShowDialog(message, "App Restart Required");

                // Check if the user confirmed the restart
                if (confirmRestart)
                {
                    if (cfg.DebugModeShowLog != newValue)
                    {
                        cfg.DebugModeShowLog = newValue; // Update the setting
                        ConfigGlobal.SaveConfig(); // Save the updated config
                        DebugLogger.Info($"DebugModeShowLog changed to {newValue}. Initiating application restart...");
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
                    this.DebugModeShowLog.CheckedChanged -= DebugModeShowLog_CheckedChanged;
                    DebugModeShowLog.Checked = currentValue;
                    this.DebugModeShowLog.CheckedChanged += DebugModeShowLog_CheckedChanged;
                }
            }
            else
            {
                // If the checkbox state matches the saved config, do nothing.
                // This handles cases where the event might fire without a logical change.
                // DebugLogger.Info($"DebugMode checkbox checked changed, but value is already {newValue}. No action needed.");
            }

        }

    }
}