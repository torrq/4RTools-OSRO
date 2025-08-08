using _ORTools.Utils;

namespace _ORTools.Forms
{
    partial class DebuffForm
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
            this.GreenPotionPanel = new System.Windows.Forms.Panel();
            this.txtGreenPotionKey = new System.Windows.Forms.TextBox();
            this.GreenPotionLabel = new System.Windows.Forms.Label();
            this.PanaceaPanel = new System.Windows.Forms.Panel();
            this.txtPanaceaKey = new System.Windows.Forms.TextBox();
            this.PanaceaLabel = new System.Windows.Forms.Label();
            this.RoyalJellyPanel = new System.Windows.Forms.Panel();
            this.txtRoyalJellyKey = new System.Windows.Forms.TextBox();
            this.RoyalJellyLabel = new System.Windows.Forms.Label();
            this.GreenPotionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PanaceaToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RoyalJellyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.GreenPotionPanel.SuspendLayout();
            this.PanaceaPanel.SuspendLayout();
            this.RoyalJellyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTipPanacea
            // 
            this.toolTipPanacea.AutomaticDelay = 10;
            this.toolTipPanacea.AutoPopDelay = 20000;
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
            // GreenPotionPanel
            // 
            this.GreenPotionPanel.Controls.Add(this.txtGreenPotionKey);
            this.GreenPotionPanel.Controls.Add(this.GreenPotionLabel);
            this.GreenPotionPanel.Location = new System.Drawing.Point(29, 15);
            this.GreenPotionPanel.Name = "GreenPotionPanel";
            this.GreenPotionPanel.Size = new System.Drawing.Size(165, 25);
            this.GreenPotionPanel.TabIndex = 0;
            // 
            // txtGreenPotionKey
            // 
            this.txtGreenPotionKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGreenPotionKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGreenPotionKey.Location = new System.Drawing.Point(102, 0);
            this.txtGreenPotionKey.Name = "txtGreenPotionKey";
            this.txtGreenPotionKey.Size = new System.Drawing.Size(60, 25);
            this.txtGreenPotionKey.TabIndex = 305;
            // 
            // GreenPotionLabel
            // 
            this.GreenPotionLabel.Cursor = System.Windows.Forms.Cursors.Help;
            this.GreenPotionLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GreenPotionLabel.Image = global::_ORTools.Resources.Media.Icons.green_potion;
            this.GreenPotionLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GreenPotionLabel.Location = new System.Drawing.Point(2, 0);
            this.GreenPotionLabel.Name = "GreenPotionLabel";
            this.GreenPotionLabel.Size = new System.Drawing.Size(103, 25);
            this.GreenPotionLabel.TabIndex = 304;
            this.GreenPotionLabel.Text = "Green Potion";
            this.GreenPotionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GreenPotionToolTip.SetToolTip(this.GreenPotionLabel, "Silence & Poison");
            // 
            // PanaceaPanel
            // 
            this.PanaceaPanel.Controls.Add(this.txtPanaceaKey);
            this.PanaceaPanel.Controls.Add(this.PanaceaLabel);
            this.PanaceaPanel.Location = new System.Drawing.Point(234, 15);
            this.PanaceaPanel.Name = "PanaceaPanel";
            this.PanaceaPanel.Size = new System.Drawing.Size(140, 25);
            this.PanaceaPanel.TabIndex = 302;
            // 
            // txtPanaceaKey
            // 
            this.txtPanaceaKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPanaceaKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPanaceaKey.Location = new System.Drawing.Point(77, 0);
            this.txtPanaceaKey.Name = "txtPanaceaKey";
            this.txtPanaceaKey.Size = new System.Drawing.Size(60, 25);
            this.txtPanaceaKey.TabIndex = 306;
            // 
            // PanaceaLabel
            // 
            this.PanaceaLabel.Cursor = System.Windows.Forms.Cursors.Help;
            this.PanaceaLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PanaceaLabel.Image = global::_ORTools.Resources.Media.Icons.panacea;
            this.PanaceaLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PanaceaLabel.Location = new System.Drawing.Point(4, 0);
            this.PanaceaLabel.Name = "PanaceaLabel";
            this.PanaceaLabel.Size = new System.Drawing.Size(77, 25);
            this.PanaceaLabel.TabIndex = 307;
            this.PanaceaLabel.Text = "Panacea";
            this.PanaceaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PanaceaToolTip.SetToolTip(this.PanaceaLabel, "Silence, Poison, Blind, Curse, Chaos, Hallucination & Bleeding");
            // 
            // RoyalJellyPanel
            // 
            this.RoyalJellyPanel.Controls.Add(this.txtRoyalJellyKey);
            this.RoyalJellyPanel.Controls.Add(this.RoyalJellyLabel);
            this.RoyalJellyPanel.Location = new System.Drawing.Point(403, 15);
            this.RoyalJellyPanel.Name = "RoyalJellyPanel";
            this.RoyalJellyPanel.Size = new System.Drawing.Size(150, 25);
            this.RoyalJellyPanel.TabIndex = 303;
            // 
            // txtRoyalJellyKey
            // 
            this.txtRoyalJellyKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoyalJellyKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoyalJellyKey.Location = new System.Drawing.Point(88, 0);
            this.txtRoyalJellyKey.Name = "txtRoyalJellyKey";
            this.txtRoyalJellyKey.Size = new System.Drawing.Size(60, 25);
            this.txtRoyalJellyKey.TabIndex = 309;
            // 
            // RoyalJellyLabel
            // 
            this.RoyalJellyLabel.Cursor = System.Windows.Forms.Cursors.Help;
            this.RoyalJellyLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RoyalJellyLabel.Image = global::_ORTools.Resources.Media.Icons.royal_jelly;
            this.RoyalJellyLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RoyalJellyLabel.Location = new System.Drawing.Point(3, 0);
            this.RoyalJellyLabel.Name = "RoyalJellyLabel";
            this.RoyalJellyLabel.Size = new System.Drawing.Size(88, 25);
            this.RoyalJellyLabel.TabIndex = 308;
            this.RoyalJellyLabel.Text = "Royal Jelly";
            this.RoyalJellyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RoyalJellyToolTip.SetToolTip(this.RoyalJellyLabel, "Silence, Poison, Blind, Curse, Chaos, Hallucination & Bleeding");
            // 
            // GreenPotionToolTip
            // 
            this.GreenPotionToolTip.AutomaticDelay = 300;
            this.GreenPotionToolTip.AutoPopDelay = 20000;
            this.GreenPotionToolTip.InitialDelay = 300;
            this.GreenPotionToolTip.ReshowDelay = 60;
            this.GreenPotionToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.GreenPotionToolTip.ToolTipTitle = "Green Potion";
            // 
            // PanaceaToolTip
            // 
            this.PanaceaToolTip.AutomaticDelay = 300;
            this.PanaceaToolTip.AutoPopDelay = 20000;
            this.PanaceaToolTip.InitialDelay = 300;
            this.PanaceaToolTip.ReshowDelay = 60;
            this.PanaceaToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.PanaceaToolTip.ToolTipTitle = "Panacea";
            // 
            // RoyalJellyToolTip
            // 
            this.RoyalJellyToolTip.AutomaticDelay = 300;
            this.RoyalJellyToolTip.AutoPopDelay = 20000;
            this.RoyalJellyToolTip.InitialDelay = 300;
            this.RoyalJellyToolTip.ReshowDelay = 60;
            this.RoyalJellyToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.RoyalJellyToolTip.ToolTipTitle = "Royal Jelly";
            // 
            // AutoBuffStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(626, 452);
            this.Controls.Add(this.RoyalJellyPanel);
            this.Controls.Add(this.PanaceaPanel);
            this.Controls.Add(this.GreenPotionPanel);
            this.Controls.Add(this.DebuffsGP);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoBuffStatusForm";
            this.Text = "AutoBuffStatusForm";
            this.GreenPotionPanel.ResumeLayout(false);
            this.GreenPotionPanel.PerformLayout();
            this.PanaceaPanel.ResumeLayout(false);
            this.PanaceaPanel.PerformLayout();
            this.RoyalJellyPanel.ResumeLayout(false);
            this.RoyalJellyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipPanacea;
        private System.Windows.Forms.GroupBox DebuffsGP;
        private System.Windows.Forms.Panel RoyalJellyPanel;
        private System.Windows.Forms.Panel GreenPotionPanel;
        private System.Windows.Forms.Panel PanaceaPanel;
        private System.Windows.Forms.TextBox txtGreenPotionKey;
        private System.Windows.Forms.Label GreenPotionLabel;
        private System.Windows.Forms.TextBox txtPanaceaKey;
        private System.Windows.Forms.Label PanaceaLabel;
        private System.Windows.Forms.TextBox txtRoyalJellyKey;
        private System.Windows.Forms.Label RoyalJellyLabel;
        private System.Windows.Forms.ToolTip GreenPotionToolTip;
        private System.Windows.Forms.ToolTip PanaceaToolTip;
        private System.Windows.Forms.ToolTip RoyalJellyToolTip;
    }
}