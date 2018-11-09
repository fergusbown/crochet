using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal static class ImageGridPersistence
    {
        public static ImageGrid Read(BinaryReader reader, Corner2CornerPalette palette)
        {
            bool haveGrid = reader.ReadBoolean();

            if (haveGrid)
            {
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();

                ImageGrid result = new ImageGrid(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (reader.ReadBoolean())
                        {
                            IPaletteItem cell;
                            palette.Find(Color.FromArgb(reader.ReadInt32()), out cell);
                            result[x, y] = cell;
                        }
                    }
                }

                return result;
            }
            else
            {
                return null;
            }
        }

        public static void Write(BinaryWriter writer, ImageGrid grid)
        {
            if (grid == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);

                writer.Write(grid.Width);
                writer.Write(grid.Height);

                for (int x = 0; x < grid.Width; x++)
                {
                    for (int y = 0; y < grid.Height; y++)
                    {
                        IPaletteItem cell = grid[x, y];

                        if (cell == null)
                        {
                            writer.Write(false);
                        }
                        else
                        {
                            writer.Write(true);
                            writer.Write(cell.Color.ToArgb());
                        }
                    }
                }
            }
        }
    }
}
