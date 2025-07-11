using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace _4RTools.Utils
{
    public static class ScaleImageNearestNeighbor
    {
        /// <summary>
        /// Scales an image using nearest neighbor interpolation.
        /// </summary>
        /// <param name="original">The original image to scale.</param>
        /// <param name="scale">The scale factor (e.g., 2 for 2x scale).</param>
        /// <returns>A new scaled image.</returns>
        public static Image Scale(Image original, double scale)
        {
            if (original == null) throw new ArgumentNullException(nameof(original));
            if (scale < 1) throw new ArgumentOutOfRangeException(nameof(scale), "Scale must be >= 1");

            double newWidth = original.Width * scale;
            double newHeight = original.Height * scale;

            Bitmap scaled = new Bitmap((int)newWidth, (int)newHeight);
            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half; // Helps keep pixels sharp
                g.DrawImage(original, 0, 0, (int)newWidth, (int)newHeight);
            }

            return scaled;
        }
    }
}
