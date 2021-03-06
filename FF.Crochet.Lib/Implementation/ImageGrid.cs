﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal class ImageGrid : IImageGrid
    {
        private class RowCoordinates
        {
            public Point Start { get; }

            public Point End { get; }

            public RowCoordinates(Point start, Point end)
            {
                this.Start = start;
                this.End = end;
            }
        }

        private readonly IPaletteItem[,] grid;
        public ImageGrid(int width, int height)
        {
            this.grid = new PaletteItem[width, height];
        }

        public IPaletteItem this[int x, int y]
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

        private int RowCount
        {
            get
            {
                return this.Width + this.Height - 1;
            }
        }

        IPaletteItem IImageGrid.this[int x, int y] => this[x, y];

        private IEnumerable<RowCoordinates> GetRowCoordinates()
        {
            int startX = 0;
            int startY = 0;
            int endX = 0;
            int endY = 0;

            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                yield return new RowCoordinates(
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

        private List<List<IPaletteItem>> GenerateRows(TextPatternStart textPatternStart)
        {
            var result = new List<List<IPaletteItem>>(this.RowCount);
            var rows = GetRowCoordinates();

            if (textPatternStart == TextPatternStart.BottomRight)
            {
                rows = rows.Reverse();
            }

            int rowIndex = 0;
            foreach (var rowCoordinates in rows)
            {
                List<IPaletteItem> row = new List<IPaletteItem>();

                int x = rowCoordinates.Start.X;
                int y = rowCoordinates.Start.Y;
                for (; x <= rowCoordinates.End.X && y >= rowCoordinates.End.Y; x++, y--)
                {
                    row.Add(this[x, y]);
                }

                if (rowIndex++ % 2 == 0)
                {
                    row.Reverse();
                }

                result.Add(row);
            }

            return result;
        }

        public string GenerateTextPattern(TextPatternStart textPatternStart = TextPatternStart.BottomRight)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Pattern starts {(textPatternStart == TextPatternStart.TopLeft ? "top left" : "bottom right")}");
            sb.AppendLine("Odd rows (1, 3 etc) are down rows");
            sb.AppendLine();

            int rowNumber = 1;
            foreach (List<IPaletteItem> row in GenerateRows(textPatternStart))
            {
                IPaletteItem lastColor = null;
                int lastColorCount = 0;

                Action<bool> appendColor = (includeComma) =>
                {
                    if (lastColorCount > 0)
                    {
                        string colorString = lastColor.Text;
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
