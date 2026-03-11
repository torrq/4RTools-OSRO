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

        private void ClearLabels()
        {
            this.CharacterNameLabel = "";
            this.CharacterInfoLabel = "";
            this.CharacterMapLabel  = "";
            this.MapLink            = "";
        }

        /// <summary>
        /// Updates character information with client data and formats it for display
        /// </summary>
        public void UpdateCharacterInfo(Client client)
        {
            // Check if client is null, has no process, or is not logged in
            if (client?.Process == null || !IsClientLoggedIn(client))
            {
                ClearLabels();
                return;
            }

            // Read all data in bulk — 3 RPM calls instead of 10
            var hpSp   = client.ReadHpSp();
            var jobSnap = client.ReadJobBlock();
            string currentMap = client.ReadCurrentMap() ?? "";
            string characterName = client.ReadCharacterName();

            if (jobSnap == null) { ClearLabels(); return; }
            var job = jobSnap.Value;

            int currentLevel     = (int)job.Level;
            int currentJobLevel  = (int)job.JobLevel;
            int currentJobId     = (int)job.JobId;
            int currentExpToLevel = (int)job.ExpToLevel;
            int currentExp       = (int)job.Exp;
            int currentHP        = (int)hpSp.CurrentHp;
            int currentMaxHP     = (int)hpSp.MaxHp;
            int currentSP        = (int)hpSp.CurrentSp;
            int currentMaxSP     = (int)hpSp.MaxSp;

            // Validate data (example: check if level is reasonable)
            if (!IsValidCharacterData(currentLevel, currentJobLevel, currentHP, currentMaxHP))
            {
                ClearLabels();
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
            this.CharacterMapLabel = currentMap;
            this.MapLink = "https://ro.kokotewa.com/db/map_info?id=" + currentMap;
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
    }
}