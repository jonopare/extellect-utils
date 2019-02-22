#pragma warning disable 1591
using System;
using System.Collections.Generic;

namespace Extellect.Collections
{
    public class UniqueSortedIndex<TKey, TValue> : ICollection<TValue>, IIndex<TKey, TValue> where TKey: IComparable<TKey>
    {
        private SortedList<TKey, TValue> _data;

        public Func<TValue, TKey> KeySelector { get; }

        public UniqueSortedIndex(Func<TValue, TKey> keySelector)
        {
            KeySelector = keySelector;
            _data = new SortedList<TKey, TValue>();
        }

        public void Add(TValue item)
        {
            _data.Add(KeySelector(item), item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(TValue item) => _data.ContainsKey(KeySelector(item));

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public bool Remove(TValue item) => _data.Remove(KeySelector(item));

        public IEnumerator<TValue> GetEnumerator()
        {
            return _data.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(TKey key) => _data.ContainsKey(key);

        public TValue this[TKey key] => _data[key];
    }
}
