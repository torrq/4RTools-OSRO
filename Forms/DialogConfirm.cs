using System;
using System.Windows.Forms;

namespace BruteGamingMacros.UI.Forms
{
    public partial class DialogConfirm : Form
    {
        public DialogConfirm(string message, string title)
        {
            InitializeComponent();
            this.Text = title;
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

        public static bool ShowDialog(string message, string title)
        {
            using (var dialog = new DialogConfirm(message, title))
            {
                return dialog.ShowDialog() == DialogResult.Yes;
            }
        }
    }
}