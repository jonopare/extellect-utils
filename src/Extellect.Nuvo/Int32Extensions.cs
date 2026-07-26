namespace Extellect.Nuvo
{
    public static class Int32Extensions
    {
        /// <summary>
        /// Gets a DateOnly from a human-readable Int32 representation of a date.
        /// </summary>
        public static DateOnly ToDate(this int value)
        {
            return new DateOnly(value / 10000, (value / 100) % 100, value % 100);
        }

        /// <summary>
        /// Gets a TimeOnly from a human-readable Int32 representation of a time.
        /// </summary>
        public static TimeOnly ToTime(this int value)
        {
            return new TimeOnly(value / 10000, value / 100 % 100, value % 100);
        }
    }
}
