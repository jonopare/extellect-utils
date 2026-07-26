namespace Extellect.Nuvo
{
    public sealed class Uptime
    {
        private readonly DateTime _started;
        private readonly IClock _clock;

        public Uptime(IClock clock)
        {
            _clock = clock;
            _started = clock.UtcNow;
        }

        public TimeSpan GetCurrentProcessUptime()
        {
            return _clock.UtcNow - _started;
        }
    }
}
