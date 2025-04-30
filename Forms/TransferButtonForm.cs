using System;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class TransferButtonForm : Form, IObserver
    {

        private Custom custom;

        public TransferButtonForm(Subject subject)
        {
            InitializeComponent();
            string toolTipText = "Simulates Alt+Right Click for quick item transfer between storage and inventory";

            tooltipTransferKey.SetToolTip(label1, toolTipText);
            tooltipTransferKey.SetToolTip(pictureBox2, toolTipText);

            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {

            switch ((subject as Subject).Message.Code)
            {
                case MessageCode.PROFILE_CHANGED:
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    this.custom.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.custom.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            this.custom = ProfileSingleton.GetCurrent().Custom;
            this.txtTransferKey.Text = custom.tiMode.ToString();

            this.txtTransferKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtTransferKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtTransferKey.TextChanged += new EventHandler(OnTransferKeyChange);
            this.ActiveControl = null;
        }

        private void OnTransferKeyChange(object sender, EventArgs e)
        {
            Key key = (Key)Enum.Parse(typeof(Key), this.txtTransferKey.Text.ToString());
            try
            {
                this.custom.tiMode = key;
                ProfileSingleton.SetConfiguration(this.custom);
            }
            catch { }
            this.ActiveControl = null;
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void TxtTransferKey_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
