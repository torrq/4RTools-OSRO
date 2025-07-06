namespace _4RTools.Forms
{
    partial class TransferHelperForm
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
            this.txtTransferKey = new System.Windows.Forms.TextBox();
            this.TransferItemLabel = new System.Windows.Forms.Label();
            this.tooltipTransferKey = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txtTransferKey
            // 
            this.txtTransferKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTransferKey.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransferKey.Location = new System.Drawing.Point(111, 8);
            this.txtTransferKey.Name = "txtTransferKey";
            this.txtTransferKey.Size = new System.Drawing.Size(50, 23);
            this.txtTransferKey.TabIndex = 11;
            this.txtTransferKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tooltipTransferKey.SetToolTip(this.txtTransferKey, "Simulates Alt+Right Click for quick item transfer between storage and inventory");
            this.txtTransferKey.TextChanged += new System.EventHandler(this.TxtTransferKey_TextChanged);
            // 
            // TransferItemLabel
            // 
            this.TransferItemLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TransferItemLabel.Location = new System.Drawing.Point(8, 6);
            this.TransferItemLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TransferItemLabel.Name = "TransferItemLabel";
            this.TransferItemLabel.Size = new System.Drawing.Size(99, 27);
            this.TransferItemLabel.TabIndex = 13;
            this.TransferItemLabel.Text = "Item Transfer";
            this.TransferItemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tooltipTransferKey
            // 
            this.tooltipTransferKey.Popup += new System.Windows.Forms.PopupEventHandler(this.tooltipTransferKey_Popup);
            // 
            // TransferHelperForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(175, 40);
            this.Controls.Add(this.txtTransferKey);
            this.Controls.Add(this.TransferItemLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TransferHelperForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "StatusEffect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtTransferKey;
        private System.Windows.Forms.Label TransferItemLabel;
        private System.Windows.Forms.ToolTip tooltipTransferKey;
    }
}