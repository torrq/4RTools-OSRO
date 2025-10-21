using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.UI.Forms
{
    public partial class AutopotForm : Form, IObserver
    {
        private Autopot autopot;
        private readonly bool isYgg;

        public AutopotForm(Subject subject, bool isYgg)
        {
            InitializeComponent();

            FormUtils.SetNumericUpDownMinimumDelays(this);

            if (isYgg)
            {
                this.picBoxHP.Image = _4RTools.Resources._4RTools.Icons.ygg;
                this.picBoxSP.Image = _4RTools.Resources._4RTools.Icons.ygg;
                this.chkStopOnCriticalInjury.Hide();
            }
            subject.Attach(this);

            this.isYgg = isYgg;
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopot = this.isYgg ? ProfileSingleton.GetCurrent().AutopotYgg : ProfileSingleton.GetCurrent().Autopot;
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
            this.txtHpKey.Text = this.autopot.HPKey.ToString();
            this.txtSPKey.Text = this.autopot.SPKey.ToString();
            this.txtHPpct.Text = this.autopot.HPPercent.ToString();
            this.txtSPpct.Text = this.autopot.SPPercent.ToString();
            this.numAutopotDelay.Text = this.autopot.Delay.ToString();
            this.chkStopOnCriticalInjury.Checked = this.autopot.StopOnCriticalInjury;
            RadioButton rdHealFirst = (RadioButton)this.Controls[ProfileSingleton.GetCurrent().Autopot.FirstHeal];
            if (rdHealFirst != null) { rdHealFirst.Checked = true; }
            ;

            txtHpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHpKey.TextChanged += new EventHandler(this.OnHpTextChange);
            txtSPKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSPKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSPKey.TextChanged += new EventHandler(this.OnSpTextChange);
        }

        private void OnHpTextChange(object sender, EventArgs e)
        {
            Key key = (Key)Enum.Parse(typeof(Key), txtHpKey.Text.ToString());
            this.autopot.HPKey = key;
            ProfileSingleton.SetConfiguration(this.autopot);
            this.ActiveControl = null;
        }

        private void OnSpTextChange(object sender, EventArgs e)
        {
            Key key = (Key)Enum.Parse(typeof(Key), txtSPKey.Text.ToString());
            this.autopot.SPKey = key;
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

                this.autopot.Delay = short.Parse(this.numAutopotDelay.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in NumAutopotDelayTextChanged: {ex.Message}");
            }

        }

        private void TxtHPpctTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.autopot == null)
                {
                    return;
                }

                this.autopot.HPPercent = short.Parse(this.txtHPpct.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in TxtHPpctTextChanged: {ex.Message}");
            }

        }

        private void ChkStopOnCriticalInjury_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            this.autopot.StopOnCriticalInjury = chk.Checked;
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

                this.autopot.SPPercent = short.Parse(this.txtSPpct.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"Error in TxtSPpctTextChanged: {ex.Message}");
            }
        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                this.autopot.FirstHeal = rb.Name;
                ProfileSingleton.SetConfiguration(this.autopot);
            }
        }

        private void AutopotForm_Load(object sender, EventArgs e)
        {

        }
    }
}
