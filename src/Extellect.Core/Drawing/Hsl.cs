#pragma warning disable 1591
using System.Drawing;

namespace Extellect.Drawing
{
    public static class Hsl
    {
        public static Tuple<float, float, float> ToHsl(this Color color)
        {
            var r = color.R / 255f;
            var g = color.G / 255f;
            var b = color.B / 255f;
            var max = System.Math.Max(System.Math.Max(r, g), b);
            var min = System.Math.Min(System.Math.Min(r, g), b);
            var l = (max + min) / 2f; // lightness is halfway between min and max 
            float h, s;
            if (max == min)
            {
                // all three channels have the same value so there's
                // no hue (and no saturation)
                h = s = 0;
            }
            else
            {
                var d = max - min;
                s = l > 0.5f ? d / (2 - max - min) : (d / (max + min));
                if (max == r)
                {
                    // hue is difference between blue and green
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    // hue is difference between red and blue but shifted 1/3rd
                    h = (b - r) / d + 2;
                }
                else
                {
                    // hue is difference between green and red but shifted 2/3rds
                    h = (r - g) / d + 4;
                }
                h /= 6;
            }
            return new Tuple<float, float, float>(h, s, l);
        }

        public static Color FromHsl(float h, float s, float l)
        {
            float r, g, b;
            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                var q = l < 0.5f ? l * (1 + s) : (l + s - l * s);
                var p = 2 * l - q;
                r = Hue2Rgb(p, q, h + 1 / 3f);
                g = Hue2Rgb(p, q, h);
                b = Hue2Rgb(p, q, h - 1 / 3f);
            }
            return Color.FromArgb(
                (byte)System.Math.Round(r * 255),
                (byte)System.Math.Round(g * 255),
                (byte)System.Math.Round(b * 255));
        }

        private static float Hue2Rgb(float p, float q, float t)
        {
            if (t < 0)
                t += 1;
            else if (t > 1)
                t -= 1;

            if (t < 1 / 6f)
                return p + (q - p) * 6 * t;
            else if (t < 1 / 2f)
                return q;
            else if (t < 2 / 3f)
                return p + (q - p) * (2 / 3f - t) * 6;
            else
                return p;

        }
    }
}