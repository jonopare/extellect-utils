namespace Extellect.Nuvo
{
    public sealed class HashSetInternPool<T> : IInternPool<T>, IDisposable where T : class
    {
        private readonly HashSet<T> _items;

        public int Count => _items.Count;

        public HashSetInternPool(int capacity = 0, IEqualityComparer<T>? equalityComparer = null)
        {
            _items = new HashSet<T>(capacity, equalityComparer ?? EqualityComparer<T>.Default);
        }

        public T Intern(T item)
        {
            if (_items.TryGetValue(item, out var found))
            {
                return found;
            }
            else
            {
                _ = _items.Add(item);
                return item;
            }
        }

        public bool IsInterned(T item)
        {
            return _items.TryGetValue(item, out var found)
                && ReferenceEquals(item, found);
        }

        public void Dispose()
        {
            _items.Clear();
        }
    }
}
