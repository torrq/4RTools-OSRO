using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace _ORTools.Utils
{
    public class CharacterInfoLabel : Label
    {
        private ContentAlignment textAlign = ContentAlignment.TopLeft;
        private int spacePadding = 0; // Configurable space padding for centering
        private int autoPaddingThreshold = 50; // Length threshold above which no auto-padding is applied

        public CharacterInfoLabel()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        public new ContentAlignment TextAlign
        {
            get => textAlign;
            set
            {
                textAlign = value;
                Invalidate(); // Redraw on change
            }
        }

        /// <summary>
        /// Number of spaces to pad on each side for manual centering adjustment
        /// </summary>
        public int SpacePadding
        {
            get => spacePadding;
            set
            {
                spacePadding = Math.Max(0, value); // Don't allow negative values
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush backBrush = new SolidBrush(this.BackColor))
                e.Graphics.FillRectangle(backBrush, this.ClientRectangle);

            e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

            string text = this.Text ?? string.Empty;
            if (string.IsNullOrEmpty(text)) return;

            // Apply padding based on alignment
            string paddedText = ApplyTextPadding(text, this.textAlign);

            // Simple top-left positioning since we're using padding for alignment
            Point location = new Point(0, 0);

            // Handle vertical alignment with basic positioning
            if (textAlign == ContentAlignment.MiddleLeft || textAlign == ContentAlignment.MiddleCenter || textAlign == ContentAlignment.MiddleRight)
            {
                // Rough vertical centering
                location.Y = (this.ClientSize.Height - this.Font.Height) / 2;
            }
            else if (textAlign == ContentAlignment.BottomLeft || textAlign == ContentAlignment.BottomCenter || textAlign == ContentAlignment.BottomRight)
            {
                location.Y = this.ClientSize.Height - this.Font.Height;
            }

            // Use DrawString (honors TextRenderingHint) with location
            using (Brush textBrush = new SolidBrush(this.ForeColor))
                e.Graphics.DrawString(paddedText, this.Font, textBrush, location);
        }

        private string ApplyTextPadding(string text, ContentAlignment align)
        {
            // Only apply padding for center alignment
            if (align == ContentAlignment.TopCenter || align == ContentAlignment.MiddleCenter || align == ContentAlignment.BottomCenter)
            {
                int effectivePadding = CalculateEffectivePadding(text);

                if (effectivePadding > 0)
                {
                    // For multi-line text, pad each line
                    if (text.Contains("\n"))
                    {
                        string padding = new string(' ', effectivePadding);
                        return padding + text.Replace("\n", "\n" + padding);
                    }
                    else
                    {
                        // Single line
                        return new string(' ', effectivePadding) + text;
                    }
                }
            }

            return text; // No padding for other alignments or zero padding
        }

        private int CalculateEffectivePadding(string text)
        {
            // Find the longest line
            string[] lines = text.Split('\n');
            int maxLineLength = 0;

            foreach (string line in lines)
            {
                if (line.Length > maxLineLength)
                    maxLineLength = line.Length;
            }

            // If longest line exceeds threshold, no auto-padding
            if (maxLineLength >= autoPaddingThreshold)
                return spacePadding; // Only use manual padding

            // Calculate auto-padding based on line length
            int autoPadding = Math.Max(0, (autoPaddingThreshold - maxLineLength) / 4); // Divide by 4 for reasonable padding

            // Combine manual and auto padding
            return spacePadding + autoPadding;
        }
    }
}