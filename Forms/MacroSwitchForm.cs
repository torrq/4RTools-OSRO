using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.UI.Forms
{
    public partial class MacroSwitchForm : Form, IObserver
    {
        public static int TOTAL_MACRO_LANES = 5;

        public MacroSwitchForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();
            string[] resetButtonNames = { "btnResMac1", "btnResMac2", "btnResMac3", "btnResMac4", "btnResMac5" };
            FormUtils.ApplyColorToButtons(this, resetButtonNames, AppConfig.ResetButtonBackColor);
            FormUtils.SetNumericUpDownMinimumDelays(this);
            ConfigureMacroLanes();
            AddCommonResetButtonTooltip();
        }

        private void AddCommonResetButtonTooltip()
        {
            ToolTip tooltip = new ToolTip();
            string tooltipText = "Reset this macro row to default values";
            int totalResetButtons = TOTAL_MACRO_LANES;

            for (int i = 1; i <= totalResetButtons; i++)
            {
                string buttonName = $"btnResMac{i}";
                Control[] foundControls = this.Controls.Find(buttonName, true);

                if (foundControls.Length > 0 && foundControls[0] is Button resetButton)
                {
                    tooltip.SetToolTip(resetButton, tooltipText);
                }
            }
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    UpdateUi();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().MacroSwitch.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().MacroSwitch.Stop();
                    break;
            }
        }

        private void UpdatePanelData(int id)
        {
            try
            {
                GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + id, true)[0];
                ChainConfig exist = ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Find(config => config.id == id);
                if (exist == null)
                {
                    ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Add(new ChainConfig(id, Key.None));
                    exist = ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Find(config => config.id == id);
                }
                ChainConfig chainConfig = exist;

                // Keep track of which control keys were loaded from the profile
                var loadedControlNames = new HashSet<string>();

                // 1: Load existing entries
                foreach (var entry in chainConfig.macroEntries)
                {
                    string cbName = entry.Key;
                    // Only add if the key is not None? Or always add? Let's add always for now.
                    loadedControlNames.Add(cbName); // Mark this control name as having loaded data

                    MacroKey macroKey = entry.Value;

                    Control[] keyControls = group.Controls.Find(cbName, true);
                    Control[] delayControls = group.Controls.Find($"{cbName}delay", true);

                    if (keyControls.Length > 0 && keyControls[0] is TextBox textBox)
                    {
                        if (delayControls.Length > 0 && delayControls[0] is NumericUpDown delayInput)
                        {
                            // Detach handlers before setting values
                            textBox.TextChanged -= this.OnTextChange;
                            delayInput.ValueChanged -= this.OnDelayChange;

                            // Set values from loaded config
                            textBox.Text = macroKey.Key.ToString();
                            int delayValue = Math.Max((int)delayInput.Minimum, Math.Min((int)delayInput.Maximum, macroKey.Delay));
                            // Ensure value is set even if it's the same, to overwrite previous state
                            if (delayInput.Value != delayValue)
                            {
                                delayInput.Value = delayValue;
                            }
                            else
                            {
                                // If the value is the same, force refresh just in case? Optional.
                                // delayInput.Value = delayValue + 1;
                                // delayInput.Value = delayValue;
                            }


                            // Reattach handlers
                            textBox.TextChanged += this.OnTextChange;
                            delayInput.ValueChanged += this.OnDelayChange;
                        }
                    }
                }

                // 2: Reset controls that were NOT loaded (missing entries)
                // Iterate through specific controls we expect (TextBoxes for keys)
                foreach (Control control in group.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        // Check if this textbox's name was NOT in the set of loaded keys
                        if (!loadedControlNames.Contains(textBox.Name))
                        {
                            textBox.TextChanged -= this.OnTextChange;

                            // Reset to default Key.None if not already set
                            if (textBox.Text != Key.None.ToString())
                            {
                                textBox.Text = Key.None.ToString();
                            }

                            textBox.TextChanged += this.OnTextChange;

                            // Also find and reset the corresponding delay NUD
                            Control[] delayControls = group.Controls.Find($"{textBox.Name}delay", true);
                            if (delayControls.Length > 0 && delayControls[0] is NumericUpDown delayInput)
                            {
                                delayInput.ValueChanged -= this.OnDelayChange;

                                // Reset to default delay (use AppConfig or the designer value)
                                // Ensure we set it even if it visually seems correct already
                                decimal defaultDelay = AppConfig.MacroDefaultDelay; // Or read from delayInput.Value if designer default is preferred
                                if (delayInput.Value != defaultDelay)
                                {
                                    delayInput.Value = defaultDelay;
                                }

                                delayInput.ValueChanged += this.OnDelayChange;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                DebugLogger.Error($"Error in UpdatePanelData for ID {id}: {ex.ToString()}");
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            int chainID = short.Parse(textBox.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
            GroupBox group = (GroupBox)this.Controls.Find("chainGroup" + chainID, true)[0];
            ChainConfig chainConfig = ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Find(config => config.id == chainID);

            Key key = Key.None;
            Enum.TryParse(textBox.Text.ToString(), out key);
            NumericUpDown delayInput = (NumericUpDown)group.Controls.Find($"{textBox.Name}delay", true)[0];

            if (!chainConfig.macroEntries.ContainsKey(textBox.Name))
            {
                // Create new entry: Use the default delay from AppConfig initially
                chainConfig.macroEntries[textBox.Name] = new MacroKey(key, AppConfig.MacroDefaultDelay);

                // If a valid key was just entered, immediately update the delay
                // in the new entry from the associated NumericUpDown control.
                if (key != Key.None)
                {
                    chainConfig.macroEntries[textBox.Name].Delay = decimal.ToInt16(delayInput.Value);
                }
            }
            else // Update existing entry
            {
                // --- Reverted Change ---
                // Only update the Key property of the existing entry.
                chainConfig.macroEntries[textBox.Name].Key = key;
            }

            // Update Trigger key if this is the first input
            bool isFirstInput = Regex.IsMatch(textBox.Name, $"in1mac{chainID}");
            if (isFirstInput) { chainConfig.Trigger = key; }

            // Save configuration
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
        }

        private void OnDelayChange(object sender, EventArgs e)
        {
            NumericUpDown delayInput = (NumericUpDown)sender;
            int chainID = short.Parse(delayInput.Parent.Name.Split(new[] { "chainGroup" }, StringSplitOptions.None)[1]);
            ChainConfig chainConfig = ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Find(config => config.id == chainID);

            string cbName = delayInput.Name.Split(new[] { "delay" }, StringSplitOptions.None)[0];
            if (!chainConfig.macroEntries.ContainsKey(cbName))
            {
                chainConfig.macroEntries[cbName] = new MacroKey(Key.None, decimal.ToInt16(delayInput.Value));
            }
            else
            {
                chainConfig.macroEntries[cbName].Delay = decimal.ToInt16(delayInput.Value);
            }

            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
        }

        private void UpdateUi()
        {
            for (int i = 1; i <= TOTAL_MACRO_LANES; i++)
            {
                UpdatePanelData(i);
            }
        }

        private void ConfigureMacroLanes()
        {
            for (int i = 1; i <= TOTAL_MACRO_LANES; i++)
            {
                InitializeLane(i);
            }
        }

        private void InitializeLane(int id)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("chainGroup" + id, true)[0];
                foreach (Control control in p.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        textBox.KeyDown += FormUtils.OnKeyDown;
                        textBox.KeyPress += FormUtils.OnKeyPress;
                        textBox.TextChanged += this.OnTextChange;
                    }

                    if (control is NumericUpDown delayInput)
                    {
                        delayInput.ValueChanged += this.OnDelayChange;
                    }

                    if (control is Button resetButton)
                    {
                        resetButton.Click += this.OnReset;
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in MacroSwitchForm.InitializeLane: {ex}");
            }

        }

        private void OnReset(object sender, EventArgs e)
        {
            Button resetButton = (Button)sender;
            int btnResetID = short.Parse(resetButton.Name.Split(new[] { "btnResMac" }, StringSplitOptions.None)[1]);

            Macro macroSwitch = ProfileSingleton.GetCurrent().MacroSwitch;
            ChainConfig chainConfig = macroSwitch.ChainConfigs.Find(config => config.id == btnResetID);

            if (chainConfig != null)
            {
                // Reset all chain config properties
                chainConfig.Trigger = Key.None;
                chainConfig.Delay = AppConfig.MacroDefaultDelay; // Assuming there's a default delay constant
                chainConfig.macroEntries.Clear();

                // Update the UI
                GroupBox panel = (GroupBox)this.Controls.Find("chainGroup" + btnResetID, true)[0];
                if (panel != null)
                {
                    // Reset all text boxes and numeric up downs
                    foreach (Control c in panel.Controls)
                    {
                        if (c is TextBox textBox)
                        {
                            textBox.Text = Key.None.ToString();
                        }
                        else if (c is NumericUpDown numericUpDown)
                        {
                            numericUpDown.Value = AppConfig.MacroDefaultDelay;
                        }
                    }
                }

                // Save the changes
                ProfileSingleton.SetConfiguration(macroSwitch);
            }
        }
    }
}