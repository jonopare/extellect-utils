#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math
{
    public static class Solver
    {
        public static double GoalSeek(Func<double, double> func, double target, out double error, double low = double.MinValue, double high = double.MaxValue, int iterations = 100)
        {
            if (low >= high)
            {
                throw new ArgumentException();
            }
            if (iterations < 1)
            {
                throw new ArgumentException("Number of iterations must be positive", nameof(iterations));
            }

            var ld = func(low) - target;
            var hd = func(high) - target;

            if (ld > hd)
            {
                var temp = low;
                low = high;
                high = temp;

                temp = ld;
                ld = hd;
                hd = temp;
            }

            Debug.Assert(ld < 0);
            Debug.Assert(hd > 0);

            error = 0d;
            for (var i = 0; i < iterations; i++)
            {
                var guess = low + (high - low) / 2;
                error = func(guess) - target;

                if (error == 0d)
                {
                    return guess;
                }
                else if (error > 0d)
                {
                    high = guess;
                }
                else if (error < 0d)
                {
                    low = guess;
                }
            }

            return low + (high - low) / 2;
        }
    }
}
