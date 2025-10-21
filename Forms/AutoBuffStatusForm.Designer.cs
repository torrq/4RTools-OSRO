using BruteGamingMacros.Core.Utils;

namespace BruteGamingMacros.UI.Forms
{
    partial class AutoBuffStatusForm
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
            this.toolTipPanacea = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DebuffsGP = new System.Windows.Forms.GroupBox();
            this.txtPanaceaKey = new System.Windows.Forms.TextBox();
            this.WeightDebuffsGP = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTipPanacea
            // 
            this.toolTipPanacea.AutomaticDelay = 10;
            this.toolTipPanacea.AutoPopDelay = 5000;
            this.toolTipPanacea.InitialDelay = 10;
            this.toolTipPanacea.ReshowDelay = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BruteGamingMacros.Resources.BruteGaming.Icons.panacea;
            this.pictureBox1.Location = new System.Drawing.Point(295, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 296;
            this.pictureBox1.TabStop = false;
            this.toolTipPanacea.SetToolTip(this.pictureBox1, "Panacea");
            // 
            // DebuffsGP
            // 
            this.DebuffsGP.AutoSize = true;
            this.DebuffsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DebuffsGP.Location = new System.Drawing.Point(33, 52);
            this.DebuffsGP.Name = "DebuffsGP";
            this.DebuffsGP.Size = new System.Drawing.Size(563, 29);
            this.DebuffsGP.TabIndex = 294;
            this.DebuffsGP.TabStop = false;
            this.DebuffsGP.Text = "Debuffs";
            // 
            // txtPanaceaKey
            // 
            this.txtPanaceaKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanaceaKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPanaceaKey.Location = new System.Drawing.Point(325, 12);
            this.txtPanaceaKey.Name = "txtPanaceaKey";
            this.txtPanaceaKey.Size = new System.Drawing.Size(45, 25);
            this.txtPanaceaKey.TabIndex = 295;
            // 
            // WeightDebuffsGP
            // 
            this.WeightDebuffsGP.AutoSize = true;
            this.WeightDebuffsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WeightDebuffsGP.Location = new System.Drawing.Point(33, 110);
            this.WeightDebuffsGP.Name = "WeightDebuffsGP";
            this.WeightDebuffsGP.Size = new System.Drawing.Size(563, 88);
            this.WeightDebuffsGP.TabIndex = 298;
            this.WeightDebuffsGP.TabStop = false;
            this.WeightDebuffsGP.Text = "Overweight";
            // 
            // AutoBuffStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(635, 248);
            this.Controls.Add(this.WeightDebuffsGP);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtPanaceaKey);
            this.Controls.Add(this.DebuffsGP);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoBuffStatusForm";
            this.Text = "AutoBuffStatusForm";
            this.Load += new System.EventHandler(this.AutoBuffStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipPanacea;
        private System.Windows.Forms.GroupBox DebuffsGP;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtPanaceaKey;
        private System.Windows.Forms.GroupBox WeightDebuffsGP;
    }
}