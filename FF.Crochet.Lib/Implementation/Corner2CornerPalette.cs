using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    internal class Corner2CornerPalette : IEnumerable<IPaletteItem>
    {
        private Dictionary<Color, PaletteItem> palette = new Dictionary<Color, PaletteItem>();

        public Corner2CornerPalette()
        {
        }

        public IEnumerator<IPaletteItem> GetEnumerator()
        {
            return this.palette.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryUpdate(Color color, string newText, out IPaletteItem original)
        {
            PaletteItem current;
            if (this.palette.TryGetValue(color, out current))
            {
                original = new PaletteItem(current);
                current.Text = newText;
                return true;
            }

            original = null;
            return false;
        }

        public bool Add(Color color)
        {
            if (!this.palette.ContainsKey(color))
            {
                this.palette.Add(color, new PaletteItem(color:color));
                return true;
            }

            return false;
        }

        public bool Remove(Color color)
        {
            return this.palette.Remove(color);
        }

        public bool Find(Color color, out IPaletteItem paletteItem)
        {
            if (this.palette.TryGetValue(color, out PaletteItem item))
            {
                paletteItem = item;
                return true;
            }

            paletteItem = null;
            return false;
        }
    }
}
