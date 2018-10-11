using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corner2CornerClient
{
    public class MousePixelEventsArgs
    {
        public Point Location { get; }
        public Color Color { get; }

        public MousePixelEventsArgs(Point Location, Color color)
        {
            this.Location = Location;
            this.Color = color;
        }
    }

    public class PictureBoxAdapter
    {
        private readonly PictureBox pictureBox;

        public PictureBoxAdapter(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            this.pictureBox.MouseMove += PictureBox_MouseMove;
            this.pictureBox.MouseClick += PictureBox_MouseClick;
        }

        public event EventHandler<MousePixelEventsArgs> MouseOverPixel;
        public event EventHandler<MousePixelEventsArgs> MouseClickPixel;

        private Point? TranslateToImage(Point point)
        {
            double widthRatio = (double)pictureBox.Image.Width / (double)pictureBox.Width;
            double heightRatio = (double)pictureBox.Image.Height / (double)pictureBox.Height;

            int xOffset, yOffset;
            double zoomRatio;

            if (widthRatio > heightRatio)
            {
                zoomRatio = widthRatio;
                xOffset = 0;
                yOffset = (int)((pictureBox.Height - pictureBox.Image.Height / widthRatio) / 2.0);
            }
            else if (heightRatio > widthRatio)
            {
                zoomRatio = heightRatio;
                xOffset = (int)((pictureBox.Width - pictureBox.Image.Width / heightRatio) / 2.0);
                yOffset = 0;
            }
            else
            {
                zoomRatio = widthRatio;
                xOffset = 0;
                yOffset = 0;
            }

            Point result = new Point(
                (int)((point.X - xOffset) * zoomRatio),
                (int)((point.Y - yOffset) * zoomRatio));

            Rectangle imageRectangle = new Rectangle(0, 0, pictureBox.Image.Width, pictureBox.Image.Height);
            if (imageRectangle.Contains(result))
                return result;
            else
                return null;
        }

        private bool TryGetColor(Point point, out MousePixelEventsArgs args)
        {
            if (this.pictureBox.Image != null)
            {
                Point? imagePoint = TranslateToImage(point);

                if (imagePoint.HasValue)
                {
                    args = new MousePixelEventsArgs(
                        imagePoint.Value,
                        ((Bitmap)this.pictureBox.Image).GetPixel(imagePoint.Value.X, imagePoint.Value.Y));
                    return true;
                }
            }

            args = null;
            return false;
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && TryGetColor(e.Location, out MousePixelEventsArgs args))
            {
                this.MouseClickPixel?.Invoke(this, args);
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (TryGetColor(e.Location, out MousePixelEventsArgs args))
            {
                this.MouseOverPixel?.Invoke(this, args);
            }
        }
    }
}
