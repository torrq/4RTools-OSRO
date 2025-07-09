using _4RTools.Utils;

namespace _4RTools.Forms
{
    partial class AutopotSPForm
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
            this.AutopotSPDelay = new System.Windows.Forms.NumericUpDown();
            this.picBoxSP1 = new System.Windows.Forms.PictureBox();
            this.delayLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.spKey1 = new System.Windows.Forms.TextBox();
            this.spLabel = new System.Windows.Forms.Label();
            this.spPct1 = new System.Windows.Forms.NumericUpDown();
            this.spPanel1 = new System.Windows.Forms.Panel();
            this.labelPercent2 = new System.Windows.Forms.Label();
            this.spPanel2 = new System.Windows.Forms.Panel();
            this.picBoxSP2 = new System.Windows.Forms.PictureBox();
            this.spKey2 = new System.Windows.Forms.TextBox();
            this.spPct2 = new System.Windows.Forms.NumericUpDown();
            this.spPanel3 = new System.Windows.Forms.Panel();
            this.picBoxSP3 = new System.Windows.Forms.PictureBox();
            this.spKey3 = new System.Windows.Forms.TextBox();
            this.spPct3 = new System.Windows.Forms.NumericUpDown();
            this.spPanel4 = new System.Windows.Forms.Panel();
            this.picBoxSP4 = new System.Windows.Forms.PictureBox();
            this.spKey4 = new System.Windows.Forms.TextBox();
            this.spPct4 = new System.Windows.Forms.NumericUpDown();
            this.panelCritStopAndDelay = new System.Windows.Forms.Panel();
            this.spPanel5 = new System.Windows.Forms.Panel();
            this.picBoxSP5 = new System.Windows.Forms.PictureBox();
            this.spKey5 = new System.Windows.Forms.TextBox();
            this.spPct5 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.AutopotSPDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct1)).BeginInit();
            this.spPanel1.SuspendLayout();
            this.spPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct2)).BeginInit();
            this.spPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct3)).BeginInit();
            this.spPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct4)).BeginInit();
            this.panelCritStopAndDelay.SuspendLayout();
            this.spPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct5)).BeginInit();
            this.SuspendLayout();
            // 
            // AutopotSPDelay
            // 
            this.AutopotSPDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AutopotSPDelay.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutopotSPDelay.Location = new System.Drawing.Point(245, 3);
            this.AutopotSPDelay.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.AutopotSPDelay.Name = "AutopotSPDelay";
            this.AutopotSPDelay.Size = new System.Drawing.Size(48, 22);
            this.AutopotSPDelay.TabIndex = 36;
            this.AutopotSPDelay.ValueChanged += new System.EventHandler(this.NumAutopotDelayTextChanged);
            // 
            // picBoxSP1
            // 
            this.picBoxSP1.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSP1.Image = global::_4RTools.Resources._4RTools.Icons.blue_potion;
            this.picBoxSP1.Location = new System.Drawing.Point(30, 0);
            this.picBoxSP1.Name = "picBoxSP1";
            this.picBoxSP1.Size = new System.Drawing.Size(24, 24);
            this.picBoxSP1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSP1.TabIndex = 35;
            this.picBoxSP1.TabStop = false;
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delayLabel.Location = new System.Drawing.Point(209, 6);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(36, 15);
            this.delayLabel.TabIndex = 41;
            this.delayLabel.Text = "Delay";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(294, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 15);
            this.label1.TabIndex = 42;
            this.label1.Text = "ms";
            // 
            // spKey1
            // 
            this.spKey1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spKey1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spKey1.Location = new System.Drawing.Point(132, 0);
            this.spKey1.Name = "spKey1";
            this.spKey1.Size = new System.Drawing.Size(65, 23);
            this.spKey1.TabIndex = 44;
            // 
            // spLabel
            // 
            this.spLabel.AutoSize = true;
            this.spLabel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spLabel.Location = new System.Drawing.Point(12, 2);
            this.spLabel.Name = "spLabel";
            this.spLabel.Size = new System.Drawing.Size(28, 18);
            this.spLabel.TabIndex = 46;
            this.spLabel.Text = "SP";
            this.spLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // spPct1
            // 
            this.spPct1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spPct1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spPct1.Location = new System.Drawing.Point(223, 0);
            this.spPct1.Name = "spPct1";
            this.spPct1.Size = new System.Drawing.Size(44, 23);
            this.spPct1.TabIndex = 40;
            this.spPct1.ValueChanged += new System.EventHandler(this.TxtSPpctTextChanged);
            // 
            // spPanel1
            // 
            this.spPanel1.Controls.Add(this.picBoxSP1);
            this.spPanel1.Controls.Add(this.spKey1);
            this.spPanel1.Controls.Add(this.spPct1);
            this.spPanel1.Location = new System.Drawing.Point(1, 23);
            this.spPanel1.Name = "spPanel1";
            this.spPanel1.Size = new System.Drawing.Size(319, 24);
            this.spPanel1.TabIndex = 52;
            // 
            // labelPercent2
            // 
            this.labelPercent2.AutoSize = true;
            this.labelPercent2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPercent2.Location = new System.Drawing.Point(234, 2);
            this.labelPercent2.Name = "labelPercent2";
            this.labelPercent2.Size = new System.Drawing.Size(23, 18);
            this.labelPercent2.TabIndex = 53;
            this.labelPercent2.Text = "%";
            // 
            // spPanel2
            // 
            this.spPanel2.Controls.Add(this.picBoxSP2);
            this.spPanel2.Controls.Add(this.spKey2);
            this.spPanel2.Controls.Add(this.spPct2);
            this.spPanel2.Location = new System.Drawing.Point(1, 47);
            this.spPanel2.Name = "spPanel2";
            this.spPanel2.Size = new System.Drawing.Size(319, 24);
            this.spPanel2.TabIndex = 53;
            // 
            // picBoxSP2
            // 
            this.picBoxSP2.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSP2.Image = global::_4RTools.Resources._4RTools.Icons.ygg;
            this.picBoxSP2.Location = new System.Drawing.Point(30, 0);
            this.picBoxSP2.Name = "picBoxSP2";
            this.picBoxSP2.Size = new System.Drawing.Size(24, 24);
            this.picBoxSP2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSP2.TabIndex = 35;
            this.picBoxSP2.TabStop = false;
            // 
            // spKey2
            // 
            this.spKey2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spKey2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spKey2.Location = new System.Drawing.Point(132, 0);
            this.spKey2.Name = "spKey2";
            this.spKey2.Size = new System.Drawing.Size(65, 23);
            this.spKey2.TabIndex = 44;
            // 
            // spPct2
            // 
            this.spPct2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spPct2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spPct2.Location = new System.Drawing.Point(223, 0);
            this.spPct2.Name = "spPct2";
            this.spPct2.Size = new System.Drawing.Size(44, 23);
            this.spPct2.TabIndex = 40;
            // 
            // spPanel3
            // 
            this.spPanel3.Controls.Add(this.picBoxSP3);
            this.spPanel3.Controls.Add(this.spKey3);
            this.spPanel3.Controls.Add(this.spPct3);
            this.spPanel3.Location = new System.Drawing.Point(1, 71);
            this.spPanel3.Name = "spPanel3";
            this.spPanel3.Size = new System.Drawing.Size(319, 24);
            this.spPanel3.TabIndex = 54;
            // 
            // picBoxSP3
            // 
            this.picBoxSP3.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSP3.Image = global::_4RTools.Resources._4RTools.Icons.ygg_seed;
            this.picBoxSP3.Location = new System.Drawing.Point(30, 0);
            this.picBoxSP3.Name = "picBoxSP3";
            this.picBoxSP3.Size = new System.Drawing.Size(24, 24);
            this.picBoxSP3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSP3.TabIndex = 35;
            this.picBoxSP3.TabStop = false;
            // 
            // spKey3
            // 
            this.spKey3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spKey3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spKey3.Location = new System.Drawing.Point(132, 0);
            this.spKey3.Name = "spKey3";
            this.spKey3.Size = new System.Drawing.Size(65, 23);
            this.spKey3.TabIndex = 44;
            // 
            // spPct3
            // 
            this.spPct3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spPct3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spPct3.Location = new System.Drawing.Point(223, 0);
            this.spPct3.Name = "spPct3";
            this.spPct3.Size = new System.Drawing.Size(44, 23);
            this.spPct3.TabIndex = 40;
            // 
            // spPanel4
            // 
            this.spPanel4.Controls.Add(this.picBoxSP4);
            this.spPanel4.Controls.Add(this.spKey4);
            this.spPanel4.Controls.Add(this.spPct4);
            this.spPanel4.Location = new System.Drawing.Point(1, 95);
            this.spPanel4.Name = "spPanel4";
            this.spPanel4.Size = new System.Drawing.Size(319, 24);
            this.spPanel4.TabIndex = 54;
            // 
            // picBoxSP4
            // 
            this.picBoxSP4.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSP4.Image = global::_4RTools.Resources._4RTools.Icons.royal_jelly;
            this.picBoxSP4.Location = new System.Drawing.Point(30, 0);
            this.picBoxSP4.Name = "picBoxSP4";
            this.picBoxSP4.Size = new System.Drawing.Size(24, 24);
            this.picBoxSP4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSP4.TabIndex = 35;
            this.picBoxSP4.TabStop = false;
            // 
            // spKey4
            // 
            this.spKey4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spKey4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spKey4.Location = new System.Drawing.Point(132, 0);
            this.spKey4.Name = "spKey4";
            this.spKey4.Size = new System.Drawing.Size(65, 23);
            this.spKey4.TabIndex = 44;
            // 
            // spPct4
            // 
            this.spPct4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spPct4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spPct4.Location = new System.Drawing.Point(223, 0);
            this.spPct4.Name = "spPct4";
            this.spPct4.Size = new System.Drawing.Size(44, 23);
            this.spPct4.TabIndex = 40;
            // 
            // panelCritStopAndDelay
            // 
            this.panelCritStopAndDelay.Controls.Add(this.delayLabel);
            this.panelCritStopAndDelay.Controls.Add(this.label1);
            this.panelCritStopAndDelay.Controls.Add(this.AutopotSPDelay);
            this.panelCritStopAndDelay.Location = new System.Drawing.Point(1, 169);
            this.panelCritStopAndDelay.Name = "panelCritStopAndDelay";
            this.panelCritStopAndDelay.Size = new System.Drawing.Size(319, 28);
            this.panelCritStopAndDelay.TabIndex = 55;
            // 
            // spPanel5
            // 
            this.spPanel5.Controls.Add(this.picBoxSP5);
            this.spPanel5.Controls.Add(this.spKey5);
            this.spPanel5.Controls.Add(this.spPct5);
            this.spPanel5.Location = new System.Drawing.Point(1, 119);
            this.spPanel5.Name = "spPanel5";
            this.spPanel5.Size = new System.Drawing.Size(319, 24);
            this.spPanel5.TabIndex = 55;
            // 
            // picBoxSP5
            // 
            this.picBoxSP5.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSP5.Image = global::_4RTools.Resources._4RTools.Icons.royal_jelly;
            this.picBoxSP5.Location = new System.Drawing.Point(30, 0);
            this.picBoxSP5.Name = "picBoxSP5";
            this.picBoxSP5.Size = new System.Drawing.Size(24, 24);
            this.picBoxSP5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSP5.TabIndex = 35;
            this.picBoxSP5.TabStop = false;
            // 
            // spKey5
            // 
            this.spKey5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spKey5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spKey5.Location = new System.Drawing.Point(132, 0);
            this.spKey5.Name = "spKey5";
            this.spKey5.Size = new System.Drawing.Size(65, 23);
            this.spKey5.TabIndex = 44;
            // 
            // spPct5
            // 
            this.spPct5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spPct5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spPct5.Location = new System.Drawing.Point(223, 0);
            this.spPct5.Name = "spPct5";
            this.spPct5.Size = new System.Drawing.Size(44, 23);
            this.spPct5.TabIndex = 40;
            // 
            // AutopotSPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(322, 209);
            this.Controls.Add(this.spPanel5);
            this.Controls.Add(this.panelCritStopAndDelay);
            this.Controls.Add(this.spPanel4);
            this.Controls.Add(this.spPanel3);
            this.Controls.Add(this.spPanel2);
            this.Controls.Add(this.labelPercent2);
            this.Controls.Add(this.spPanel1);
            this.Controls.Add(this.spLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutopotSPForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutopotForm";
            this.Load += new System.EventHandler(this.AutopotForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AutopotSPDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct1)).EndInit();
            this.spPanel1.ResumeLayout(false);
            this.spPanel1.PerformLayout();
            this.spPanel2.ResumeLayout(false);
            this.spPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct2)).EndInit();
            this.spPanel3.ResumeLayout(false);
            this.spPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct3)).EndInit();
            this.spPanel4.ResumeLayout(false);
            this.spPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct4)).EndInit();
            this.panelCritStopAndDelay.ResumeLayout(false);
            this.panelCritStopAndDelay.PerformLayout();
            this.spPanel5.ResumeLayout(false);
            this.spPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPct5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picBoxSP1;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox spKey1;
        private System.Windows.Forms.Label spLabel;
        private System.Windows.Forms.NumericUpDown spPct1;
        private System.Windows.Forms.NumericUpDown AutopotSPDelay;
        private System.Windows.Forms.Panel spPanel1;
        private System.Windows.Forms.Label labelPercent2;
        private System.Windows.Forms.Panel spPanel2;
        private System.Windows.Forms.PictureBox picBoxSP2;
        private System.Windows.Forms.TextBox spKey2;
        private System.Windows.Forms.NumericUpDown spPct2;
        private System.Windows.Forms.Panel spPanel3;
        private System.Windows.Forms.PictureBox picBoxSP3;
        private System.Windows.Forms.TextBox spKey3;
        private System.Windows.Forms.NumericUpDown spPct3;
        private System.Windows.Forms.Panel spPanel4;
        private System.Windows.Forms.PictureBox picBoxSP4;
        private System.Windows.Forms.TextBox spKey4;
        private System.Windows.Forms.NumericUpDown spPct4;
        private System.Windows.Forms.Panel panelCritStopAndDelay;
        private System.Windows.Forms.Panel spPanel5;
        private System.Windows.Forms.PictureBox picBoxSP5;
        private System.Windows.Forms.TextBox spKey5;
        private System.Windows.Forms.NumericUpDown spPct5;
    }
}