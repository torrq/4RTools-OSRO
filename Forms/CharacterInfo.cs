using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace _4RTools.Forms
{
    public partial class CharacterInfo : Form
    {
        private string mapLink = "";

        public CharacterInfo()
        {
            InitializeComponent();
            this.CharacterNameLabel = "";
            this.CharacterInfoLabel = "";
            this.CharacterMapLabel = "";
            this.MapLink = "";

            this.characterMapLabel.Cursor = Cursors.Hand;
            this.characterMapLabel.Click += CharacterMapLabel_Click;
        }

        public string CharacterNameLabel
        {
            get { return characterNameLabel.Text; }
            set { characterNameLabel.Text = value; }
        }

        public string CharacterInfoLabel
        {
            get { return characterInfoLabel.Text; }
            set { characterInfoLabel.Text = value; }
        }

        public string CharacterMapLabel
        {
            get { return characterMapLabel.Text; }
            set { characterMapLabel.Text = value; }
        }

        public string MapLink
        {
            get { return mapLink; }
            set { mapLink = value ?? ""; }
        }

        private void CharacterMapLabel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mapLink))
            {
                try
                {
                    Process.Start(new ProcessStartInfo(mapLink) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open link:\n" + ex.Message);
                }
            }
        }

        private void CharacterInfo_Load(object sender, EventArgs e)
        {
        }
    }
}
