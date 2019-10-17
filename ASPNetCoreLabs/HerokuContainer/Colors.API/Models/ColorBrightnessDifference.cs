using System;
using System.Drawing;
using Colors.API.Services;

namespace Colors.API.Models
{
    public class ColorBrightnessDifference
    {
        public Color Color1 { get; }
        public Color Color2 { get; }
        public double Diff { get; }
        public bool Acceptable { get; }
        public const double Threshold = 125.0;

        public ColorBrightnessDifference(Color color1, Color color2)
        {
            Color1 = color1;
            Color2 = color2;
            Diff = Math.Round(Math.Abs(ColorServices.GetBrightness(color1) - ColorServices.GetBrightness(color2)), 3);
            Acceptable = Diff >= Threshold;
        }
    }
}
