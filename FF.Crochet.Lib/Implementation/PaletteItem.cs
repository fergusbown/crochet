using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public class PaletteItem : IPaletteItem
    {
        public Color Color { get; }
        public string Text { get; set; }

        public PaletteItem(IPaletteItem item = null, Color? color = null, string text = null)
        {
            this.Color = color ?? item?.Color ?? Color.Empty;
            this.Text = text ?? item?.Text;
        }
    }
}
