using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Utils; // Assumes optimized FormUtils
using _4RTools.Model; // Assumes ProfileSingleton, EquipConfig, MessageCode etc. exist
using System.Windows.Input; // For Key enum

namespace _4RTools.Forms
{
    public partial class ATKDEFForm : Form, IObserver
    {
        // Constants are fine
        public static int TOTAL_ATKDEF_LANES = 2;
        public static int TOTAL_EQUIPS = 6;

        // Subject reference
        private readonly Subject _subject; // Keep a reference if needed beyond constructor

        // Control Cache (Example - Adapt names to your actual controls)
        // Best practice: Initialize these after InitializeComponent() based on actual control names/structure
        // If not using member fields, use a Dictionary<string, Control> populated once.
        private Dictionary<int, GroupBox> _equipGroupCache = new Dictionary<int, GroupBox>();
        private Dictionary<string, Control> _controlCache = new Dictionary<string, Control>();


        public ATKDEFForm(Subject subject)
        {
            // Argument validation
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            this._subject = subject;

            InitializeComponent(); // Essential: Initializes controls from the designer

            // Populate control caches AFTER InitializeComponent
            CacheControls();

            SetupInputs(); // Setup event handlers using optimized method
            subject.Attach(this); // Attach AFTER UI is ready

            // Initial UI update based on the current profile
            UpdateUi();
        }

        // Populate the control cache - Call this ONCE after InitializeComponent
        private void CacheControls()
        {
            try
            {
                // Cache GroupBoxes
                for (int i = 1; i <= TOTAL_ATKDEF_LANES; i++)
                {
                    string groupName = "equipGroup" + i;
                    Control[] found = this.Controls.Find(groupName, true);
                    if (found.Length > 0 && found[0] is GroupBox gb)
                    {
                        _equipGroupCache[i] = gb;
                        // Cache controls within the group box for faster access in UpdatePanelData
                        CacheChildControls(gb, i);
                    }
                    else
                    {
                        DebugLogger.Error($"Control Cache Error: GroupBox '{groupName}' not found or not a GroupBox.");
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error during control caching: {ex.Message} - Stack: {ex.StackTrace}");
            }
        }

        private void CacheChildControls(Control parent, int laneId)
        {
            // Cache specific named controls within each lane's groupbox
            string[] controlSuffixes = {
                "SpammerKey", "SpammerDelay", "SwitchDelay", "SpammerClick"
            };

            foreach (var suffix in controlSuffixes)
            {
                string controlName = $"in{laneId}{suffix}";
                Control[] found = parent.Controls.Find(controlName, true); // Search within the parent group
                if (found.Length > 0)
                {
                    _controlCache[controlName] = found[0];
                }
                else
                {
                    DebugLogger.Info($"Control Cache Info: Control '{controlName}' not found in GroupBox for lane {laneId}.");
                }
            }

            for (int i = 1; i <= TOTAL_EQUIPS; i++)
            {
                string[] equipSuffixes = { $"Def{i}", $"Atk{i}" };
                foreach (var suffix in equipSuffixes)
                {
                    string controlName = $"in{laneId}{suffix}";
                    Control[] found = parent.Controls.Find(controlName, true); // Search within the parent group
                    if (found.Length > 0)
                    {
                        _controlCache[controlName] = found[0];
                    }
                    else
                    {
                        DebugLogger.Info($"Control Cache Info: Control '{controlName}' not found in GroupBox for lane {laneId}.");
                    }
                }
            }
        }


        public void Update(ISubject subject)
        {
            // Use the class field _subject or cast the parameter
            var subj = subject as Subject;
            if (subj == null) return; // Ignore updates from unexpected subjects

            switch (subj.Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    UpdateUi();
                    break;
                case MessageCode.TURN_ON:
                    // Assuming GetCurrent() is fast enough, else cache profile locally
                    ProfileSingleton.GetCurrent().AtkDefMode?.Start(); // Use null-conditional
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AtkDefMode?.Stop(); // Use null-conditional
                    break;
            }
        }

        private void UpdatePanelData(int id)
        {
            // Get GroupBox from cache
            if (!_equipGroupCache.TryGetValue(id, out GroupBox group))
            {
                DebugLogger.Error($"UpdatePanelData Error: GroupBox for ID {id} not found in cache.");
                return;
            }

            group.SuspendLayout(); // Suspend layout for this group
            try
            {
                // Cache profile and config access
                Profile currentProfile = ProfileSingleton.GetCurrent();
                if (currentProfile?.AtkDefMode == null)
                {
                    DebugLogger.Error($"UpdatePanelData Error: Current profile or AtkDefMode is null.");
                    return;
                }

                // Optimize config finding/creation
                // Using List.Find is okay for small lists, for large lists consider Dictionary<int, EquipConfig>
                EquipConfig equipConfig = currentProfile.AtkDefMode.EquipConfigs.Find(config => config.id == id);

                // Create and add if not found
                if (equipConfig == null)
                {
                    equipConfig = new EquipConfig(id, Key.None);
                    currentProfile.AtkDefMode.EquipConfigs.Add(equipConfig);
                    // Consider if SetConfiguration is needed here - depends on its implementation
                    // If it just saves, maybe defer saving?
                    ProfileSingleton.SetConfiguration(currentProfile.AtkDefMode);
                }

                // --- No need for FormUtils.ResetForm(group) if we set all values below ---

                // Update controls using the CACHED controls map
                // Spammer Key TextBox
                if (_controlCache.TryGetValue($"in{id}SpammerKey", out Control cKey) && cKey is TextBox txtSpammerKey)
                {
                    txtSpammerKey.Text = equipConfig.KeySpammer.ToString();
                }

                // Spammer Delay NumericUpDown
                if (_controlCache.TryGetValue($"in{id}SpammerDelay", out Control cSpammerDelay) && cSpammerDelay is NumericUpDown nudSpammerDelay)
                {
                    // Ensure value is within bounds before setting
                    decimal value = equipConfig.AHKDelay;
                    if (value >= nudSpammerDelay.Minimum && value <= nudSpammerDelay.Maximum)
                        nudSpammerDelay.Value = value;
                    else
                        DebugLogger.Error($"Value {value} out of range for {nudSpammerDelay.Name}");
                }

                // Switch Delay NumericUpDown
                if (_controlCache.TryGetValue($"in{id}SwitchDelay", out Control cSwitchDelay) && cSwitchDelay is NumericUpDown nudSwitchDelay)
                {
                    decimal value = equipConfig.SwitchDelay;
                    if (value >= nudSwitchDelay.Minimum && value <= nudSwitchDelay.Maximum)
                        nudSwitchDelay.Value = value;
                    else
                        DebugLogger.Error($"Value {value} out of range for {nudSwitchDelay.Name}");
                }

                // Spammer Click CheckBox
                if (_controlCache.TryGetValue($"in{id}SpammerClick", out Control cSpammerClick) && cSpammerClick is CheckBox chkSpammerClick)
                {
                    chkSpammerClick.Checked = equipConfig.KeySpammerWithClick;
                }


                // ATK/DEF Key TextBoxes
                // Avoid creating dictionary copies if only reading
                var atkKeys = equipConfig.AtkKeys; // Direct reference
                var defKeys = equipConfig.DefKeys; // Direct reference

                for (int i = 1; i <= TOTAL_EQUIPS; i++)
                {
                    // DEF Key
                    string defKeyName = $"in{id}Def{i}";
                    if (_controlCache.TryGetValue(defKeyName, out Control cDef) && cDef is TextBox tbDef)
                    {
                        // Use TryGetValue for dictionary lookup
                        tbDef.Text = defKeys.TryGetValue(tbDef.Name, out Key key) ? key.ToString() : Key.None.ToString();
                    }

                    // ATK Key
                    string atkKeyName = $"in{id}Atk{i}";
                    if (_controlCache.TryGetValue(atkKeyName, out Control cAtk) && cAtk is TextBox tbAtk)
                    {
                        tbAtk.Text = atkKeys.TryGetValue(tbAtk.Name, out Key key) ? key.ToString() : Key.None.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception properly!
                DebugLogger.Error($"Error updating panel data for ID {id}: {ex.Message} - Stack: {ex.StackTrace}");
            }
            finally
            {
                group.ResumeLayout(true); // Resume layout for this group
            }
        }

        private void UpdateUi()
        {
            this.SuspendLayout(); // Suspend layout for the entire form
            try
            {
                for (int i = 1; i <= TOTAL_ATKDEF_LANES; i++)
                {
                    UpdatePanelData(i); // Calls the optimized version
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error during UpdateUi: {ex.Message} - Stack: {ex.StackTrace}");
            }
            finally
            {
                this.ResumeLayout(true); // Resume layout for the entire form
            }
        }

        // --- Event Handlers ---

        private void onDelayChange(object sender, EventArgs e)
        {
            // Use 'as' for safe casting
            NumericUpDown delayInput = sender as NumericUpDown;
            if (delayInput == null || delayInput.Tag == null) return; // Basic validation

            try
            {
                string[] inputTag = delayInput.Tag.ToString().Split(':'); // Simpler split
                if (inputTag.Length < 2) return; // Invalid tag format

                if (!int.TryParse(inputTag[0], out int id)) return; // Use TryParse
                string type = inputTag[1];

                // Cache profile/config access
                Profile currentProfile = ProfileSingleton.GetCurrent();
                EquipConfig equipConfig = currentProfile?.AtkDefMode?.EquipConfigs.Find(config => config.id == id);
                if (equipConfig == null) return; // Config not found

                // Use short for consistency with original, though int is generally fine
                short value = (short)delayInput.Value; // Direct cast from decimal might lose precision? Check bounds
                try
                {
                    // Safer conversion if Value can exceed short.MaxValue
                    value = Convert.ToInt16(delayInput.Value);
                }
                catch (OverflowException)
                {
                    value = delayInput.Value > 0 ? short.MaxValue : short.MinValue;
                    delayInput.Value = value; // Correct UI if value was out of range for short
                    DebugLogger.Error($"NumericUpDown value {delayInput.Value} overflowed Int16 for {delayInput.Name}");
                }


                if (type == "spammerDelay")
                {
                    equipConfig.AHKDelay = value;
                }
                else if (type == "switchDelay") // Assuming the other type is switchDelay
                {
                    equipConfig.SwitchDelay = value;
                }

                // PERFORMANCE WARNING: Calling SetConfiguration on every ValueChanged event
                // might be slow if it performs I/O (e.g., saving to disk).
                // Consider debouncing or saving only when necessary.
                ProfileSingleton.SetConfiguration(currentProfile.AtkDefMode);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in onDelayChange for {delayInput?.Name}: {ex.Message} - Stack: {ex.StackTrace}");
            }
        }

        private void onTextChange(object sender, EventArgs e)
        {
            // Use 'as' for safe casting
            TextBox textBox = sender as TextBox;
            if (textBox == null || textBox.Tag == null || string.IsNullOrEmpty(textBox.Text)) return; // Basic validation

            try
            {
                // Use TryParse for Enum for safety
                if (!Enum.TryParse<Key>(textBox.Text, out Key key))
                {
                    // Handle invalid key text if necessary, maybe revert or log
                    DebugLogger.Error($"Invalid Key Enum text in {textBox.Name}: {textBox.Text}");
                    return;
                }

                string[] inputTag = textBox.Tag.ToString().Split(':');
                if (inputTag.Length < 2) return;

                if (!int.TryParse(inputTag[0], out int id)) return;
                string type = inputTag[1];

                Profile currentProfile = ProfileSingleton.GetCurrent();
                EquipConfig equipConfig = currentProfile?.AtkDefMode?.EquipConfigs.Find(config => config.id == id);
                if (equipConfig == null) return;

                if (type.Equals("spammerKey", StringComparison.OrdinalIgnoreCase)) // Case-insensitive compare
                {
                    equipConfig.KeySpammer = key;
                }
                else // Assuming ATK/DEF keys
                {
                    // Original logic seemed complex, verify this simpler approach meets needs:
                    // We need to know if it's ATK or DEF based on the *tag* or *name*
                    // Assuming tag format is like "1:Atk1" or "1:Def3"
                    string keyType = type.Length > 0 ? type.Substring(0, Math.Min(3, type.Length)).ToUpperInvariant() : ""; // Get "ATK" or "DEF"

                    if (keyType == "ATK" || keyType == "DEF")
                    {
                        // Use the TextBox Name as the key in the dictionary (as per original UpdatePanelData)
                        string dictKey = textBox.Name;
                        currentProfile.AtkDefMode.AddSwitchItem(id, dictKey, key, keyType); // Assuming this method handles adding/updating correctly
                    }
                    else
                    {
                        DebugLogger.Error($"Unrecognized type in tag for {textBox.Name}: {type}");
                    }

                }

                // PERFORMANCE WARNING: See note in onDelayChange about SetConfiguration.
                ProfileSingleton.SetConfiguration(currentProfile.AtkDefMode);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in onTextChange for {textBox?.Name}: {ex.Message} - Stack: {ex.StackTrace}");
            }
        }

        private void ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null || checkBox.Tag == null) return;

            try
            {
                string[] inputTag = checkBox.Tag.ToString().Split(':');
                if (inputTag.Length < 1) return; // Expecting at least the ID

                if (!int.TryParse(inputTag[0], out int id)) return;

                Profile currentProfile = ProfileSingleton.GetCurrent();
                EquipConfig equipConfig = currentProfile?.AtkDefMode?.EquipConfigs.Find(config => config.id == id);

                if (equipConfig != null)
                {
                    equipConfig.KeySpammerWithClick = checkBox.Checked;
                    // PERFORMANCE WARNING: See note in onDelayChange about SetConfiguration.
                    ProfileSingleton.SetConfiguration(currentProfile.AtkDefMode);
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in ChkBox_CheckedChanged for {checkBox?.Name}: {ex.Message} - Stack: {ex.StackTrace}");
            }
        }

        // Optimized SetupInputs using single traversal
        public void SetupInputs()
        {
            try
            {
                SetupInputsRecursive(this);
            }
            catch (Exception ex)
            {
                // Log errors during setup
                DebugLogger.Error($"Error during SetupInputs: {ex.Message} - Stack: {ex.StackTrace}");
            }
        }

        private void SetupInputsRecursive(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Attach handlers based on type
                if (control is TextBox tb)
                {
                    // Check if handlers are already attached if SetupInputs could be called multiple times
                    // Simple approach: Remove existing before adding (if applicable)
                    // tb.KeyDown -= FormUtils.OnKeyDown; // Example removal
                    tb.KeyDown += FormUtils.OnKeyDown; // Assumes FormUtils.OnKeyDown is optimized
                    tb.KeyPress += FormUtils.OnKeyPress;
                    tb.TextChanged += this.onTextChange;
                }
                else if (control is NumericUpDown nud)
                {
                    nud.ValueChanged += this.onDelayChange;
                }
                else if (control is CheckBox chk)
                {
                    chk.CheckedChanged += this.ChkBox_CheckedChanged;
                }
                // Add other control types if needed

                // Recursively setup children
                if (control.HasChildren)
                {
                    SetupInputsRecursive(control);
                }
            }
        }

    } // End Class ATKDEFForm
} // End Namespace