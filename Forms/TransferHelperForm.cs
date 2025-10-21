using BruteGamingMacros.Core.Model;
using BruteGamingMacros.Core.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace BruteGamingMacros.UI.Forms
{
    public partial class TransferHelperForm : Form, IObserver
    {
        private TransferHelper transferHelper;

        public TransferHelperForm(Subject subject)
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
            catch (Exception ex)
            {
                // Silently ignore invalid key input - this is expected during user typing
                if (!(ex is ArgumentException || ex is FormatException))
                {
                    Console.WriteLine($"Error in TransferHelperForm key parsing: {ex.Message}");
                }
            }
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