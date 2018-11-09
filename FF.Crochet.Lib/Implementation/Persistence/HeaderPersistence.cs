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
    internal static class HeaderPersistence
    {
        private static string headerText = "Corner2CornerProject";
        private static byte[] header = Encoding.UTF8.GetBytes(headerText);

        public static bool Read(BinaryReader reader)
        {
            byte[] buffer = new byte[header.Length];
            if (reader.Read(buffer, 0, buffer.Length) == buffer.Length)
            {
                if (buffer.SequenceEqual(header))
                {
                    return true;
                }
            }

            return false;
        }

        public static void Write(BinaryWriter writer)
        {
            writer.Write(header);
        }
    }
}
