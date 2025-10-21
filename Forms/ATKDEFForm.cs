using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;

namespace BruteGamingMacros.UI.Forms
{
    public partial class ATKDEFForm : Form, IObserver
    {
        public static int TOTAL_ATKDEF_LANES = 2;
        public static int TOTAL_EQUIPS = 6;
        public ATKDEFForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();
            SetupInputs();
            InitializeResetButtonTooltips();
            FormUtils.ApplyColorToButtons(this, new[] { "btnResetAtkDef1", "btnResetAtkDef2" }, AppConfig.ResetButtonBackColor);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    UpdateUi();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AtkDefMode?.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AtkDefMode?.Stop();
                    break;
            }
        }

        private void UpdatePanelData(int id)
        {
            try
            {
                GroupBox group = (GroupBox)this.Controls.Find("equipGroup" + id, true).FirstOrDefault();
                if (group == null) return;

                ATKDEF currentMode = ProfileSingleton.GetCurrent().AtkDefMode;
                if (currentMode == null) return;

                EquipConfig equipConfig = currentMode.EquipConfigs.FirstOrDefault(config => config.id == id);
                if (equipConfig == null)
                {
                    equipConfig = new EquipConfig(id, Key.None);
                    currentMode.EquipConfigs.Add(equipConfig);
                }

                Control[] cKey = group.Controls.Find("in" + id + "SpammerKey", true);
                if (cKey.Length > 0 && cKey[0] is TextBox txtSpammerKey)
                {
                    txtSpammerKey.Text = equipConfig.KeySpammer.ToString();
                }

                Control[] cSpammerDelay = group.Controls.Find("in" + id + "SpammerDelay", true);
                if (cSpammerDelay.Length > 0 && cSpammerDelay[0] is NumericUpDown numSpammerDelay)
                {
                    numSpammerDelay.Value = Math.Max(numSpammerDelay.Minimum, Math.Min(numSpammerDelay.Maximum, equipConfig.AHKDelay));
                }

                Control[] cSwitchDelay = group.Controls.Find("in" + id + "SwitchDelay", true);
                if (cSwitchDelay.Length > 0 && cSwitchDelay[0] is NumericUpDown numSwitchDelay)
                {
                    numSwitchDelay.Value = Math.Max(numSwitchDelay.Minimum, Math.Min(numSwitchDelay.Maximum, equipConfig.SwitchDelay));
                }

                Control[] cSpammerClick = group.Controls.Find("in" + id + "SpammerClick", true);
                if (cSpammerClick.Length > 0 && cSpammerClick[0] is CheckBox chkSpammerClick)
                {
                    chkSpammerClick.Checked = equipConfig.KeySpammerWithClick;
                }

                for (int i = 1; i <= TOTAL_EQUIPS; i++)
                {
                    Control[] equipDefCtrl = group.Controls.Find("in" + id + "Def" + i, true);
                    if (equipDefCtrl.Length > 0 && equipDefCtrl[0] is TextBox tbDef)
                    {
                        tbDef.Text = equipConfig.DefKeys.ContainsKey(tbDef.Name) ? equipConfig.DefKeys[tbDef.Name].ToString() : Key.None.ToString();
                    }

                    Control[] equipAtkCtrl = group.Controls.Find("in" + id + "Atk" + i, true);
                    if (equipAtkCtrl.Length > 0 && equipAtkCtrl[0] is TextBox tbAtk)
                    {
                        tbAtk.Text = equipConfig.AtkKeys.ContainsKey(tbAtk.Name) ? equipConfig.AtkKeys[tbAtk.Name].ToString() : Key.None.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdatePanelData for ID " + id + ": " + ex.Message);
            }
        }

        private void UpdateUi()
        {
            ATKDEF currentMode = ProfileSingleton.GetCurrent().AtkDefMode;
            if (currentMode == null) return;

            for (int i = 1; i <= currentMode.EquipConfigs.Count && i <= TOTAL_ATKDEF_LANES; i++)
            {
                if (this.Controls.Find("equipGroup" + i, true).FirstOrDefault() != null)
                {
                    UpdatePanelData(i);
                }
            }
        }

        private void onDelayChange(object sender, EventArgs e)
        {
            NumericUpDown delayInput = (NumericUpDown)sender;
            string[] inputTag = delayInput.Tag.ToString().Split(new[] { ":" }, StringSplitOptions.None);
            int id = short.Parse(inputTag[0]);
            string type = inputTag[1];

            ATKDEF currentMode = ProfileSingleton.GetCurrent().AtkDefMode;
            if (currentMode == null) return;
            EquipConfig equipConfig = currentMode.EquipConfigs.FirstOrDefault(config => config.id == id);
            if (equipConfig == null) return;

            if (type == "spammerDelay")
            {
                equipConfig.AHKDelay = decimal.ToInt16(delayInput.Value);
            }
            else
            {
                equipConfig.SwitchDelay = decimal.ToInt16(delayInput.Value);
            }
            ProfileSingleton.SetConfiguration(currentMode);
        }

        private void onTextChange(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Key key = Key.None;
            if (Enum.TryParse<Key>(textBox.Text, true, out Key parsedKey))
            {
                key = parsedKey;
            }
            else if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                key = Key.None;
            }

            string[] inputTag = textBox.Tag.ToString().Split(new[] { ":" }, StringSplitOptions.None);
            int id = short.Parse(inputTag[0]);
            string type = inputTag[1];

            ATKDEF currentMode = ProfileSingleton.GetCurrent().AtkDefMode;
            if (currentMode == null) return;
            EquipConfig equipConfig = currentMode.EquipConfigs.FirstOrDefault(config => config.id == id);
            if (equipConfig == null) return;

            if (type.Equals("spammerKey"))
            {
                equipConfig.KeySpammer = key;
            }
            else
            {
                string itemTypeCategory = type.Substring(0, 3).ToUpper();
                currentMode.AddSwitchItem(id, textBox.Name, key, itemTypeCategory);
            }
            ProfileSingleton.SetConfiguration(currentMode);
        }

        private void ChkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string[] inputTag = checkBox.Tag.ToString().Split(new[] { ":" }, StringSplitOptions.None);
            int id = short.Parse(inputTag[0]);

            ATKDEF currentMode = ProfileSingleton.GetCurrent().AtkDefMode;
            if (currentMode == null) return;
            EquipConfig equipConfig = currentMode.EquipConfigs.FirstOrDefault(config => config.id == id);
            if (equipConfig == null) return;

            equipConfig.KeySpammerWithClick = checkBox.Checked;
            ProfileSingleton.SetConfiguration(currentMode);
        }

        public void SetupInputs()
        {
            try
            {
                Control[] groups = { this.equipGroup1, this.equipGroup2 };
                foreach (Control group in groups)
                {
                    if (group == null) continue;

                    foreach (Control c in FormUtils.GetAll(group, typeof(TextBox)))
                    {
                        TextBox textBox = (TextBox)c;
                        textBox.KeyDown -= FormUtils.OnKeyDown;
                        textBox.KeyPress -= FormUtils.OnKeyPress;
                        textBox.TextChanged -= this.onTextChange;

                        textBox.KeyDown += FormUtils.OnKeyDown;
                        textBox.KeyPress += FormUtils.OnKeyPress;
                        textBox.TextChanged += this.onTextChange;
                    }

                    foreach (Control c in FormUtils.GetAll(group, typeof(NumericUpDown)))
                    {
                        NumericUpDown numericUpDown = (NumericUpDown)c;
                        numericUpDown.ValueChanged -= this.onDelayChange;
                        numericUpDown.ValueChanged += this.onDelayChange;
                    }

                    foreach (Control c in FormUtils.GetAll(group, typeof(CheckBox)))
                    {
                        CheckBox checkBox = (CheckBox)c;
                        checkBox.CheckedChanged -= this.ChkBox_CheckedChanged;
                        checkBox.CheckedChanged += this.ChkBox_CheckedChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SetupInputs: " + ex.Message);
            }
        }

        private void InitializeResetButtonTooltips()
        {
            ToolTip resetTooltip = new ToolTip();
            string tooltipText = "Reset this group to default values";
            if (this.btnResetAtkDef1 != null) resetTooltip.SetToolTip(this.btnResetAtkDef1, tooltipText);
            if (this.btnResetAtkDef2 != null) resetTooltip.SetToolTip(this.btnResetAtkDef2, tooltipText);
        }

        private void btnResetAtkDefGroup_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            int configIdToReset = -1;

            if (clickedButton.Name == "btnResetAtkDef1")
            {
                configIdToReset = 1;
            }
            else if (clickedButton.Name == "btnResetAtkDef2")
            {
                configIdToReset = 2;
            }

            if (configIdToReset == -1) return;

            ATKDEF currentAtkDefMode = ProfileSingleton.GetCurrent().AtkDefMode;
            if (currentAtkDefMode == null) return;

            int groupIndex = currentAtkDefMode.EquipConfigs.FindIndex(ec => ec.id == configIdToReset);
            if (groupIndex == -1)
            {
                Console.WriteLine("Config ID " + configIdToReset + " not found for reset.");
                EquipConfig newDefaultConfig = new EquipConfig(configIdToReset);
                currentAtkDefMode.EquipConfigs.Add(newDefaultConfig); // Add if missing
                                                                      // Find index again after adding, though UpdatePanelData might create it too.
                groupIndex = currentAtkDefMode.EquipConfigs.FindIndex(ec => ec.id == configIdToReset);
                if (groupIndex == -1) return; // Still not found, something is wrong
            }

            EquipConfig defaultConfig = new EquipConfig(configIdToReset);
            currentAtkDefMode.EquipConfigs[groupIndex] = defaultConfig;

            ProfileSingleton.SetConfiguration(currentAtkDefMode);
            UpdatePanelData(configIdToReset);
        }

        private void panelSwitch2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}