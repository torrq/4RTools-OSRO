using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Forms
{
    public partial class AutopotSPForm : Form, IObserver
    {
        private AutopotSP autopot;

        public AutopotSPForm(Subject subject)
        {
            InitializeComponent();
            FormUtils.SetNumericUpDownMinimumDelays(this);
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopot = ProfileSingleton.GetCurrent().AutopotSP;
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    this.autopot.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.autopot.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            this.spKey1.Text = this.autopot.SPKey1.ToString();
            this.spPct1.Text = this.autopot.SPPercent1.ToString();
            this.AutopotSPDelay.Text = this.autopot.Delay.ToString();
            RadioButton rdHealFirst = (RadioButton)this.Controls[ProfileSingleton.GetCurrent().Autopot.FirstHeal];
            if (rdHealFirst != null) { rdHealFirst.Checked = true; }
            ;

            spKey1.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            spKey1.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            spKey1.TextChanged += new EventHandler(this.OnSpTextChange);
        }

        private void OnSpTextChange(object sender, EventArgs e)
        {
            Key key = (Key)Enum.Parse(typeof(Key), spKey1.Text.ToString());
            this.autopot.SPKey1 = key;
            ProfileSingleton.SetConfiguration(this.autopot);
            this.ActiveControl = null;
        }

        private void NumAutopotDelayTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.autopot == null)
                {
                    return;
                }

                this.autopot.Delay = short.Parse(this.AutopotSPDelay.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in NumAutopotDelayTextChanged: {ex.Message}");
            }

        }

        private void ChkStopOnCriticalInjury_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ProfileSingleton.SetConfiguration(this.autopot);
        }

        private void TxtSPpctTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.autopot == null)
                {
                    return;
                }

                this.autopot.SPPercent1 = short.Parse(this.spPct1.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in TxtSPpctTextChanged: {ex.Message}");
            }
        }

        private void AutopotForm_Load(object sender, EventArgs e)
        {

        }
    }
}
