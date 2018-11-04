using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal class ImageGridder
    {
        private const int pixelsPerCell = 10;
        private readonly int columns;

        private ImageGridder(int columns)
        {
            this.columns = columns;
        }

        private ImageGrid Load(Image originalImage, IEnumerable<IPaletteItem> palette)
        {
            var paletteByColor = palette.ToDictionary(p => p.Color);

            int requiredWidth = columns * pixelsPerCell;
            using (var resizedImage = ImageHelper.ResizeImage(originalImage, requiredWidth))
            {
                ColorHelper colorHelper = new ColorHelper(resizedImage, paletteByColor.Keys);
                int rows = resizedImage.Height / pixelsPerCell;

                ImageGrid grid = new ImageGrid(columns, rows);

                for (int x = 0; x < columns; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        Rectangle cellRect = new Rectangle(x * pixelsPerCell, y * pixelsPerCell, pixelsPerCell, pixelsPerCell);
                        grid[x, y] = paletteByColor[colorHelper.GetColor(cellRect)];
                    }
                }

                return grid;
            }
        }

        public static ImageGrid Create(int columns, Image originalImage, IEnumerable<IPaletteItem> palette)
        {
            ImageGridder gridder = new ImageGridder(columns);
            return gridder.Load(originalImage, palette);
        }
    }
}
