using System.Diagnostics.CodeAnalysis;

namespace Extellect.Nuvo
{
    public static class EnumerableExtensions
    {
        public static bool TryGetFirst<T>(this IEnumerable<T> items, [MaybeNullWhen(false)] out T first)
        {
            using (var enumerator = items.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    first = enumerator.Current;
                    return true;
                }
            }
            first = default;
            return false;
        }

        public static bool TryGetFirst<T>(this IEnumerable<T> items, Func<T, bool> predicate, [MaybeNullWhen(false)] out T first)
        {
            return items.Where(predicate).TryGetFirst(out first);
        }

        public static bool TryGetSingle<T>(this IEnumerable<T> items, [MaybeNullWhen(false)] out T single)
        {
            using (var enumerator = items.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    single = enumerator.Current;
                    return !enumerator.MoveNext();
                }
                else
                {
                    single = default;
                    return false;
                }
            }
        }

        public static bool TryGetSingle<T>(this IEnumerable<T> items, Func<T, bool> predicate, [MaybeNullWhen(false)] out T single)
        {
            return items.Where(predicate).TryGetSingle(out single);
        }

        public static IEnumerable<T[]> Batch<T>(this IEnumerable<T> items, int batchSize)
        {
            if (batchSize <= 0)
            {
                yield return items.ToArray();
            }
            using (var e = items.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    yield return BatchOf(e, batchSize);
                }
            }
        }

        private static T[] BatchOf<T>(IEnumerator<T> e, int batchSize)
        {
            var result = new T[batchSize];
            for (var i = 0; i < batchSize; i++)
            {
                if (i > 0 && !e.MoveNext())
                {
                    var temp = result;
                    result = new T[i];
                    Array.Copy(temp, result, i);
                    break;
                }
                result[i] = e.Current;
            }
            return result;
        }

        public static bool SetEquals<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            if (left == null || !left.Any())
            {
                return right == null || !right.Any();
            }
            else if (right == null || !right.Any())
            {
                return false;
            }
            else
            {
                return left.ToHashSet()
                    .SetEquals(right);
            }
        }

        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _ = hashSet.Add(item);
            }
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
            Chunk<TKey, TSource>? current = null;
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

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> items)
        {
            foreach (var item in items)
            {
                yield return item ?? throw new NullReferenceException();
            }
        }
    }
}
