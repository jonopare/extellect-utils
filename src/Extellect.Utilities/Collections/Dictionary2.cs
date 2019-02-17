using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Collections
{
    /// <summary>
    /// Encapsulates a dictionary object where a default value will be added and
    /// returned when a request is made for a key that doesn't exist. The default
    /// value can be a constant value, or a function of the key.
    /// </summary>
    public class Dictionary2<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> inner = new Dictionary<TKey, TValue>();
        private Func<TKey, TValue> defaultFunction;

        /// <summary>
        /// Constructs a new dictionary with a "default" default function.
        /// </summary>
        public Dictionary2() : this(default(TValue))
        {   
        }

        /// <summary>
        /// Constructs a new dictionary with a specific default value that is always used.
        /// </summary>
        public Dictionary2(TValue defaultValue)
        {
            defaultFunction = key => defaultValue;
        }

        /// <summary>
        /// Constructs a new dicitionary with a specific default function.
        /// </summary>
        public Dictionary2(Func<TKey, TValue> defaultFunction)
        {
            this.defaultFunction = defaultFunction;
        }

        /// <summary>
        /// Gets a value from the dictionary given a specific key.
        /// This is where the magic happens.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                if (!inner.ContainsKey(key))
                {
                    inner.Add(key, defaultFunction(key));
                }
                return inner[key];
            }
            set
            {
                inner[key] = value;
            }
        }

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds an element with the provided key and value to the collection. 
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            inner.Add(key, value);
        }

        /// <summary>
        /// Determines whether the collection contains an element with the specified key.
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return inner.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection containing all the keys of this dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return inner.Keys; }
        }

        /// <summary>
        /// Removes the element with the specified key from the dictionary.
        /// </summary>
        public bool Remove(TKey key)
        {
            return inner.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key. Always returns true.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = this[key];
            return true;
        }

        /// <summary>
        /// Gets a collection containing the values in this dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return inner.Values; }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            inner.Add(item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            inner.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains a specific item.
        /// </summary>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return inner.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the collection to a System.Array, starting at a particular arrayIndex.
        /// </summary>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get { return inner.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return inner.IsReadOnly; }
        }

        /// <summary>
        /// Removes the first occurence of the specified object from the collection.
        /// </summary>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return inner.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Returns an enumerator that iterates over the collection.
        /// </summary>
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
