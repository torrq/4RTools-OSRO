using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Forms
{
    public partial class MacroSwitchForm : Form, IObserver
    {
        private List<GroupBox> dynamicGroups = new List<GroupBox>();

        public MacroSwitchForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();

            // Hide the template group - we'll use it as a template only
            templateGroup.Visible = false;

            CreateDynamicRows();

            // Dynamically generate reset button names based on TOTAL_MACRO_LANES
            string[] resetButtonNames = Enumerable.Range(1, MacroSwitchKey.TOTAL_MACRO_LANES)
                .Select(i => $"btnResetMac{i}")
                .ToArray();
            FormHelper.ApplyColorToButtons(this, resetButtonNames, AppConfig.ResetButtonBackColor);
            FormHelper.SetNumericUpDownMinimumDelays(this);
            ConfigureMacroLanes();
            AddCommonResetButtonTooltip();
        }

        private void CreateDynamicRows()
        {
            for (int i = 1; i <= MacroSwitchKey.TOTAL_MACRO_LANES; i++)
            {
                GroupBox newGroup = CloneGroupBox(templateGroup, i);
                newGroup.Location = new Point(templateGroup.Location.X, templateGroup.Location.Y + (i - 1) * 100);
                newGroup.Text = $"Switch {i}";
                newGroup.Visible = true;

                this.Controls.Add(newGroup);
                dynamicGroups.Add(newGroup);
            }

            // Adjust form height to accommodate all rows
            this.Height = Math.Max(this.Height, templateGroup.Location.Y + (MacroSwitchKey.TOTAL_MACRO_LANES * 100) + 50);
        }

        private GroupBox CloneGroupBox(GroupBox original, int id)
        {
            GroupBox clone = new GroupBox();
            clone.Size = original.Size;
            clone.Text = original.Text;
            clone.FlatStyle = original.FlatStyle;
            clone.Name = $"chainGroup{id}";

            foreach (Control control in original.Controls)
            {
                Control clonedControl = CloneControl(control, id);
                if (clonedControl != null)
                {
                    clone.Controls.Add(clonedControl);
                }
            }

            return clone;
        }

        private Control CloneControl(Control original, int id)
        {
            Control clone = null;

            if (original is TextBox textBox)
            {
                clone = new TextBox();
                ((TextBox)clone).BorderStyle = textBox.BorderStyle;
                ((TextBox)clone).TextAlign = textBox.TextAlign;
                ((TextBox)clone).Font = textBox.Font;

                // Update name from Template to actual ID
                string newName = textBox.Name.Replace("Template", id.ToString());
                clone.Name = newName;
            }
            else if (original is NumericUpDown numericUpDown)
            {
                clone = new NumericUpDown();
                ((NumericUpDown)clone).BorderStyle = numericUpDown.BorderStyle;
                ((NumericUpDown)clone).TextAlign = numericUpDown.TextAlign;
                ((NumericUpDown)clone).Font = numericUpDown.Font;
                ((NumericUpDown)clone).Maximum = numericUpDown.Maximum;
                ((NumericUpDown)clone).Minimum = numericUpDown.Minimum;

                // Update name from Template to actual ID
                string newName = numericUpDown.Name.Replace("Template", id.ToString());
                clone.Name = newName;
            }
            else if (original is CheckBox checkBox)
            {
                clone = new CheckBox();
                ((CheckBox)clone).AutoSize = checkBox.AutoSize;
                ((CheckBox)clone).UseVisualStyleBackColor = checkBox.UseVisualStyleBackColor;

                // Update name from Template to actual ID
                string newName = checkBox.Name.Replace("Template", id.ToString());
                clone.Name = newName;
            }
            else if (original is Button button)
            {
                clone = new Button();
                ((Button)clone).FlatStyle = button.FlatStyle;
                ((Button)clone).ForeColor = button.ForeColor;
                ((Button)clone).UseVisualStyleBackColor = button.UseVisualStyleBackColor;
                ((Button)clone).Cursor = button.Cursor;
                clone.Text = button.Text;
                // Update name from Template to actual ID
                string newName = button.Name.Replace("Template", $"Mac{id}");
                clone.Name = newName;
            }
            else if (original is PictureBox pictureBox)
            {
                clone = new PictureBox();
                ((PictureBox)clone).Image = pictureBox.Image;
                ((PictureBox)clone).SizeMode = pictureBox.SizeMode;
                ((PictureBox)clone).TabStop = pictureBox.TabStop;
                clone.Name = $"{pictureBox.Name}_Group{id}";
            }
            else if (original is Label label)
            {
                clone = new Label();
                clone.Text = label.Text;
                ((Label)clone).AutoSize = label.AutoSize;
                clone.Name = $"{label.Name}_Group{id}";
            }

            if (clone != null)
            {
                clone.Location = original.Location;
                clone.Size = original.Size;
                clone.TabIndex = original.TabIndex;
            }

            return clone;
        }

        private void AddCommonResetButtonTooltip()
        {
            ToolTip tooltip = new ToolTip();
            string tooltipText = "Reset this macro row to default values";

            for (int i = 1; i <= MacroSwitchKey.TOTAL_MACRO_LANES; i++)
            {
                string buttonName = $"btnResetMac{i}";
                Control[] foundControls = this.Controls.Find(buttonName, true);

                if (foundControls.Length > 0 && foundControls[0] is Button resetButton)
                {
                    tooltip.SetToolTip(resetButton, tooltipText);
                }
                else
                {
                    DebugLogger.Warning($"Reset button '{buttonName}' not found.");
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
                MacroSwitchChainConfig chainConfig = ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs
                    .Find(config => config.id == id);

                if (chainConfig == null)
                {
                    chainConfig = new MacroSwitchChainConfig(id);
                    ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Add(chainConfig);
                }

                // Update trigger key
                string triggerControlName = $"MacroSwitchTrigger{id}";
                if (group.Controls.Find(triggerControlName, true).FirstOrDefault() is TextBox triggerTextBox)
                {
                    triggerTextBox.TextChanged -= OnTriggerTextChange;
                    triggerTextBox.Text = chainConfig.TriggerKey.ToString();
                    triggerTextBox.TextChanged += OnTriggerTextChange;

                    FormHelper.ApplyInputKeyStyle(
                        triggerTextBox,
                        !string.IsNullOrWhiteSpace(triggerTextBox.Text) && triggerTextBox.Text != AppConfig.TEXT_NONE
                    );
                }

                // Update macro keys and their properties
                for (int step = 0; step < chainConfig.macroEntries.Count; step++)
                {
                    MacroSwitchKey macroKey = chainConfig.macroEntries[step];
                    string keyControlName = $"MacroSwitch{id}_{step + 1}";
                    string delayControlName = $"MacroSwitchDelay{id}_{step + 1}";
                    string clickControlName = $"MacroSwitchClick{id}_{step + 1}";

                    // Update key textbox
                    if (group.Controls.Find(keyControlName, true).FirstOrDefault() is TextBox textBox)
                    {
                        textBox.TextChanged -= OnTextChange;
                        textBox.Text = macroKey.Key.ToString();
                        textBox.TextChanged += OnTextChange;

                        FormHelper.ApplyInputKeyStyle(
                            textBox,
                            !string.IsNullOrWhiteSpace(textBox.Text) && textBox.Text != AppConfig.TEXT_NONE
                        );
                    }

                    // Update delay numeric
                    if (group.Controls.Find(delayControlName, true).FirstOrDefault() is NumericUpDown delayInput)
                    {
                        delayInput.ValueChanged -= OnDelayChange;
                        delayInput.Value = macroKey.Delay;
                        delayInput.ValueChanged += OnDelayChange;
                    }

                    // Update click checkbox
                    if (group.Controls.Find(clickControlName, true).FirstOrDefault() is CheckBox clickCheckBox)
                    {
                        clickCheckBox.CheckedChanged -= OnClickModeChange;
                        clickCheckBox.Checked = macroKey.ClickMode == 1;
                        clickCheckBox.CheckedChanged += OnClickModeChange;
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in UpdatePanelData for ID {id}: {ex}");
            }
        }

        private void OnTriggerTextChange(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string triggerName = textBox.Name.Replace("MacroSwitchTrigger", "");
                int chainID = int.Parse(triggerName);

                MacroSwitchChainConfig chainConfig = ProfileSingleton.GetCurrent()
                    .MacroSwitch.ChainConfigs.Find(c => c.id == chainID);

                if (chainConfig == null) return;

                if (Enum.TryParse(textBox.Text, out Keys key))
                {
                    chainConfig.TriggerKey = key;
                }
                else
                {
                    chainConfig.TriggerKey = Keys.None;
                }

                FormHelper.ApplyInputKeyStyle(
                    textBox,
                    chainConfig.TriggerKey != Keys.None
                );

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string[] parts = textBox.Name.Replace("MacroSwitch", "").Split('_');
                int chainID = int.Parse(parts[0]);
                int stepIndex = int.Parse(parts[1]) - 1;

                MacroSwitchChainConfig chainConfig = ProfileSingleton.GetCurrent()
                    .MacroSwitch.ChainConfigs.Find(c => c.id == chainID);

                if (chainConfig == null) return;

                // Make sure macroEntries list is large enough
                while (chainConfig.macroEntries.Count <= stepIndex)
                {
                    chainConfig.macroEntries.Add(new MacroSwitchKey(Keys.None, AppConfig.MacroDefaultDelay));
                }

                if (Enum.TryParse(textBox.Text, out Keys key))
                {
                    chainConfig.macroEntries[stepIndex].Key = key;
                }
                else
                {
                    chainConfig.macroEntries[stepIndex].Key = Keys.None;
                }

                FormHelper.ApplyInputKeyStyle(
                    textBox,
                    chainConfig.macroEntries[stepIndex].Key != Keys.None
                );

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
            }
        }

        private void OnDelayChange(object sender, EventArgs e)
        {
            if (sender is NumericUpDown delayInput)
            {
                string keyControlName = delayInput.Name.Replace("MacroSwitchDelay", "MacroSwitch");
                string[] parts = keyControlName.Replace("MacroSwitch", "").Split('_');
                int chainID = int.Parse(parts[0]);
                int stepIndex = int.Parse(parts[1]) - 1;

                MacroSwitchChainConfig chainConfig = ProfileSingleton.GetCurrent()
                    .MacroSwitch.ChainConfigs.Find(c => c.id == chainID);

                if (chainConfig == null) return;

                // Make sure macroEntries list is large enough
                while (chainConfig.macroEntries.Count <= stepIndex)
                {
                    chainConfig.macroEntries.Add(new MacroSwitchKey(Keys.None, AppConfig.MacroDefaultDelay));
                }

                chainConfig.macroEntries[stepIndex].Delay = (int)delayInput.Value;

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
            }
        }

        private void OnClickModeChange(object sender, EventArgs e)
        {
            if (sender is CheckBox clickCheckBox)
            {
                string[] parts = clickCheckBox.Name.Replace("MacroSwitchClick", "").Split('_');
                int chainID = int.Parse(parts[0]);
                int stepIndex = int.Parse(parts[1]) - 1;

                MacroSwitchChainConfig chainConfig = ProfileSingleton.GetCurrent()
                    .MacroSwitch.ChainConfigs.Find(c => c.id == chainID);

                if (chainConfig == null) return;

                // Make sure macroEntries list is large enough
                while (chainConfig.macroEntries.Count <= stepIndex)
                {
                    chainConfig.macroEntries.Add(new MacroSwitchKey(Keys.None, AppConfig.MacroDefaultDelay));
                }

                chainConfig.macroEntries[stepIndex].ClickMode = clickCheckBox.Checked ? 1 : 0;

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
            }
        }

        private void UpdateUi()
        {
            for (int i = 1; i <= MacroSwitchKey.TOTAL_MACRO_LANES; i++)
            {
                UpdatePanelData(i);
            }
        }

        private void ConfigureMacroLanes()
        {
            for (int i = 1; i <= MacroSwitchKey.TOTAL_MACRO_LANES; i++)
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
                        textBox.KeyDown += FormHelper.OnKeyDown;
                        textBox.KeyPress += FormHelper.OnKeyPress;

                        // Hook up appropriate event handlers based on control name
                        if (textBox.Name.Contains("MacroSwitchTrigger"))
                        {
                            textBox.TextChanged += this.OnTriggerTextChange;
                        }
                        else
                        {
                            textBox.TextChanged += this.OnTextChange;
                        }
                    }

                    if (control is NumericUpDown delayInput)
                    {
                        delayInput.ValueChanged += this.OnDelayChange;
                    }

                    if (control is CheckBox clickCheckBox)
                    {
                        clickCheckBox.CheckedChanged += this.OnClickModeChange;
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
            int btnResetID = short.Parse(resetButton.Name.Split(new[] { "btnResetMac" }, StringSplitOptions.None)[1]);

            MacroSwitch macroSwitch = ProfileSingleton.GetCurrent().MacroSwitch;
            MacroSwitchChainConfig chainConfig = macroSwitch.ChainConfigs.Find(config => config.id == btnResetID);

            if (chainConfig != null)
            {
                // Reset all chain config properties
                chainConfig.macroEntries.Clear();
                chainConfig.TriggerKey = Keys.None; // Reset trigger key

                // Update the UI
                GroupBox panel = (GroupBox)this.Controls.Find("chainGroup" + btnResetID, true)[0];
                if (panel != null)
                {
                    // Reset all controls
                    foreach (Control c in panel.Controls)
                    {
                        if (c is TextBox textBox)
                        {
                            textBox.Text = Keys.None.ToString();
                        }
                        else if (c is NumericUpDown numericUpDown)
                        {
                            numericUpDown.Value = AppConfig.MacroDefaultDelay;
                        }
                        else if (c is CheckBox checkBox)
                        {
                            checkBox.Checked = false;
                        }
                    }
                }

                // Save the changes
                ProfileSingleton.SetConfiguration(macroSwitch);
            }
        }
    }
}