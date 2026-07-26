namespace Extellect.Nuvo
{
    public interface ICheck
    {
        string? Context { get; }

        IEnumerable<string> Passes { get; }
        IEnumerable<string> Warnings { get; }
        IEnumerable<string> Failures { get; }

        void Pass(string message);
        void Pass(IEnumerable<string> messages);
        void Warn(string message);
        void Warn(IEnumerable<string> messages);
        void Fail(string message);
        void Fail(IEnumerable<string> messages);

        void Merge(ICheck other);
    }
}
