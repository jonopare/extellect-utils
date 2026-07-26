namespace Extellect.Nuvo
{
    [Obsolete("Use HashSetInternPool instead")]
    public sealed class ListInternPool<T> : IInternPool<T>, IDisposable where T : class
    {
        private readonly List<T> _items;
        private readonly IComparer<T> _comparer;

        public int Count => _items.Count;

        public ListInternPool(int capacity = 0, IComparer<T>? comparer = null)
        {
            _items = new List<T>(capacity);
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public T Intern(T item)
        {
            var index = _items.BinarySearch(item, _comparer);
            if (index < 0)
            {
                _items.Insert(~index, item);
                return item;
            }
            else
            {
                return _items[index];
            }
        }

        public bool IsInterned(T item)
        {
            var index = _items.BinarySearch(item, _comparer);
            return index >= 0 && ReferenceEquals(item, _items[index]);
        }

        public void Dispose()
        {
            _items.Clear();
        }
    }
}
