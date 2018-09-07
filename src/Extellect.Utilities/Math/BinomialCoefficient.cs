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
        public static int Combinations(int n, int r)
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
            else
            {
                var numerator = Enumerable.Range(1 + n - r, r).Aggregate((x, y) => x * y);
                var denominator = Enumerable.Range(1, r).Aggregate((x, y) => x * y);
                return numerator / denominator;
            }
        }
    }
}
