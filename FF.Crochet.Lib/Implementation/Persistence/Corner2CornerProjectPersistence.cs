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
    internal class Corner2CornerProjectPersistence
    {
        public static void Save(string fileName, Corner2CornerProject project)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                Save(stream, project);
                project.FileName = fileName;
            }
        }

        public static void Save(Stream stream, Corner2CornerProject project)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                HeaderPersistence.Write(writer);
                ImagePersistence.Write(writer, project.Image);
                writer.Write(project.Width);

                List<IPaletteItem> palette = project.Palette.ToList();
                writer.Write(palette.Count);

                foreach (var paletteItem in palette)
                {
                    PaletteItemPersistence.Write(writer, paletteItem);
                }

                PaletteItemPersistence.Write(writer, project.SelectedPaletteItem);
                ImageGridPersistence.Write(writer, project.ImageGrid);
                writer.Write(project.GridBackgroundColor.ToArgb());
            }

            project.ChangeTracking.Track(ChangeTrackingOperation.SetSaved);
        }

        public static bool Load(string fileName, Corner2CornerProject project)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Open))
            {
                if (Load(stream, project))
                {
                    project.FileName = fileName;
                    return true;
                }
            }

            return false;
        }

        public static bool Load(Stream stream, Corner2CornerProject project)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                if (!HeaderPersistence.Read(reader))
                {
                    stream.Position = 0;
                    return false;
                }

                Image image = ImagePersistence.Read(reader);
                int width = reader.ReadInt32();

                int paletteItemCount = reader.ReadInt32();
                Corner2CornerPalette palette = new Corner2CornerPalette();

                for (int i = 0; i < paletteItemCount; i++)
                {
                    var pi = PaletteItemPersistence.Read(reader);
                    palette.Add(color:pi.Color, text:pi.Text);
                }

                IPaletteItem selectedPaletteItem = PaletteItemPersistence.Read(reader);
                palette.Find(selectedPaletteItem?.Color ?? Color.Empty, out selectedPaletteItem);

                ImageGrid imageGrid = ImageGridPersistence.Read(reader, palette);

                Color gridBackgroundColor = Color.FromArgb(reader.ReadInt32());

                //read everything successfully so populate the project;

                project.Image = image;
                project.Width = width;
                project.Palette = palette;
                project.SelectedPaletteItem = selectedPaletteItem;
                project.ImageGrid = imageGrid;
                project.GridBackgroundColor = gridBackgroundColor;
                project.ChangeTracking.Track(ChangeTrackingOperation.SetSaved);

                return true;
            }
        }
    }
}
