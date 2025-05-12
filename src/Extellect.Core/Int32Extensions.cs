namespace Extellect
{
    /// <summary>
    /// 
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static DateTime ToDateTime(this int value)
        {
            return new DateTime(value / 10000, (value / 100) % 100, value % 100);
        }

        public static DateOnly ToDate(this int value)
        {
            return new DateOnly(value / 10000, (value / 100) % 100, value % 100);
        }
    }
}
