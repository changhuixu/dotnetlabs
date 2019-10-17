using System;
using System.Drawing;

namespace Colors.API.Models
{
    public class ColorDifference
    {
        public Color Color1 { get; }
        public Color Color2 { get; }
        public int Diff { get; }
        public bool Acceptable { get; }
        public const int Threshold = 500;

        public ColorDifference(Color color1, Color color2)
        {
            Color1 = color1;
            Color2 = color2;
            Diff = (Math.Max(color1.R, color2.R) - Math.Min(color1.R, color2.R)) +
                   (Math.Max(color1.G, color2.G) - Math.Min(color1.G, color2.G)) +
                   (Math.Max(color1.B, color2.B) - Math.Min(color1.B, color2.B));
            Acceptable = Diff >= Threshold;
        }
    }
}
