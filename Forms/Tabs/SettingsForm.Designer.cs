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
            this.clientDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupSettings.SuspendLayout();
            this.groupGlobalSettings.SuspendLayout();
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
            this.groupSettings.Size = new System.Drawing.Size(300, 341);
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
            this.DebugMode.Location = new System.Drawing.Point(62, 21);
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
            this.DebugModeShowLog.Location = new System.Drawing.Point(173, 21);
            this.DebugModeShowLog.Name = "DebugModeShowLog";
            this.DebugModeShowLog.Size = new System.Drawing.Size(81, 18);
            this.DebugModeShowLog.TabIndex = 321;
            this.DebugModeShowLog.Text = "Show Log";
            this.toolTipDebugModeShowLog.SetToolTip(this.DebugModeShowLog, "Adds a debug/console log to the bottom of the app");
            this.DebugModeShowLog.UseVisualStyleBackColor = true;
            // 
            // groupGlobalSettings
            // 
            this.groupGlobalSettings.Controls.Add(this.DebugModeShowLog);
            this.groupGlobalSettings.Controls.Add(this.DebugMode);
            this.groupGlobalSettings.Location = new System.Drawing.Point(309, 367);
            this.groupGlobalSettings.Name = "groupGlobalSettings";
            this.groupGlobalSettings.Size = new System.Drawing.Size(300, 53);
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
            // clientDTOBindingSource
            // 
            this.clientDTOBindingSource.DataSource = typeof(_ORTools.Model.ClientDTO);
            // 
            // ConfigForm
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
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            this.groupSettings.ResumeLayout(false);
            this.groupSettings.PerformLayout();
            this.groupGlobalSettings.ResumeLayout(false);
            this.groupGlobalSettings.PerformLayout();
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
    }
}