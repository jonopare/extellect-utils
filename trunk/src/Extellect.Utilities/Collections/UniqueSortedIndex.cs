#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Collections
{
    public class UniqueSortedIndex<TKey, TValue> : ICollection<TValue>, IIndex<TKey, TValue> where TKey: IComparable<TKey>
    {
        private SortedList<TKey, TValue> data;

        public Func<TValue, TKey> KeySelector { get; private set; }

        public UniqueSortedIndex(Func<TValue, TKey> keySelector)
        {
            KeySelector = keySelector;
            this.data = new SortedList<TKey, TValue>();
        }

        public void Add(TValue item)
        {
            data.Add(KeySelector(item), item);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(TValue item)
        {
            return data.ContainsKey(KeySelector(item));
        }

        

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            return data.Remove(KeySelector(item));
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return data.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool IIndex<TKey, TValue>.Contains(TKey key)
        {
            return data.ContainsKey(key);
        }

        int IIndex<TKey, TValue>.Count
        {
            get
            {
                return data.Count;
            }
        }

        TValue IIndex<TKey, TValue>.this[TKey key]
        {
            get
            {
                return data[key];
            }
        }
    }
}
