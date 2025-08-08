using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _ORTools.Forms
{
    public partial class SongsForm : Form, IObserver
    {
        public static int TOTAL_MACRO_LANES_FOR_SONGS = 4;
        public SongsForm(Subject subject)
        {
            subject.Attach(this);
            InitializeComponent();
            string[] resetButtonNames = { "btnResMac1", "btnResMac2", "btnResMac3", "btnResMac4" };
            FormHelper.ApplyColorToButtons(this, resetButtonNames, AppConfig.ResetButtonBackColor);
            FormHelper.SetNumericUpDownMinimumDelays(this);
            ConfigureMacroLanes();
            AddCommonResetButtonTooltip();
        }

        private void AddCommonResetButtonTooltip()
        {
            ToolTip tooltip = new ToolTip();
            string tooltipText = "Reset this song row to default values";
            int totalResetButtons = TOTAL_MACRO_LANES_FOR_SONGS;

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
                    UpdateUI();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().SongMacro.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().SongMacro.Stop();
                    break;
            }

        }

        private void UpdatePanelData(int id)
        {
            try
            {
                Macro songMacro = ProfileSingleton.GetCurrent().SongMacro;
                GroupBox p = (GroupBox)this.Controls.Find("panelMacro" + id, true)[0];
                ChainConfig chainConfig = new ChainConfig(songMacro.ChainConfigs[id - 1]);
                FormHelper.ResetForm(p);

                //Update Trigger Macro Value
                Control[] c = p.Controls.Find("inTriggerMacro" + chainConfig.id, true);
                if (c.Length > 0)
                {
                    TextBox textBox = (TextBox)c[0];
                    textBox.Text = chainConfig.Trigger.ToString();
                }

                //Update Dagger Value
                Control[] cDagger = p.Controls.Find("inDaggerMacro" + chainConfig.id, true);
                if (cDagger.Length > 0)
                {
                    TextBox textBox = (TextBox)cDagger[0];
                    textBox.Text = chainConfig.DaggerKey.ToString();
                }

                //Update Instrument Value
                Control[] cInstrument = p.Controls.Find("inInstrumentMacro" + chainConfig.id, true);
                if (cInstrument.Length > 0)
                {
                    TextBox textBox = (TextBox)cInstrument[0];
                    textBox.Text = chainConfig.InstrumentKey.ToString();
                }


                List<string> names = new List<string>(chainConfig.macroEntries.Keys);
                foreach (string cbName in names)
                {
                    Control[] controls = p.Controls.Find(cbName, true);
                    if (controls.Length > 0)
                    {
                        TextBox textBox = (TextBox)controls[0];
                        textBox.Text = chainConfig.macroEntries[cbName].Key.ToString();
                    }
                }

                //Update Delay Macro Value
                Control[] d = p.Controls.Find("delayMac" + chainConfig.id, true);
                if (d.Length > 0)
                {
                    NumericUpDown delayInput = (NumericUpDown)d[0];
                    delayInput.Value = chainConfig.Delay;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in MacroSongForm.UpdatePanelData: {ex}");
            }

        }

        private void OnTextChange(object sender, EventArgs e)
        {
            Macro SongMacro = ProfileSingleton.GetCurrent().SongMacro;
            TextBox textBox = (TextBox)sender;
            Keys key = (Keys)Enum.Parse(typeof(Keys), textBox.Text.ToString());

            if (textBox.Tag != null)
            {
                //Could be Trigger, Dagger or Instrument input
                string[] inputTag = textBox.Tag.ToString().Split(new[] { ":" }, StringSplitOptions.None);
                int macroid = short.Parse(inputTag[0]);
                string type = inputTag[1];
                ChainConfig chainConfig = ProfileSingleton.GetCurrent().SongMacro.ChainConfigs.Find(config => config.id == macroid);

                switch (type)
                {
                    case "Dagger":
                        chainConfig.DaggerKey = key;
                        break;
                    case "Instrument":
                        chainConfig.InstrumentKey = key;
                        break;
                    case "Trigger":
                        chainConfig.Trigger = key;
                        break;
                }
            }
            else
            {
                int macroID = short.Parse(textBox.Name.Split(new[] { "mac" }, StringSplitOptions.None)[1]);
                ChainConfig chainConfig = SongMacro.ChainConfigs.Find(songMacro => songMacro.id == macroID);
                if (chainConfig == null)
                {
                    SongMacro.ChainConfigs.Add(new ChainConfig(macroID, Keys.None));
                    chainConfig = SongMacro.ChainConfigs.Find(songMacro => songMacro.id == macroID);
                }
                chainConfig.macroEntries[textBox.Name] = new MacroKey(key, chainConfig.Delay);
            }

            ProfileSingleton.SetConfiguration(SongMacro);
        }

        private void OnDelayChange(object sender, EventArgs e)
        {
            Macro SongMacro = ProfileSingleton.GetCurrent().SongMacro;
            NumericUpDown delayInput = (NumericUpDown)sender;
            int macroID = short.Parse(delayInput.Name.Split(new[] { "delayMac" }, StringSplitOptions.None)[1]);
            ChainConfig chainConfig = SongMacro.ChainConfigs.Find(songMacro => songMacro.id == macroID);

            chainConfig.Delay = decimal.ToInt16(delayInput.Value);

            List<string> names = new List<string>(chainConfig.macroEntries.Keys);
            foreach (string cbName in names)
            {
                chainConfig.macroEntries[cbName].Delay = chainConfig.Delay;
            }
            ProfileSingleton.SetConfiguration(SongMacro);
        }

        private void OnReset(object sender, EventArgs e)
        {
            Button resetButton = (Button)sender;
            int btnResetID = short.Parse(resetButton.Name.Split(new[] { "btnResMac" }, StringSplitOptions.None)[1]);

            Macro songMacro = ProfileSingleton.GetCurrent().SongMacro;
            ChainConfig chainConfig = songMacro.ChainConfigs.Find(config => config.id == btnResetID);

            if (chainConfig != null)
            {
                // Reset all chain config properties
                chainConfig.Trigger = Keys.None;
                chainConfig.DaggerKey = Keys.None;
                chainConfig.InstrumentKey = Keys.None;
                chainConfig.Delay = AppConfig.MacroDefaultDelay; // Assuming there's a default delay constant
                chainConfig.macroEntries.Clear();

                // Update the UI
                GroupBox panel = (GroupBox)this.Controls.Find("panelMacro" + btnResetID, true)[0];
                if (panel != null)
                {
                    // Reset all text boxes
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
                ProfileSingleton.SetConfiguration(songMacro);
            }
        }


        private void UpdateUI()
        {
            for (int i = 1; i <= TOTAL_MACRO_LANES_FOR_SONGS; i++)
            {
                UpdatePanelData(i);
            }
        }

        private void ConfigureMacroLanes()
        {
            for (int i = 1; i <= TOTAL_MACRO_LANES_FOR_SONGS; i++)
            {
                InitializeLane(i);
            }
        }

        private void InitializeLane(int id)
        {
            try
            {
                GroupBox p = (GroupBox)this.Controls.Find("panelMacro" + id, true)[0];
                foreach (Control c in p.Controls)
                {
                    if (c is TextBox textBox)
                    {
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormHelper.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormHelper.OnKeyPress);
                        textBox.TextChanged += new EventHandler(this.OnTextChange);
                    }

                    if (c is Button resetButton)
                    {
                        resetButton.Click += new EventHandler(this.OnReset);
                    }

                    if (c is NumericUpDown numericUpDown)
                    {
                        numericUpDown.ValueChanged += new EventHandler(this.OnDelayChange);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in MacroSongForm.InitializeLane: {ex}");
            }

        }
    }
}
