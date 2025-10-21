using BruteGamingMacros.Core.Utils;

namespace BruteGamingMacros.UI.Forms
{
    partial class ToggleStateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip toolTipStatusToggle;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToggleStateForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtStatusToggleKey = new System.Windows.Forms.TextBox();
            this.lblStatusToggle = new System.Windows.Forms.Label();
            this.btnStatusToggle = new System.Windows.Forms.Button();
            this.toolTipStatusToggle = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtStatusToggleKey);
            this.groupBox1.Controls.Add(this.lblStatusToggle);
            this.groupBox1.Controls.Add(this.btnStatusToggle);
            this.groupBox1.Location = new System.Drawing.Point(47, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 112);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Status";
            // 
            // txtStatusToggleKey
            // 
            this.txtStatusToggleKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStatusToggleKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusToggleKey.Location = new System.Drawing.Point(102, 34);
            this.txtStatusToggleKey.Name = "txtStatusToggleKey";
            this.txtStatusToggleKey.Size = new System.Drawing.Size(72, 25);
            this.txtStatusToggleKey.TabIndex = 23;
            this.txtStatusToggleKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStatusToggle
            // 
            this.lblStatusToggle.AllowDrop = true;
            this.lblStatusToggle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusToggle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblStatusToggle.Location = new System.Drawing.Point(4, 72);
            this.lblStatusToggle.MaximumSize = new System.Drawing.Size(190, 18);
            this.lblStatusToggle.Name = "lblStatusToggle";
            this.lblStatusToggle.Size = new System.Drawing.Size(190, 18);
            this.lblStatusToggle.TabIndex = 22;
            this.lblStatusToggle.Text = "Press the key to start!";
            this.lblStatusToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatusToggle.Click += new System.EventHandler(this.lblStatusToggle_Click);
            // 
            // btnStatusToggle
            // 
            this.btnStatusToggle.BackColor = System.Drawing.Color.Transparent;
            this.btnStatusToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStatusToggle.FlatAppearance.BorderSize = 0;
            this.btnStatusToggle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStatusToggle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStatusToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatusToggle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatusToggle.ForeColor = System.Drawing.SystemColors.Window;
            this.btnStatusToggle.Image = global::BruteGamingMacros.Resources.BruteGaming.Icons.toggle_off;
            this.btnStatusToggle.Location = new System.Drawing.Point(18, 22);
            this.btnStatusToggle.Margin = new System.Windows.Forms.Padding(0);
            this.btnStatusToggle.Name = "btnStatusToggle";
            this.btnStatusToggle.Size = new System.Drawing.Size(68, 48);
            this.btnStatusToggle.TabIndex = 21;
            this.toolTipStatusToggle.SetToolTip(this.btnStatusToggle, "Toggle application state ON/OFF");
            this.btnStatusToggle.UseVisualStyleBackColor = false;
            this.btnStatusToggle.Click += new System.EventHandler(this.btnToggleStatusHandler);
            // 
            // ToggleApplicationStateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(290, 136);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ToggleApplicationStateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ToggleApplicationStateForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStatusToggle;
        private System.Windows.Forms.Label lblStatusToggle;
        private System.Windows.Forms.TextBox txtStatusToggleKey;
    }
}