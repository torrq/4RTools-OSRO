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

        /// <summary>When true, the HP segment on line 2 is drawn in red.</summary>
        public bool HpLow { get; set; }

        /// <summary>When true, the SP segment on line 2 is drawn in red.</summary>
        public bool SpLow { get; set; }

        private static readonly Color LowColor = Color.FromArgb(220, 50, 50);

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Brush backBrush = new SolidBrush(this.BackColor))
                e.Graphics.FillRectangle(backBrush, this.ClientRectangle);

            e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

            string text = this.Text ?? string.Empty;
            if (string.IsNullOrEmpty(text)) return;

            // Split into lines; only line 2 (index 1) gets segment coloring
            string[] lines = text.Split('\n');

            float y = 0f;
            if (textAlign == ContentAlignment.MiddleLeft || textAlign == ContentAlignment.MiddleCenter || textAlign == ContentAlignment.MiddleRight)
                y = (this.ClientSize.Height - this.Font.Height * lines.Length) / 2f;
            else if (textAlign == ContentAlignment.BottomLeft || textAlign == ContentAlignment.BottomCenter || textAlign == ContentAlignment.BottomRight)
                y = this.ClientSize.Height - this.Font.Height * lines.Length;

            using (Brush normalBrush = new SolidBrush(this.ForeColor))
            using (Brush lowBrush = new SolidBrush(LowColor))
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string paddedLine = ApplyTextPadding(line, this.textAlign);

                    // Line 2 (0-based index 1) with HpLow or SpLow: draw HP / SP segments in color
                    if (i == 1 && (HpLow || SpLow) && paddedLine.Contains("HP ") && paddedLine.Contains("| SP "))
                    {
                        DrawHpSpLine(e.Graphics, paddedLine, y, normalBrush, lowBrush);
                    }
                    else
                    {
                        e.Graphics.DrawString(paddedLine, this.Font, normalBrush, 0f, y);
                    }

                    y += this.Font.Height;
                }
            }
        }

        /// <summary>
        /// Draws "HP x / y | SP x / y" with per-segment color based on HpLow/SpLow.
        /// Segments: [HP part] [ | ] [SP part]
        /// </summary>
        private void DrawHpSpLine(Graphics g, string line, float y, Brush normalBrush, Brush lowBrush)
        {
            int sepIdx = line.IndexOf("| SP ");
            if (sepIdx < 0)
            {
                g.DrawString(line, this.Font, normalBrush, 0f, y);
                return;
            }

            // hpPart = everything up to and including the space before |
            // spStart = index of 'S' in "SP"
            int spStart = sepIdx + 2; // pipe + space, then 'S'

            // Draw the full line in normal color first — this establishes correct spacing
            g.DrawString(line, this.Font, normalBrush, 0f, y);

            // Now overdraw only the segments that need a different color
            // Measure character offsets within the full string using MeasureCharacterRanges
            var fmt = new StringFormat();

            if (HpLow && lowBrush != normalBrush)
            {
                // HP segment: chars 0..sepIdx-1
                fmt.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, sepIdx) });
                var regions = g.MeasureCharacterRanges(line, this.Font,
                    new RectangleF(0, y, 2000, 100), fmt);
                RectangleF hpBounds = regions[0].GetBounds(g);
                // Clip to HP region and redraw
                g.SetClip(new RectangleF(0f, y, hpBounds.Right, this.Font.Height + 2));
                g.DrawString(line, this.Font, lowBrush, 0f, y);
                g.ResetClip();
            }

            if (SpLow && lowBrush != normalBrush)
            {
                // SP segment: chars spStart..end
                fmt.SetMeasurableCharacterRanges(new[] { new CharacterRange(spStart, line.Length - spStart) });
                var regions = g.MeasureCharacterRanges(line, this.Font,
                    new RectangleF(0, y, 2000, 100), fmt);
                RectangleF spBounds = regions[0].GetBounds(g);
                g.SetClip(new RectangleF(spBounds.Left - 3, y, spBounds.Width + 7, this.Font.Height + 2));
                g.DrawString(line, this.Font, lowBrush, 0f, y);
                g.ResetClip();
            }
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