using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Crochet.Lib
{
    public static class ImageHelper
    {
        private static Bitmap CropImage(Image image, Rectangle cropRectangle)
        {
            Bitmap target = new Bitmap(cropRectangle.Width, cropRectangle.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(
                    image, 
                    new Rectangle(0, 0, target.Width, target.Height),
                    cropRectangle,
                    GraphicsUnit.Pixel);
            }

            return target;
        }

        private static Bitmap ResizeImage(Image image, Rectangle? imageRect, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            var sourceRect = imageRect ?? new Rectangle(0, 0, image.Width, image.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            return ResizeImage(image, null, width, height);
        }

        public static Bitmap ResizeImage(Image image, int width)
        {
            double ratio = (double)image.Width / (double)width;
            int height = (int)((double)image.Height / ratio);

            return ResizeImage(image, width, height);
        }

        private const int pixelSize = 10;
        private const int gridSize = 1;

        public static Point ToImageGridPoint(Point imageLocation)
        {
            return new Point(imageLocation.X / pixelSize, imageLocation.Y / pixelSize);
        }


        private static int GetOffsetForGrid(int index, bool highlightRows)
        {
            if (!highlightRows)
                return 2 * gridSize;

            switch ((index + 1) % 10)
            {
                case 0:
                    return 4 * gridSize;
                case 5:
                    return 3 * gridSize;
                default:
                    return 2 * gridSize;
            }
        }

        public static Bitmap FromImageGrid(
            ImageGrid grid, 
            Color gridColor, 
            bool hightlightRows = false)
        {
            Bitmap bitmap = new Bitmap(
                grid.Width * pixelSize, 
                grid.Height * pixelSize);
            
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(gridColor);

                for (int x = 0; x < grid.Width; x++)
                {
                    for (int y = 0; y < grid.Height; y++)
                    {
                        Color color = grid[x, y];

                        Rectangle colorRect = new Rectangle(
                            x * pixelSize + gridSize,
                            y * pixelSize + gridSize,
                            pixelSize - GetOffsetForGrid(x, hightlightRows),
                            pixelSize - GetOffsetForGrid(y, hightlightRows));

                        using (Brush brush = new SolidBrush(color))
                        {
                            g.FillRectangle(brush, colorRect);
                        }
                    }
                }
            }

            return bitmap;
        }
    }
}
