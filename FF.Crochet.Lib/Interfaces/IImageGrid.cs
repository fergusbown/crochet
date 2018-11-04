using System.Collections.Generic;
using System.Drawing;

namespace FF.Corner2Corner.Lib
{
    public interface IImageGrid
    {
        IPaletteItem this[int x, int y] { get; }

        int Height { get; }
        int Width { get; }

        string GenerateTextPattern();
    }
}