using BruteGamingMacros.Core.Utils;

namespace BruteGamingMacros.UI.Forms
{
    partial class AutobuffItemForm
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.FoodsGP = new System.Windows.Forms.GroupBox();
            this.PotionsGP = new System.Windows.Forms.GroupBox();
            this.BoxesGP = new System.Windows.Forms.GroupBox();
            this.ElementalsGP = new System.Windows.Forms.GroupBox();
            this.ScrollBuffsGP = new System.Windows.Forms.GroupBox();
            this.EtcGP = new System.Windows.Forms.GroupBox();
            this.btnResetAutobuff = new System.Windows.Forms.Button();
            this.toolTipDelayReset = new System.Windows.Forms.ToolTip(this.components);
            this.numericDelay = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 10;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 2;
            // 
            // FoodsGP
            // 
            this.FoodsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FoodsGP.Location = new System.Drawing.Point(12, 150);
            this.FoodsGP.Name = "FoodsGP";
            this.FoodsGP.Size = new System.Drawing.Size(575, 30);
            this.FoodsGP.TabIndex = 293;
            this.FoodsGP.TabStop = false;
            this.FoodsGP.Text = "Foods";
            // 
            // PotionsGP
            // 
            this.PotionsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PotionsGP.Location = new System.Drawing.Point(12, 40);
            this.PotionsGP.Name = "PotionsGP";
            this.PotionsGP.Size = new System.Drawing.Size(575, 30);
            this.PotionsGP.TabIndex = 294;
            this.PotionsGP.TabStop = false;
            this.PotionsGP.Text = "ASPD Potions";
            // 
            // BoxesGP
            // 
            this.BoxesGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BoxesGP.Location = new System.Drawing.Point(12, 113);
            this.BoxesGP.Name = "BoxesGP";
            this.BoxesGP.Size = new System.Drawing.Size(575, 30);
            this.BoxesGP.TabIndex = 295;
            this.BoxesGP.TabStop = false;
            this.BoxesGP.Text = "Boxes / Speed / Status";
            // 
            // ElementalsGP
            // 
            this.ElementalsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ElementalsGP.Location = new System.Drawing.Point(12, 75);
            this.ElementalsGP.Name = "ElementalsGP";
            this.ElementalsGP.Size = new System.Drawing.Size(575, 30);
            this.ElementalsGP.TabIndex = 296;
            this.ElementalsGP.TabStop = false;
            this.ElementalsGP.Text = "Elementals";
            // 
            // ScrollBuffsGP
            // 
            this.ScrollBuffsGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScrollBuffsGP.Location = new System.Drawing.Point(12, 189);
            this.ScrollBuffsGP.Name = "ScrollBuffsGP";
            this.ScrollBuffsGP.Size = new System.Drawing.Size(575, 30);
            this.ScrollBuffsGP.TabIndex = 297;
            this.ScrollBuffsGP.TabStop = false;
            this.ScrollBuffsGP.Text = "Scrolls";
            // 
            // EtcGP
            // 
            this.EtcGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EtcGP.Location = new System.Drawing.Point(12, 228);
            this.EtcGP.Name = "EtcGP";
            this.EtcGP.Size = new System.Drawing.Size(575, 30);
            this.EtcGP.TabIndex = 298;
            this.EtcGP.TabStop = false;
            this.EtcGP.Text = "Rate Boosters";
            // 
            // btnResetAutobuff
            // 
            this.btnResetAutobuff.BackColor = System.Drawing.Color.White;
            this.btnResetAutobuff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResetAutobuff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetAutobuff.Location = new System.Drawing.Point(535, 12);
            this.btnResetAutobuff.Name = "btnResetAutobuff";
            this.btnResetAutobuff.Size = new System.Drawing.Size(60, 23);
            this.btnResetAutobuff.TabIndex = 299;
            this.btnResetAutobuff.Text = "Reset\r\n";
            this.toolTipDelayReset.SetToolTip(this.btnResetAutobuff, "Reset all values in this tab to default");
            this.btnResetAutobuff.UseVisualStyleBackColor = false;
            this.btnResetAutobuff.Click += new System.EventHandler(this.btnResetAutobuff_Click);
            // 
            // toolTipDelayReset
            // 
            this.toolTipDelayReset.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipDelayReset_Popup);
            // 
            // numericDelay
            // 
            this.numericDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericDelay.Location = new System.Drawing.Point(469, 12);
            this.numericDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericDelay.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericDelay.Name = "numericDelay";
            this.numericDelay.Size = new System.Drawing.Size(60, 22);
            this.numericDelay.TabIndex = 302;
            this.numericDelay.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericDelay.ValueChanged += new System.EventHandler(this.numericDelay_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(409, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 300;
            this.label5.Text = "Delay (ms)";
            // 
            // AutobuffItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(607, 274);
            this.Controls.Add(this.numericDelay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnResetAutobuff);
            this.Controls.Add(this.EtcGP);
            this.Controls.Add(this.ScrollBuffsGP);
            this.Controls.Add(this.ElementalsGP);
            this.Controls.Add(this.BoxesGP);
            this.Controls.Add(this.PotionsGP);
            this.Controls.Add(this.FoodsGP);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutobuffItemForm";
            this.Text = "AutobuffItemForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox FoodsGP;
        private System.Windows.Forms.GroupBox PotionsGP;
        private System.Windows.Forms.GroupBox BoxesGP;
        private System.Windows.Forms.GroupBox ElementalsGP;
        private System.Windows.Forms.GroupBox ScrollBuffsGP;
        private System.Windows.Forms.GroupBox EtcGP;
        private System.Windows.Forms.Button btnResetAutobuff;
        private System.Windows.Forms.ToolTip toolTipDelayReset;
        private System.Windows.Forms.NumericUpDown numericDelay;
        private System.Windows.Forms.Label label5;
    }
}