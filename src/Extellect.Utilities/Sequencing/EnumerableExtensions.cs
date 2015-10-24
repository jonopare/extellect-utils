using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Sequencing
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items, int count)
        {
            for (var i = 0; i < count; i++)
                foreach (var item in items)
                    yield return item;
        }
    }
}
