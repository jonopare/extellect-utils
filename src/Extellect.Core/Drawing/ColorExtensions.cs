using System.Drawing;
using System.Numerics;

namespace Extellect.Drawing
{
    public static class ColorExtensions
    {
        private const float Scale = 1f / 255f;

        public static Vector3 ToHslVector(this Color color)
        {
            return Hsl.ToHslVector(color);
        }

        public static Vector3 ToRgbVector(this Color value)
        {
            return new Vector3(value.R * Scale, value.G * Scale, value.B * Scale);
        }
    }
}