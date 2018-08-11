using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Crochet.Lib
{
    public class RowCoordinates
    {
        public int RowIndex { get; }
        public Point Start { get; }

        public Point End { get; }

        public RowCoordinates(int rowIndex, Point start, Point end)
        {
            this.RowIndex = rowIndex;
            this.Start = start;
            this.End = end;
        }
    }

    public class ImageGrid
    {
        private readonly Color[,] grid;
        public ImageGrid(int width, int height)
        {
            this.grid = new Color[width, height];
        }

        public Color this[int x, int y]
        {
            get
            {
                return grid[x, y];
            }
            set
            {
                grid[x, y] = value;
            }
        }

        public int Width => this.grid.GetLength(0);
        public int Height => this.grid.GetLength(1);

        public int RowCount
        {
            get
            {
                return this.Width + this.Height - 1;
            }
        }

        public IEnumerable<RowCoordinates> GetRowCoordinates()
        {
            int startX = 0;
            int startY = 0;
            int endX = 0;
            int endY = 0;

            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                yield return new RowCoordinates(
                    rowIndex,
                    new Point(startX, startY),
                    new Point(endX, endY));

                if (++startY >= this.Height)
                {
                    startY--;
                    startX++;
                }

                if (++endX >= this.Width)
                {
                    endX--;
                    endY++;
                }
            }
        }

        private List<List<Color>> GenerateRows()
        {
            List<List<Color>> result = new List<List<Color>>(this.RowCount);

            foreach (var rowCoordinates in GetRowCoordinates())
            {
                List<Color> row = new List<Color>();

                int x = rowCoordinates.Start.X;
                int y = rowCoordinates.Start.Y;
                for (; x <= rowCoordinates.End.X && y >= rowCoordinates.End.Y; x++, y--)
                {
                    row.Add(this[x, y]);
                }

                if (rowCoordinates.RowIndex % 2 == 0)
                {
                    row.Reverse();
                }

                result.Add(row);
            }

            return result;
        }

        public string GenerateTextPattern(IEnumerable<IPaletteItem> palette)
        {
            StringBuilder sb = new StringBuilder();

            var colorDictionary = palette.ToDictionary(p => p.Color, p => p.Text);

            sb.AppendLine("Pattern starts top left");
            sb.AppendLine("Odd rows (1, 3 etc) are down rows");
            sb.AppendLine();

            int rowNumber = 1;
            foreach (List<Color> row in GenerateRows())
            {
                Color lastColor = Color.Empty;
                int lastColorCount = 0;

                Action<bool> appendColor = (includeComma) =>
                {
                    if (lastColorCount > 0)
                    {
                        string colorString = colorDictionary[lastColor];
                        sb.Append(lastColorCount);
                        sb.Append(colorString);
                        if (includeComma)
                            sb.Append(",");
                    }

                    lastColorCount = 0;
                };

                sb.Append(rowNumber++);
                sb.Append(":");
                sb.Append("\t");

                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] != lastColor)
                    {
                        appendColor(true);
                        lastColor = row[i];
                    }

                    lastColorCount++;
                }

                appendColor(false);
                sb.AppendFormat("({0})", row.Count);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
