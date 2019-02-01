using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math
{
    /// <summary>
    /// 
    /// </summary>
    public static class BinomialCoefficient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static long Combinations(int n, int r)
        {
            if (n < 1 || r < 0 || r > n)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (r > n / 2d)
            {
                r = n - r;
            }
            if (r == 0)
            {
                return 1;
            }
            else if (r == 1)
            {
                return n;
            }
            //if (n >= 18) // int
            if (n >= 30) // long, you could probably go further with ulong or double
            {
                throw new ArgumentOutOfRangeException(nameof(n), "Maximum value is 18 before operation results in overflow");
            }
            else
            {
                checked
                {
                    var numerator = Enumerable.Range(1 + n - r, r).Aggregate(1L, (x, y) => x * y);
                    var denominator = Enumerable.Range(1, r).Aggregate(1L, (x, y) => x * y);
                    return numerator / denominator;
                }
            }
        }
    }
}
