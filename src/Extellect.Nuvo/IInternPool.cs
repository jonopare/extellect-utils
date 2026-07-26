namespace Extellect.Nuvo
{
    public interface IInternPool<T> where T : class
    {
        T Intern(T item);
        bool IsInterned(T item);
        int Count { get; }
    }
}
