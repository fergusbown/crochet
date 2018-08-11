using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Crochet.Lib
{
    public class ImageGridder
    {
        private const int pixelsPerCell = 10;
        private readonly int columns;
        public ImageGridder(int columns)
        {
            this.columns = columns;
        }

        public ImageGrid Load(Stream stream, IEnumerable<Color> palette)
        {
            using (var originalImage = Image.FromStream(stream))
            {
                int requiredWidth = columns * pixelsPerCell;
                using (var resizedImage = ImageHelper.ResizeImage(originalImage, requiredWidth))
                {
                    ColorHelper colorHelper = new ColorHelper(resizedImage, palette);
                    int rows = resizedImage.Height / pixelsPerCell;

                    ImageGrid grid = new ImageGrid(columns, rows);

                    for (int x = 0; x < columns; x++)
                    {
                        for (int y = 0; y < rows; y++)
                        {
                            Rectangle cellRect = new Rectangle(x * pixelsPerCell, y * pixelsPerCell, pixelsPerCell, pixelsPerCell);
                            grid[x, y] = colorHelper.GetColor(cellRect);
                        }
                    }

                    return grid;
                }
            }
        }
    }
}
