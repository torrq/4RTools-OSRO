using _4RTools.Utils;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static _4RTools.Utils.DebugLogger;

namespace _4RTools.Controls
{
    /// <summary>
    /// A custom CheckBox control that manually paints itself to allow for a
    /// colored border on the checkbox square itself, while preserving the image content.
    /// </summary>
    public class BorderedCheckBox : CheckBox
    {
        public BorderedCheckBox()
        {
            // We will handle all painting to avoid flicker and gain full control.
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.TextAlign = ContentAlignment.MiddleLeft; // Ensures text/image is to the right of the box.
            this.ThreeState = true; // Enable three-state by default, can be overridden
            this.CheckStateChanged += BorderedCheckBox_CheckStateChanged; // Debug event
            this.Size = new Size(20, 20); // Adjust control size to accommodate 14x14 box with padding
        }

        private void BorderedCheckBox_CheckStateChanged(object sender, System.EventArgs e)
        {
            DebugLogger.Log(LogLevel.DEBUG, $"BorderedCheckBox {this.Name} state changed to: {this.CheckState}");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // We don't call base.OnPaint(e) because we are drawing everything from scratch.
            e.Graphics.Clear(this.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Default smoothing for overall rendering

            // --- 1. Define Rectangles ---
            // Fixed 14x14 pixel box, centered vertically
            Rectangle boxRect = new Rectangle(3, (this.Height - 14) / 2, 14, 14);
            Rectangle contentRect = new Rectangle(boxRect.Right, 0, this.Width - boxRect.Right, this.Height);

            // --- 2. Determine Colors and State ---
            Color borderColor;
            Color fillColor = default;
            Color markColor = Color.White; // Color of the checkmark or dash

            if (!this.Enabled)
            {
                borderColor = AppConfig.CheckBoxDisabledBorderColor;
                fillColor = AppConfig.CheckBoxDisabledBorderColor;
                markColor = Color.FromArgb(150, 150, 150); // Slightly darker gray for disabled mark
            }
            else
            {
                switch (this.CheckState)
                {
                    case CheckState.Checked:
                        borderColor = AppConfig.CheckBoxCheckedBorderColor;
                        fillColor = AppConfig.CheckBoxCheckedBorderColor;
                        break;
                    case CheckState.Indeterminate:
                        borderColor = AppConfig.CheckBoxIndeterminateBorderColor;
                        fillColor = AppConfig.CheckBoxIndeterminateBorderColor;
                        break;
                    default: // Unchecked
                        borderColor = AppConfig.CheckBoxUncheckedBorderColor;
                        fillColor = Color.White;
                        break;
                }
            }

            // --- 3. Draw the CheckBox Box and Mark ---
            // Draw the background fill of the box.
            using (SolidBrush b = new SolidBrush(fillColor))
            {
                e.Graphics.FillRectangle(b, boxRect);
            }

            // Draw the border of the box.
            using (Pen p = new Pen(borderColor, 1))
            {
                e.Graphics.DrawRectangle(p, boxRect);
            }

            // Draw the checkmark for Checked state
            if (this.CheckState == CheckState.Checked)
            {
                using (Pen p = new Pen(markColor, 2))
                {
                    e.Graphics.DrawLines(p, new Point[] {
                        new Point(boxRect.Left + 3, boxRect.Top + 6),
                        new Point(boxRect.Left + 6, boxRect.Top + 9),
                        new Point(boxRect.Left + 10, boxRect.Top + 4)
                    });
                }
            }
            // Draw dash for Indeterminate state
            else if (this.CheckState == CheckState.Indeterminate)
            {
                // Temporarily disable smoothing for sharp edges
                SmoothingMode originalMode = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.None;

                // Draw a centered horizontal white dash on orange background
                int dashWidth = 8; // Fixed width to fit 14x14 box
                int dashHeight = 2; // Thin dash height
                int dashX = boxRect.Left + (boxRect.Width - dashWidth) / 2; // Centered horizontally
                int dashY = boxRect.Top + (boxRect.Height - dashHeight) / 2; // Centered vertically
                Rectangle dashRect = new Rectangle(dashX, dashY, dashWidth, dashHeight);

                using (SolidBrush dashBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(dashBrush, dashRect);
                }

                // Restore original smoothing mode
                e.Graphics.SmoothingMode = originalMode;
            }

            // --- 4. Draw the Content (Image) ---
            if (this.Image != null)
            {
                int imageX = contentRect.X;
                int imageY = contentRect.Y;

                if (this.ImageAlign == ContentAlignment.BottomCenter)
                {
                    imageX = contentRect.X + (contentRect.Width - this.Image.Width) / 2;
                    imageY = contentRect.Bottom - this.Image.Height;
                }

                e.Graphics.DrawImage(this.Image, imageX +2, imageY - 2);
            }

            // --- 5. Draw Focus Rectangle ---
            if (this.Focused && this.ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
            }
        }
    }
}