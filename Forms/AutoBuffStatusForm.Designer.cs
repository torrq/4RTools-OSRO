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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoBuffStatusForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DebuffsGP = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatusKey = new System.Windows.Forms.TextBox();
            this.WeightDebuffsGP = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 10;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(266, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 296;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Panacea");
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(293, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 297;
            this.label1.Text = "Status";
            // 
            // txtStatusKey
            // 
            this.txtStatusKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatusKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusKey.Location = new System.Drawing.Point(335, 12);
            this.txtStatusKey.Name = "txtStatusKey";
            this.txtStatusKey.Size = new System.Drawing.Size(45, 25);
            this.txtStatusKey.TabIndex = 295;
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
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtStatusKey);
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
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox DebuffsGP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtStatusKey;
        private System.Windows.Forms.GroupBox WeightDebuffsGP;
    }
}