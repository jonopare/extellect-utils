using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue, Func<TValue, TValue> updateValue)
        {
            if (!dictionary.TryGetValue(key, out TValue foundValue))
            {
                dictionary.Add(key, addValue);
            }
            else
            {
                dictionary[key] = addValue = updateValue(foundValue);
            }
            return addValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValue)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue();
        }

        /// <summary>
        /// 
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : default(TValue);
        }
    }
}
