namespace Extellect.Nuvo
{
    public class MonthBoundaryException : Exception
    {
        public DateOnly Open { get; }
        public DateOnly Close { get; }

        public MonthBoundaryException(DateOnly open, DateOnly close)
            : base($"Cannot get whole number of months between {open.ToInt32()} and {close.ToInt32()}")
        {
            Open = open;
            Close = close;
        }
    }
}
