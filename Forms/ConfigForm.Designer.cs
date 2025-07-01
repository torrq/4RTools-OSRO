using System;
using System.Windows.Forms;
using _4RTools.Utils;

namespace _4RTools.Forms
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ammoTriggerLabel = new System.Windows.Forms.Label();
            this.ammoTrigger = new System.Windows.Forms.TextBox();
            this.chkSoundEnabled = new System.Windows.Forms.CheckBox();
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
            this.toolTipWeight90 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipSwitchAmmoCB = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipAmmoTrigger = new System.Windows.Forms.ToolTip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.label4);
            this.groupSettings.Controls.Add(this.label3);
            this.groupSettings.Controls.Add(this.ammoTriggerLabel);
            this.groupSettings.Controls.Add(this.ammoTrigger);
            this.groupSettings.Controls.Add(this.chkSoundEnabled);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(239, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 321;
            this.label4.Text = "Ammo 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(189, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 320;
            this.label3.Text = "Ammo 1";
            // 
            // ammoTriggerLabel
            // 
            this.ammoTriggerLabel.AutoSize = true;
            this.ammoTriggerLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammoTriggerLabel.Location = new System.Drawing.Point(140, 135);
            this.ammoTriggerLabel.Name = "ammoTriggerLabel";
            this.ammoTriggerLabel.Size = new System.Drawing.Size(41, 13);
            this.ammoTriggerLabel.TabIndex = 319;
            this.ammoTriggerLabel.Text = "Trigger";
            // 
            // ammoTrigger
            // 
            this.ammoTrigger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammoTrigger.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammoTrigger.Location = new System.Drawing.Point(141, 107);
            this.ammoTrigger.Name = "ammoTrigger";
            this.ammoTrigger.Size = new System.Drawing.Size(45, 23);
            this.ammoTrigger.TabIndex = 318;
            this.toolTipAmmoTrigger.SetToolTip(this.ammoTrigger, "Ammo Swap key");
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
            // ammo2textBox
            // 
            this.ammo2textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammo2textBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammo2textBox.Location = new System.Drawing.Point(243, 107);
            this.ammo2textBox.Name = "ammo2textBox";
            this.ammo2textBox.Size = new System.Drawing.Size(45, 23);
            this.ammo2textBox.TabIndex = 309;
            this.toolTipAmmo2.SetToolTip(this.ammo2textBox, "Ammo #2");
            // 
            // ammo1textBox
            // 
            this.ammo1textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammo1textBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammo1textBox.Location = new System.Drawing.Point(192, 107);
            this.ammo1textBox.Name = "ammo1textBox";
            this.ammo1textBox.Size = new System.Drawing.Size(45, 23);
            this.ammo1textBox.TabIndex = 308;
            this.toolTipAmmo1.SetToolTip(this.ammo1textBox, "Ammo #1");
            // 
            // switchAmmoCheckBox
            // 
            this.switchAmmoCheckBox.AutoSize = true;
            this.switchAmmoCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchAmmoCheckBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.switchAmmoCheckBox.Location = new System.Drawing.Point(13, 107);
            this.switchAmmoCheckBox.Name = "switchAmmoCheckBox";
            this.switchAmmoCheckBox.Size = new System.Drawing.Size(116, 20);
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
            this.chkStopBuffsOnCity.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStopBuffsOnCity.Location = new System.Drawing.Point(13, 27);
            this.chkStopBuffsOnCity.Name = "chkStopBuffsOnCity";
            this.chkStopBuffsOnCity.Size = new System.Drawing.Size(141, 20);
            this.chkStopBuffsOnCity.TabIndex = 0;
            this.chkStopBuffsOnCity.Text = "Pause when in town";
            this.chkStopBuffsOnCity.UseVisualStyleBackColor = true;
            this.chkStopBuffsOnCity.CheckedChanged += new System.EventHandler(this.ChkStopBuffsOnCity_CheckedChanged);
            // 
            // chkDebugMode
            // 
            this.chkDebugMode.AutoSize = true;
            this.chkDebugMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDebugMode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDebugMode.Location = new System.Drawing.Point(94, 21);
            this.chkDebugMode.Name = "chkDebugMode";
            this.chkDebugMode.Size = new System.Drawing.Size(120, 18);
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
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
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
        private TextBox ammo2textBox;
        private TextBox ammo1textBox;
        private CheckBox switchAmmoCheckBox;
        private ToolTip toolTipDebugMode;
        private ToolTip toolTipchkSoundEnabled;
        private ToolTip toolTipAmmo1;
        private ToolTip toolTipAmmo2;
        private ToolTip toolTipOverweightKey;
        private ToolTip toolTipchkStopBuffsOnCity;
        private CheckBox chkSoundEnabled;
        private GroupBox groupGlobalSettings;
        private CheckBox chkDebugMode;
        private ToolTip toolTipReqRestart;
        private ToolTip toolTipWeight90;
        private ToolTip toolTipSwitchAmmoCB;
        private TextBox ammoTrigger;
        private Label label4;
        private Label label3;
        private Label ammoTriggerLabel;
        private ToolTip toolTipAmmoTrigger;
        private ImageList imageList1;
    }
}