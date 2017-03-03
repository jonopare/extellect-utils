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
            TValue foundValue;
            if (!dictionary.TryGetValue(key, out foundValue))
            {
                dictionary.Add(key, addValue);
            }
            else
            {
                dictionary[key] = addValue = updateValue(foundValue);
            }
            return addValue;
        }
    }
}
