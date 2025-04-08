using _4RTools.Utils;
using System.Windows.Forms;
using System;

namespace _4RTools.Forms
{
    partial class AutoSwitchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoSwitchForm));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AutoSwitchGP = new System.Windows.Forms.GroupBox();
            this.btnAddAutoSwitch = new System.Windows.Forms.Button();
            this.skillCB = new System.Windows.Forms.ComboBox();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.numSwitchDelay = new System.Windows.Forms.NumericUpDown();
            this.NEXTITEMin319 = new System.Windows.Forms.TextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ITEMin319 = new System.Windows.Forms.TextBox();
            this.NEXTITEMin2015 = new System.Windows.Forms.TextBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.ITEMin2015 = new System.Windows.Forms.TextBox();
            this.NEXTITEMin25 = new System.Windows.Forms.TextBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.ITEMin25 = new System.Windows.Forms.TextBox();
            this.NEXTITEMin355 = new System.Windows.Forms.TextBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.ITEMin355 = new System.Windows.Forms.TextBox();
            this.NEXTITEMin461 = new System.Windows.Forms.TextBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.ITEMin461 = new System.Windows.Forms.TextBox();
            this.NEXTITEMin126 = new System.Windows.Forms.TextBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.ITEMin126 = new System.Windows.Forms.TextBox();
            this.ProcSwitchGP = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSwitchDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.ProcSwitchGP.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 10;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(330, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 321;
            this.label1.Tag = "";
            this.label1.Text = "Delay";
            this.toolTip1.SetToolTip(this.label1, "Delay geral Recomendado 300 ms");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(435, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 322;
            this.label4.Text = "Exchange Delay";
            this.toolTip1.SetToolTip(this.label4, "Delay entre Item e Próximo Item Recomendado 1000 ms");
            // 
            // AutoSwitchGP
            // 
            this.AutoSwitchGP.AutoSize = true;
            this.AutoSwitchGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AutoSwitchGP.Location = new System.Drawing.Point(12, 38);
            this.AutoSwitchGP.Name = "AutoSwitchGP";
            this.AutoSwitchGP.Size = new System.Drawing.Size(302, 37);
            this.AutoSwitchGP.TabIndex = 314;
            this.AutoSwitchGP.TabStop = false;
            this.AutoSwitchGP.Text = "Customized";
            // 
            // btnAddAutoSwitch
            // 
            this.btnAddAutoSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAutoSwitch.Location = new System.Drawing.Point(269, 5);
            this.btnAddAutoSwitch.Name = "btnAddAutoSwitch";
            this.btnAddAutoSwitch.Size = new System.Drawing.Size(45, 23);
            this.btnAddAutoSwitch.TabIndex = 316;
            this.btnAddAutoSwitch.Text = "Add";
            this.btnAddAutoSwitch.UseVisualStyleBackColor = false;
            this.btnAddAutoSwitch.Click += new System.EventHandler(this.btnNewSwitch);
            // 
            // skillCB
            // 
            this.skillCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.skillCB.Location = new System.Drawing.Point(12, 7);
            this.skillCB.Name = "skillCB";
            this.skillCB.Size = new System.Drawing.Size(251, 21);
            this.skillCB.TabIndex = 317;
            // 
            // numDelay
            // 
            this.numDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numDelay.Location = new System.Drawing.Point(369, 8);
            this.numDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(60, 22);
            this.numDelay.TabIndex = 319;
            this.numDelay.Tag = "delay";
            this.numDelay.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numDelay.ValueChanged += new System.EventHandler(this.txtDelay_TextChanged);
            // 
            // numSwitchDelay
            // 
            this.numSwitchDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numSwitchDelay.Location = new System.Drawing.Point(526, 8);
            this.numSwitchDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numSwitchDelay.Name = "numSwitchDelay";
            this.numSwitchDelay.Size = new System.Drawing.Size(60, 22);
            this.numSwitchDelay.TabIndex = 320;
            this.numSwitchDelay.Tag = "switchDelay";
            this.numSwitchDelay.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSwitchDelay.ValueChanged += new System.EventHandler(this.txtSwitchDelay_TextChanged);
            // 
            // NEXTITEMin319
            // 
            this.NEXTITEMin319.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NEXTITEMin319.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.NEXTITEMin319.Location = new System.Drawing.Point(158, 30);
            this.NEXTITEMin319.Name = "NEXTITEMin319";
            this.NEXTITEMin319.Size = new System.Drawing.Size(45, 25);
            this.NEXTITEMin319.TabIndex = 308;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(116, 36);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(19, 11);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 313;
            this.pictureBox4.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 305;
            this.label3.Text = "Next item";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 304;
            this.label2.Text = "Item";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 296;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "319";
            // 
            // ITEMin319
            // 
            this.ITEMin319.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ITEMin319.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ITEMin319.Location = new System.Drawing.Point(51, 30);
            this.ITEMin319.Name = "ITEMin319";
            this.ITEMin319.Size = new System.Drawing.Size(45, 25);
            this.ITEMin319.TabIndex = 295;
            // 
            // NEXTITEMin2015
            // 
            this.NEXTITEMin2015.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NEXTITEMin2015.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.NEXTITEMin2015.Location = new System.Drawing.Point(158, 60);
            this.NEXTITEMin2015.Name = "NEXTITEMin2015";
            this.NEXTITEMin2015.Size = new System.Drawing.Size(45, 25);
            this.NEXTITEMin2015.TabIndex = 316;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(116, 66);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(19, 11);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 317;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(10, 59);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 24);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 315;
            this.pictureBox2.TabStop = false;
            // 
            // ITEMin2015
            // 
            this.ITEMin2015.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ITEMin2015.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ITEMin2015.Location = new System.Drawing.Point(51, 60);
            this.ITEMin2015.Name = "ITEMin2015";
            this.ITEMin2015.Size = new System.Drawing.Size(45, 25);
            this.ITEMin2015.TabIndex = 314;
            // 
            // NEXTITEMin25
            // 
            this.NEXTITEMin25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NEXTITEMin25.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.NEXTITEMin25.Location = new System.Drawing.Point(158, 90);
            this.NEXTITEMin25.Name = "NEXTITEMin25";
            this.NEXTITEMin25.Size = new System.Drawing.Size(45, 25);
            this.NEXTITEMin25.TabIndex = 320;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.Location = new System.Drawing.Point(116, 96);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(19, 11);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 321;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(10, 89);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(24, 24);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 319;
            this.pictureBox5.TabStop = false;
            // 
            // ITEMin25
            // 
            this.ITEMin25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ITEMin25.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ITEMin25.Location = new System.Drawing.Point(51, 90);
            this.ITEMin25.Name = "ITEMin25";
            this.ITEMin25.Size = new System.Drawing.Size(45, 25);
            this.ITEMin25.TabIndex = 318;
            // 
            // NEXTITEMin355
            // 
            this.NEXTITEMin355.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NEXTITEMin355.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.NEXTITEMin355.Location = new System.Drawing.Point(158, 120);
            this.NEXTITEMin355.Name = "NEXTITEMin355";
            this.NEXTITEMin355.Size = new System.Drawing.Size(45, 25);
            this.NEXTITEMin355.TabIndex = 324;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox8.Image")));
            this.pictureBox8.Location = new System.Drawing.Point(116, 128);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(19, 11);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 325;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox7.Image")));
            this.pictureBox7.Location = new System.Drawing.Point(10, 119);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(24, 24);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 323;
            this.pictureBox7.TabStop = false;
            // 
            // ITEMin355
            // 
            this.ITEMin355.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ITEMin355.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ITEMin355.Location = new System.Drawing.Point(51, 120);
            this.ITEMin355.Name = "ITEMin355";
            this.ITEMin355.Size = new System.Drawing.Size(45, 25);
            this.ITEMin355.TabIndex = 322;
            // 
            // NEXTITEMin461
            // 
            this.NEXTITEMin461.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NEXTITEMin461.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.NEXTITEMin461.Location = new System.Drawing.Point(158, 150);
            this.NEXTITEMin461.Name = "NEXTITEMin461";
            this.NEXTITEMin461.Size = new System.Drawing.Size(45, 25);
            this.NEXTITEMin461.TabIndex = 328;
            // 
            // pictureBox10
            // 
            this.pictureBox10.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox10.Image")));
            this.pictureBox10.Location = new System.Drawing.Point(116, 156);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(19, 11);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox10.TabIndex = 329;
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox9.Image")));
            this.pictureBox9.Location = new System.Drawing.Point(10, 149);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(24, 24);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox9.TabIndex = 327;
            this.pictureBox9.TabStop = false;
            // 
            // ITEMin461
            // 
            this.ITEMin461.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ITEMin461.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ITEMin461.Location = new System.Drawing.Point(51, 150);
            this.ITEMin461.Name = "ITEMin461";
            this.ITEMin461.Size = new System.Drawing.Size(45, 25);
            this.ITEMin461.TabIndex = 326;
            // 
            // NEXTITEMin126
            // 
            this.NEXTITEMin126.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NEXTITEMin126.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.NEXTITEMin126.Location = new System.Drawing.Point(158, 180);
            this.NEXTITEMin126.Name = "NEXTITEMin126";
            this.NEXTITEMin126.Size = new System.Drawing.Size(45, 25);
            this.NEXTITEMin126.TabIndex = 332;
            // 
            // pictureBox12
            // 
            this.pictureBox12.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox12.Image")));
            this.pictureBox12.Location = new System.Drawing.Point(116, 186);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(19, 11);
            this.pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox12.TabIndex = 333;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox11.Image")));
            this.pictureBox11.Location = new System.Drawing.Point(10, 179);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(24, 24);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox11.TabIndex = 331;
            this.pictureBox11.TabStop = false;
            // 
            // ITEMin126
            // 
            this.ITEMin126.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ITEMin126.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ITEMin126.Location = new System.Drawing.Point(51, 180);
            this.ITEMin126.Name = "ITEMin126";
            this.ITEMin126.Size = new System.Drawing.Size(45, 25);
            this.ITEMin126.TabIndex = 330;
            // 
            // ProcSwitchGP
            // 
            this.ProcSwitchGP.AutoSize = true;
            this.ProcSwitchGP.Controls.Add(this.ITEMin126);
            this.ProcSwitchGP.Controls.Add(this.pictureBox11);
            this.ProcSwitchGP.Controls.Add(this.pictureBox12);
            this.ProcSwitchGP.Controls.Add(this.NEXTITEMin126);
            this.ProcSwitchGP.Controls.Add(this.ITEMin461);
            this.ProcSwitchGP.Controls.Add(this.pictureBox9);
            this.ProcSwitchGP.Controls.Add(this.pictureBox10);
            this.ProcSwitchGP.Controls.Add(this.NEXTITEMin461);
            this.ProcSwitchGP.Controls.Add(this.ITEMin355);
            this.ProcSwitchGP.Controls.Add(this.pictureBox7);
            this.ProcSwitchGP.Controls.Add(this.pictureBox8);
            this.ProcSwitchGP.Controls.Add(this.NEXTITEMin355);
            this.ProcSwitchGP.Controls.Add(this.ITEMin25);
            this.ProcSwitchGP.Controls.Add(this.pictureBox5);
            this.ProcSwitchGP.Controls.Add(this.pictureBox6);
            this.ProcSwitchGP.Controls.Add(this.NEXTITEMin25);
            this.ProcSwitchGP.Controls.Add(this.ITEMin2015);
            this.ProcSwitchGP.Controls.Add(this.pictureBox2);
            this.ProcSwitchGP.Controls.Add(this.pictureBox3);
            this.ProcSwitchGP.Controls.Add(this.NEXTITEMin2015);
            this.ProcSwitchGP.Controls.Add(this.ITEMin319);
            this.ProcSwitchGP.Controls.Add(this.pictureBox1);
            this.ProcSwitchGP.Controls.Add(this.label2);
            this.ProcSwitchGP.Controls.Add(this.label3);
            this.ProcSwitchGP.Controls.Add(this.pictureBox4);
            this.ProcSwitchGP.Controls.Add(this.NEXTITEMin319);
            this.ProcSwitchGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProcSwitchGP.Location = new System.Drawing.Point(333, 38);
            this.ProcSwitchGP.Name = "ProcSwitchGP";
            this.ProcSwitchGP.Size = new System.Drawing.Size(244, 226);
            this.ProcSwitchGP.TabIndex = 318;
            this.ProcSwitchGP.TabStop = false;
            this.ProcSwitchGP.Text = "Exclusives";
            // 
            // AutoSwitchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(606, 388);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numSwitchDelay);
            this.Controls.Add(this.numDelay);
            this.Controls.Add(this.ProcSwitchGP);
            this.Controls.Add(this.skillCB);
            this.Controls.Add(this.btnAddAutoSwitch);
            this.Controls.Add(this.AutoSwitchGP);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoSwitchForm";
            this.Text = "AutoSwitchForm";
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSwitchDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.ProcSwitchGP.ResumeLayout(false);
            this.ProcSwitchGP.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private GroupBox AutoSwitchGP;
        private Button btnAddAutoSwitch;
        private ComboBox skillCB;
        private NumericUpDown numDelay;
        private NumericUpDown numSwitchDelay;
        private Label label1;
        private Label label4;
        private TextBox NEXTITEMin319;
        private PictureBox pictureBox4;
        private Label label3;
        private Label label2;
        private PictureBox pictureBox1;
        private TextBox ITEMin319;
        private TextBox NEXTITEMin2015;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private TextBox ITEMin2015;
        private TextBox NEXTITEMin25;
        private PictureBox pictureBox6;
        private PictureBox pictureBox5;
        private TextBox ITEMin25;
        private TextBox NEXTITEMin355;
        private PictureBox pictureBox8;
        private PictureBox pictureBox7;
        private TextBox ITEMin355;
        private TextBox NEXTITEMin461;
        private PictureBox pictureBox10;
        private PictureBox pictureBox9;
        private TextBox ITEMin461;
        private TextBox NEXTITEMin126;
        private PictureBox pictureBox12;
        private PictureBox pictureBox11;
        private TextBox ITEMin126;
        private GroupBox ProcSwitchGP;
    }
}