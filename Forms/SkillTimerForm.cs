using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;
namespace BruteGamingMacros.UI.Forms
{
    public partial class SkillTimerForm : Form, IObserver
    {
        public static int TOTAL_SKILL_TIMER = 4;
        public SkillTimerForm(Subject subject)
        {
            InitializeComponent();
            //FormUtils.SetNumericUpDownMinimumDelays(this);
            subject.Attach(this);
            ConfigureTimerLanes();
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    FormValidate();
                    UpdateUI();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().SkillTimer.Start();
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().SkillTimer.Stop();
                    break;
            }
        }

        private void FormValidate()
        {
            for (int i = 1; i <= TOTAL_SKILL_TIMER; i++)
            {
                ValidateAllSkillTimer(i);
            }
        }

        private void UpdateUI()
        {
            for (int i = 1; i <= TOTAL_SKILL_TIMER; i++)
            {
                UpdatePanelData(i);
            }
        }

        private void ConfigureTimerLanes()
        {
            for (int i = 1; i <= TOTAL_SKILL_TIMER; i++)
            {
                InitializeLane(i);
            }
        }

        private void ValidateAllSkillTimer(int id)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;

                if (!Spammers.skillTimer.ContainsKey(id))
                {
                    Spammers.skillTimer.Add(id, new MacroKey(Key.None, AppConfig.SkillTimerDefaultDelay));

                    Control[] c = this.Controls.Find("txtSkillTimerKey" + id, true);
                    if (c.Length > 0)
                    {
                        TextBox keyTextBox = (TextBox)c[0];
                        keyTextBox.Text = Spammers.skillTimer[id].Key.ToString();
                    }

                    //Update Delay Macro Value
                    Control[] d = this.Controls.Find("txtAutoRefreshDelay" + id, true);
                    if (d.Length > 0)
                    {
                        NumericUpDown delayInput = (NumericUpDown)d[0];
                        delayInput.Value = Spammers.skillTimer[id].Delay;
                    }
                }

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().SkillTimer);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Exception in ValidateAllSkillTimer: {ex}");
            }
        }

        private void InitializeLane(int id)
        {
            try
            {

                TextBox textBox = (TextBox)this.Controls.Find("txtSkillTimerKey" + id, true)[0];
                textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                textBox.TextChanged += new EventHandler(this.OnTextChange);

                NumericUpDown txtAutoRefreshDelay = (NumericUpDown)this.Controls.Find("txtAutoRefreshDelay" + id, true)[0];
                txtAutoRefreshDelay.ValueChanged += new EventHandler(this.TxtAutoRefreshDelayTextChanged);

            }
            catch (Exception ex)
            {
                // Log error if controls not found
                Console.WriteLine($"Error initializing SkillTimer panel {id}: {ex.Message}");
            }
        }

        private void UpdatePanelData(int id)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;

                MacroKey skillTimer = Spammers.skillTimer[id];

                //Update Trigger Macro Value
                Control[] c = this.Controls.Find("txtSkillTimerKey" + id, true);
                if (c.Length > 0)
                {
                    TextBox keyTextBox = (TextBox)c[0];
                    keyTextBox.Text = skillTimer.Key.ToString();
                }

                //Update Delay Macro Value
                Control[] d = this.Controls.Find("txtAutoRefreshDelay" + id, true);
                if (d.Length > 0)
                {
                    NumericUpDown delayInput = (NumericUpDown)d[0];
                    delayInput.Value = skillTimer.Delay;
                }
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }

        private void OnTextChange(object sender, EventArgs e)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;
                TextBox textBox = (TextBox)sender;
                Key key = (Key)Enum.Parse(typeof(Key), textBox.Text.ToString());

                var id = int.Parse(textBox.Name[textBox.Name.Length - 1].ToString());

                if (Spammers.skillTimer.ContainsKey(id))
                {
                    MacroKey skillTimer = Spammers.skillTimer[id];
                    skillTimer.Key = key;
                }
                else
                {
                    Spammers.skillTimer.Add(id, new MacroKey(Key.None, AppConfig.SkillTimerDefaultDelay));
                }

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().SkillTimer);
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }


        private void TxtAutoRefreshDelayTextChanged(object sender, EventArgs e)
        {
            try
            {
                SkillTimer Spammers = ProfileSingleton.GetCurrent().SkillTimer;
                NumericUpDown numericUpDown = (NumericUpDown)sender;
                int delay = (int)numericUpDown.Value;

                var id = int.Parse(numericUpDown.Name[numericUpDown.Name.Length - 1].ToString());

                if (Spammers.skillTimer.ContainsKey(id))
                {
                    var skillTimer = Spammers.skillTimer[id];
                    skillTimer.Delay = delay;
                }
                else
                {
                    Spammers.skillTimer.Add(id, new MacroKey(Key.None, AppConfig.SkillTimerDefaultDelay));
                }

                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().SkillTimer);
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
        }

    }
}
