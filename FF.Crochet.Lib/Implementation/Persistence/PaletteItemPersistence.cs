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
    internal static class PaletteItemPersistence
    {
        public static IPaletteItem Read(BinaryReader reader)
        {
            bool havePaletteItem = reader.ReadBoolean();

            if (havePaletteItem)
            {
                Color color = Color.FromArgb(reader.ReadInt32());
                bool haveText = reader.ReadBoolean();

                string text = null;

                if (haveText)
                {
                    text = reader.ReadString();
                }

                return new PaletteItem(color: color, text: text);
            }
            else
            {
                return null;
            }
        }

        public static void Write(BinaryWriter writer, IPaletteItem paletteItem)
        {
            if (paletteItem == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.Write(paletteItem.Color.ToArgb());

                if (String.IsNullOrWhiteSpace(paletteItem.Text))
                {
                    writer.Write(false);
                }
                else
                {
                    writer.Write(true);
                    writer.Write(paletteItem.Text);
                }
            }
        }
    }
}
