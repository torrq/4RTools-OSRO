using System.Windows.Forms;
using BruteGamingMacros.Core.Utils;

namespace BruteGamingMacros.UI.Forms
{
    partial class Container
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TabControl atkDefMode;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Container));
            this.tabPageSpammer = new System.Windows.Forms.TabPage();
            this.tabPageDebuffs = new System.Windows.Forms.TabPage();
            this.tabPageAutobuffSkill = new System.Windows.Forms.TabPage();
            this.tabPageAutobuffItem = new System.Windows.Forms.TabPage();
            this.atkDef = new System.Windows.Forms.TabPage();
            this.tabPageMacroSongs = new System.Windows.Forms.TabPage();
            this.tabMacroSwitch = new System.Windows.Forms.TabPage();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.tabPageProfiles = new System.Windows.Forms.TabPage();
            this.lblProcessName = new System.Windows.Forms.Label();
            this.processCB = new System.Windows.Forms.ComboBox();
            this.labelProfile = new System.Windows.Forms.Label();
            this.profileCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCharacterName = new System.Windows.Forms.Label();
            this.characterName = new System.Windows.Forms.Label();
            this.tabControlAutopot = new System.Windows.Forms.TabControl();
            this.tabPageAutopot = new System.Windows.Forms.TabPage();
            this.tabPageYggAutopot = new System.Windows.Forms.TabPage();
            this.tabPageSkillTimer = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.characterMap = new System.Windows.Forms.Label();
            atkDefMode = new System.Windows.Forms.TabControl();
            atkDefMode.SuspendLayout();
            this.tabControlAutopot.SuspendLayout();
            this.SuspendLayout();
            // 
            // atkDefMode
            // 
            atkDefMode.Controls.Add(this.tabPageSpammer);
            atkDefMode.Controls.Add(this.tabPageDebuffs);
            atkDefMode.Controls.Add(this.tabPageAutobuffSkill);
            atkDefMode.Controls.Add(this.tabPageAutobuffItem);
            atkDefMode.Controls.Add(this.atkDef);
            atkDefMode.Controls.Add(this.tabPageMacroSongs);
            atkDefMode.Controls.Add(this.tabMacroSwitch);
            atkDefMode.Controls.Add(this.tabConfig);
            atkDefMode.Controls.Add(this.tabPageProfiles);
            atkDefMode.Location = new System.Drawing.Point(15, 274);
            atkDefMode.Name = "atkDefMode";
            atkDefMode.SelectedIndex = 0;
            atkDefMode.Size = new System.Drawing.Size(637, 475);
            atkDefMode.TabIndex = 6;
            // 
            // tabPageSpammer
            // 
            this.tabPageSpammer.BackColor = AppConfig.AccentBackColor;
            this.tabPageSpammer.Location = new System.Drawing.Point(4, 22);
            this.tabPageSpammer.Name = "tabPageSpammer";
            this.tabPageSpammer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpammer.Size = new System.Drawing.Size(629, 449);
            this.tabPageSpammer.TabIndex = 1;
            this.tabPageSpammer.Text = "Skill Spammer";
            // 
            // tabPageDebuffs
            // 
            this.tabPageDebuffs.Location = new System.Drawing.Point(4, 22);
            this.tabPageDebuffs.Name = "tabPageDebuffs";
            this.tabPageDebuffs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebuffs.Size = new System.Drawing.Size(629, 449);
            this.tabPageDebuffs.TabIndex = 7;
            this.tabPageDebuffs.Text = "Debuffs";
            // 
            // tabPageAutobuffSkill
            // 
            this.tabPageAutobuffSkill.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutobuffSkill.Name = "tabPageAutobuffSkill";
            this.tabPageAutobuffSkill.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutobuffSkill.Size = new System.Drawing.Size(629, 449);
            this.tabPageAutobuffSkill.TabIndex = 3;
            this.tabPageAutobuffSkill.Text = "Autobuff Skills";
            // 
            // tabPageAutobuffItem
            // 
            this.tabPageAutobuffItem.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutobuffItem.Name = "tabPageAutobuffItem";
            this.tabPageAutobuffItem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutobuffItem.Size = new System.Drawing.Size(629, 449);
            this.tabPageAutobuffItem.TabIndex = 4;
            this.tabPageAutobuffItem.Text = "Autobuff Items";
            // 
            // atkDef
            // 
            this.atkDef.Location = new System.Drawing.Point(4, 22);
            this.atkDef.Name = "atkDef";
            this.atkDef.Padding = new System.Windows.Forms.Padding(3);
            this.atkDef.Size = new System.Drawing.Size(629, 449);
            this.atkDef.TabIndex = 5;
            this.atkDef.Text = "ATK x DEF";
            // 
            // tabPageMacroSongs
            // 
            this.tabPageMacroSongs.Location = new System.Drawing.Point(4, 22);
            this.tabPageMacroSongs.Name = "tabPageMacroSongs";
            this.tabPageMacroSongs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMacroSongs.Size = new System.Drawing.Size(629, 449);
            this.tabPageMacroSongs.TabIndex = 6;
            this.tabPageMacroSongs.Text = "Songs";
            // 
            // tabMacroSwitch
            // 
            this.tabMacroSwitch.Location = new System.Drawing.Point(4, 22);
            this.tabMacroSwitch.Name = "tabMacroSwitch";
            this.tabMacroSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabMacroSwitch.Size = new System.Drawing.Size(629, 449);
            this.tabMacroSwitch.TabIndex = 8;
            this.tabMacroSwitch.Text = "Macro Switch";
            // 
            // tabConfig
            // 
            this.tabConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(629, 449);
            this.tabConfig.TabIndex = 10;
            this.tabConfig.Text = "Settings";
            // 
            // tabPageProfiles
            // 
            this.tabPageProfiles.Location = new System.Drawing.Point(4, 22);
            this.tabPageProfiles.Name = "tabPageProfiles";
            this.tabPageProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProfiles.Size = new System.Drawing.Size(629, 449);
            this.tabPageProfiles.TabIndex = 9;
            this.tabPageProfiles.Text = "Profiles";
            // 
            // lblProcessName
            // 
            this.lblProcessName.AutoSize = true;
            this.lblProcessName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessName.Location = new System.Drawing.Point(13, 9);
            this.lblProcessName.Name = "lblProcessName";
            this.lblProcessName.Size = new System.Drawing.Size(42, 17);
            this.lblProcessName.TabIndex = 3;
            this.lblProcessName.Text = "Client";
            // 
            // processCB
            // 
            this.processCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.processCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processCB.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processCB.Location = new System.Drawing.Point(18, 29);
            this.processCB.Name = "processCB";
            this.processCB.Size = new System.Drawing.Size(184, 25);
            this.processCB.ItemHeight = 25; // Match profileCB height
            this.processCB.DropDownHeight = 200; // Already set in code, but keep for consistency
            this.processCB.TabIndex = 2;
            this.processCB.SelectedIndexChanged += new System.EventHandler(this.ProcessCB_SelectedIndexChanged);
            // 
            // labelProfile
            // 
            this.labelProfile.AutoSize = true;
            this.labelProfile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProfile.Location = new System.Drawing.Point(252, 9);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(46, 17);
            this.labelProfile.TabIndex = 15;
            this.labelProfile.Text = "Profile";
            // 
            // profileCB
            // 
            this.profileCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.profileCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileCB.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profileCB.Location = new System.Drawing.Point(256, 29);
            this.profileCB.Name = "profileCB";
            this.profileCB.Size = new System.Drawing.Size(181, 25);
            this.profileCB.TabIndex = 14;
            this.profileCB.SelectedIndexChanged += new System.EventHandler(this.ProfileCB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 18;
            // 
            // lblCharacterName
            // 
            this.lblCharacterName.AutoSize = true;
            this.lblCharacterName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharacterName.Location = new System.Drawing.Point(463, 9);
            this.lblCharacterName.Name = "lblCharacterName";
            this.lblCharacterName.Size = new System.Drawing.Size(66, 17);
            this.lblCharacterName.TabIndex = 19;
            this.lblCharacterName.Text = "Character";
            // 
            // characterName
            // 
            this.characterName.AutoSize = true;
            this.characterName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.characterName.ForeColor = System.Drawing.Color.DarkGreen;
            this.characterName.Location = new System.Drawing.Point(475, 30);
            this.characterName.Name = "characterName";
            this.characterName.Size = new System.Drawing.Size(22, 17);
            this.characterName.TabIndex = 20;
            this.characterName.Text = "- -";
            // 
            // tabControlAutopot
            // 
            this.tabControlAutopot.Controls.Add(this.tabPageAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageYggAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageSkillTimer);
            this.tabControlAutopot.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlAutopot.ItemSize = new System.Drawing.Size(68, 20);
            this.tabControlAutopot.Location = new System.Drawing.Point(15, 83);
            this.tabControlAutopot.Multiline = true;
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.Padding = new System.Drawing.Point(12, 3);
            this.tabControlAutopot.SelectedIndex = 0;
            this.tabControlAutopot.Size = new System.Drawing.Size(328, 180);
            this.tabControlAutopot.TabIndex = 25;
            // 
            // tabPageAutopot
            // 
            this.tabPageAutopot.BackColor = AppConfig.AccentBackColor;
            this.tabPageAutopot.Location = new System.Drawing.Point(4, 24);
            this.tabPageAutopot.Name = "tabPageAutopot";
            this.tabPageAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutopot.Size = new System.Drawing.Size(320, 152);
            this.tabPageAutopot.TabIndex = 0;
            this.tabPageAutopot.Text = "Autopot";
            // 
            // tabPageYggAutopot
            // 
            this.tabPageYggAutopot.BackColor = AppConfig.AccentBackColor;
            this.tabPageYggAutopot.Location = new System.Drawing.Point(4, 26);
            this.tabPageYggAutopot.Name = "tabPageYggAutopot";
            this.tabPageYggAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageYggAutopot.Size = new System.Drawing.Size(320, 150);
            this.tabPageYggAutopot.TabIndex = 1;
            this.tabPageYggAutopot.Text = "Yggdrasil";
            // 
            // tabPageSkillTimer
            // 
            this.tabPageSkillTimer.BackColor = AppConfig.AccentBackColor;
            this.tabPageSkillTimer.Location = new System.Drawing.Point(4, 26);
            this.tabPageSkillTimer.Name = "tabPageSkillTimer";
            this.tabPageSkillTimer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkillTimer.Size = new System.Drawing.Size(320, 150);
            this.tabPageSkillTimer.TabIndex = 2;
            this.tabPageSkillTimer.Text = "Skill Timer";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Location = new System.Drawing.Point(16, 73);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(635, 1);
            this.panel4.TabIndex = 17;
            // 
            // characterMap
            // 
            this.characterMap.AutoSize = true;
            this.characterMap.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic);
            this.characterMap.ForeColor = System.Drawing.Color.DarkCyan;
            this.characterMap.Location = new System.Drawing.Point(475, 47);
            this.characterMap.Name = "characterMap";
            this.characterMap.Size = new System.Drawing.Size(0, 17);
            this.characterMap.TabIndex = 26;
            // 
            // Container
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(668, 761);
            this.Controls.Add(this.characterMap);
            this.Controls.Add(atkDefMode);
            this.Controls.Add(this.tabControlAutopot);
            this.Controls.Add(this.characterName);
            this.Controls.Add(this.lblCharacterName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.profileCB);
            this.Controls.Add(this.lblProcessName);
            this.Controls.Add(this.processCB);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Container";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OSRO Tools";
            this.Load += new System.EventHandler(this.Container_Load);
            this.Resize += new System.EventHandler(this.ContainerResize);
            atkDefMode.ResumeLayout(false);
            this.tabControlAutopot.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProcessName;
        private System.Windows.Forms.ComboBox processCB;
        private System.Windows.Forms.TabPage tabPageSpammer;
        private System.Windows.Forms.Label labelProfile;
        public System.Windows.Forms.ComboBox profileCB;
        private System.Windows.Forms.TabPage tabPageAutobuffSkill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCharacterName;
        private System.Windows.Forms.Label characterName;
        private TabPage tabPageAutobuffItem;
        private TabPage tabPageMacroSongs;
        private TabPage atkDef;
        private TabControl tabControlAutopot;
        private TabPage tabPageAutopot;
        private TabPage tabPageYggAutopot;
        private TabPage tabPageProfiles;
        private TabPage tabMacroSwitch;
        private TabPage tabPageSkillTimer;
        private TabPage tabPageDebuffs;
        private TabPage tabConfig;
        private Panel panel4;
        private Label characterMap;
    }
}