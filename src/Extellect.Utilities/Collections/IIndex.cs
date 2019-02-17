using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Collections
{
    /// <summary>
    /// Abstracts the access to a set of key/value pairs in the way that could be useful as an index.
    /// </summary>
    public interface IIndex<TKey, TValue>
    {
        /// <summary>
        /// Gets the number of key/value pairs
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Returns true iff the index contains the specified key.
        /// </summary>
        bool Contains(TKey key);
    }


}
