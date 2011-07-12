using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Collections
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
    }
}
