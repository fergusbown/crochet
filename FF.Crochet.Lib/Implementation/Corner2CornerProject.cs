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
    internal class Corner2CornerProject : ICorner2CornerProject
    {
        public Image Image { get; set; }

        public ImageGrid ImageGrid { get; set; }

        public int Width { get; set; } = 100;

        public ICommandFactory<Corner2CornerCommand> Commands { get; internal set; }

        public IPaletteItem SelectedPaletteItem { get; set; }

        IPaletteItem ICorner2CornerProject.SelectedPaletteItem => this.SelectedPaletteItem;

        public Corner2CornerPalette Palette { get; set; } = new Corner2CornerPalette();

        IEnumerable<IPaletteItem> ICorner2CornerProject.Palette => this.Palette;

        IImageGrid ICorner2CornerProject.ImageGrid => this.ImageGrid;

        public Color GridBackgroundColor { get; set; } = Color.Aqua;

        public void Save(Stream stream)
        {

        }
    }
}
