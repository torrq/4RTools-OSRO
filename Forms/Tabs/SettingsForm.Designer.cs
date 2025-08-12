using System;
using System.Windows.Forms;
using _ORTools.Utils;

namespace _ORTools.Forms
{
    partial class SettingsForm
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
            this.skillsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupSettings = new System.Windows.Forms.GroupBox();
            this.chkSoundEnabled = new System.Windows.Forms.CheckBox();
            this.chkStopBuffsOnCity = new System.Windows.Forms.CheckBox();
            this.toolTipDebugMode = new System.Windows.Forms.ToolTip(this.components);
            this.DebugMode = new System.Windows.Forms.CheckBox();
            this.DebugModeShowLog = new System.Windows.Forms.CheckBox();
            this.groupGlobalSettings = new System.Windows.Forms.GroupBox();
            this.toolTipchkSoundEnabled = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipchkStopBuffsOnCity = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipReqRestart = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipDebugModeShowLog = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.MacroSwitchRowsLabel = new System.Windows.Forms.Label();
            this.MacroSwitchRows = new System.Windows.Forms.NumericUpDown();
            this.SongRowsLabel = new System.Windows.Forms.Label();
            this.SongRows = new System.Windows.Forms.NumericUpDown();
            this.clientDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupSettings.SuspendLayout();
            this.groupGlobalSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MacroSwitchRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SongRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // skillsListBox
            // 
            this.skillsListBox.AllowDrop = true;
            this.skillsListBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skillsListBox.FormattingEnabled = true;
            this.skillsListBox.ItemHeight = 16;
            this.skillsListBox.Location = new System.Drawing.Point(13, 25);
            this.skillsListBox.Name = "skillsListBox";
            this.skillsListBox.Size = new System.Drawing.Size(273, 388);
            this.skillsListBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Autobuff skill order";
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.chkSoundEnabled);
            this.groupSettings.Controls.Add(this.chkStopBuffsOnCity);
            this.groupSettings.Location = new System.Drawing.Point(309, 20);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(300, 266);
            this.groupSettings.TabIndex = 0;
            this.groupSettings.TabStop = false;
            this.groupSettings.Text = "Profile Settings";
            // 
            // chkSoundEnabled
            // 
            this.chkSoundEnabled.AutoSize = true;
            this.chkSoundEnabled.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSoundEnabled.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSoundEnabled.Location = new System.Drawing.Point(13, 68);
            this.chkSoundEnabled.Name = "chkSoundEnabled";
            this.chkSoundEnabled.Size = new System.Drawing.Size(86, 20);
            this.chkSoundEnabled.TabIndex = 317;
            this.chkSoundEnabled.Text = "Sounds on";
            this.toolTipchkSoundEnabled.SetToolTip(this.chkSoundEnabled, "Play sounds when toggling on and off");
            this.chkSoundEnabled.UseVisualStyleBackColor = true;
            this.chkSoundEnabled.CheckedChanged += new System.EventHandler(this.ChkSoundEnabled_CheckedChanged);
            // 
            // chkStopBuffsOnCity
            // 
            this.chkStopBuffsOnCity.AutoSize = true;
            this.chkStopBuffsOnCity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkStopBuffsOnCity.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStopBuffsOnCity.Location = new System.Drawing.Point(13, 27);
            this.chkStopBuffsOnCity.Name = "chkStopBuffsOnCity";
            this.chkStopBuffsOnCity.Size = new System.Drawing.Size(141, 20);
            this.chkStopBuffsOnCity.TabIndex = 0;
            this.chkStopBuffsOnCity.Text = "Pause when in town";
            this.chkStopBuffsOnCity.UseVisualStyleBackColor = true;
            this.chkStopBuffsOnCity.CheckedChanged += new System.EventHandler(this.ChkStopBuffsOnCity_CheckedChanged);
            // 
            // toolTipDebugMode
            // 
            this.toolTipDebugMode.AutoPopDelay = 15000;
            this.toolTipDebugMode.InitialDelay = 300;
            this.toolTipDebugMode.ReshowDelay = 100;
            this.toolTipDebugMode.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipDebugMode.ToolTipTitle = "Debug Mode";
            // 
            // DebugMode
            // 
            this.DebugMode.AutoSize = true;
            this.DebugMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DebugMode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DebugMode.Location = new System.Drawing.Point(3, 3);
            this.DebugMode.Name = "DebugMode";
            this.DebugMode.Size = new System.Drawing.Size(96, 18);
            this.DebugMode.TabIndex = 320;
            this.DebugMode.Text = "Debug Mode";
            this.toolTipDebugMode.SetToolTip(this.DebugMode, "Toggles Debug Mode, which logs useful dev info to debug.log");
            this.DebugMode.UseVisualStyleBackColor = true;
            this.DebugMode.CheckedChanged += new System.EventHandler(this.DebugMode_CheckedChanged);
            // 
            // DebugModeShowLog
            // 
            this.DebugModeShowLog.AutoSize = true;
            this.DebugModeShowLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DebugModeShowLog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DebugModeShowLog.Location = new System.Drawing.Point(114, 3);
            this.DebugModeShowLog.Name = "DebugModeShowLog";
            this.DebugModeShowLog.Size = new System.Drawing.Size(81, 18);
            this.DebugModeShowLog.TabIndex = 321;
            this.DebugModeShowLog.Text = "Show Log";
            this.toolTipDebugModeShowLog.SetToolTip(this.DebugModeShowLog, "Adds a debug/console log to the bottom of the app");
            this.DebugModeShowLog.UseVisualStyleBackColor = true;
            // 
            // groupGlobalSettings
            // 
            this.groupGlobalSettings.Controls.Add(this.SongRows);
            this.groupGlobalSettings.Controls.Add(this.SongRowsLabel);
            this.groupGlobalSettings.Controls.Add(this.MacroSwitchRows);
            this.groupGlobalSettings.Controls.Add(this.MacroSwitchRowsLabel);
            this.groupGlobalSettings.Controls.Add(this.panel1);
            this.groupGlobalSettings.Location = new System.Drawing.Point(309, 292);
            this.groupGlobalSettings.Name = "groupGlobalSettings";
            this.groupGlobalSettings.Size = new System.Drawing.Size(300, 128);
            this.groupGlobalSettings.TabIndex = 318;
            this.groupGlobalSettings.TabStop = false;
            this.groupGlobalSettings.Text = "Global Settings";
            // 
            // toolTipReqRestart
            // 
            this.toolTipReqRestart.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipReqRestart.ToolTipTitle = "Debug Mode";
            // 
            // toolTipDebugModeShowLog
            // 
            this.toolTipDebugModeShowLog.AutoPopDelay = 15000;
            this.toolTipDebugModeShowLog.InitialDelay = 300;
            this.toolTipDebugModeShowLog.ReshowDelay = 100;
            this.toolTipDebugModeShowLog.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipDebugModeShowLog.ToolTipTitle = "Show Debug Log";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DebugMode);
            this.panel1.Controls.Add(this.DebugModeShowLog);
            this.panel1.Location = new System.Drawing.Point(52, 96);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 23);
            this.panel1.TabIndex = 323;
            // 
            // MacroSwitchRowsLabel
            // 
            this.MacroSwitchRowsLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MacroSwitchRowsLabel.Location = new System.Drawing.Point(58, 66);
            this.MacroSwitchRowsLabel.Name = "MacroSwitchRowsLabel";
            this.MacroSwitchRowsLabel.Size = new System.Drawing.Size(113, 14);
            this.MacroSwitchRowsLabel.TabIndex = 324;
            this.MacroSwitchRowsLabel.Text = "Macro Switch Rows";
            this.MacroSwitchRowsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MacroSwitchRows
            // 
            this.MacroSwitchRows.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MacroSwitchRows.Location = new System.Drawing.Point(179, 62);
            this.MacroSwitchRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MacroSwitchRows.Name = "MacroSwitchRows";
            this.MacroSwitchRows.Size = new System.Drawing.Size(50, 22);
            this.MacroSwitchRows.TabIndex = 325;
            this.MacroSwitchRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MacroSwitchRows.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // SongRowsLabel
            // 
            this.SongRowsLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SongRowsLabel.Location = new System.Drawing.Point(58, 30);
            this.SongRowsLabel.Name = "SongRowsLabel";
            this.SongRowsLabel.Size = new System.Drawing.Size(113, 14);
            this.SongRowsLabel.TabIndex = 326;
            this.SongRowsLabel.Text = "Song Rows";
            this.SongRowsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SongRows
            // 
            this.SongRows.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SongRows.Location = new System.Drawing.Point(179, 26);
            this.SongRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SongRows.Name = "SongRows";
            this.SongRows.Size = new System.Drawing.Size(50, 22);
            this.SongRows.TabIndex = 327;
            this.SongRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SongRows.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // clientDTOBindingSource
            // 
            this.clientDTOBindingSource.DataSource = typeof(_ORTools.Model.ClientDTO);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(630, 440);
            this.Controls.Add(this.groupGlobalSettings);
            this.Controls.Add(this.groupSettings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.skillsListBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingsForm";
            this.Text = "ConfigForm";
            this.groupSettings.ResumeLayout(false);
            this.groupSettings.PerformLayout();
            this.groupGlobalSettings.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MacroSwitchRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SongRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      
        #endregion
        private System.Windows.Forms.BindingSource clientDTOBindingSource;
        private ListBox skillsListBox;
        private Label label2;
        private GroupBox groupSettings;
        private CheckBox chkStopBuffsOnCity;
        private ToolTip toolTipDebugMode;
        private ToolTip toolTipchkSoundEnabled;
        private ToolTip toolTipchkStopBuffsOnCity;
        private CheckBox chkSoundEnabled;
        private GroupBox groupGlobalSettings;
        private CheckBox DebugMode;
        private ToolTip toolTipReqRestart;
        private CheckBox DebugModeShowLog;
        private ToolTip toolTipDebugModeShowLog;
        private Panel panel1;
        private Label MacroSwitchRowsLabel;
        private NumericUpDown SongRows;
        private Label SongRowsLabel;
        private NumericUpDown MacroSwitchRows;
    }
}