using System;
using System.Windows.Forms;

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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip5 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip6 = new System.Windows.Forms.ToolTip(this.components);
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.btnChangeLanguage = new System.Windows.Forms.Button();
            this.groupGlobalSettings = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkDebugMode = new System.Windows.Forms.CheckBox();
            this.clientDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Order of using autobuffs";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.chkSoundEnabled);
            this.groupSettings.Controls.Add(this.groupOverweight);
            this.groupSettings.Controls.Add(this.ammo2textBox);
            this.groupSettings.Controls.Add(this.ammo1textBox);
            this.groupSettings.Controls.Add(this.switchAmmoCheckBox);
            this.groupSettings.Controls.Add(this.chkStopBuffsOnCity);
            this.groupSettings.Location = new System.Drawing.Point(309, 20);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(300, 287);
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
            this.chkSoundEnabled.Size = new System.Drawing.Size(121, 21);
            this.chkSoundEnabled.TabIndex = 317;
            this.chkSoundEnabled.Text = "Sounds Enabled";
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
            this.groupOverweight.Location = new System.Drawing.Point(6, 172);
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
            this.label1.Location = new System.Drawing.Point(122, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 17);
            this.label1.TabIndex = 316;
            this.label1.Text = "Also send ALT -";
            // 
            // overweight50
            // 
            this.overweight50.AutoSize = true;
            this.overweight50.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overweight50.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweight50.ForeColor = System.Drawing.Color.Crimson;
            this.overweight50.Location = new System.Drawing.Point(38, 43);
            this.overweight50.Name = "overweight50";
            this.overweight50.Size = new System.Drawing.Size(95, 21);
            this.overweight50.TabIndex = 313;
            this.overweight50.TabStop = true;
            this.overweight50.Text = "50% Weight";
            this.overweight50.UseVisualStyleBackColor = true;
            this.overweight50.CheckedChanged += new System.EventHandler(this.OverweightMode_CheckedChanged);
            // 
            // overweightKey
            // 
            this.overweightKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overweightKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightKey.Location = new System.Drawing.Point(220, 76);
            this.overweightKey.Name = "overweightKey";
            this.overweightKey.Size = new System.Drawing.Size(55, 25);
            this.overweightKey.TabIndex = 311;
            this.overweightKey.TextChanged += new System.EventHandler(this.OverweightKey_TextChanged);
            // 
            // overweightOff
            // 
            this.overweightOff.AutoSize = true;
            this.overweightOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overweightOff.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightOff.Location = new System.Drawing.Point(38, 21);
            this.overweightOff.Name = "overweightOff";
            this.overweightOff.Size = new System.Drawing.Size(73, 21);
            this.overweightOff.TabIndex = 315;
            this.overweightOff.TabStop = true;
            this.overweightOff.Text = "Disabled";
            this.overweightOff.UseVisualStyleBackColor = true;
            this.overweightOff.CheckedChanged += new System.EventHandler(this.OverweightMode_CheckedChanged);
            // 
            // overweight90
            // 
            this.overweight90.AutoSize = true;
            this.overweight90.Cursor = System.Windows.Forms.Cursors.Hand;
            this.overweight90.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweight90.ForeColor = System.Drawing.Color.Crimson;
            this.overweight90.Location = new System.Drawing.Point(141, 43);
            this.overweight90.Name = "overweight90";
            this.overweight90.Size = new System.Drawing.Size(95, 21);
            this.overweight90.TabIndex = 314;
            this.overweight90.TabStop = true;
            this.overweight90.Text = "90% Weight";
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
            // 
            // ammo1textBox
            // 
            this.ammo1textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ammo1textBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ammo1textBox.Location = new System.Drawing.Point(192, 107);
            this.ammo1textBox.Name = "ammo1textBox";
            this.ammo1textBox.Size = new System.Drawing.Size(45, 25);
            this.ammo1textBox.TabIndex = 308;
            // 
            // switchAmmoCheckBox
            // 
            this.switchAmmoCheckBox.AutoSize = true;
            this.switchAmmoCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchAmmoCheckBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.switchAmmoCheckBox.Location = new System.Drawing.Point(13, 107);
            this.switchAmmoCheckBox.Name = "switchAmmoCheckBox";
            this.switchAmmoCheckBox.Size = new System.Drawing.Size(162, 21);
            this.switchAmmoCheckBox.TabIndex = 307;
            this.switchAmmoCheckBox.Text = "Automatic Ammo Swap";
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
            this.chkStopBuffsOnCity.Size = new System.Drawing.Size(266, 21);
            this.chkStopBuffsOnCity.TabIndex = 0;
            this.chkStopBuffsOnCity.Text = "Pause autopot, buffs && skill timer in town";
            this.chkStopBuffsOnCity.UseVisualStyleBackColor = true;
            this.chkStopBuffsOnCity.CheckedChanged += new System.EventHandler(this.ChkStopBuffsOnCity_CheckedChanged);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Location = new System.Drawing.Point(156, 37);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(80, 23);
            this.cmbLanguage.TabIndex = 318;
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnChangeLanguage
            // 
            this.btnChangeLanguage.BackColor = System.Drawing.Color.White;
            this.btnChangeLanguage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangeLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeLanguage.Location = new System.Drawing.Point(48, 36);
            this.btnChangeLanguage.Name = "btnChangeLanguage";
            this.btnChangeLanguage.Size = new System.Drawing.Size(100, 25);
            this.btnChangeLanguage.TabIndex = 319;
            this.btnChangeLanguage.Text = "Set Language";
            this.toolTip1.SetToolTip(this.btnChangeLanguage, "Changes application language");
            this.btnChangeLanguage.UseVisualStyleBackColor = false;
            this.btnChangeLanguage.Click += new System.EventHandler(this.btnChangeLanguage_Click);
            // 
            // groupGlobalSettings
            // 
            this.groupGlobalSettings.Controls.Add(this.label3);
            this.groupGlobalSettings.Controls.Add(this.chkDebugMode);
            this.groupGlobalSettings.Controls.Add(this.btnChangeLanguage);
            this.groupGlobalSettings.Controls.Add(this.cmbLanguage);
            this.groupGlobalSettings.Location = new System.Drawing.Point(309, 313);
            this.groupGlobalSettings.Name = "groupGlobalSettings";
            this.groupGlobalSettings.Size = new System.Drawing.Size(300, 107);
            this.groupGlobalSettings.TabIndex = 318;
            this.groupGlobalSettings.TabStop = false;
            this.groupGlobalSettings.Text = "Global Settings";
            this.toolTip1.SetToolTip(this.groupGlobalSettings, "W");
            this.groupGlobalSettings.Enter += new System.EventHandler(this.groupGlobalSettings_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkRed;
            this.label3.Location = new System.Drawing.Point(221, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 321;
            this.label3.Text = "requires restart";
            // 
            // chkDebugMode
            // 
            this.chkDebugMode.AutoSize = true;
            this.chkDebugMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkDebugMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDebugMode.Location = new System.Drawing.Point(98, 71);
            this.chkDebugMode.Name = "chkDebugMode";
            this.chkDebugMode.Size = new System.Drawing.Size(95, 19);
            this.chkDebugMode.TabIndex = 320;
            this.chkDebugMode.Text = "Debug Mode";
            this.toolTip1.SetToolTip(this.chkDebugMode, "Toggles debug mode, which logs info to a text file");
            this.chkDebugMode.UseVisualStyleBackColor = true;
            this.chkDebugMode.CheckedChanged += new System.EventHandler(this.chkDebugMode_CheckedChanged);
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
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolTip toolTip3;
        private ToolTip toolTip4;
        private ToolTip toolTip5;
        private ToolTip toolTip6;
        private TextBox overweightKey;
        private RadioButton overweight90;
        private RadioButton overweight50;
        private RadioButton overweightOff;
        private GroupBox groupOverweight;
        private Label label1;
        private CheckBox chkSoundEnabled;
        private ComboBox cmbLanguage;
        private Button btnChangeLanguage;
        private GroupBox groupGlobalSettings;
        private CheckBox chkDebugMode;
        private Label label3;
    }
}