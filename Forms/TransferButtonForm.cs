using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace _4RTools.Forms
{
    public partial class TransferButtonForm : Form, IObserver
    {
        private TransferHelper transferHelper;

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
                    this.transferHelper.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.transferHelper.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            this.transferHelper = ProfileSingleton.GetCurrent().TransferHelper;
            this.txtTransferKey.Text = transferHelper.TransferKey.ToString();

            this.txtTransferKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtTransferKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtTransferKey.TextChanged += new EventHandler(OnTransferKeyChange);
            this.ActiveControl = null;
        }

        private void OnTransferKeyChange(object sender, EventArgs e)
        {
            try
            {
                Key key = (Key)Enum.Parse(typeof(Key), this.txtTransferKey.Text.ToString());
                this.transferHelper.TransferKey = key;
                ProfileSingleton.SetConfiguration(this.transferHelper);
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