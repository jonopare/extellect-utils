namespace Extellect.Nuvo
{
    public class Check : ICheck
    {
        private readonly HashSet<string> _passes;
        private readonly HashSet<string> _warnings;
        private readonly HashSet<string> _failures;
        public string? Context { get; }
        public IEnumerable<string> Passes => _passes;
        public IEnumerable<string> Warnings => _warnings;
        public IEnumerable<string> Failures => _failures;

        public Check()
        {
            _passes = new HashSet<string>();
            _warnings = new HashSet<string>();
            _failures = new HashSet<string>();
        }

        public void Pass(string message)
        {
            _passes.Add(message);
        }

        public void Pass(IEnumerable<string> messages)
        {
            _passes.AddRange(messages);
        }

        public void Warn(string message)
        {
            _warnings.Add(message);
        }

        public void Warn(IEnumerable<string> messages)
        {
            _warnings.AddRange(messages);
        }

        public void Fail(string message)
        {
            _failures.Add(message);
        }

        public void Fail(IEnumerable<string> messages)
        {
            _failures.AddRange(messages);
        }

        public void Merge(ICheck other)
        {
            _passes.AddRange(other.Passes);
            _warnings.AddRange(other.Warnings);
            _failures.AddRange(other.Failures);
        }
    }
}
