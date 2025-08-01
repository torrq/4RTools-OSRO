﻿using _ORTools.Utils;

namespace _ORTools.Forms
{
    partial class CharacterInfo
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
            this.characterNameLabel = new System.Windows.Forms.Label();
            this.characterMapLabel = new System.Windows.Forms.Label();
            this.MapLinkToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.characterInfoLabel = new _ORTools.Utils.CharacterInfoLabel();
            this.SuspendLayout();
            // 
            // characterNameLabel
            // 
            this.characterNameLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.characterNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.characterNameLabel.Location = new System.Drawing.Point(0, 0);
            this.characterNameLabel.Name = "characterNameLabel";
            this.characterNameLabel.Size = new System.Drawing.Size(141, 22);
            this.characterNameLabel.TabIndex = 21;
            this.characterNameLabel.Text = "Name";
            this.characterNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // characterMapLabel
            // 
            this.characterMapLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.characterMapLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.characterMapLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.characterMapLabel.Location = new System.Drawing.Point(129, 0);
            this.characterMapLabel.Name = "characterMapLabel";
            this.characterMapLabel.Size = new System.Drawing.Size(109, 22);
            this.characterMapLabel.TabIndex = 28;
            this.characterMapLabel.Text = "Map";
            this.characterMapLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.MapLinkToolTip.SetToolTip(this.characterMapLabel, "Link to https://ro.kokotewa.com info on this map");
            this.characterMapLabel.Click += new System.EventHandler(this.characterMapLabel_Click_1);
            // 
            // MapLinkToolTip
            // 
            this.MapLinkToolTip.AutomaticDelay = 300;
            this.MapLinkToolTip.AutoPopDelay = 15000;
            this.MapLinkToolTip.InitialDelay = 300;
            this.MapLinkToolTip.ReshowDelay = 60;
            this.MapLinkToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.MapLinkToolTip.ToolTipTitle = "Map Info Link (Kokotewa)";
            // 
            // characterInfoLabel
            // 
            this.characterInfoLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.characterInfoLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.characterInfoLabel.ForeColor = System.Drawing.Color.Black;
            this.characterInfoLabel.Location = new System.Drawing.Point(-1, 23);
            this.characterInfoLabel.Margin = new System.Windows.Forms.Padding(0);
            this.characterInfoLabel.Name = "characterInfoLabel";
            this.characterInfoLabel.Size = new System.Drawing.Size(241, 38);
            this.characterInfoLabel.SpacePadding = 0;
            this.characterInfoLabel.TabIndex = 27;
            this.characterInfoLabel.Text = "Info";
            this.characterInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.characterInfoLabel.Click += new System.EventHandler(this.characterInfoLabel_Click);
            // 
            // CharacterInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(240, 60);
            this.Controls.Add(this.characterNameLabel);
            this.Controls.Add(this.characterInfoLabel);
            this.Controls.Add(this.characterMapLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CharacterInfo";
            this.ShowIcon = false;
            this.Text = "CharacterInfo";
            this.Load += new System.EventHandler(this.CharacterInfo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label characterNameLabel;
        private System.Windows.Forms.Label characterMapLabel;
        private _ORTools.Utils.CharacterInfoLabel characterInfoLabel;
        private System.Windows.Forms.ToolTip MapLinkToolTip;
    }
}