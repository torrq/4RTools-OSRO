using System;
using System.Windows.Forms;

namespace _4RTools.Forms
{
    public partial class CharacterInfo : Form
    {
        public CharacterInfo()
        {
            InitializeComponent();
            this.CharacterNameLabel = "";
            this.CharacterInfoLabel = "";
            this.CharacterMapLabel = "";
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

        private void CharacterInfo_Load(object sender, EventArgs e)
        {
        }
    }
}