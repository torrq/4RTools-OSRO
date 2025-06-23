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
            this.btnSet3Hours = new System.Windows.Forms.Button();
            this.btnSet4Hours = new System.Windows.Forms.Button();
            this.btnSet8Hours = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarTime
            // 
            this.trackBarTime.Location = new System.Drawing.Point(20, 50);
            this.trackBarTime.Maximum = 480;
            this.trackBarTime.Minimum = 1;
            this.trackBarTime.Name = "trackBarTime";
            this.trackBarTime.Size = new System.Drawing.Size(300, 45);
            this.trackBarTime.TabIndex = 0;
            this.trackBarTime.Value = 1;
            this.trackBarTime.Scroll += new System.EventHandler(this.TrackBarTime_Scroll);
            // 
            // lblSelectedTime
            // 
            this.lblSelectedTime.AutoSize = true;
            this.lblSelectedTime.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTime.Location = new System.Drawing.Point(20, 20);
            this.lblSelectedTime.Name = "lblSelectedTime";
            this.lblSelectedTime.Size = new System.Drawing.Size(116, 16);
            this.lblSelectedTime.TabIndex = 1;
            this.lblSelectedTime.Text = "Selected Time: 0m";
            // 
            // btnToggleTimer
            // 
            this.btnToggleTimer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleTimer.Location = new System.Drawing.Point(20, 100);
            this.btnToggleTimer.Name = "btnToggleTimer";
            this.btnToggleTimer.Size = new System.Drawing.Size(80, 30);
            this.btnToggleTimer.TabIndex = 2;
            this.btnToggleTimer.Text = "Start Timer";
            this.btnToggleTimer.UseVisualStyleBackColor = true;
            this.btnToggleTimer.Click += new System.EventHandler(this.BtnToggleTimer_Click);
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.AutoSize = true;
            this.lblRemainingTime.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainingTime.Location = new System.Drawing.Point(20, 35);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(142, 16);
            this.lblRemainingTime.TabIndex = 3;
            this.lblRemainingTime.Text = "Remaining: Not running";
            // 
            // btnSet3Hours
            // 
            this.btnSet3Hours.Location = new System.Drawing.Point(110, 100);
            this.btnSet3Hours.Name = "btnSet3Hours";
            this.btnSet3Hours.Size = new System.Drawing.Size(60, 30);
            this.btnSet3Hours.TabIndex = 4;
            this.btnSet3Hours.Text = "3 Hours";
            this.btnSet3Hours.UseVisualStyleBackColor = true;
            this.btnSet3Hours.Click += new System.EventHandler(this.BtnSet3Hours_Click);
            // 
            // btnSet4Hours
            // 
            this.btnSet4Hours.Location = new System.Drawing.Point(180, 100);
            this.btnSet4Hours.Name = "btnSet4Hours";
            this.btnSet4Hours.Size = new System.Drawing.Size(60, 30);
            this.btnSet4Hours.TabIndex = 5;
            this.btnSet4Hours.Text = "4 Hours";
            this.btnSet4Hours.UseVisualStyleBackColor = true;
            this.btnSet4Hours.Click += new System.EventHandler(this.BtnSet4Hours_Click);
            // 
            // btnSet8Hours
            // 
            this.btnSet8Hours.Location = new System.Drawing.Point(250, 100);
            this.btnSet8Hours.Name = "btnSet8Hours";
            this.btnSet8Hours.Size = new System.Drawing.Size(60, 30);
            this.btnSet8Hours.TabIndex = 6;
            this.btnSet8Hours.Text = "8 Hours";
            this.btnSet8Hours.UseVisualStyleBackColor = true;
            this.btnSet8Hours.Click += new System.EventHandler(this.BtnSet8Hours_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(280, 10);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(50, 20);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // AutoOffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(340, 150);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSet8Hours);
            this.Controls.Add(this.btnSet4Hours);
            this.Controls.Add(this.btnSet3Hours);
            this.Controls.Add(this.lblRemainingTime);
            this.Controls.Add(this.btnToggleTimer);
            this.Controls.Add(this.lblSelectedTime);
            this.Controls.Add(this.trackBarTime);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoOffForm";
            this.Text = "AutoOffForm";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.TrackBar trackBarTime;
        private System.Windows.Forms.Label lblSelectedTime;
        private System.Windows.Forms.Button btnToggleTimer;
        private System.Windows.Forms.Label lblRemainingTime;
        private System.Windows.Forms.Button btnSet3Hours;
        private System.Windows.Forms.Button btnSet4Hours;
        private System.Windows.Forms.Button btnSet8Hours;
        private System.Windows.Forms.Button btnReset;
    }
}