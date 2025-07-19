using System;
using System.Windows.Forms;

namespace _4RTools.Forms
{
    public partial class DialogOK : Form
    {
        public DialogOK(string message, string title)
        {
            InitializeComponent();
            this.Text = title;
            this.lblMessage.Text = message;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        public static bool ShowDialog(string message, string title)
        {
            using (var dialog = new DialogOK(message, title))
            {
                return dialog.ShowDialog() == DialogResult.Yes;
            }
        }
    }
}