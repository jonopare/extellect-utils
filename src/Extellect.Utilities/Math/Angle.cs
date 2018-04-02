#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Math
{
    /// <summary>
    /// Encapsulates an angle as a fraction of a complete circle to avoid calculation problems 
    /// caused by different units.
    /// </summary>
    public struct Angle : IEquatable<Angle>, IComparable<Angle>
    {
        public readonly static Angle TwoPi = FromRadians(System.Math.PI * 2);
        public readonly static Angle Pi = FromRadians(System.Math.PI);
        public readonly static Angle PiOverTwo = FromRadians(System.Math.PI / 2);
        public readonly static Angle PiOverFour = FromRadians(System.Math.PI / 4);
        public readonly static Angle Zero = new Angle();

        private double value;

        private Angle(double radians)
        {
            value = radians;
        }

        public static Angle FromFraction(double fraction)
        {
            return new Angle(TwoPi.Radians * fraction);
        }

        public static Angle FromFraction(double numerator, double denominator)
        {
            return Angle.FromFraction(numerator / denominator);
        }

        public static Angle FromDegrees(double value)
        {
            return Angle.FromFraction(value, 360d);
        }

        public static Angle FromDegrees(double degrees, double minutes, double seconds)
        {
            var sign = Sign(degrees, minutes, seconds);
            return Angle.FromFraction(degrees + sign * System.Math.Abs(minutes) / 60d + sign * System.Math.Abs(seconds) / 3600d, 360d);
        }

        private static int Sign(double a, double b, double c)
        {
            var sign = System.Math.Sign(a);
            if (sign == 0)
            {
                sign = System.Math.Sign(b);
                if (sign == 0)
                {
                    sign = System.Math.Sign(c);
                }
            }
            return sign;
        }

        public static Angle FromRadians(double value)
        {
            return new Angle(value);
        }

        public static Angle FromHours(double value)
        {
            return Angle.FromFraction(value, 24);
        }

        public static Angle FromHours(double hours, double minutes, double seconds)
        {
            var sign = Sign(hours, minutes, seconds);
            return Angle.FromFraction(hours + sign * System.Math.Abs(minutes) / 60d + sign * System.Math.Abs(seconds) / 3600d, 24d);
        }

        public double Degrees
        {
            get
            {
                return Radians * 180d / System.Math.PI;
            }
        }

        public double Radians
        {
            get
            {
                return value;
            }
        }

        public double Hours
        {
            get
            {
                return Radians * 12d / System.Math.PI;
            }
        }

        public double Fraction
        {
            get
            {
                return Radians / 2d / System.Math.PI;
            }
        }

        public Angle Constrain(double maxExclusive)
        {
            if (maxExclusive > 1d || maxExclusive <= -1d)
            {
                throw new ArgumentOutOfRangeException();
            }
            double minInclusive = maxExclusive - 1d; 
            if (Fraction < minInclusive)
            {
                return Angle.FromFraction(1d + (Fraction % 1d));
            }
            else if (Fraction >= maxExclusive)
            {
                return Angle.FromFraction(Fraction % 1d);
            }
            else
            {
                return this;
            }
        }

        public Angle Constrain()
        {
            return Constrain(1);
        }

        public static bool operator <(Angle a, Angle b)
        {
            return a.Radians < b.Radians;
        }

        public static bool operator >(Angle a, Angle b)
        {
            return a.Radians > b.Radians;
        }

        public static bool operator <=(Angle a, Angle b)
        {
            return a.Radians <= b.Radians;
        }

        public static bool operator >=(Angle a, Angle b)
        {
            return a.Radians >= b.Radians;
        }

        public static bool operator !=(Angle a, Angle b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Angle a, Angle b)
        {
            return a.Equals(b);
        }

        public override bool Equals(object other)
        {
            Angle otherAngle = (Angle)other;
            return Equals(otherAngle);
        }

        public bool Equals(Angle other)
        {
            return Radians == other.Radians;
        }

        public override int GetHashCode()
        {
            return Radians.GetHashCode();
        }

        public static Angle operator +(Angle a, Angle b)
        {
            return new Angle(a.Radians + b.Radians);
        }

        public static Angle operator -(Angle a, Angle b)
        {
            return new Angle(a.Radians - b.Radians);
        }

        public static Angle operator -(Angle a)
        {
            return new Angle(0 - a.Radians);
        }

        public static Angle operator *(Angle angle, double scale)
        {
            return new Angle(angle.Radians * scale);
        }

        public static Angle operator *(double scale, Angle angle)
        {
            return Angle.FromRadians(scale * angle.Radians);
        }

        public static Angle operator /(Angle angle, double scale)
        {
            return Angle.FromRadians(angle.Radians / scale);
        }

        public override string ToString()
        {
            return Radians + " radians";
        }

        public int CompareTo(Angle other)
        {
            return Radians.CompareTo(other.Radians);
        }
    }
}
