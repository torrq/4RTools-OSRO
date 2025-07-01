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
            this.animatedClockImage = new System.Windows.Forms.PictureBox();
            this.AutoOffOverweightCB = new System.Windows.Forms.CheckBox();
            this.overweightAltKeyPlusLabel = new System.Windows.Forms.Label();
            this.overweightAltKeyLabel = new System.Windows.Forms.Label();
            this.overweightKey = new System.Windows.Forms.TextBox();
            this.arrowDown = new System.Windows.Forms.PictureBox();
            this.arrowRight = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animatedClockImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowRight)).BeginInit();
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
            this.lblSelectedTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTime.Location = new System.Drawing.Point(280, 48);
            this.lblSelectedTime.Name = "lblSelectedTime";
            this.lblSelectedTime.Size = new System.Drawing.Size(50, 18);
            this.lblSelectedTime.TabIndex = 1;
            this.lblSelectedTime.Text = "0m";
            this.lblSelectedTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnToggleTimer
            // 
            this.btnToggleTimer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleTimer.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleTimer.Location = new System.Drawing.Point(11, 66);
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
            this.lblRemainingTime.Size = new System.Drawing.Size(60, 16);
            this.lblRemainingTime.TabIndex = 3;
            this.lblRemainingTime.Text = "0m";
            this.lblRemainingTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSelectedTimeText
            // 
            this.lblSelectedTimeText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTimeText.Location = new System.Drawing.Point(204, 48);
            this.lblSelectedTimeText.Name = "lblSelectedTimeText";
            this.lblSelectedTimeText.Size = new System.Drawing.Size(80, 18);
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
            // animatedClockImage
            // 
            this.animatedClockImage.Image = global::_4RTools.Resources._4RTools.Icons.clock_animated;
            this.animatedClockImage.Location = new System.Drawing.Point(316, 2);
            this.animatedClockImage.Name = "animatedClockImage";
            this.animatedClockImage.Size = new System.Drawing.Size(24, 24);
            this.animatedClockImage.TabIndex = 5;
            this.animatedClockImage.TabStop = false;
            // 
            // AutoOffOverweightCB
            // 
            this.AutoOffOverweightCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoOffOverweightCB.Image = global::_4RTools.Resources._4RTools.Icons.weight90;
            this.AutoOffOverweightCB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoOffOverweightCB.Location = new System.Drawing.Point(10, 112);
            this.AutoOffOverweightCB.Name = "AutoOffOverweightCB";
            this.AutoOffOverweightCB.Size = new System.Drawing.Size(116, 40);
            this.AutoOffOverweightCB.TabIndex = 318;
            this.AutoOffOverweightCB.Text = "Overweight Auto-off";
            this.AutoOffOverweightCB.UseVisualStyleBackColor = true;
            this.AutoOffOverweightCB.CheckedChanged += new System.EventHandler(this.AutoOffOverweight_CheckedChanged);
            // 
            // overweightAltKeyPlusLabel
            // 
            this.overweightAltKeyPlusLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightAltKeyPlusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.overweightAltKeyPlusLabel.Location = new System.Drawing.Point(266, 118);
            this.overweightAltKeyPlusLabel.Name = "overweightAltKeyPlusLabel";
            this.overweightAltKeyPlusLabel.Size = new System.Drawing.Size(16, 24);
            this.overweightAltKeyPlusLabel.TabIndex = 317;
            this.overweightAltKeyPlusLabel.Text = "+";
            this.overweightAltKeyPlusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overweightAltKeyLabel
            // 
            this.overweightAltKeyLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightAltKeyLabel.Image = global::_4RTools.Resources._4RTools.Icons.key_alt;
            this.overweightAltKeyLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.overweightAltKeyLabel.Location = new System.Drawing.Point(161, 119);
            this.overweightAltKeyLabel.Name = "overweightAltKeyLabel";
            this.overweightAltKeyLabel.Size = new System.Drawing.Size(107, 24);
            this.overweightAltKeyLabel.TabIndex = 316;
            this.overweightAltKeyLabel.Text = "Then send";
            this.overweightAltKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // overweightKey
            // 
            this.overweightKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overweightKey.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightKey.Location = new System.Drawing.Point(283, 119);
            this.overweightKey.Name = "overweightKey";
            this.overweightKey.Size = new System.Drawing.Size(49, 23);
            this.overweightKey.TabIndex = 311;
            this.overweightKey.TextChanged += new System.EventHandler(this.OverweightKey_TextChanged);
            // 
            // arrowDown
            // 
            this.arrowDown.Image = global::_4RTools.Resources._4RTools.Icons.arrow_down;
            this.arrowDown.Location = new System.Drawing.Point(249, 102);
            this.arrowDown.Name = "arrowDown";
            this.arrowDown.Size = new System.Drawing.Size(11, 14);
            this.arrowDown.TabIndex = 319;
            this.arrowDown.TabStop = false;
            // 
            // arrowRight
            // 
            this.arrowRight.Image = global::_4RTools.Resources._4RTools.Icons.arrow_right;
            this.arrowRight.Location = new System.Drawing.Point(134, 127);
            this.arrowRight.Name = "arrowRight";
            this.arrowRight.Size = new System.Drawing.Size(19, 11);
            this.arrowRight.TabIndex = 320;
            this.arrowRight.TabStop = false;
            // 
            // AutoOffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(340, 150);
            this.Controls.Add(this.arrowRight);
            this.Controls.Add(this.arrowDown);
            this.Controls.Add(this.AutoOffOverweightCB);
            this.Controls.Add(this.overweightAltKeyPlusLabel);
            this.Controls.Add(this.animatedClockImage);
            this.Controls.Add(this.overweightAltKeyLabel);
            this.Controls.Add(this.overweightKey);
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
            ((System.ComponentModel.ISupportInitialize)(this.animatedClockImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowRight)).EndInit();
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
        private System.Windows.Forms.PictureBox animatedClockImage;
        private System.Windows.Forms.CheckBox AutoOffOverweightCB;
        private System.Windows.Forms.Label overweightAltKeyPlusLabel;
        private System.Windows.Forms.Label overweightAltKeyLabel;
        private System.Windows.Forms.TextBox overweightKey;
        private System.Windows.Forms.PictureBox arrowDown;
        private System.Windows.Forms.PictureBox arrowRight;
    }
}