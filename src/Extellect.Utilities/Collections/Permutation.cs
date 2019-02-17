using System;
using System.Collections.Generic;
using System.Linq;

namespace Extellect.Collections
{
    /// <summary>
    /// Systematic generation of all permutations.
    /// Algorithm as described at http://en.wikipedia.org/wiki/Permutation
    /// </summary>
    public static class Permutation
    {
        /// <summary>
        /// Generates all permutations of the given set of values. 
        /// The algorithm is based on finding the next permutation in lexicographic ordering, which 
        /// requires T to be comparable with itself.
        /// </summary>
        public static IEnumerable<string> Permute(this string set)
        {
            foreach (var p in ((IEnumerable<char>)set).Permute())
            {
                yield return new string(p.ToArray());
            }
        }

        /// <summary>
        /// Generates all permutations of the given set of values. 
        /// The algorithm is based on finding the next permutation in lexicographic ordering, which 
        /// requires T to be comparable with itself.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> set) where T : IComparable<T>
        {
            T[] result = set.OrderBy(item => item).ToArray();

            while (true)
            {
                yield return result.ToList();

                int k = -1, l = -1;
                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (result[i - 1].CompareTo(result[i]) < 0)
                    {
                        k = i - 1;
                        break;
                    }
                }

                if (k == -1)
                {
                    yield break;
                }

                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (result[k].CompareTo(result[i]) < 0)
                    {
                        l = i;
                        break;
                    }
                }

                if (l == -1)
                {
                    throw new InvalidOperationException();
                }

                T temp = result[k];
                result[k] = result[l];
                result[l] = temp;

                result = result.Take(k + 1).Concat(result.Skip(k + 1).Reverse()).ToArray();
            }
        }

        /// <summary>
        /// Generates all permutations of the given set of values. 
        /// The algorithm is based on finding the next permutation in lexicographic ordering.
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> set, IComparer<T> comparer)
        {
            T[] result = set.OrderBy(item => item, comparer).ToArray();

            while (true)
            {
                yield return result.ToList();

                int k = -1, l = -1;
                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (comparer.Compare(result[i - 1], result[i]) < 0)
                    {
                        k = i - 1;
                        break;
                    }
                }

                if (k == -1)
                {
                    yield break;
                }

                for (int i = result.Length - 1; i > 0; i--)
                {
                    if (comparer.Compare(result[k], result[i]) < 0)
                    {
                        l = i;
                        break;
                    }
                }

                if (l == -1)
                {
                    throw new InvalidOperationException();
                }

                T temp = result[k];
                result[k] = result[l];
                result[l] = temp;

                result = result.Take(k + 1).Concat(result.Skip(k + 1).Reverse()).ToArray();
            }
        }
    }
}
