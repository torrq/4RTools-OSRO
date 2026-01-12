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

            // Detach event handlers before setting initial state
            this.DebugMode.CheckedChanged -= this.DebugMode_CheckedChanged;
            this.DebugMode.Checked = cfg.DebugMode;
            this.DebugModeShowLog.CheckedChanged -= this.DebugModeShowLog_CheckedChanged;
            this.DebugModeShowLog.Checked = cfg.DebugModeShowLog;
            this.ChkDisableSystray.CheckedChanged -= this.ChkDisableSystray_CheckedChanged;
            this.ChkDisableSystray.Checked = cfg.DisableSystray;
            this.SongRows.ValueChanged -= this.SongRows_ValueChanged;
            this.SongRows.Value = cfg.SongRows;
            this.MacroSwitchRows.ValueChanged -= this.MacroSwitchRows_ValueChanged;
            this.MacroSwitchRows.Value = cfg.MacroSwitchRows;

            // Initialize DefaultToggleStateKey with proper styling
            this.DefaultToggleStateKey.KeyDown += new KeyEventHandler(FormHelper.OnKeyDown);
            this.DefaultToggleStateKey.KeyPress += new KeyPressEventHandler(FormHelper.OnKeyPress);
            this.DefaultToggleStateKey.TextChanged -= this.DefaultToggleStateKey_TextChanged;

            Keys initialKey = Keys.None;
            string keyString = cfg.DefaultToggleStateKey;

            try
            {
                if (!string.IsNullOrWhiteSpace(keyString) && keyString != "None")
                {
                    if (Enum.TryParse<Keys>(keyString, true, out Keys parsedKey))
                    {
                        initialKey = parsedKey;
                    }
                }
            }
            catch
            {
                initialKey = Keys.None;
            }

            // Show "None" when None, otherwise show the key name
            this.DefaultToggleStateKey.Text = initialKey.ToString();

            // Apply appropriate style
            FormHelper.ApplyInputKeyStyle(this.DefaultToggleStateKey, initialKey != Keys.None);

            // Reattach event handlers after setting initial state
            this.DebugMode.CheckedChanged += this.DebugMode_CheckedChanged;
            this.DebugModeShowLog.CheckedChanged += this.DebugModeShowLog_CheckedChanged;
            this.ChkDisableSystray.CheckedChanged += this.ChkDisableSystray_CheckedChanged;
            this.SongRows.ValueChanged += this.SongRows_ValueChanged;
            this.MacroSwitchRows.ValueChanged += this.MacroSwitchRows_ValueChanged;
            this.DefaultToggleStateKey.TextChanged += this.DefaultToggleStateKey_TextChanged;

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
            Config cfg = ConfigGlobal.GetConfig();
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
                this.ChkDisableSystray.CheckedChanged -= ChkDisableSystray_CheckedChanged;
                this.SongRows.ValueChanged -= SongRows_ValueChanged;
                this.MacroSwitchRows.ValueChanged -= MacroSwitchRows_ValueChanged;

                this.chkStopBuffsOnCity.Checked = prefs.StopBuffsCity;
                this.chkSoundEnabled.Checked = prefs.SoundEnabled;
                this.SongRows.Value = cfg.SongRows;
                this.MacroSwitchRows.Value = cfg.MacroSwitchRows;

                // Reattach event handlers
                this.chkStopBuffsOnCity.CheckedChanged += ChkStopBuffsOnCity_CheckedChanged;
                this.chkSoundEnabled.CheckedChanged += ChkSoundEnabled_CheckedChanged;
                this.DebugMode.CheckedChanged += DebugMode_CheckedChanged;
                this.DebugModeShowLog.CheckedChanged += DebugModeShowLog_CheckedChanged;
                this.ChkDisableSystray.CheckedChanged += ChkDisableSystray_CheckedChanged;
                this.SongRows.ValueChanged += SongRows_ValueChanged;
                this.MacroSwitchRows.ValueChanged += MacroSwitchRows_ValueChanged;

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
        }

        private void ChkDisableSystray_CheckedChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing) return;

            Config cfg = ConfigGlobal.GetConfig();
            bool newValue = ChkDisableSystray.Checked;
            bool currentValue = cfg.DisableSystray;

            // Only proceed if the checkbox state actually changed from the saved config
            if (newValue != currentValue)
            {
                string action = newValue ? "disable" : "enable";
                string message = $"Restart to {action} systray icon now?";

                // Prompt for restart confirmation
                bool confirmRestart = DialogConfirm.ShowDialog(message, "App Restart Required");

                // Check if the user confirmed the restart
                if (confirmRestart)
                {
                    if (cfg.DisableSystray != newValue)
                    {
                        cfg.DisableSystray = newValue; // Update the setting
                        ConfigGlobal.SaveConfig(); // Save the updated config
                        DebugLogger.Info($"ChkDisableSystray changed to {newValue}. Initiating application restart...");
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
                    this.ChkDisableSystray.CheckedChanged -= ChkDisableSystray_CheckedChanged;
                    ChkDisableSystray.Checked = currentValue;
                    this.ChkDisableSystray.CheckedChanged += ChkDisableSystray_CheckedChanged;
                }
            }
        }

        private void SongRows_ValueChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing) return;

            Config cfg = ConfigGlobal.GetConfig();
            int newValue = (int)SongRows.Value;
            int currentValue = cfg.SongRows;

            // Only proceed if the value actually changed from the saved config
            if (newValue != currentValue)
            {
                cfg.SongRows = newValue; // Update the setting
                ConfigGlobal.SaveConfig(); // Save the updated config
                DebugLogger.Info($"SongRows changed to {newValue}.");
            }
        }

        private void MacroSwitchRows_ValueChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing) return;

            Config cfg = ConfigGlobal.GetConfig();
            int newValue = (int)MacroSwitchRows.Value;
            int currentValue = cfg.MacroSwitchRows;

            // Only proceed if the value actually changed from the saved config
            if (newValue != currentValue)
            {
                cfg.MacroSwitchRows = newValue; // Update the setting
                ConfigGlobal.SaveConfig(); // Save the updated config
                DebugLogger.Info($"MacroSwitchRows changed to {newValue}.");
            }
        }

        private void DefaultToggleStateKey_TextChanged(object sender, EventArgs e)
        {
            // Prevent this logic from running during form initialization
            if (isInitializing) return;

            Config cfg = ConfigGlobal.GetConfig();

            try
            {
                TextBox textBox = this.DefaultToggleStateKey;
                string currentText = textBox?.Text ?? string.Empty;

                // Validate input before parsing
                if (string.IsNullOrWhiteSpace(currentText))
                {
                    // Handle empty case - shouldn't happen with FormHelper, but just in case
                    if (cfg.DefaultToggleStateKey != Keys.None.ToString())
                    {
                        cfg.DefaultToggleStateKey = Keys.None.ToString();
                        ConfigGlobal.SaveConfig();
                        DebugLogger.Info("DefaultToggleStateKey cleared (set to None).");
                    }

                    FormHelper.ApplyInputKeyStyle(textBox, false);
                    return;
                }

                string keyText = currentText.Trim();

                // Attempt to parse the key
                if (!Enum.TryParse<Keys>(keyText, true, out Keys newKey))
                {
                    // Failed to parse - just return, don't revert text
                    return;
                }

                // Apply styling based on key value
                FormHelper.ApplyInputKeyStyle(textBox, newKey != Keys.None);

                // Update the configuration
                string keyValueToSave = newKey.ToString();

                if (cfg.DefaultToggleStateKey != keyValueToSave)
                {
                    cfg.DefaultToggleStateKey = keyValueToSave;
                    ConfigGlobal.SaveConfig();
                    DebugLogger.Info($"DefaultToggleStateKey changed to {keyValueToSave}.");
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in DefaultToggleStateKey_TextChanged: {ex.Message}");
                // Don't revert text on error - let user continue editing
            }
            finally
            {
                // Always clear focus regardless of success or failure
                this.ActiveControl = null;
            }
        }
    }
}