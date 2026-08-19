using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Extellect.Nuvo
{
    public struct Color : IEquatable<Color>
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Color FromRgb(byte r, byte g, byte b) => new Color(r, g, b);

        public static Color FromRgb(int r, int g, int b)
        {
            checked
            {
                return new Color((byte)r, (byte)g, (byte)b);
            }
        }

        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }

        public override string ToString()
        {
            return ToRgb();
        }

        public string ToRgb() => $"rgb({R},{G},{B})";

        public string ToHex() => $"#{R:x2}{G:x2}{B:x2}";

        public Color Interpolate(Color other, double f)
        {
            return new Color(
                Lerp(R, other.R, f),
                Lerp(G, other.G, f),
                Lerp(B, other.B, f)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte Lerp(byte a, byte b, double f)
        {
            return (byte)Math.Clamp(a + (b - a) * f, 0, 255);
        }
    }
}
