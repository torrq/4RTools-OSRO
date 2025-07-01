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
            this.tabControlAutopot = new System.Windows.Forms.TabControl();
            this.tabPageAutopot = new System.Windows.Forms.TabPage();
            this.tabPageYggAutopot = new System.Windows.Forms.TabPage();
            this.tabPageSkillTimer = new System.Windows.Forms.TabPage();
            this.tabPageAutoOff = new System.Windows.Forms.TabPage();
            this.topSplitterPanel = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.btnToggleMiniMode = new NoFocusButton();
            this.tabControl2.SuspendLayout();
            this.tabControlAutopot.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPageSpammer);
            this.tabControl2.Controls.Add(this.tabPageDebuffs);
            this.tabControl2.Controls.Add(this.tabPageAutobuffSkill);
            this.tabControl2.Controls.Add(this.tabPageAutobuffItem);
            this.tabControl2.Controls.Add(this.atkDef);
            this.tabControl2.Controls.Add(this.tabPageMacroSongs);
            this.tabControl2.Controls.Add(this.tabMacroSwitch);
            this.tabControl2.Controls.Add(this.tabConfig);
            this.tabControl2.Controls.Add(this.tabPageProfiles);
            this.tabControl2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(5, 292);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(632, 475);
            this.tabControl2.TabIndex = 6;
            // 
            // btnToggleMiniMode
            // 
            this.btnToggleMiniMode.BackColor = System.Drawing.Color.White;
            this.btnToggleMiniMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleMiniMode.FlatAppearance.BorderSize = 0;
            this.btnToggleMiniMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnToggleMiniMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnToggleMiniMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleMiniMode.Image = global::_4RTools.Resources._4RTools.Icons.minimode_less;
            this.btnToggleMiniMode.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnToggleMiniMode.Location = new System.Drawing.Point(2, 266);
            this.btnToggleMiniMode.Name = "btnToggleMiniMode";
            this.btnToggleMiniMode.Size = new System.Drawing.Size(636, 18);
            this.btnToggleMiniMode.TabIndex = 27;
            this.btnToggleMiniMode.Text = "";
            this.btnToggleMiniMode.UseVisualStyleBackColor = false;
            this.btnToggleMiniMode.Click += new System.EventHandler(this.BtnToggleMiniMode_Click);
            // 
            // tabPageSpammer
            // 
            this.tabPageSpammer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageSpammer.Location = new System.Drawing.Point(4, 23);
            this.tabPageSpammer.Name = "tabPageSpammer";
            this.tabPageSpammer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpammer.Size = new System.Drawing.Size(629, 448);
            this.tabPageSpammer.TabIndex = 1;
            this.tabPageSpammer.Text = "Skill Spammer";
            // 
            // tabPageDebuffs
            // 
            this.tabPageDebuffs.Location = new System.Drawing.Point(4, 23);
            this.tabPageDebuffs.Name = "tabPageDebuffs";
            this.tabPageDebuffs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDebuffs.Size = new System.Drawing.Size(629, 448);
            this.tabPageDebuffs.TabIndex = 7;
            this.tabPageDebuffs.Text = "Debuffs";
            // 
            // tabPageAutobuffSkill
            // 
            this.tabPageAutobuffSkill.Location = new System.Drawing.Point(4, 23);
            this.tabPageAutobuffSkill.Name = "tabPageAutobuffSkill";
            this.tabPageAutobuffSkill.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutobuffSkill.Size = new System.Drawing.Size(629, 448);
            this.tabPageAutobuffSkill.TabIndex = 3;
            this.tabPageAutobuffSkill.Text = "Autobuff Skills";
            // 
            // tabPageAutobuffItem
            // 
            this.tabPageAutobuffItem.Location = new System.Drawing.Point(4, 23);
            this.tabPageAutobuffItem.Name = "tabPageAutobuffItem";
            this.tabPageAutobuffItem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutobuffItem.Size = new System.Drawing.Size(629, 448);
            this.tabPageAutobuffItem.TabIndex = 4;
            this.tabPageAutobuffItem.Text = "Autobuff Items";
            // 
            // atkDef
            // 
            this.atkDef.Location = new System.Drawing.Point(4, 23);
            this.atkDef.Name = "atkDef";
            this.atkDef.Padding = new System.Windows.Forms.Padding(3);
            this.atkDef.Size = new System.Drawing.Size(629, 448);
            this.atkDef.TabIndex = 5;
            this.atkDef.Text = "ATK x DEF";
            // 
            // tabPageMacroSongs
            // 
            this.tabPageMacroSongs.Location = new System.Drawing.Point(4, 23);
            this.tabPageMacroSongs.Name = "tabPageMacroSongs";
            this.tabPageMacroSongs.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMacroSongs.Size = new System.Drawing.Size(629, 448);
            this.tabPageMacroSongs.TabIndex = 6;
            this.tabPageMacroSongs.Text = "Songs";
            // 
            // tabMacroSwitch
            // 
            this.tabMacroSwitch.Location = new System.Drawing.Point(4, 23);
            this.tabMacroSwitch.Name = "tabMacroSwitch";
            this.tabMacroSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.tabMacroSwitch.Size = new System.Drawing.Size(629, 448);
            this.tabMacroSwitch.TabIndex = 8;
            this.tabMacroSwitch.Text = "Macro Switch";
            // 
            // tabConfig
            // 
            this.tabConfig.Location = new System.Drawing.Point(4, 23);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(629, 448);
            this.tabConfig.TabIndex = 10;
            this.tabConfig.Text = "Settings";
            // 
            // tabPageProfiles
            // 
            this.tabPageProfiles.Location = new System.Drawing.Point(4, 23);
            this.tabPageProfiles.Name = "tabPageProfiles";
            this.tabPageProfiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProfiles.Size = new System.Drawing.Size(629, 448);
            this.tabPageProfiles.TabIndex = 9;
            this.tabPageProfiles.Text = "Profiles";
            // 
            // lblProcessName
            // 
            this.lblProcessName.AutoSize = true;
            this.lblProcessName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessName.Location = new System.Drawing.Point(10, 9);
            this.lblProcessName.Name = "lblProcessName";
            this.lblProcessName.Size = new System.Drawing.Size(42, 14);
            this.lblProcessName.TabIndex = 3;
            this.lblProcessName.Text = "Client";
            // 
            // processCB
            // 
            this.processCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.processCB.DropDownHeight = 200;
            this.processCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processCB.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processCB.IntegralHeight = false;
            this.processCB.ItemHeight = 14;
            this.processCB.Location = new System.Drawing.Point(18, 29);
            this.processCB.Name = "processCB";
            this.processCB.Size = new System.Drawing.Size(180, 22);
            this.processCB.TabIndex = 2;
            this.processCB.SelectedIndexChanged += new System.EventHandler(this.ProcessCB_SelectedIndexChanged);
            // 
            // labelProfile
            // 
            this.labelProfile.AutoSize = true;
            this.labelProfile.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProfile.Location = new System.Drawing.Point(206, 9);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(46, 14);
            this.labelProfile.TabIndex = 15;
            this.labelProfile.Text = "Profile";
            // 
            // profileCB
            // 
            this.profileCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.profileCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileCB.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profileCB.Location = new System.Drawing.Point(210, 29);
            this.profileCB.Name = "profileCB";
            this.profileCB.Size = new System.Drawing.Size(150, 22);
            this.profileCB.TabIndex = 14;
            this.profileCB.SelectedIndexChanged += new System.EventHandler(this.ProfileCB_SelectedIndexChanged);
            // 
            // tabControlAutopot
            // 
            this.tabControlAutopot.Controls.Add(this.tabPageAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageYggAutopot);
            this.tabControlAutopot.Controls.Add(this.tabPageSkillTimer);
            this.tabControlAutopot.Controls.Add(this.tabPageAutoOff);
            this.tabControlAutopot.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlAutopot.ItemSize = new System.Drawing.Size(68, 20);
            this.tabControlAutopot.Location = new System.Drawing.Point(15, 83);
            this.tabControlAutopot.Multiline = true;
            this.tabControlAutopot.Name = "tabControlAutopot";
            this.tabControlAutopot.Padding = new System.Drawing.Point(12, 3);
            this.tabControlAutopot.SelectedIndex = 0;
            this.tabControlAutopot.Size = new System.Drawing.Size(350, 180);
            this.tabControlAutopot.TabIndex = 25;
            // 
            // tabPageAutopot
            // 
            this.tabPageAutopot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageAutopot.Location = new System.Drawing.Point(4, 24);
            this.tabPageAutopot.Name = "tabPageAutopot";
            this.tabPageAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutopot.Size = new System.Drawing.Size(342, 152);
            this.tabPageAutopot.TabIndex = 0;
            this.tabPageAutopot.Text = "Autopot";
            // 
            // tabPageYggAutopot
            // 
            this.tabPageYggAutopot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageYggAutopot.Location = new System.Drawing.Point(4, 24);
            this.tabPageYggAutopot.Name = "tabPageYggAutopot";
            this.tabPageYggAutopot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageYggAutopot.Size = new System.Drawing.Size(342, 152);
            this.tabPageYggAutopot.TabIndex = 1;
            this.tabPageYggAutopot.Text = "Yggdrasil";
            // 
            // tabPageSkillTimer
            // 
            this.tabPageSkillTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.tabPageSkillTimer.Location = new System.Drawing.Point(4, 24);
            this.tabPageSkillTimer.Name = "tabPageSkillTimer";
            this.tabPageSkillTimer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSkillTimer.Size = new System.Drawing.Size(342, 152);
            this.tabPageSkillTimer.TabIndex = 2;
            this.tabPageSkillTimer.Text = "Skill Timer";
            // 
            // tabPageAutoOff
            // 
            this.tabPageAutoOff.Location = new System.Drawing.Point(4, 24);
            this.tabPageAutoOff.Name = "tabPageAutoOff";
            this.tabPageAutoOff.Size = new System.Drawing.Size(342, 152);
            this.tabPageAutoOff.TabIndex = 3;
            this.tabPageAutoOff.Text = "Auto-Off";
            // 
            // topSplitterPanel
            // 
            this.topSplitterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(247)))));
            this.topSplitterPanel.Location = new System.Drawing.Point(2, 70);
            this.topSplitterPanel.Name = "topSplitterPanel";
            this.topSplitterPanel.Size = new System.Drawing.Size(635, 2);
            this.topSplitterPanel.TabIndex = 17;
            // 
            // Container
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(640, 770);
            this.Controls.Add(this.btnToggleMiniMode);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControlAutopot);
            this.Controls.Add(this.topSplitterPanel);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.profileCB);
            this.Controls.Add(this.lblProcessName);
            this.Controls.Add(this.processCB);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Container";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OSRO Tools";
            this.Load += new System.EventHandler(this.Container_Load);
            this.Resize += new System.EventHandler(this.ContainerResize);
            this.tabControl2.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPageAutobuffItem;
        private System.Windows.Forms.TabPage tabPageMacroSongs;
        private System.Windows.Forms.TabPage atkDef;
        private System.Windows.Forms.TabControl tabControlAutopot;
        private System.Windows.Forms.TabPage tabPageAutopot;
        private System.Windows.Forms.TabPage tabPageYggAutopot;
        private System.Windows.Forms.TabPage tabPageProfiles;
        private System.Windows.Forms.TabPage tabMacroSwitch;
        private System.Windows.Forms.TabPage tabPageSkillTimer;
        private System.Windows.Forms.TabPage tabPageDebuffs;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.Panel topSplitterPanel;
        private System.Windows.Forms.TabPage tabPageAutoOff;
        private System.Windows.Forms.TabControl tabControl2;
        private NoFocusButton btnToggleMiniMode;
    }
}