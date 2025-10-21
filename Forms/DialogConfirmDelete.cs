using System;
using System.Windows.Forms;

namespace BruteGamingMacros.UI.Forms
{
    public partial class DialogConfirmDelete : Form
    {
        public DialogConfirmDelete(string message)
        {
            InitializeComponent();
            this.lblMessage.Text = message;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        public static bool ShowDialog(string message)
        {
            using (var dialog = new DialogConfirmDelete(message))
            {
                return dialog.ShowDialog() == DialogResult.Yes;
            }
        }
    }
}