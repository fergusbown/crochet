﻿using FF.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public interface ICorner2CornerProject
    {
        Image Image { get; }

        int Width { get; }

        IEnumerable<IPaletteItem> Palette { get; }

        IPaletteItem SelectedPaletteItem { get; }

        IImageGrid ImageGrid { get; }

        Color GridBackgroundColor { get; }

        ICommandFactory<Corner2CornerCommand> Commands { get; }

        string FileName { get; }

        IChangeTracking ChangeTracking { get; }
    }
}
