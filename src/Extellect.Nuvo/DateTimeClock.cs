namespace Extellect.Nuvo
{
    public class DateTimeClock : IClock
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public static DateTimeClock Instance { get; } = new DateTimeClock();
    }
}
