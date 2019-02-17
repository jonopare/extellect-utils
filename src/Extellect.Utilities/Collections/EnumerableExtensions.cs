using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Collections
{
    /// <summary>
    /// Extensions to Enumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// SequenceEqual that goes one level deeper - tests equality of a sequence of sequences.
        /// </summary>
        public static bool DeepSequenceEqual<T>(this IEnumerable<IEnumerable<T>> left, IEnumerable<IEnumerable<T>> right)
        {
            IEnumerator<IEnumerable<T>> l = left.GetEnumerator();
            IEnumerator<IEnumerable<T>> r = right.GetEnumerator();

            bool x, y;
            while ((x = l.MoveNext()) & (y = r.MoveNext()))
            {
                if (!l.Current.SequenceEqual(r.Current))
                {
                    return false;
                }
            }

            return !x & !y;
        }

        /// <summary>
        /// SequenceEqual that goes one level deeper - tests equality of a sequence of sequences.
        /// </summary>
        public static bool DeepSequenceEqual<T>(this IEnumerable<IEnumerable<T>> left, IEnumerable<IEnumerable<T>> right, IEqualityComparer<T> comparer)
        {
            IEnumerator<IEnumerable<T>> l = left.GetEnumerator();
            IEnumerator<IEnumerable<T>> r = right.GetEnumerator();

            bool x, y;
            while ((x = l.MoveNext()) & (y = r.MoveNext()))
            {
                if (!l.Current.SequenceEqual(r.Current, comparer))
                {
                    return false;
                }
            }

            return !x & !y;
        }

        /// <summary>
        /// Splits a collection of records arranged into chunks of contiguous keys into a set of groupings. Uses the default equality comparer for the key's type.
        /// </summary>
        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.ChunkBy(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Splits a collection of records arranged into chunks of contiguous keys into a set of groupings. See the Chunk class.
        /// </summary>
        public static IEnumerable<IGrouping<TKey, TSource>> ChunkBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            // Flag to signal end of source sequence.
            const bool noMoreSourceElements = true;

            // Auto-generated iterator for the source array.       
            var enumerator = source.GetEnumerator();

            // Move to the first element in the source sequence.
            if (!enumerator.MoveNext()) yield break;

            // Iterate through source sequence and create a copy of each Chunk.
            // On each pass, the iterator advances to the first element of the next "Chunk"
            // in the source sequence. This loop corresponds to the outer foreach loop that
            // executes the query.
            Chunk<TKey, TSource> current = null;
            while (true)
            {
                // Get the key for the current Chunk. The source iterator will churn through
                // the source sequence until it finds an element with a key that doesn't match.
                var key = keySelector(enumerator.Current);

                // Make a new Chunk (group) object that initially has one GroupItem, which is a copy of the current source element.
                current = new Chunk<TKey, TSource>(key, enumerator, value => comparer.Equals(key, keySelector(value)));

                // Return the Chunk. A Chunk is an IGrouping<TKey,TSource>, which is the return value of the ChunkBy method.
                // At this point the Chunk only has the first element in its source sequence. The remaining elements will be
                // returned only when the client code foreach's over this chunk. See Chunk.GetEnumerator for more info.
                yield return current;

                // Check to see whether (a) the chunk has made a copy of all its source elements or 
                // (b) the iterator has reached the end of the source sequence. If the caller uses an inner
                // foreach loop to iterate the chunk items, and that loop ran to completion,
                // then the Chunk.GetEnumerator method will already have made
                // copies of all chunk items before we get here. If the Chunk.GetEnumerator loop did not
                // enumerate all elements in the chunk, we need to do it here to avoid corrupting the iterator
                // for clients that may be calling us on a separate thread.
                if (current.CopyAllChunkElements() == noMoreSourceElements)
                {
                    yield break;
                }
            }
        }
    }
}
