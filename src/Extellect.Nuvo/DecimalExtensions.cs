namespace Extellect.Nuvo
{
    public static class DecimalExtensions
    {
        public const decimal PositiveZero = 0.00001m;// 0.0000001m;
        public const decimal NegativeZero = -PositiveZero;

        /// <summary>
        /// Gets a DateTime from a human-readable Decimal representation of a date and time.
        /// </summary>
        public static DateTime ToDateTime(this decimal value)
        {
            var datePart = (int)value;
            var timePart = (int)((value - datePart) * 1_000_000);
            return datePart.ToDate().ToDateTime(timePart.ToTime());
        }

        public static bool IsWithinToleranceOf(this decimal value, decimal other, decimal tolerance)
        {
            return Math.Abs(value - other) <= tolerance;
        }

        public static decimal SnapToZero(this decimal value, decimal tolerance = PositiveZero)
        {
            return value.SnapTo(0m, tolerance);
        }

        public static decimal SnapTo(this decimal value, decimal target, decimal tolerance = PositiveZero)
        {
            if (value.IsWithinToleranceOf(target, tolerance))
            {
                return target;
            }
            return value;
        }

        public static bool IsPositive(this decimal value) => value > PositiveZero;

        public static bool IsNegative(this decimal value) => value < NegativeZero;

        public static bool IsZero(this decimal value) => !value.IsPositive() && !value.IsNegative();

        public static bool IsExcelZero(this decimal value) => (double)value == 0;

    }
}
