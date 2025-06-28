using _4RTools.Utils;

namespace _4RTools.Forms
{
    partial class AutoOffForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.trackBarTime = new System.Windows.Forms.TrackBar();
            this.lblSelectedTime = new System.Windows.Forms.Label();
            this.btnToggleTimer = new System.Windows.Forms.Button();
            this.lblRemainingTime = new System.Windows.Forms.Label();
            this.lblSelectedTimeText = new System.Windows.Forms.Label();
            this.lblRemainingTimeText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarTime
            // 
            this.trackBarTime.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.trackBarTime.Location = new System.Drawing.Point(0, 21);
            this.trackBarTime.Maximum = 480;
            this.trackBarTime.Minimum = 1;
            this.trackBarTime.Name = "trackBarTime";
            this.trackBarTime.Size = new System.Drawing.Size(340, 45);
            this.trackBarTime.TabIndex = 0;
            this.trackBarTime.Value = 1;
            this.trackBarTime.Scroll += new System.EventHandler(this.TrackBarTime_Scroll);
            // 
            // lblSelectedTime
            // 
            this.lblSelectedTime.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTime.Location = new System.Drawing.Point(202, 50);
            this.lblSelectedTime.Name = "lblSelectedTime";
            this.lblSelectedTime.Size = new System.Drawing.Size(126, 15);
            this.lblSelectedTime.TabIndex = 1;
            this.lblSelectedTime.Text = "0m";
            this.lblSelectedTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnToggleTimer
            // 
            this.btnToggleTimer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleTimer.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleTimer.Location = new System.Drawing.Point(12, 70);
            this.btnToggleTimer.Name = "btnToggleTimer";
            this.btnToggleTimer.Size = new System.Drawing.Size(86, 24);
            this.btnToggleTimer.TabIndex = 2;
            this.btnToggleTimer.Text = "Start Timer";
            this.btnToggleTimer.UseVisualStyleBackColor = true;
            this.btnToggleTimer.Click += new System.EventHandler(this.BtnToggleTimer_Click);
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainingTime.Location = new System.Drawing.Point(113, 4);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(187, 16);
            this.lblRemainingTime.TabIndex = 3;
            this.lblRemainingTime.Text = "0m";
            this.lblRemainingTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSelectedTimeText
            // 
            this.lblSelectedTimeText.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTimeText.Location = new System.Drawing.Point(149, 50);
            this.lblSelectedTimeText.Name = "lblSelectedTimeText";
            this.lblSelectedTimeText.Size = new System.Drawing.Size(126, 15);
            this.lblSelectedTimeText.TabIndex = 4;
            this.lblSelectedTimeText.Text = "Selected Time:";
            this.lblSelectedTimeText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRemainingTimeText
            // 
            this.lblRemainingTimeText.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainingTimeText.Location = new System.Drawing.Point(2, 4);
            this.lblRemainingTimeText.Name = "lblRemainingTimeText";
            this.lblRemainingTimeText.Size = new System.Drawing.Size(109, 16);
            this.lblRemainingTimeText.TabIndex = 4;
            this.lblRemainingTimeText.Text = "Remaining Time:";
            this.lblRemainingTimeText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AutoOffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(340, 150);
            this.Controls.Add(this.lblSelectedTimeText);
            this.Controls.Add(this.btnToggleTimer);
            this.Controls.Add(this.lblSelectedTime);
            this.Controls.Add(this.trackBarTime);
            this.Controls.Add(this.lblRemainingTimeText);
            this.Controls.Add(this.lblRemainingTime);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoOffForm";
            this.Text = "AutoOffForm";
            this.Load += new System.EventHandler(this.AutoOffForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.TrackBar trackBarTime;
        private System.Windows.Forms.Label lblSelectedTime;
        private System.Windows.Forms.Button btnToggleTimer;
        private System.Windows.Forms.Label lblRemainingTime;
        private System.Windows.Forms.Label lblSelectedTimeText;
        private System.Windows.Forms.Label lblRemainingTimeText;
    }
}