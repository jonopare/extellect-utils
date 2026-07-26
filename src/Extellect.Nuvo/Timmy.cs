using System.Diagnostics;

namespace Extellect.Nuvo
{
    public sealed class Timmy : IDisposable
    {
        private readonly string _operation;
        private readonly Action<string> _onDispose;
        private readonly Stopwatch _stopwatch;

        public Timmy(string operation, Action<string> onDispose)
        {
            _operation = operation;
            _onDispose = onDispose;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                _onDispose.Invoke($"{_operation} took {_stopwatch.Elapsed}");
            }
        }

        public static Timmy Start(string operation, Action<string> onDispose)
        {
            return new Timmy(operation, onDispose);
        }
    }
}
