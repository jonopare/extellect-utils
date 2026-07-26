namespace Extellect.Nuvo
{
    public static class DoubleExtensions
    {
        public const double PositiveZero = 0.00001;//0.00001;
        public const double NegativeZero = -PositiveZero;

        public static bool IsWithinToleranceOf(this double value, double other, double tolerance)
        {
            return Math.Abs(value - other) <= tolerance;
        }

        public static double SnapToZero(this double value, double tolerance = PositiveZero)
        {
            return value.SnapTo(0d, tolerance);
        }

        public static double SnapTo(this double value, double target, double tolerance = PositiveZero)
        {
            if (value.IsWithinToleranceOf(target, tolerance))
            {
                return target;
            }
            return value;
        }

        public static bool IsPositive(this double value) => value > PositiveZero;

        public static bool IsNegative(this double value) => value < NegativeZero;

        public static bool IsZero(this double value) => !value.IsPositive() && !value.IsNegative();
    }
}
