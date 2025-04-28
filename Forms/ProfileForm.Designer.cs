namespace _4RTools.Forms
{
    partial class ProfileForm
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
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRemoveProfile = new System.Windows.Forms.Button();
            this.lblProfilesList = new System.Windows.Forms.Label();
            this.lbProfilesList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // txtProfileName
            // 
            this.txtProfileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProfileName.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtProfileName.Location = new System.Drawing.Point(125, 29);
            this.txtProfileName.Multiline = true;
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(360, 23);
            this.txtProfileName.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::_4RTools.Resources._4RTools.Icons.add;
            this.btnSave.Location = new System.Drawing.Point(496, 27);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 26);
            this.btnSave.TabIndex = 1;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label1.Location = new System.Drawing.Point(109, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Create a new profile";
            // 
            // btnRemoveProfile
            // 
            this.btnRemoveProfile.BackColor = System.Drawing.Color.Pink;
            this.btnRemoveProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProfile.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnRemoveProfile.Image = global::_4RTools.Resources._4RTools.Icons.delete;
            this.btnRemoveProfile.Location = new System.Drawing.Point(496, 400);
            this.btnRemoveProfile.Name = "btnRemoveProfile";
            this.btnRemoveProfile.Size = new System.Drawing.Size(48, 26);
            this.btnRemoveProfile.TabIndex = 3;
            this.btnRemoveProfile.UseVisualStyleBackColor = false;
            this.btnRemoveProfile.Click += new System.EventHandler(this.btnRemoveProfile_Click);
            // 
            // lblProfilesList
            // 
            this.lblProfilesList.AutoSize = true;
            this.lblProfilesList.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblProfilesList.Location = new System.Drawing.Point(109, 61);
            this.lblProfilesList.Name = "lblProfilesList";
            this.lblProfilesList.Size = new System.Drawing.Size(68, 17);
            this.lblProfilesList.TabIndex = 6;
            this.lblProfilesList.Text = "Profile List";
            this.lblProfilesList.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbProfilesList
            // 
            this.lbProfilesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbProfilesList.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbProfilesList.FormattingEnabled = true;
            this.lbProfilesList.ItemHeight = 17;
            this.lbProfilesList.Location = new System.Drawing.Point(125, 84);
            this.lbProfilesList.Name = "lbProfilesList";
            this.lbProfilesList.ScrollAlwaysVisible = true;
            this.lbProfilesList.Size = new System.Drawing.Size(360, 342);
            this.lbProfilesList.TabIndex = 8;
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(630, 440);
            this.Controls.Add(this.lbProfilesList);
            this.Controls.Add(this.lblProfilesList);
            this.Controls.Add(this.btnRemoveProfile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtProfileName);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProfileForm";
            this.Text = "ProfileForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemoveProfile;
        private System.Windows.Forms.Label lblProfilesList;
        private System.Windows.Forms.ListBox lbProfilesList;
    }
}