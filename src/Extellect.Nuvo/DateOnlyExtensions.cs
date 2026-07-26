namespace Extellect.Nuvo
{
    public static class DateOnlyExtensions
    {
        /// <summary>
        /// Truncates the TimeOfDay and returns an Int32 representation of the specified value's Date.
        /// </summary>
        public static int ToInt32(this DateOnly value)
        {
            return value.Year * 10000 + value.Month * 100 + value.Day;
        }
    }
}
