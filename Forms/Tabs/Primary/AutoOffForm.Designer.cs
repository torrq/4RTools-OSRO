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
            this.components = new System.ComponentModel.Container();
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
            this.arrowDown1 = new System.Windows.Forms.PictureBox();
            this.arrowRight = new System.Windows.Forms.PictureBox();
            this.AutoOffKey1 = new System.Windows.Forms.TextBox();
            this.arrowDown2 = new System.Windows.Forms.PictureBox();
            this.overweightAltKey2Label = new System.Windows.Forms.Label();
            this.AutoOffKey2 = new System.Windows.Forms.TextBox();
            this.arrowDown3 = new System.Windows.Forms.PictureBox();
            this.AutoOffKillClientChk = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.overweightToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ToggleTimerToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.AltKey1ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.AltKey2ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.StopClientToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animatedClockImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown3)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarTime
            // 
            this.trackBarTime.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.trackBarTime.Location = new System.Drawing.Point(0, 22);
            this.trackBarTime.Maximum = 480;
            this.trackBarTime.Minimum = 1;
            this.trackBarTime.Name = "trackBarTime";
            this.trackBarTime.Size = new System.Drawing.Size(323, 45);
            this.trackBarTime.TabIndex = 0;
            this.trackBarTime.Value = 1;
            this.trackBarTime.Scroll += new System.EventHandler(this.TrackBarTime_Scroll);
            // 
            // lblSelectedTime
            // 
            this.lblSelectedTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedTime.Location = new System.Drawing.Point(275, 48);
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
            this.btnToggleTimer.Location = new System.Drawing.Point(11, 68);
            this.btnToggleTimer.Name = "btnToggleTimer";
            this.btnToggleTimer.Size = new System.Drawing.Size(86, 24);
            this.btnToggleTimer.TabIndex = 2;
            this.btnToggleTimer.Text = "Start Timer";
            this.ToggleTimerToolTip.SetToolTip(this.btnToggleTimer, "Starts auto-off timer with the selected time");
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
            this.lblSelectedTimeText.Location = new System.Drawing.Point(199, 48);
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
            this.animatedClockImage.Location = new System.Drawing.Point(325, 2);
            this.animatedClockImage.Name = "animatedClockImage";
            this.animatedClockImage.Size = new System.Drawing.Size(24, 24);
            this.animatedClockImage.TabIndex = 5;
            this.animatedClockImage.TabStop = false;
            // 
            // AutoOffOverweightCB
            // 
            this.AutoOffOverweightCB.BackColor = System.Drawing.Color.Pink;
            this.AutoOffOverweightCB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoOffOverweightCB.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoOffOverweightCB.Image = global::_4RTools.Resources._4RTools.Icons.weight90;
            this.AutoOffOverweightCB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoOffOverweightCB.Location = new System.Drawing.Point(2, 110);
            this.AutoOffOverweightCB.Name = "AutoOffOverweightCB";
            this.AutoOffOverweightCB.Padding = new System.Windows.Forms.Padding(6, 0, 2, 0);
            this.AutoOffOverweightCB.Size = new System.Drawing.Size(131, 40);
            this.AutoOffOverweightCB.TabIndex = 318;
            this.AutoOffOverweightCB.Text = "Overweight Auto-off";
            this.overweightToolTip.SetToolTip(this.AutoOffOverweightCB, "Enable this to toggle off when at 90% weight");
            this.AutoOffOverweightCB.UseVisualStyleBackColor = false;
            this.AutoOffOverweightCB.CheckedChanged += new System.EventHandler(this.AutoOffOverweight_CheckedChanged);
            // 
            // overweightAltKeyPlusLabel
            // 
            this.overweightAltKeyPlusLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightAltKeyPlusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.overweightAltKeyPlusLabel.Location = new System.Drawing.Point(287, 122);
            this.overweightAltKeyPlusLabel.Name = "overweightAltKeyPlusLabel";
            this.overweightAltKeyPlusLabel.Size = new System.Drawing.Size(16, 24);
            this.overweightAltKeyPlusLabel.TabIndex = 317;
            this.overweightAltKeyPlusLabel.Text = "+";
            this.overweightAltKeyPlusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overweightAltKeyLabel
            // 
            this.overweightAltKeyLabel.Cursor = System.Windows.Forms.Cursors.Help;
            this.overweightAltKeyLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightAltKeyLabel.Image = global::_4RTools.Resources._4RTools.Icons.key_alt;
            this.overweightAltKeyLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.overweightAltKeyLabel.Location = new System.Drawing.Point(182, 123);
            this.overweightAltKeyLabel.Name = "overweightAltKeyLabel";
            this.overweightAltKeyLabel.Size = new System.Drawing.Size(107, 24);
            this.overweightAltKeyLabel.TabIndex = 316;
            this.overweightAltKeyLabel.Text = "Then send";
            this.overweightAltKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AltKey1ToolTip.SetToolTip(this.overweightAltKeyLabel, "Sends an Alt + # key, to use RO\'s Alt-M macros");
            // 
            // arrowDown1
            // 
            this.arrowDown1.Image = global::_4RTools.Resources._4RTools.Icons.arrow_down;
            this.arrowDown1.Location = new System.Drawing.Point(263, 102);
            this.arrowDown1.Name = "arrowDown1";
            this.arrowDown1.Size = new System.Drawing.Size(11, 14);
            this.arrowDown1.TabIndex = 319;
            this.arrowDown1.TabStop = false;
            // 
            // arrowRight
            // 
            this.arrowRight.Image = global::_4RTools.Resources._4RTools.Icons.arrow_right;
            this.arrowRight.Location = new System.Drawing.Point(148, 130);
            this.arrowRight.Name = "arrowRight";
            this.arrowRight.Size = new System.Drawing.Size(19, 11);
            this.arrowRight.TabIndex = 320;
            this.arrowRight.TabStop = false;
            // 
            // AutoOffKey1
            // 
            this.AutoOffKey1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AutoOffKey1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoOffKey1.Location = new System.Drawing.Point(303, 123);
            this.AutoOffKey1.Name = "AutoOffKey1";
            this.AutoOffKey1.Size = new System.Drawing.Size(41, 23);
            this.AutoOffKey1.TabIndex = 311;
            this.AutoOffKey1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AutoOffKey1.TextChanged += new System.EventHandler(this.AutoOffKey1_TextChanged);
            // 
            // arrowDown2
            // 
            this.arrowDown2.Image = global::_4RTools.Resources._4RTools.Icons.arrow_down;
            this.arrowDown2.Location = new System.Drawing.Point(263, 153);
            this.arrowDown2.Name = "arrowDown2";
            this.arrowDown2.Size = new System.Drawing.Size(11, 14);
            this.arrowDown2.TabIndex = 322;
            this.arrowDown2.TabStop = false;
            // 
            // overweightAltKey2Label
            // 
            this.overweightAltKey2Label.Cursor = System.Windows.Forms.Cursors.Help;
            this.overweightAltKey2Label.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overweightAltKey2Label.Image = global::_4RTools.Resources._4RTools.Icons.key_alt;
            this.overweightAltKey2Label.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.overweightAltKey2Label.Location = new System.Drawing.Point(182, 174);
            this.overweightAltKey2Label.Name = "overweightAltKey2Label";
            this.overweightAltKey2Label.Size = new System.Drawing.Size(107, 24);
            this.overweightAltKey2Label.TabIndex = 323;
            this.overweightAltKey2Label.Text = "Then send";
            this.overweightAltKey2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AltKey2ToolTip.SetToolTip(this.overweightAltKey2Label, "Sends an Alt + # key, to use RO\'s Alt-M macros");
            // 
            // AutoOffKey2
            // 
            this.AutoOffKey2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AutoOffKey2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoOffKey2.Location = new System.Drawing.Point(303, 176);
            this.AutoOffKey2.Name = "AutoOffKey2";
            this.AutoOffKey2.Size = new System.Drawing.Size(41, 23);
            this.AutoOffKey2.TabIndex = 324;
            this.AutoOffKey2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AutoOffKey2.TextChanged += new System.EventHandler(this.AutoOffKey2_TextChanged);
            // 
            // arrowDown3
            // 
            this.arrowDown3.Image = global::_4RTools.Resources._4RTools.Icons.arrow_down;
            this.arrowDown3.Location = new System.Drawing.Point(263, 206);
            this.arrowDown3.Name = "arrowDown3";
            this.arrowDown3.Size = new System.Drawing.Size(11, 14);
            this.arrowDown3.TabIndex = 326;
            this.arrowDown3.TabStop = false;
            // 
            // AutoOffKillClientChk
            // 
            this.AutoOffKillClientChk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AutoOffKillClientChk.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoOffKillClientChk.Location = new System.Drawing.Point(185, 218);
            this.AutoOffKillClientChk.Name = "AutoOffKillClientChk";
            this.AutoOffKillClientChk.Size = new System.Drawing.Size(161, 33);
            this.AutoOffKillClientChk.TabIndex = 328;
            this.AutoOffKillClientChk.Text = "Then stop running client";
            this.StopClientToolTip.SetToolTip(this.AutoOffKillClientChk, "Stops the associated game client");
            this.AutoOffKillClientChk.UseVisualStyleBackColor = true;
            this.AutoOffKillClientChk.CheckedChanged += new System.EventHandler(this.AutoOffKillClientChk_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(287, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 24);
            this.label1.TabIndex = 329;
            this.label1.Text = "+";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // overweightToolTip
            // 
            this.overweightToolTip.AutoPopDelay = 15000;
            this.overweightToolTip.InitialDelay = 300;
            this.overweightToolTip.ReshowDelay = 100;
            this.overweightToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.overweightToolTip.ToolTipTitle = "Overweight Auto-off";
            // 
            // ToggleTimerToolTip
            // 
            this.ToggleTimerToolTip.AutomaticDelay = 300;
            this.ToggleTimerToolTip.AutoPopDelay = 15000;
            this.ToggleTimerToolTip.InitialDelay = 300;
            this.ToggleTimerToolTip.ReshowDelay = 60;
            this.ToggleTimerToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ToggleTimerToolTip.ToolTipTitle = "Start Timer";
            // 
            // AltKey1ToolTip
            // 
            this.AltKey1ToolTip.AutoPopDelay = 15000;
            this.AltKey1ToolTip.InitialDelay = 300;
            this.AltKey1ToolTip.ReshowDelay = 100;
            this.AltKey1ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.AltKey1ToolTip.ToolTipTitle = "Send ALT + __";
            this.AltKey1ToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip2_Popup);
            // 
            // AltKey2ToolTip
            // 
            this.AltKey2ToolTip.AutoPopDelay = 15000;
            this.AltKey2ToolTip.InitialDelay = 300;
            this.AltKey2ToolTip.ReshowDelay = 100;
            this.AltKey2ToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.AltKey2ToolTip.ToolTipTitle = "Send ALT + __";
            // 
            // StopClientToolTip
            // 
            this.StopClientToolTip.AutoPopDelay = 15000;
            this.StopClientToolTip.InitialDelay = 300;
            this.StopClientToolTip.ReshowDelay = 100;
            this.StopClientToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.StopClientToolTip.ToolTipTitle = "Kill Client";
            // 
            // AutoOffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(350, 263);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.arrowDown3);
            this.Controls.Add(this.AutoOffKillClientChk);
            this.Controls.Add(this.AutoOffKey2);
            this.Controls.Add(this.overweightAltKey2Label);
            this.Controls.Add(this.arrowDown2);
            this.Controls.Add(this.arrowRight);
            this.Controls.Add(this.arrowDown1);
            this.Controls.Add(this.AutoOffOverweightCB);
            this.Controls.Add(this.overweightAltKeyPlusLabel);
            this.Controls.Add(this.animatedClockImage);
            this.Controls.Add(this.overweightAltKeyLabel);
            this.Controls.Add(this.AutoOffKey1);
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
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arrowDown3)).EndInit();
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
        private System.Windows.Forms.PictureBox arrowDown1;
        private System.Windows.Forms.PictureBox arrowRight;
        private System.Windows.Forms.TextBox AutoOffKey1;
        private System.Windows.Forms.PictureBox arrowDown2;
        private System.Windows.Forms.Label overweightAltKey2Label;
        private System.Windows.Forms.TextBox AutoOffKey2;
        private System.Windows.Forms.PictureBox arrowDown3;
        private System.Windows.Forms.CheckBox AutoOffKillClientChk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip overweightToolTip;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolTip ToggleTimerToolTip;
        private System.Windows.Forms.ToolTip AltKey1ToolTip;
        private System.Windows.Forms.ToolTip AltKey2ToolTip;
        private System.Windows.Forms.ToolTip StopClientToolTip;
    }
}