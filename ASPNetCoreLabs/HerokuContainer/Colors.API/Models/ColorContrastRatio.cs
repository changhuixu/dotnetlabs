using System.Drawing;
using Colors.API.Services;

namespace Colors.API.Models
{
    public class ColorContrastRatio
    {
        public Color Color1 { get; }
        public Color Color2 { get; }
        public double Ratio { get; }

        public ColorContrastRatio(Color color1, Color color2)
        {
            Color1 = color1;
            Color2 = color2;
            Ratio = ColorServices.GetContrastRatio(color1, color2);
        }
    }
}
