namespace Extellect
{
    /// <summary>
    /// 
    /// </summary>
    public class TickingClock : IClock
    {
        private DateTime _utc;

        public TickingClock(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
                _utc = value;
            else if (value.Kind == DateTimeKind.Local)
                _utc = value.ToUniversalTime();
            else
                _utc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public void Tick(TimeSpan delta)
        {
            _utc += delta;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Now
        {
            get { return _utc.ToLocalTime(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UtcNow
        {
            get { return _utc; }
        }
    }
}
