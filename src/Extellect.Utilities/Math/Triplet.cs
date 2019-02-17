#pragma warning disable 1591
using System;
using System.Diagnostics;

namespace Extellect.Math
{
    [DebuggerDisplay("X={X} Y={Y} Z={Z} Len={Length}")]
    public struct Triplet
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Triplet(double x, double y, double z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length => System.Math.Sqrt(LengthSquared);

        public double LengthSquared => System.Math.Pow(X, 2) + System.Math.Pow(Y, 2) + System.Math.Pow(Z, 2);

        public static Triplet operator +(Triplet a, Triplet b)
        {
            return new Triplet(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Triplet operator -(Triplet a, Triplet b)
        {
            return new Triplet(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Triplet operator *(Triplet value, double scalar)
        {
            return new Triplet(value.X * scalar, value.Y * scalar, value.Z * scalar);
        }

        public static Triplet operator *(double scalar, Triplet value)
        {
            return new Triplet(value.X * scalar, value.Y * scalar, value.Z * scalar);
        }

        public static Triplet operator /(Triplet value, double scalar)
        {
            return new Triplet(value.X / scalar, value.Y / scalar, value.Z / scalar);
        }

        /// <summary>
        /// Undefined when Magnitude is zero.
        /// </summary>
        public Triplet Unit => this / Length;

        public double Dot(Triplet other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public Triplet Cross(Triplet other)
        {
            return new Triplet(
                Y * other.Z - other.Y * Z,
                Z * other.X - other.Z * X,
                X * other.Y - other.X * Y);
        }

        public Angle AngularDistance(Triplet other)
        {
            return Angle.FromRadians(System.Math.Acos(Dot(other) / Length / other.Length));
        }

        public Triplet RotateZ(Angle clockwise)
        {
            var s = System.Math.Sin(clockwise.Radians);
            var c = System.Math.Cos(clockwise.Radians);
            return new Triplet(X * c + Y * s, Y * c - X * s, Z);
        }

        public override string ToString()
        {
            return $"X={X} Y={Y} Z={Z} Len={Length}";
        }
    }
}
