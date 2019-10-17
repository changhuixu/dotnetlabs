using System;
using System.Drawing;

namespace Colors.API.Services
{
    public class ColorServices
    {
        // https://www.w3.org/TR/AERT/#color-contrast
        /*
         * Two colors provide good color visibility if the brightness difference and the color difference between the two colors are greater than a set range.

            Color brightness is determined by the following formula:
                ((Red value X 299) + (Green value X 587) + (Blue value X 114)) / 1000
            Note: This algorithm is taken from a formula for converting RGB values to YIQ values. This brightness value gives a perceived brightness for a color.

            Color difference is determined by the following formula:
            (maximum (Red value 1, Red value 2) - minimum (Red value 1, Red value 2)) + (maximum (Green value 1, Green value 2) - minimum (Green value 1, Green value 2)) + (maximum (Blue value 1, Blue value 2) - minimum (Blue value 1, Blue value 2))

            The rage for color brightness difference is 125. The range for color difference is 500.
         */
        public static double GetBrightness(Color color)
        {
            return (color.R * 299 + color.G * 587 + color.B * 114) / 1000.0;
        }

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
