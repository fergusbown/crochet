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
    internal static class ImagePersistence
    {
        public static Image Read(BinaryReader reader)
        {
            int imageLength = reader.ReadInt32();

            if (imageLength == 0)
            {
                return null;
            }
            else
            {
                byte[] imageBytes = new byte[imageLength];
                reader.Read(imageBytes, 0, imageLength);

                //don't dispose memory stream or save will fail
                return Image.FromStream(new MemoryStream(imageBytes));
            }
        }

        public static void Write(BinaryWriter writer, Image image)
        {
            if (image == null)
            {
                writer.Write(0);
            }
            else
            {
                using (MemoryStream imageStream = new MemoryStream())
                {
                    image.Save(imageStream, image.RawFormat);
                    byte[] imageBytes = imageStream.ToArray();
                    writer.Write(imageBytes.Length);
                    writer.Write(imageBytes);
                }
            }
        }
    }
}
