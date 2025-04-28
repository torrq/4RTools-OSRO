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
            this.components = new System.ComponentModel.Container();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemoveProfile = new System.Windows.Forms.Button();
            this.btnRenameProfile = new System.Windows.Forms.Button();
            this.lblProfilesList = new System.Windows.Forms.Label();
            this.lbProfilesList = new System.Windows.Forms.ListBox();
            this.toolTipAdd = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipRename = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipRemove = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::_4RTools.Resources._4RTools.Icons.add;
            this.btnSave.Location = new System.Drawing.Point(496, 33);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(48, 26);
            this.btnSave.TabIndex = 1;
            this.toolTipAdd.SetToolTip(this.btnSave, "Create a new profile");
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemoveProfile
            // 
            this.btnRemoveProfile.BackColor = System.Drawing.Color.Pink;
            this.btnRemoveProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProfile.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnRemoveProfile.Image = global::_4RTools.Resources._4RTools.Icons.delete;
            this.btnRemoveProfile.Location = new System.Drawing.Point(496, 97);
            this.btnRemoveProfile.Name = "btnRemoveProfile";
            this.btnRemoveProfile.Size = new System.Drawing.Size(48, 26);
            this.btnRemoveProfile.TabIndex = 3;
            this.toolTipRemove.SetToolTip(this.btnRemoveProfile, "Delete the selected profile");
            this.btnRemoveProfile.UseVisualStyleBackColor = false;
            this.btnRemoveProfile.Click += new System.EventHandler(this.btnRemoveProfile_Click);
            // 
            // btnRenameProfile
            // 
            this.btnRenameProfile.BackColor = System.Drawing.Color.LightBlue;
            this.btnRenameProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRenameProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameProfile.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnRenameProfile.Image = global::_4RTools.Resources._4RTools.Icons.edit;
            this.btnRenameProfile.Location = new System.Drawing.Point(496, 65);
            this.btnRenameProfile.Name = "btnRenameProfile";
            this.btnRenameProfile.Size = new System.Drawing.Size(48, 26);
            this.btnRenameProfile.TabIndex = 4;
            this.toolTipRename.SetToolTip(this.btnRenameProfile, "Rename the selected profile");
            this.btnRenameProfile.UseVisualStyleBackColor = false;
            this.btnRenameProfile.Click += new System.EventHandler(this.btnRenameProfile_Click);
            // 
            // lblProfilesList
            // 
            this.lblProfilesList.AutoSize = true;
            this.lblProfilesList.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblProfilesList.Location = new System.Drawing.Point(109, 13);
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
            this.lbProfilesList.Location = new System.Drawing.Point(125, 33);
            this.lbProfilesList.Name = "lbProfilesList";
            this.lbProfilesList.ScrollAlwaysVisible = true;
            this.lbProfilesList.Size = new System.Drawing.Size(365, 393);
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
            this.Controls.Add(this.btnRenameProfile);
            this.Controls.Add(this.btnRemoveProfile);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProfileForm";
            this.Text = "ProfileForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRemoveProfile;
        private System.Windows.Forms.Button btnRenameProfile;
        private System.Windows.Forms.Label lblProfilesList;
        private System.Windows.Forms.ListBox lbProfilesList;
        private System.Windows.Forms.ToolTip toolTipAdd;
        private System.Windows.Forms.ToolTip toolTipRename;
        private System.Windows.Forms.ToolTip toolTipRemove;
    }
}