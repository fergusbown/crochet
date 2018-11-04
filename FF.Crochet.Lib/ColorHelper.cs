using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Corner2Corner.Lib
{
    public class ColorHelper
    {
        private readonly Bitmap image;
        private readonly IEnumerable<Color> palette;
        private readonly Dictionary<Color, Dictionary<Color, int>> distances; 

        public ColorHelper(Bitmap image, IEnumerable<Color> palette)
        {
            this.image = image;
            this.palette = palette;
            this.distances = new Dictionary<Color, Dictionary<Color, int>>();
        }

        private Color Map(Color original)
        {
            Color map = Color.Empty;
            int distance = int.MaxValue;

            foreach (var contender in palette)
            {
                Dictionary<Color, int> contenderDistances;
                if (!this.distances.TryGetValue(contender, out contenderDistances))
                {
                    contenderDistances = new Dictionary<Color, int>();
                    this.distances[contender] = contenderDistances;
                }

                int contenderDistance;
                if (!contenderDistances.TryGetValue(original, out contenderDistance))
                {
                    contenderDistance = GetDistance(contender, original);
                    contenderDistances[original] = contenderDistance;
                }

                if (contenderDistance < distance)
                {
                    distance = contenderDistance;
                    map = contender;
                }
            }

            return map;
        }

        private static int GetDistance(Color current, Color match)
        {
            int redDifference;
            int greenDifference;
            int blueDifference;
            int alphaDifference;

            alphaDifference = current.A - match.A;
            redDifference = current.R - match.R;
            greenDifference = current.G - match.G;
            blueDifference = current.B - match.B;

            return alphaDifference * alphaDifference + redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference;
        }

        public Color GetColor(Rectangle rectangle)
        {
            Dictionary<Color, int> colors = new Dictionary<Color, int>();
            for (int x = rectangle.X; x < rectangle.Right; x++)
            {
                for (int y = rectangle.Y; y < rectangle.Bottom; y++)
                {
                    var pixel = image.GetPixel(x, y);
                    var mappedColor = this.Map(pixel);
                    int count = 0;
                    colors.TryGetValue(mappedColor, out count);
                    colors[mappedColor] = count + 1;
                }
            }

            return colors.OrderByDescending(kvp => kvp.Value).First().Key;
        }

        public static Color ContrastingColor(Color c)
        {
            var l = c.R * 0.299 + c.G * 0.587 + c.B * 0.114;
            //var l = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;

            return l < 186 ? Color.White : Color.Black;
        }
    }
}
