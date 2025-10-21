using System;
using System.Windows.Forms;
using BruteGamingMacros.Core.Utils;

namespace BruteGamingMacros.UI.Forms
{
    partial class ConfigForm
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
            this.groupOverweight = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.overweight50 = new System.Windows.Forms.RadioButton();
            this.overweightKey = new System.Windows.Forms.TextBox();
            this.overweightOff = new System.Windows.Forms.RadioButton();
            this.overweight90 = new System.Windows.Forms.RadioButton();
            this.ammo2textBox = new System.Windows.Forms.TextBox();
            this.ammo1textBox = new System.Windows.Forms.TextBox();
            this.switchAmmoCheckBox = new System.Windows.Forms.CheckBox();
            this.chkStopBuffsOnCity = new System.Windows.Forms.CheckBox();
            this.toolTipDebugMode = new System.Windows.Forms.ToolTip(this.components);
            this.chkDebugMode = new System.Windows.Forms.CheckBox();
            this.groupGlobalSettings = new System.Windows.Forms.GroupBox();
            this.toolTipchkSoundEnabled = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipAmmo1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipAmmo2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipOverweightKey = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipchkStopBuffsOnCity = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipReqRestart = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipWeight50 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipWeight90 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipSwitchAmmoCB = new System.Windows.Forms.ToolTip(this.components);
            this.ammoTrigger = new System.Windows.Forms.TextBox();
            this.ammoTriggerLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.clientDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolTipAmmoTrigger = new System.Windows.Forms.ToolTip(this.components);
            this.groupSettings.SuspendLayout();
            this.groupOverweight.SuspendLayout();
            this.groupGlobalSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // skillsListBox
            // 
            this.skillsListBox.AllowDrop = true;
            this.skillsListBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skillsListBox.FormattingEnabled = true;
            this.skillsListBox.ItemHeight = 17;
            this.skillsListBox.Location = new System.Drawing.Point(13, 25);
            this.skillsListBox.Name = "skillsListBox";
            this.skillsListBox.Size = new System.Drawing.Size(273, 395);
            this.skillsListBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Autobuff skill order";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.label4);
            this.groupSettings.Controls.Add(this.label3);
            this.groupSettings.Controls.Add(this.ammoTriggerLabel);
            this.groupSettings.Controls.Add(this.ammoTrigger);
            this.groupSettings.Controls.Add(this.chkSoundEnabled);
            this.groupSettings.Controls.Add(this.groupOverweight);
            this.groupSettings.Controls.Add(this.ammo2textBox);
            this.groupSettings.Controls.Add(this.ammo1textBox);
            this.groupSettings.Controls.Add(this.switchAmmoCheckBox);
            this.groupSettings.Controls.Add(this.chkStopBuffsOnCity);
            this.groupSettings.Location = new System.Drawing.Point(309, 20);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(300, 341);
            this.groupSettings.TabIndex = 0;
            this.groupSettings.TabStop = false;
            this.groupSettings.Text = "Profile Settings";
            this.groupSettings.Enter += new System.EventHandler(this.groupSettings_Enter);
            // 
            // chkSoundEnabled
            // 
            this.chkSoundEnabled.AutoSize = true;
            this.chkSoundEnabled.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkSoundEnabled.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSoundEnabled.Location = new System.Drawing.Point(13, 68);
            this.chkSoundEnabled.Name = "chkSoundEnabled";
            this.chkSoundEnabled.Size = new System.Drawing.Size(89, 21);
            this.chkSoundEnabled.TabIndex = 317;
            this.chkSoundEnabled.Text = "Sounds on";
            this.toolTipchkSoundEnabled.SetToolTip(this.chkSoundEnabled, "Play sounds when toggling on and off");
            this.chkSoundEnabled.UseVisualStyleBackColor = true;
            this.chkSoundEnabled.CheckedChanged += new System.EventHandler(this.ChkSoundEnabled_CheckedChanged);
            // 
            // groupOverweight
            // 
            this.groupOverweight.Controls.Add(this.label1);
            this.groupOverweight.Controls.Add(this.overweight50);
            this.groupOverweight.Controls.Add(this.overweightKey);
            this.groupOverweight.Controls.Add(this.overweightOff);
            this.groupOverweight.Controls.Add(this.overweight90);
            this.groupOverweight.Location = new System.Drawing.Point(6, 226);
            this.groupOverweight.Name = "groupOverweight";
            this.groupOverweight.Size = new System.Drawing.Size(288, 109);
            this.groupOverweight.TabIndex = 316;
            this.groupOverweight.TabStop = false;
            this.groupOverweight.Text = "Turn off when overweight";
            this.groupOverweight.Enter += new System.EventHandler(this.GroupBox1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(120, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 316;
            this.label1.Text = ".. also send Alt -";
            // 
            // overweight50
            // 
            this.overweight50.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overweight50.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweight50.ForeColor = System.Drawing.Color.Crimson;
            this.overweight50.Image = global::BruteGamingMacros.Resources.BruteGaming.Icons.weight50;
            this.overweight50.Location = new System.Drawing.Point(166, 30);
            this.overweight50.Name = "overweight50";
            this.overweight50.Size = new System.Drawing.Size(54, 40);
            this.overweight50.TabIndex = 313;
            this.overweight50.TabStop = true;
            this.overweight50.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTipWeight50.SetToolTip(this.overweight50, "Auto-disable when weight is over 50%");
            this.overweight50.UseVisualStyleBackColor = true;
            this.overweight50.CheckedChanged += new System.EventHandler(this.OverweightMode_CheckedChanged);
            // 
            // overweightKey
            // 
            this.overweightKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overweightKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightKey.Location = new System.Drawing.Point(222, 76);
            this.overweightKey.Name = "overweightKey";
            this.overweightKey.Size = new System.Drawing.Size(55, 25);
            this.overweightKey.TabIndex = 311;
            this.toolTipOverweightKey.SetToolTip(this.overweightKey, "Alt-# macro to send when overweight. Tip: set this to your @aaoff macro in RO!");
            this.overweightKey.TextChanged += new System.EventHandler(this.OverweightKey_TextChanged);
            // 
            // overweightOff
            // 
            this.overweightOff.AutoSize = true;
            this.overweightOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overweightOff.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightOff.Location = new System.Drawing.Point(45, 40);
            this.overweightOff.Name = "overweightOff";
            this.overweightOff.Size = new System.Drawing.Size(77, 21);
            this.overweightOff.TabIndex = 315;
            this.overweightOff.TabStop = true;
            this.overweightOff.Text = "Disabled";
            this.overweightOff.UseVisualStyleBackColor = true;
            this.overweightOff.CheckedChanged += new System.EventHandler(this.OverweightMode_CheckedChanged);
            // 
            // overweight90
            // 
            this.overweight90.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overweight90.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweight90.ForeColor = System.Drawing.Color.Crimson;
            this.overweight90.Image = global::BruteGamingMacros.Resources.BruteGaming.Icons.weight90;
            this.overweight90.Location = new System.Drawing.Point(228, 30);
            this.overweight90.Name = "overweight90";
            this.overweight90.Size = new System.Drawing.Size(54, 40);
            this.overweight90.TabIndex = 314;
            this.overweight90.TabStop = true;
            this.toolTipWeight90.SetToolTip(this.overweight90, "Auto-disable when weight is over 90%");
            this.overweight90.UseVisualStyleBackColor = true;
            this.overweight90.CheckedChanged += new System.EventHandler(this.OverweightMode_CheckedChanged);
            // 
            // ammo2textBox
            // 
            this.ammo2textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammo2textBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammo2textBox.Location = new System.Drawing.Point(243, 107);
            this.ammo2textBox.Name = "ammo2textBox";
            this.ammo2textBox.Size = new System.Drawing.Size(45, 25);
            this.ammo2textBox.TabIndex = 309;
            this.toolTipAmmo2.SetToolTip(this.ammo2textBox, "Ammo #2");
            // 
            // ammo1textBox
            // 
            this.ammo1textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammo1textBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammo1textBox.Location = new System.Drawing.Point(192, 107);
            this.ammo1textBox.Name = "ammo1textBox";
            this.ammo1textBox.Size = new System.Drawing.Size(45, 25);
            this.ammo1textBox.TabIndex = 308;
            this.toolTipAmmo1.SetToolTip(this.ammo1textBox, "Ammo #1");
            // 
            // switchAmmoCheckBox
            // 
            this.switchAmmoCheckBox.AutoSize = true;
            this.switchAmmoCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchAmmoCheckBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.switchAmmoCheckBox.Location = new System.Drawing.Point(13, 107);
            this.switchAmmoCheckBox.Name = "switchAmmoCheckBox";
            this.switchAmmoCheckBox.Size = new System.Drawing.Size(119, 21);
            this.switchAmmoCheckBox.TabIndex = 307;
            this.switchAmmoCheckBox.Text = "Ammo swapper";
            this.toolTipSwitchAmmoCB.SetToolTip(this.switchAmmoCheckBox, "Switch between ammunition");
            this.switchAmmoCheckBox.UseVisualStyleBackColor = true;
            this.switchAmmoCheckBox.CheckedChanged += new System.EventHandler(this.SwitchAmmoCheckBox_CheckedChanged);
            // 
            // chkStopBuffsOnCity
            // 
            this.chkStopBuffsOnCity.AutoSize = true;
            this.chkStopBuffsOnCity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkStopBuffsOnCity.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStopBuffsOnCity.Location = new System.Drawing.Point(13, 27);
            this.chkStopBuffsOnCity.Name = "chkStopBuffsOnCity";
            this.chkStopBuffsOnCity.Size = new System.Drawing.Size(141, 21);
            this.chkStopBuffsOnCity.TabIndex = 0;
            this.chkStopBuffsOnCity.Text = "Pause when in town";
            this.chkStopBuffsOnCity.UseVisualStyleBackColor = true;
            this.chkStopBuffsOnCity.CheckedChanged += new System.EventHandler(this.ChkStopBuffsOnCity_CheckedChanged);
            // 
            // chkDebugMode
            // 
            this.chkDebugMode.AutoSize = true;
            this.chkDebugMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDebugMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDebugMode.Location = new System.Drawing.Point(94, 21);
            this.chkDebugMode.Name = "chkDebugMode";
            this.chkDebugMode.Size = new System.Drawing.Size(118, 19);
            this.chkDebugMode.TabIndex = 320;
            this.chkDebugMode.Text = "Debug Log Mode";
            this.toolTipDebugMode.SetToolTip(this.chkDebugMode, "Toggles debug mode, which logs info to a text file");
            this.chkDebugMode.UseVisualStyleBackColor = true;
            this.chkDebugMode.CheckedChanged += new System.EventHandler(this.chkDebugMode_CheckedChanged);
            // 
            // groupGlobalSettings
            // 
            this.groupGlobalSettings.Controls.Add(this.chkDebugMode);
            this.groupGlobalSettings.Location = new System.Drawing.Point(309, 367);
            this.groupGlobalSettings.Name = "groupGlobalSettings";
            this.groupGlobalSettings.Size = new System.Drawing.Size(300, 53);
            this.groupGlobalSettings.TabIndex = 318;
            this.groupGlobalSettings.TabStop = false;
            this.groupGlobalSettings.Text = "Global Settings";
            this.groupGlobalSettings.Enter += new System.EventHandler(this.groupGlobalSettings_Enter);
            // 
            // toolTipAmmo1
            // 
            this.toolTipAmmo1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip3_Popup);
            // 
            // toolTipOverweightKey
            // 
            this.toolTipOverweightKey.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip5_Popup);
            // 
            // toolTipWeight90
            // 
            this.toolTipWeight90.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipWeight90_Popup);
            // 
            // ammoTrigger
            // 
            this.ammoTrigger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammoTrigger.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammoTrigger.Location = new System.Drawing.Point(141, 107);
            this.ammoTrigger.Name = "ammoTrigger";
            this.ammoTrigger.Size = new System.Drawing.Size(45, 25);
            this.ammoTrigger.TabIndex = 318;
            this.toolTipAmmoTrigger.SetToolTip(this.ammoTrigger, "Ammo Swap key");
            // 
            // ammoTriggerLabel
            // 
            this.ammoTriggerLabel.AutoSize = true;
            this.ammoTriggerLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammoTriggerLabel.Location = new System.Drawing.Point(140, 135);
            this.ammoTriggerLabel.Name = "ammoTriggerLabel";
            this.ammoTriggerLabel.Size = new System.Drawing.Size(41, 13);
            this.ammoTriggerLabel.TabIndex = 319;
            this.ammoTriggerLabel.Text = "Trigger";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(189, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 320;
            this.label3.Text = "Ammo 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(239, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 321;
            this.label4.Text = "Ammo 2";
            // 
            // clientDTOBindingSource
            // 
            this.clientDTOBindingSource.DataSource = typeof(_4RTools.Model.ClientDTO);
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
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            this.groupSettings.ResumeLayout(false);
            this.groupSettings.PerformLayout();
            this.groupOverweight.ResumeLayout(false);
            this.groupOverweight.PerformLayout();
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
        private TextBox ammo2textBox;
        private TextBox ammo1textBox;
        private CheckBox switchAmmoCheckBox;
        private ToolTip toolTipDebugMode;
        private ToolTip toolTipchkSoundEnabled;
        private ToolTip toolTipAmmo1;
        private ToolTip toolTipAmmo2;
        private ToolTip toolTipOverweightKey;
        private ToolTip toolTipchkStopBuffsOnCity;
        private TextBox overweightKey;
        private RadioButton overweight90;
        private RadioButton overweight50;
        private RadioButton overweightOff;
        private GroupBox groupOverweight;
        private Label label1;
        private CheckBox chkSoundEnabled;
        private GroupBox groupGlobalSettings;
        private CheckBox chkDebugMode;
        private ToolTip toolTipReqRestart;
        private ToolTip toolTipWeight50;
        private ToolTip toolTipWeight90;
        private ToolTip toolTipSwitchAmmoCB;
        private TextBox ammoTrigger;
        private Label label4;
        private Label label3;
        private Label ammoTriggerLabel;
        private ToolTip toolTipAmmoTrigger;
    }
}