using System.Windows.Forms;
using _4RTools.Utils;

namespace _4RTools.Forms
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
            System.Windows.Forms.TabControl atkDefMode;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Container));
            this.tabPageSpammer = new System.Windows.Forms.TabPage();
            this.tabPageDebuffs = new System.Windows.Forms.TabPage();
            this.tabPageAutobuffSkill = new System.Windows.Forms.TabPage();
            this.tabPageAutobuffStuff = new System.Windows.Forms.TabPage();
            this.atkDef = new System.Windows.Forms.TabPage();
            this.tabPageMacroSongs = new System.Windows.Forms.TabPage();
            this.tabMacroSwitch = new System.Windows.Forms.TabPage();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.tabPageProfiles = new System.Windows.Forms.TabPage();
            this.lblProcessName = new System.Windows.Forms.Label();
            this.processCB = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
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
            resources.ApplyResources(atkDefMode, "atkDefMode");
            atkDefMode.Controls.Add(this.tabPageSpammer);
            atkDefMode.Controls.Add(this.tabPageDebuffs);
            atkDefMode.Controls.Add(this.tabPageAutobuffSkill);
            atkDefMode.Controls.Add(this.tabPageAutobuffStuff);
            atkDefMode.Controls.Add(this.atkDef);
            atkDefMode.Controls.Add(this.tabPageMacroSongs);
            atkDefMode.Controls.Add(this.tabMacroSwitch);
            atkDefMode.Controls.Add(this.tabConfig);
            atkDefMode.Controls.Add(this.tabPageProfiles);
            atkDefMode.Name = "atkDefMode";
            atkDefMode.SelectedIndex = 0;
            // 
            // tabPageSpammer
            // 
            resources.ApplyResources(this.tabPageSpammer, "tabPageSpammer");
            this.tabPageSpammer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageSpammer.Name = "tabPageSpammer";
            // 
            // tabPageDebuffs
            // 
            resources.ApplyResources(this.tabPageDebuffs, "tabPageDebuffs");
            this.tabPageDebuffs.Name = "tabPageDebuffs";
            // 
            // tabPageAutobuffSkill
            // 
            resources.ApplyResources(this.tabPageAutobuffSkill, "tabPageAutobuffSkill");
            this.tabPageAutobuffSkill.Name = "tabPageAutobuffSkill";
            // 
            // tabPageAutobuffStuff
            // 
            resources.ApplyResources(this.tabPageAutobuffStuff, "tabPageAutobuffStuff");
            this.tabPageAutobuffStuff.Name = "tabPageAutobuffStuff";
            // 
            // atkDef
            // 
            resources.ApplyResources(this.atkDef, "atkDef");
            this.atkDef.Name = "atkDef";
            // 
            // tabPageMacroSongs
            // 
            resources.ApplyResources(this.tabPageMacroSongs, "tabPageMacroSongs");
            this.tabPageMacroSongs.Name = "tabPageMacroSongs";
            // 
            // tabMacroSwitch
            // 
            resources.ApplyResources(this.tabMacroSwitch, "tabMacroSwitch");
            this.tabMacroSwitch.Name = "tabMacroSwitch";
            // 
            // tabConfig
            // 
            resources.ApplyResources(this.tabConfig, "tabConfig");
            this.tabConfig.Name = "tabConfig";
            // 
            // tabPageProfiles
            // 
            resources.ApplyResources(this.tabPageProfiles, "tabPageProfiles");
            this.tabPageProfiles.Name = "tabPageProfiles";
            // 
            // lblProcessName
            // 
            resources.ApplyResources(this.lblProcessName, "lblProcessName");
            this.lblProcessName.Name = "lblProcessName";
            // 
            // processCB
            // 
            resources.ApplyResources(this.processCB, "processCB");
            this.processCB.FormattingEnabled = true;
            this.processCB.Name = "processCB";
            this.processCB.SelectedIndexChanged += new System.EventHandler(this.ProcessCB_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // labelProfile
            // 
            resources.ApplyResources(this.labelProfile, "labelProfile");
            this.labelProfile.Name = "labelProfile";
            // 
            // profileCB
            // 
            resources.ApplyResources(this.profileCB, "profileCB");
            this.profileCB.FormattingEnabled = true;
            this.profileCB.Name = "profileCB";
            this.profileCB.SelectedIndexChanged += new System.EventHandler(this.ProfileCB_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblCharacterName
            // 
            resources.ApplyResources(this.lblCharacterName, "lblCharacterName");
            this.lblCharacterName.Name = "lblCharacterName";
            // 
            // characterName
            // 
            resources.ApplyResources(this.characterName, "characterName");
            this.characterName.ForeColor = System.Drawing.Color.DarkGreen;
            this.characterName.Name = "characterName";
            // 
            // tabControlAutopot
            // 
            resources.ApplyResources(this.tabControlAutopot, "tabControlAutopot");
            this.tabControlAutopot.Controls.Add(this.tabPageAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageYggAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageSkillTimer);
            this.tabControlAutopot.Multiline = true;
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.SelectedIndex = 0;
            // 
            // tabPageAutopot
            // 
            resources.ApplyResources(this.tabPageAutopot, "tabPageAutopot");
            this.tabPageAutopot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageAutopot.Name = "tabPageAutopot";
            // 
            // tabPageYggAutopot
            // 
            resources.ApplyResources(this.tabPageYggAutopot, "tabPageYggAutopot");
            this.tabPageYggAutopot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageYggAutopot.Name = "tabPageYggAutopot";
            // 
            // tabPageSkillTimer
            // 
            resources.ApplyResources(this.tabPageSkillTimer, "tabPageSkillTimer");
            this.tabPageSkillTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageSkillTimer.Name = "tabPageSkillTimer";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.BackColor = System.Drawing.Color.Silver;
            this.panel4.Name = "panel4";
            // 
            // characterMap
            // 
            resources.ApplyResources(this.characterMap, "characterMap");
            this.characterMap.ForeColor = System.Drawing.Color.DarkCyan;
            this.characterMap.Name = "characterMap";
            // 
            // Container
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.characterMap);
            this.Controls.Add(atkDefMode);
            this.Controls.Add(this.tabControlAutopot);
            this.Controls.Add(this.characterName);
            this.Controls.Add(this.lblCharacterName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.profileCB);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblProcessName);
            this.Controls.Add(this.processCB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Container";
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
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TabPage tabPageSpammer;
        private System.Windows.Forms.Label labelProfile;
        public System.Windows.Forms.ComboBox profileCB;
        private System.Windows.Forms.TabPage tabPageAutobuffSkill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCharacterName;
        private System.Windows.Forms.Label characterName;
        private TabPage tabPageAutobuffStuff;
        private TabPage tabPageMacroSongs;
        private TabPage atkDef;
        private TabControl tabControlAutopot;
        private TabPage tabPageAutopot;
        private TabPage tabPageYggAutopot;
        private TabPage tabPageProfiles;
        private TabPage tabMacroSwitch;
        private TabPage tabPageSkillTimer;
//        private TabPage tabPageServer;
        private TabPage tabPageDebuffs;
        private TabPage tabConfig;
        private Panel panel4;
        private Label characterMap;
    }
}