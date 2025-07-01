using _4RTools.Utils;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _4RTools.Controls
{
    public class BorderedCheckBox : CheckBox
    {
        private const int BoxSize = 14;
        private const int BoxMargin = 3;
        private const int ImageOffsetX = 2;
        private const int ImageOffsetY = -2;

        public BorderedCheckBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            TextAlign = ContentAlignment.MiddleLeft;
            ThreeState = true;
            Size = new Size(20, 20);

            // Remove empty event handler since it's not being used
            // CheckStateChanged += BorderedCheckBox_CheckStateChanged;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.Clear(BackColor);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var boxRect = CalculateBoxRectangle();
            var contentRect = CalculateContentRectangle(boxRect);

            DrawCheckBox(graphics, boxRect);
            DrawImage(graphics, contentRect);
            DrawFocusRectangle(graphics);
        }

        private Rectangle CalculateBoxRectangle()
        {
            return new Rectangle(BoxMargin, (Height - BoxSize) / 2, BoxSize, BoxSize);
        }

        private Rectangle CalculateContentRectangle(Rectangle boxRect)
        {
            return new Rectangle(boxRect.Right, 0, Width - boxRect.Right, Height);
        }

        private void DrawCheckBox(Graphics graphics, Rectangle boxRect)
        {
            var (borderColor, fillColor, markColor) = GetColors();

            // Fill background
            using (var brush = new SolidBrush(fillColor))
            {
                graphics.FillRectangle(brush, boxRect);
            }

            // Draw border
            using (var pen = new Pen(borderColor, 1))
            {
                graphics.DrawRectangle(pen, boxRect);
            }

            // Draw check mark or indeterminate state
            switch (CheckState)
            {
                case CheckState.Checked:
                    DrawCheckMark(graphics, boxRect, markColor);
                    break;
                case CheckState.Indeterminate:
                    DrawIndeterminateMark(graphics, boxRect);
                    break;
            }
        }

        private (Color borderColor, Color fillColor, Color markColor) GetColors()
        {
            if (!Enabled)
            {
                return (AppConfig.CheckBoxDisabledBorderColor,
                       AppConfig.CheckBoxDisabledBorderColor,
                       Color.FromArgb(150, 150, 150));
            }

            switch (CheckState)
            {
                case CheckState.Checked:
                    return (AppConfig.CheckBoxCheckedBorderColor,
                           AppConfig.CheckBoxCheckedBorderColor,
                           Color.White);
                case CheckState.Indeterminate:
                    return (AppConfig.CheckBoxIndeterminateBorderColor,
                           AppConfig.CheckBoxIndeterminateBorderColor,
                           Color.White);
                default:
                    return (AppConfig.CheckBoxUncheckedBorderColor, Color.White, Color.White);
            }
        }

        private static void DrawCheckMark(Graphics graphics, Rectangle boxRect, Color markColor)
        {
            using (var pen = new Pen(markColor, 2))
            {
                var points = new[]
                {
                    new Point(boxRect.Left + 3, boxRect.Top + 6),
                    new Point(boxRect.Left + 6, boxRect.Top + 9),
                    new Point(boxRect.Left + 10, boxRect.Top + 4)
                };
                graphics.DrawLines(pen, points);
            }
        }

        private static void DrawIndeterminateMark(Graphics graphics, Rectangle boxRect)
        {
            var originalMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.None;

            const int dashWidth = 8;
            const int dashHeight = 2;
            var dashX = boxRect.Left + (boxRect.Width - dashWidth) / 2;
            var dashY = boxRect.Top + (boxRect.Height - dashHeight) / 2;
            var dashRect = new Rectangle(dashX, dashY, dashWidth, dashHeight);

            using (var dashBrush = new SolidBrush(Color.White))
            {
                graphics.FillRectangle(dashBrush, dashRect);
            }

            graphics.SmoothingMode = originalMode;
        }

        private void DrawImage(Graphics graphics, Rectangle contentRect)
        {
            if (Image == null) return;

            var imageX = contentRect.X;
            var imageY = contentRect.Y;

            if (ImageAlign == ContentAlignment.BottomCenter)
            {
                imageX = contentRect.X + (contentRect.Width - Image.Width) / 2;
                imageY = contentRect.Bottom - Image.Height;
            }

            graphics.DrawImage(Image, imageX + ImageOffsetX, imageY + ImageOffsetY);
        }

        private void DrawFocusRectangle(Graphics graphics)
        {
            if (Focused && ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(graphics, ClientRectangle);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Clean up any managed resources if needed in the future
            }
            base.Dispose(disposing);
        }
    }
}