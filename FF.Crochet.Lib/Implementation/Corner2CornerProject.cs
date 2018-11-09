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
    internal static class Corner2CornerProjectDefaults
    {
        public const int Width = 100;
        public static Color GridBackgroundColor => Color.Aqua;

        public const bool IsDirty = true;
    }

    internal class Corner2CornerProject : ICorner2CornerProject
    {
        private Image image;

        public string FileName { get; set; }

        public IChangeTracking ChangeTracking { get; } = new ChangeTracking();

        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                if (value != this.image)
                {
                    this.image?.Dispose();
                    this.image = value;
                }
            }
        }

        public ImageGrid ImageGrid { get; set; }

        public int Width { get; set; } = Corner2CornerProjectDefaults.Width;

        public ICommandFactory<Corner2CornerCommand> Commands { get; internal set; }

        public IPaletteItem SelectedPaletteItem { get; set; }

        IPaletteItem ICorner2CornerProject.SelectedPaletteItem => this.SelectedPaletteItem;

        public Corner2CornerPalette Palette { get; set; } = new Corner2CornerPalette();

        IEnumerable<IPaletteItem> ICorner2CornerProject.Palette => this.Palette;

        IImageGrid ICorner2CornerProject.ImageGrid => this.ImageGrid;

        public Color GridBackgroundColor { get; set; } = Corner2CornerProjectDefaults.GridBackgroundColor;

        public void Clear()
        {
            this.Image = null;
            this.SelectedPaletteItem = null;
            this.Palette.Clear();
            this.ImageGrid = null;
            this.GridBackgroundColor = Corner2CornerProjectDefaults.GridBackgroundColor;
            this.Width = Corner2CornerProjectDefaults.Width;
            this.FileName = null;
            this.ChangeTracking.Track(ChangeTrackingOperation.SetNew);
        }
    }
}
