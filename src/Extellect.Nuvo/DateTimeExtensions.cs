namespace Extellect.Nuvo
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Truncates the TimeOfDay and returns an Int32 representation of the specified value's Date.
        /// </summary>
        public static int ToInt32(this DateTime value)
        {
            return value.Year * 10000 + value.Month * 100 + value.Day;
        }

        public static decimal ToDecimal(this DateTime value)
        {
            return value.ToInt32() 
                + value.TimeOfDay.Hours / 100m
                + value.TimeOfDay.Minutes / 10000m
                + value.TimeOfDay.Seconds / 1000000m;
        }

        public static DateTime ToUtc(this DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    return new DateTime(value.Ticks, DateTimeKind.Utc);
                case DateTimeKind.Local:
                    return value.ToUniversalTime();
                case DateTimeKind.Utc:
                    return value;
                default:
                    throw new NotSupportedException($"Unsupported DateTimeKind: {value.Kind}");
            }
        }
    }
}
