using _4RTools.Utils;

namespace _4RTools.Forms
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
            this.DebuffsGP = new System.Windows.Forms.GroupBox();
            this.txtPanaceaKey = new System.Windows.Forms.TextBox();
            this.PanaceaLabel = new System.Windows.Forms.Label();
            this.GreenPotionLabel = new System.Windows.Forms.Label();
            this.txtGreenPotionKey = new System.Windows.Forms.TextBox();
            this.txtRoyalJellyKey = new System.Windows.Forms.TextBox();
            this.RoyalJellyLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // toolTipPanacea
            // 
            this.toolTipPanacea.AutomaticDelay = 10;
            this.toolTipPanacea.AutoPopDelay = 5000;
            this.toolTipPanacea.InitialDelay = 10;
            this.toolTipPanacea.ReshowDelay = 2;
            // 
            // DebuffsGP
            // 
            this.DebuffsGP.AutoSize = true;
            this.DebuffsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DebuffsGP.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DebuffsGP.Location = new System.Drawing.Point(8, 56);
            this.DebuffsGP.Name = "DebuffsGP";
            this.DebuffsGP.Size = new System.Drawing.Size(600, 380);
            this.DebuffsGP.TabIndex = 294;
            this.DebuffsGP.TabStop = false;
            this.DebuffsGP.Text = "Debuffs";
            // 
            // txtPanaceaKey
            // 
            this.txtPanaceaKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanaceaKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPanaceaKey.Location = new System.Drawing.Point(112, 16);
            this.txtPanaceaKey.Name = "txtPanaceaKey";
            this.txtPanaceaKey.Size = new System.Drawing.Size(60, 25);
            this.txtPanaceaKey.TabIndex = 295;
            // 
            // PanaceaLabel
            // 
            this.PanaceaLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PanaceaLabel.Image = global::_4RTools.Resources._4RTools.Icons.panacea;
            this.PanaceaLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PanaceaLabel.Location = new System.Drawing.Point(39, 16);
            this.PanaceaLabel.Name = "PanaceaLabel";
            this.PanaceaLabel.Size = new System.Drawing.Size(77, 25);
            this.PanaceaLabel.TabIndex = 297;
            this.PanaceaLabel.Text = "Panacea";
            this.PanaceaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GreenPotionLabel
            // 
            this.GreenPotionLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GreenPotionLabel.Image = global::_4RTools.Resources._4RTools.Icons.green_potion;
            this.GreenPotionLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GreenPotionLabel.Location = new System.Drawing.Point(410, 16);
            this.GreenPotionLabel.Name = "GreenPotionLabel";
            this.GreenPotionLabel.Size = new System.Drawing.Size(103, 25);
            this.GreenPotionLabel.TabIndex = 298;
            this.GreenPotionLabel.Text = "Green Potion";
            this.GreenPotionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGreenPotionKey
            // 
            this.txtGreenPotionKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGreenPotionKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGreenPotionKey.Location = new System.Drawing.Point(510, 16);
            this.txtGreenPotionKey.Name = "txtGreenPotionKey";
            this.txtGreenPotionKey.Size = new System.Drawing.Size(60, 25);
            this.txtGreenPotionKey.TabIndex = 299;
            // 
            // txtRoyalJellyKey
            // 
            this.txtRoyalJellyKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoyalJellyKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoyalJellyKey.Location = new System.Drawing.Point(308, 16);
            this.txtRoyalJellyKey.Name = "txtRoyalJellyKey";
            this.txtRoyalJellyKey.Size = new System.Drawing.Size(60, 25);
            this.txtRoyalJellyKey.TabIndex = 301;
            // 
            // RoyalJellyLabel
            // 
            this.RoyalJellyLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RoyalJellyLabel.Image = global::_4RTools.Resources._4RTools.Icons.royal_jelly;
            this.RoyalJellyLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RoyalJellyLabel.Location = new System.Drawing.Point(223, 16);
            this.RoyalJellyLabel.Name = "RoyalJellyLabel";
            this.RoyalJellyLabel.Size = new System.Drawing.Size(88, 25);
            this.RoyalJellyLabel.TabIndex = 300;
            this.RoyalJellyLabel.Text = "Royal Jelly";
            this.RoyalJellyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AutoBuffStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(626, 452);
            this.Controls.Add(this.txtRoyalJellyKey);
            this.Controls.Add(this.txtGreenPotionKey);
            this.Controls.Add(this.RoyalJellyLabel);
            this.Controls.Add(this.GreenPotionLabel);
            this.Controls.Add(this.txtPanaceaKey);
            this.Controls.Add(this.DebuffsGP);
            this.Controls.Add(this.PanaceaLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoBuffStatusForm";
            this.Text = "AutoBuffStatusForm";
            this.Load += new System.EventHandler(this.AutoBuffStatusForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipPanacea;
        private System.Windows.Forms.GroupBox DebuffsGP;
        private System.Windows.Forms.TextBox txtPanaceaKey;
        private System.Windows.Forms.Label PanaceaLabel;
        private System.Windows.Forms.Label GreenPotionLabel;
        private System.Windows.Forms.TextBox txtGreenPotionKey;
        private System.Windows.Forms.TextBox txtRoyalJellyKey;
        private System.Windows.Forms.Label RoyalJellyLabel;
    }
}