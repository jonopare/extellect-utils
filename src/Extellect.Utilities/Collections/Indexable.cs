#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Collections
{
    public class Indexable<TValue> : ICollection<TValue>
    {
        private List<ICollection<TValue>> indexes;

        public Indexable()
        {
            indexes = new List<ICollection<TValue>>();
        }

        public IIndex<TKey, TValue> CreateUniqueIndex<TKey>(Func<TValue, TKey> keySelector) where TKey : IComparable<TKey>
        {
            var index = new UniqueSortedIndex<TKey, TValue>(keySelector);
            indexes.Add(index);
            return index;
        }

        public void Add(TValue item)
        {
            if (indexes.Count == 0)
            {
                throw new InvalidOperationException();
            }
            foreach (var index in indexes)
            {
                index.Add(item);
            }
        }

        public void Clear()
        {
            foreach (var index in indexes)
            {
                index.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TValue item)
        {
            if (indexes.Count == 0)
            {
                throw new InvalidOperationException();
            }
            return indexes.First().Contains(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                if (indexes.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return indexes.First().Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            if (indexes.Count == 0)
            {
                throw new InvalidOperationException();
            }
            bool result = true;
            foreach (var index in indexes)
            {
                result &= Remove(item);
            }
            return result;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (indexes.Count == 0)
            {
                return Enumerable.Empty<TValue>().GetEnumerator();
            }
            else
            {
                return indexes.First().GetEnumerator();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
