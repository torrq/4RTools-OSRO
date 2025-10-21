using System;
using System.Windows.Forms;

namespace BruteGamingMacros.UI.Forms
{
    public partial class DialogInput : Form
    {
        public DialogInput(string prompt, string title, string defaultText)
        {
            InitializeComponent();
            this.Text = title;
            this.lblPrompt.Text = prompt;
            this.txtInput.Text = defaultText;
            this.txtInput.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public static string ShowDialog(string prompt, string title, string defaultText)
        {
            using (var dialog = new DialogInput(prompt, title, defaultText))
            {
                return dialog.ShowDialog() == DialogResult.OK ? dialog.txtInput.Text : null;
            }
        }
    }
}