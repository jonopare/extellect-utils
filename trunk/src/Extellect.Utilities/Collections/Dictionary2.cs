using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Collections
{
    public class Dictionary2<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> inner = new Dictionary<TKey, TValue>();
        private DefaultValueFactory<TValue> defaultFunction;

        public Dictionary2() : this(default(TValue))
        {   
        }

        public Dictionary2(TValue defaultValue)
        {
            defaultFunction = delegate()
            {
                return defaultValue;
            };
        }

        public Dictionary2(DefaultValueFactory<TValue> defaultFunction)
        {
            this.defaultFunction = defaultFunction;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!inner.ContainsKey(key))
                {
                    inner.Add(key, defaultFunction());
                }
                return inner[key];
            }
            set
            {
                inner[key] = value;
            }
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            inner.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return inner.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return inner.Keys; }
        }

        public bool Remove(TKey key)
        {
            return inner.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return inner.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return inner.Values; }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            inner.Add(item);
        }

        public void Clear()
        {
            inner.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return inner.Count; }
        }

        public bool IsReadOnly
        {
            get { return inner.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return inner.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)inner).GetEnumerator();
        }

        #endregion
    }

}
