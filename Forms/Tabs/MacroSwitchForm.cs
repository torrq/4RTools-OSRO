using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace _ORTools.Forms
{
    public partial class MacroSwitchForm : Form, IObserver
    {
        public static int TOTAL_MACRO_LANES = 5;

        public MacroSwitchForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();
            string[] resetButtonNames = { "btnResMac1", "btnResMac2", "btnResMac3", "btnResMac4", "btnResMac5" };
            FormHelper.ApplyColorToButtons(this, resetButtonNames, AppConfig.ResetButtonBackColor);
            FormHelper.SetNumericUpDownMinimumDelays(this);
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
                ChainConfig chainConfig = ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs
                    .Find(config => config.id == id);

                if (chainConfig == null)
                {
                    chainConfig = new ChainConfig(id);
                    ProfileSingleton.GetCurrent().MacroSwitch.ChainConfigs.Add(chainConfig);
                }

                for (int step = 0; step < chainConfig.macroEntries.Count; step++)
                {
                    MacroKey macroKey = chainConfig.macroEntries[step];
                    string keyControlName = $"MacroSwitch{id}_{step + 1}";
                    string delayControlName = $"MacroSwitchDelay{id}_{step + 1}";

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

                    if (group.Controls.Find(delayControlName, true).FirstOrDefault() is NumericUpDown delayInput)
                    {
                        delayInput.ValueChanged -= OnDelayChange;
                        delayInput.Value = macroKey.Delay;
                        delayInput.ValueChanged += OnDelayChange;
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in UpdatePanelData for ID {id}: {ex}");
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string[] parts = textBox.Name.Replace("MacroSwitch", "").Split('_');
                int chainID = int.Parse(parts[0]);
                int stepIndex = int.Parse(parts[1]) - 1;

                ChainConfig chainConfig = ProfileSingleton.GetCurrent()
                    .MacroSwitch.ChainConfigs.Find(c => c.id == chainID);

                if (chainConfig == null) return;

                // Make sure macroEntries list is large enough
                while (chainConfig.macroEntries.Count <= stepIndex)
                {
                    chainConfig.macroEntries.Add(new MacroKey(Keys.None, AppConfig.MacroDefaultDelay));
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

                ChainConfig chainConfig = ProfileSingleton.GetCurrent()
                    .MacroSwitch.ChainConfigs.Find(c => c.id == chainID);

                if (chainConfig == null) return;

                chainConfig.macroEntries[stepIndex].Delay = (int)delayInput.Value;

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().MacroSwitch);
            }
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
                        textBox.KeyDown += FormHelper.OnKeyDown;
                        textBox.KeyPress += FormHelper.OnKeyPress;
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

            MacroSwitch macroSwitch = ProfileSingleton.GetCurrent().MacroSwitch;
            ChainConfig chainConfig = macroSwitch.ChainConfigs.Find(config => config.id == btnResetID);

            if (chainConfig != null)
            {
                // Reset all chain config properties

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
                            textBox.Text = Keys.None.ToString();
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