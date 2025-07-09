using _4RTools.Utils;

namespace _4RTools.Forms
{
    partial class AutopotHPForm
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
            this.hpPct1 = new System.Windows.Forms.NumericUpDown();
            this.labelPercent1 = new System.Windows.Forms.Label();
            this.numAutopotDelay = new System.Windows.Forms.NumericUpDown();
            this.picBoxHP1 = new System.Windows.Forms.PictureBox();
            this.delayLabel = new System.Windows.Forms.Label();
            this.txtHPKey1 = new System.Windows.Forms.TextBox();
            this.chkStopOnCriticalInjury = new System.Windows.Forms.CheckBox();
            this.hpPanel1 = new System.Windows.Forms.Panel();
            this.HPEnabled1 = new System.Windows.Forms.CheckBox();
            this.hpPanel2 = new System.Windows.Forms.Panel();
            this.HPEnabled2 = new System.Windows.Forms.CheckBox();
            this.picBoxHP2 = new System.Windows.Forms.PictureBox();
            this.txtHPKey2 = new System.Windows.Forms.TextBox();
            this.hpPct2 = new System.Windows.Forms.NumericUpDown();
            this.hpPanel3 = new System.Windows.Forms.Panel();
            this.HPEnabled3 = new System.Windows.Forms.CheckBox();
            this.picBoxHP3 = new System.Windows.Forms.PictureBox();
            this.txtHPKey3 = new System.Windows.Forms.TextBox();
            this.hpPct3 = new System.Windows.Forms.NumericUpDown();
            this.hpPanel4 = new System.Windows.Forms.Panel();
            this.HPEnabled4 = new System.Windows.Forms.CheckBox();
            this.picBoxHP4 = new System.Windows.Forms.PictureBox();
            this.txtHPKey4 = new System.Windows.Forms.TextBox();
            this.hpPct4 = new System.Windows.Forms.NumericUpDown();
            this.delayPanel = new System.Windows.Forms.Panel();
            this.hpPanel5 = new System.Windows.Forms.Panel();
            this.HPEnabled5 = new System.Windows.Forms.CheckBox();
            this.picBoxHP5 = new System.Windows.Forms.PictureBox();
            this.txtHPKey5 = new System.Windows.Forms.TextBox();
            this.hpPct5 = new System.Windows.Forms.NumericUpDown();
            this.hpKeyColumnLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutopotDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP1)).BeginInit();
            this.hpPanel1.SuspendLayout();
            this.hpPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct2)).BeginInit();
            this.hpPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct3)).BeginInit();
            this.hpPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct4)).BeginInit();
            this.delayPanel.SuspendLayout();
            this.hpPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct5)).BeginInit();
            this.SuspendLayout();
            // 
            // hpPct1
            // 
            this.hpPct1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpPct1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpPct1.Location = new System.Drawing.Point(168, 3);
            this.hpPct1.Name = "hpPct1";
            this.hpPct1.Size = new System.Drawing.Size(48, 21);
            this.hpPct1.TabIndex = 39;
            this.hpPct1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hpPct1.ValueChanged += new System.EventHandler(this.NumHPPercent1_ValueChanged);
            // 
            // labelPercent1
            // 
            this.labelPercent1.AutoSize = true;
            this.labelPercent1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPercent1.Location = new System.Drawing.Point(175, 2);
            this.labelPercent1.Name = "labelPercent1";
            this.labelPercent1.Size = new System.Drawing.Size(23, 18);
            this.labelPercent1.TabIndex = 37;
            this.labelPercent1.Text = "%";
            // 
            // numAutopotDelay
            // 
            this.numAutopotDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numAutopotDelay.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAutopotDelay.Location = new System.Drawing.Point(11, 18);
            this.numAutopotDelay.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numAutopotDelay.Name = "numAutopotDelay";
            this.numAutopotDelay.Size = new System.Drawing.Size(48, 22);
            this.numAutopotDelay.TabIndex = 36;
            this.numAutopotDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAutopotDelay.ValueChanged += new System.EventHandler(this.NumDelay_ValueChanged);
            // 
            // picBoxHP1
            // 
            this.picBoxHP1.BackColor = System.Drawing.Color.Transparent;
            this.picBoxHP1.Image = global::_4RTools.Resources._4RTools.Icons.ygg;
            this.picBoxHP1.Location = new System.Drawing.Point(73, 0);
            this.picBoxHP1.Name = "picBoxHP1";
            this.picBoxHP1.Size = new System.Drawing.Size(24, 24);
            this.picBoxHP1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxHP1.TabIndex = 34;
            this.picBoxHP1.TabStop = false;
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delayLabel.Location = new System.Drawing.Point(4, 1);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(63, 15);
            this.delayLabel.TabIndex = 41;
            this.delayLabel.Text = "Delay (ms)";
            // 
            // txtHPKey1
            // 
            this.txtHPKey1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHPKey1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHPKey1.Location = new System.Drawing.Point(104, 3);
            this.txtHPKey1.Name = "txtHPKey1";
            this.txtHPKey1.Size = new System.Drawing.Size(55, 21);
            this.txtHPKey1.TabIndex = 43;
            this.txtHPKey1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHPKey1.TextChanged += new System.EventHandler(this.OnHPKey1Changed);
            // 
            // chkStopOnCriticalInjury
            // 
            this.chkStopOnCriticalInjury.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkStopOnCriticalInjury.Location = new System.Drawing.Point(253, 5);
            this.chkStopOnCriticalInjury.Name = "chkStopOnCriticalInjury";
            this.chkStopOnCriticalInjury.Size = new System.Drawing.Size(90, 35);
            this.chkStopOnCriticalInjury.TabIndex = 50;
            this.chkStopOnCriticalInjury.Text = "Stop on Critical Strike";
            this.chkStopOnCriticalInjury.UseVisualStyleBackColor = true;
            this.chkStopOnCriticalInjury.CheckedChanged += new System.EventHandler(this.ChkStopOnCriticalInjury_CheckedChanged);
            // 
            // hpPanel1
            // 
            this.hpPanel1.Controls.Add(this.HPEnabled1);
            this.hpPanel1.Controls.Add(this.picBoxHP1);
            this.hpPanel1.Controls.Add(this.txtHPKey1);
            this.hpPanel1.Controls.Add(this.hpPct1);
            this.hpPanel1.Location = new System.Drawing.Point(1, 22);
            this.hpPanel1.Name = "hpPanel1";
            this.hpPanel1.Size = new System.Drawing.Size(220, 24);
            this.hpPanel1.TabIndex = 51;
            // 
            // HPEnabled1
            // 
            this.HPEnabled1.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HPEnabled1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HPEnabled1.Location = new System.Drawing.Point(46, 1);
            this.HPEnabled1.Name = "HPEnabled1";
            this.HPEnabled1.Size = new System.Drawing.Size(24, 24);
            this.HPEnabled1.TabIndex = 44;
            this.HPEnabled1.UseVisualStyleBackColor = true;
            this.HPEnabled1.CheckedChanged += new System.EventHandler(this.ChkHPEnabled1_CheckedChanged);
            // 
            // hpPanel2
            // 
            this.hpPanel2.Controls.Add(this.HPEnabled2);
            this.hpPanel2.Controls.Add(this.picBoxHP2);
            this.hpPanel2.Controls.Add(this.txtHPKey2);
            this.hpPanel2.Controls.Add(this.hpPct2);
            this.hpPanel2.Location = new System.Drawing.Point(1, 46);
            this.hpPanel2.Name = "hpPanel2";
            this.hpPanel2.Size = new System.Drawing.Size(220, 24);
            this.hpPanel2.TabIndex = 52;
            // 
            // HPEnabled2
            // 
            this.HPEnabled2.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HPEnabled2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HPEnabled2.Location = new System.Drawing.Point(46, 1);
            this.HPEnabled2.Name = "HPEnabled2";
            this.HPEnabled2.Size = new System.Drawing.Size(24, 24);
            this.HPEnabled2.TabIndex = 45;
            this.HPEnabled2.UseVisualStyleBackColor = true;
            this.HPEnabled2.CheckedChanged += new System.EventHandler(this.ChkHPEnabled2_CheckedChanged);
            // 
            // picBoxHP2
            // 
            this.picBoxHP2.BackColor = System.Drawing.Color.Transparent;
            this.picBoxHP2.Image = global::_4RTools.Resources._4RTools.Icons.ygg_seed;
            this.picBoxHP2.Location = new System.Drawing.Point(73, 0);
            this.picBoxHP2.Name = "picBoxHP2";
            this.picBoxHP2.Size = new System.Drawing.Size(24, 24);
            this.picBoxHP2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxHP2.TabIndex = 34;
            this.picBoxHP2.TabStop = false;
            // 
            // txtHPKey2
            // 
            this.txtHPKey2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHPKey2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHPKey2.Location = new System.Drawing.Point(104, 3);
            this.txtHPKey2.Name = "txtHPKey2";
            this.txtHPKey2.Size = new System.Drawing.Size(55, 21);
            this.txtHPKey2.TabIndex = 43;
            this.txtHPKey2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHPKey2.TextChanged += new System.EventHandler(this.OnHPKey2Changed);
            // 
            // hpPct2
            // 
            this.hpPct2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpPct2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpPct2.Location = new System.Drawing.Point(168, 3);
            this.hpPct2.Name = "hpPct2";
            this.hpPct2.Size = new System.Drawing.Size(48, 21);
            this.hpPct2.TabIndex = 39;
            this.hpPct2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hpPct2.ValueChanged += new System.EventHandler(this.NumHPPercent2_ValueChanged);
            // 
            // hpPanel3
            // 
            this.hpPanel3.Controls.Add(this.HPEnabled3);
            this.hpPanel3.Controls.Add(this.picBoxHP3);
            this.hpPanel3.Controls.Add(this.txtHPKey3);
            this.hpPanel3.Controls.Add(this.hpPct3);
            this.hpPanel3.Location = new System.Drawing.Point(1, 70);
            this.hpPanel3.Name = "hpPanel3";
            this.hpPanel3.Size = new System.Drawing.Size(220, 24);
            this.hpPanel3.TabIndex = 53;
            // 
            // HPEnabled3
            // 
            this.HPEnabled3.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HPEnabled3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HPEnabled3.Location = new System.Drawing.Point(46, 1);
            this.HPEnabled3.Name = "HPEnabled3";
            this.HPEnabled3.Size = new System.Drawing.Size(24, 24);
            this.HPEnabled3.TabIndex = 46;
            this.HPEnabled3.UseVisualStyleBackColor = true;
            this.HPEnabled3.CheckedChanged += new System.EventHandler(this.ChkHPEnabled3_CheckedChanged);
            // 
            // picBoxHP3
            // 
            this.picBoxHP3.BackColor = System.Drawing.Color.Transparent;
            this.picBoxHP3.Image = global::_4RTools.Resources._4RTools.Icons.white_potion;
            this.picBoxHP3.Location = new System.Drawing.Point(73, 0);
            this.picBoxHP3.Name = "picBoxHP3";
            this.picBoxHP3.Size = new System.Drawing.Size(24, 24);
            this.picBoxHP3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxHP3.TabIndex = 34;
            this.picBoxHP3.TabStop = false;
            // 
            // txtHPKey3
            // 
            this.txtHPKey3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHPKey3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHPKey3.Location = new System.Drawing.Point(104, 3);
            this.txtHPKey3.Name = "txtHPKey3";
            this.txtHPKey3.Size = new System.Drawing.Size(55, 21);
            this.txtHPKey3.TabIndex = 43;
            this.txtHPKey3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHPKey3.TextChanged += new System.EventHandler(this.OnHPKey3Changed);
            // 
            // hpPct3
            // 
            this.hpPct3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpPct3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpPct3.Location = new System.Drawing.Point(168, 3);
            this.hpPct3.Name = "hpPct3";
            this.hpPct3.Size = new System.Drawing.Size(48, 21);
            this.hpPct3.TabIndex = 39;
            this.hpPct3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hpPct3.ValueChanged += new System.EventHandler(this.NumHPPercent3_ValueChanged);
            // 
            // hpPanel4
            // 
            this.hpPanel4.Controls.Add(this.HPEnabled4);
            this.hpPanel4.Controls.Add(this.picBoxHP4);
            this.hpPanel4.Controls.Add(this.txtHPKey4);
            this.hpPanel4.Controls.Add(this.hpPct4);
            this.hpPanel4.Location = new System.Drawing.Point(1, 94);
            this.hpPanel4.Name = "hpPanel4";
            this.hpPanel4.Size = new System.Drawing.Size(220, 24);
            this.hpPanel4.TabIndex = 54;
            // 
            // HPEnabled4
            // 
            this.HPEnabled4.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HPEnabled4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HPEnabled4.Location = new System.Drawing.Point(46, 1);
            this.HPEnabled4.Name = "HPEnabled4";
            this.HPEnabled4.Size = new System.Drawing.Size(24, 24);
            this.HPEnabled4.TabIndex = 47;
            this.HPEnabled4.UseVisualStyleBackColor = true;
            this.HPEnabled4.CheckedChanged += new System.EventHandler(this.ChkHPEnabled4_CheckedChanged);
            // 
            // picBoxHP4
            // 
            this.picBoxHP4.BackColor = System.Drawing.Color.Transparent;
            this.picBoxHP4.Image = global::_4RTools.Resources._4RTools.Icons.white_potion_slim;
            this.picBoxHP4.Location = new System.Drawing.Point(73, 0);
            this.picBoxHP4.Name = "picBoxHP4";
            this.picBoxHP4.Size = new System.Drawing.Size(24, 24);
            this.picBoxHP4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxHP4.TabIndex = 34;
            this.picBoxHP4.TabStop = false;
            // 
            // txtHPKey4
            // 
            this.txtHPKey4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHPKey4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHPKey4.Location = new System.Drawing.Point(104, 3);
            this.txtHPKey4.Name = "txtHPKey4";
            this.txtHPKey4.Size = new System.Drawing.Size(55, 21);
            this.txtHPKey4.TabIndex = 43;
            this.txtHPKey4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHPKey4.TextChanged += new System.EventHandler(this.OnHPKey4Changed);
            // 
            // hpPct4
            // 
            this.hpPct4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpPct4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpPct4.Location = new System.Drawing.Point(168, 3);
            this.hpPct4.Name = "hpPct4";
            this.hpPct4.Size = new System.Drawing.Size(48, 21);
            this.hpPct4.TabIndex = 39;
            this.hpPct4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hpPct4.ValueChanged += new System.EventHandler(this.NumHPPercent4_ValueChanged);
            // 
            // delayPanel
            // 
            this.delayPanel.Controls.Add(this.delayLabel);
            this.delayPanel.Controls.Add(this.numAutopotDelay);
            this.delayPanel.Location = new System.Drawing.Point(270, 102);
            this.delayPanel.Name = "delayPanel";
            this.delayPanel.Size = new System.Drawing.Size(71, 46);
            this.delayPanel.TabIndex = 56;
            // 
            // hpPanel5
            // 
            this.hpPanel5.Controls.Add(this.HPEnabled5);
            this.hpPanel5.Controls.Add(this.picBoxHP5);
            this.hpPanel5.Controls.Add(this.txtHPKey5);
            this.hpPanel5.Controls.Add(this.hpPct5);
            this.hpPanel5.Location = new System.Drawing.Point(1, 118);
            this.hpPanel5.Name = "hpPanel5";
            this.hpPanel5.Size = new System.Drawing.Size(220, 24);
            this.hpPanel5.TabIndex = 55;
            // 
            // HPEnabled5
            // 
            this.HPEnabled5.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HPEnabled5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HPEnabled5.Location = new System.Drawing.Point(46, 1);
            this.HPEnabled5.Name = "HPEnabled5";
            this.HPEnabled5.Size = new System.Drawing.Size(24, 24);
            this.HPEnabled5.TabIndex = 48;
            this.HPEnabled5.UseVisualStyleBackColor = true;
            this.HPEnabled5.CheckedChanged += new System.EventHandler(this.ChkHPEnabled5_CheckedChanged);
            // 
            // picBoxHP5
            // 
            this.picBoxHP5.BackColor = System.Drawing.Color.Transparent;
            this.picBoxHP5.Image = global::_4RTools.Resources._4RTools.Icons.mastela_fruit;
            this.picBoxHP5.Location = new System.Drawing.Point(73, 0);
            this.picBoxHP5.Name = "picBoxHP5";
            this.picBoxHP5.Size = new System.Drawing.Size(24, 24);
            this.picBoxHP5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxHP5.TabIndex = 34;
            this.picBoxHP5.TabStop = false;
            // 
            // txtHPKey5
            // 
            this.txtHPKey5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHPKey5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHPKey5.Location = new System.Drawing.Point(104, 3);
            this.txtHPKey5.Name = "txtHPKey5";
            this.txtHPKey5.Size = new System.Drawing.Size(55, 21);
            this.txtHPKey5.TabIndex = 43;
            this.txtHPKey5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHPKey5.TextChanged += new System.EventHandler(this.OnHPKey5Changed);
            // 
            // hpPct5
            // 
            this.hpPct5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hpPct5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpPct5.Location = new System.Drawing.Point(168, 3);
            this.hpPct5.Name = "hpPct5";
            this.hpPct5.Size = new System.Drawing.Size(48, 21);
            this.hpPct5.TabIndex = 39;
            this.hpPct5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hpPct5.ValueChanged += new System.EventHandler(this.NumHPPercent5_ValueChanged);
            // 
            // hpKeyColumnLabel
            // 
            this.hpKeyColumnLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpKeyColumnLabel.Image = global::_4RTools.Resources._4RTools.Icons.key_question;
            this.hpKeyColumnLabel.Location = new System.Drawing.Point(119, -1);
            this.hpKeyColumnLabel.Name = "hpKeyColumnLabel";
            this.hpKeyColumnLabel.Size = new System.Drawing.Size(25, 24);
            this.hpKeyColumnLabel.TabIndex = 65;
            // 
            // AutopotHPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(346, 153);
            this.Controls.Add(this.delayPanel);
            this.Controls.Add(this.hpKeyColumnLabel);
            this.Controls.Add(this.chkStopOnCriticalInjury);
            this.Controls.Add(this.hpPanel5);
            this.Controls.Add(this.hpPanel4);
            this.Controls.Add(this.hpPanel3);
            this.Controls.Add(this.hpPanel2);
            this.Controls.Add(this.hpPanel1);
            this.Controls.Add(this.labelPercent1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutopotHPForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutopotForm";
            this.Load += new System.EventHandler(this.AutopotHPForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.hpPct1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutopotDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP1)).EndInit();
            this.hpPanel1.ResumeLayout(false);
            this.hpPanel1.PerformLayout();
            this.hpPanel2.ResumeLayout(false);
            this.hpPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct2)).EndInit();
            this.hpPanel3.ResumeLayout(false);
            this.hpPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct3)).EndInit();
            this.hpPanel4.ResumeLayout(false);
            this.hpPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct4)).EndInit();
            this.delayPanel.ResumeLayout(false);
            this.delayPanel.PerformLayout();
            this.hpPanel5.ResumeLayout(false);
            this.hpPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hpPct5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown hpPct1;
        private System.Windows.Forms.Label labelPercent1;
        private System.Windows.Forms.PictureBox picBoxHP1;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.TextBox txtHPKey1;
        private System.Windows.Forms.CheckBox chkStopOnCriticalInjury;
        private System.Windows.Forms.NumericUpDown numAutopotDelay;
        private System.Windows.Forms.Panel hpPanel1;
        private System.Windows.Forms.Panel hpPanel2;
        private System.Windows.Forms.PictureBox picBoxHP2;
        private System.Windows.Forms.TextBox txtHPKey2;
        private System.Windows.Forms.NumericUpDown hpPct2;
        private System.Windows.Forms.Panel hpPanel3;
        private System.Windows.Forms.PictureBox picBoxHP3;
        private System.Windows.Forms.TextBox txtHPKey3;
        private System.Windows.Forms.NumericUpDown hpPct3;
        private System.Windows.Forms.Panel hpPanel4;
        private System.Windows.Forms.PictureBox picBoxHP4;
        private System.Windows.Forms.TextBox txtHPKey4;
        private System.Windows.Forms.NumericUpDown hpPct4;
        private System.Windows.Forms.Panel hpPanel5;
        private System.Windows.Forms.PictureBox picBoxHP5;
        private System.Windows.Forms.TextBox txtHPKey5;
        private System.Windows.Forms.NumericUpDown hpPct5;
        private System.Windows.Forms.Panel delayPanel;
        private System.Windows.Forms.Label hpKeyColumnLabel;
        private System.Windows.Forms.CheckBox HPEnabled1;
        private System.Windows.Forms.CheckBox HPEnabled2;
        private System.Windows.Forms.CheckBox HPEnabled3;
        private System.Windows.Forms.CheckBox HPEnabled4;
        private System.Windows.Forms.CheckBox HPEnabled5;
    }
}