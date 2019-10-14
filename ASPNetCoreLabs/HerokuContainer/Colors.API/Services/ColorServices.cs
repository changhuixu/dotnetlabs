using System;
using System.Drawing;

namespace Colors.API.Services
{
    public class ColorServices
    {
        public static double GetLuminance(Color color)
        {
            static double GetsRgb(byte c)
            {
                var s = c / 255.0;
                if (s <= 0.03928)
                {
                    return s / 12.92;
                }
                return Math.Pow((s + 0.055) / 1.055, 2.4);
            }

            return 0.2126 * GetsRgb(color.R) + 0.7152 * GetsRgb(color.G) + 0.0722 * GetsRgb(color.B);
        }

        // https://www.w3.org/TR/WCAG20/#relativeluminancedef
        public static double GetContrastRatio(Color foreground, Color background)
        {
            double ratio;
            var l1 = GetLuminance(foreground);
            var l2 = GetLuminance(background);

            if (l1 >= l2)
            {
                ratio = (l1 + 0.05) / (l2 + 0.05);
            }
            else
            {
                ratio = (l2 + 0.05) / (l1 + 0.05);
            }
            return Math.Floor(ratio * 1000) / 1000; // round to 3 decimal places
        }
    }
}
