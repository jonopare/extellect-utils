namespace Extellect
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeClock : IClock
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}
