using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _ORTools.Forms
{
    public partial class TransferHelperForm : Form, IObserver
    {
        private TransferHelper transferHelper;

        public TransferHelperForm(Subject subject)
        {
            InitializeComponent();
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

            this.txtTransferKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormHelper.OnKeyDown);
            this.txtTransferKey.KeyPress += new KeyPressEventHandler(FormHelper.OnKeyPress);
            this.txtTransferKey.TextChanged += new EventHandler(OnTransferKeyChange);
            this.ActiveControl = null;

            System.Windows.Forms.TextBox textTransferKey = this.txtTransferKey;
            if (textTransferKey.Text == Keys.None.ToString())
            {
                FormHelper.ApplyInputKeyStyle(textTransferKey, false);
            }
            else
            {
                FormHelper.ApplyInputKeyStyle(textTransferKey, true);
            }
        }

        private void OnTransferKeyChange(object sender, EventArgs e)
        {
            try
            {
                // Validate input before parsing
                if (string.IsNullOrWhiteSpace(this.txtTransferKey?.Text))
                {
                    DebugLogger.Debug("OnTransferKeyChange: Transfer key text is null or empty");
                    return;
                }

                string keyText = this.txtTransferKey.Text.Trim();

                // Attempt to parse the key
                if (!Enum.TryParse<Keys>(keyText, true, out Keys key))
                {
                    DebugLogger.Warning($"OnTransferKeyChange: Failed to parse '{keyText}' as a valid Keys enum value");
                    return;
                }

                System.Windows.Forms.TextBox textTransferKey = this.txtTransferKey;
                if (textTransferKey.Text == Keys.None.ToString())
                {
                    FormHelper.ApplyInputKeyStyle(textTransferKey, false);
                }
                else
                {
                    FormHelper.ApplyInputKeyStyle(textTransferKey, true);
                }

                // Update the transfer helper and configuration
                this.transferHelper.TransferKey = key;
                ProfileSingleton.SetConfiguration(this.transferHelper);

                //DebugLogger.Debug($"OnTransferKeyChange: Successfully set transfer key to {key}");
            }
            catch (ArgumentException ex)
            {
                DebugLogger.Error($"OnTransferKeyChange: Invalid key value '{this.txtTransferKey?.Text}': {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                DebugLogger.Error($"OnTransferKeyChange: Null reference error - transferHelper or txtTransferKey may be null: {ex.Message}");
            }
            catch (Exception ex)
            {
                DebugLogger.Error($"OnTransferKeyChange: Unexpected error while updating transfer key: {ex.Message}");
                DebugLogger.Error($"OnTransferKeyChange: Stack trace: {ex.StackTrace}");
            }
            finally
            {
                // Always clear focus regardless of success or failure
                this.ActiveControl = null;
            }
        }
    }
}