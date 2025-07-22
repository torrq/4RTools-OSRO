using _ORTools.Model;
using _ORTools.Utils;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace _ORTools.Forms
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

        /// <summary>
        /// Updates character information with client data and formats it for display
        /// </summary>
        public void UpdateCharacterInfo(Client client)
        {
            // Check if client is null, has no process, or is not logged in
            if (client?.Process == null || !IsClientLoggedIn(client))
            {
                // Clear UI labels when no valid client is logged in
                this.CharacterNameLabel = "";
                this.CharacterInfoLabel = "";
                this.CharacterMapLabel = "";
                this.MapLink = "";
                return;
            }

            // Read character data
            string characterName = client.ReadCharacterName();
            int currentLevel = (int)client.ReadCurrentLevel();
            int currentJobLevel = (int)client.ReadCurrentJobLevel();
            int currentJobId = (int)client.ReadCurrentJob();
            int currentExpToLevel = (int)client.ReadCurrentExpToLevel();
            int currentExp = (int)client.ReadCurrentExp();
            int currentHP = (int)client.ReadCurrentHp();
            int currentMaxHP = (int)client.ReadMaxHp();
            int currentSP = (int)client.ReadCurrentSp();
            int currentMaxSP = (int)client.ReadMaxSp();

            // Validate data (example: check if level is reasonable)
            if (!IsValidCharacterData(currentLevel, currentJobLevel, currentHP, currentMaxHP))
            {
                // Clear UI labels if data is invalid
                this.CharacterNameLabel = "";
                this.CharacterInfoLabel = "";
                this.CharacterMapLabel = "";
                this.MapLink = "";
                return;
            }

            // Calculate experience percentage
            string currentExpPercent;
            if (currentExpToLevel > 0)
            {
                double ratio = (double)currentExp / currentExpToLevel;
                currentExpPercent = $"{(ratio * 100):0.00}%";
            }
            else
            {
                currentExpPercent = "100%";
            }

            // Get job name
            string jobName = JobList.GetNameById(currentJobId);

            // Format the multi-line info text
            string line1 = $"Lv{currentLevel} / {jobName} / Lv{currentJobLevel} / Exp {currentExpPercent}";
            string line2 = $"HP {currentHP} / {currentMaxHP} | SP {currentSP} / {currentMaxSP}";

            string clientDebugInfo = line1 + "\n" + line2;

            // Update the form
            this.CharacterNameLabel = characterName;
            this.CharacterInfoLabel = clientDebugInfo;
            this.CharacterMapLabel = client.ReadCurrentMap() ?? "";
            this.MapLink = "https://ro.kokotewa.com/db/map_info?id=" + (client.ReadCurrentMap() ?? "");
        }

        // Helper method to check if client is logged in
        private bool IsClientLoggedIn(Client client)
        {
            // Replace with actual logic to check if client is logged in
            // Example: return client.IsLoggedIn; // Assuming Client has an IsLoggedIn property
            // If no such property exists, you might check if characterName is non-empty or other indicators
            return true;
            //!string.IsNullOrEmpty(client.ReadCharacterName());
        }

        // Helper method to validate character data
        private bool IsValidCharacterData(int level, int jobLevel, int hp, int maxHP)
        {
            DebugLogger.Debug($"Validating character data: Level={level}, JobLevel={jobLevel}, HP={hp}, MaxHP={maxHP}");
            // Example validation: ensure level and HP are within reasonable ranges
            return level > 0 && level <= 255 && // Adjust max level based on game
                   jobLevel > 0 && jobLevel <= 255 && // Adjust max job level based on game
                   hp >= 0 && maxHP > 0 && hp <= maxHP;
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

        private void characterInfoLabel_Click(object sender, EventArgs e)
        {

        }

        private void characterMapLabel_Click_1(object sender, EventArgs e)
        {

        }
    }
}