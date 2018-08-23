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
            return GoalSeek(func, target, out error, (x, y) => x - y, (x, y) => x + y, (x, y) => x - y, x => x / 2, low, high, iterations);
        }

        public static TGuess GoalSeek<TGuess, TTarget>(Func<TGuess, TTarget> func, TTarget target, out TTarget error, Func<TTarget, TTarget, TTarget> subTarget, Func<TGuess, TGuess, TGuess> add, Func<TGuess, TGuess, TGuess> subGuess, Func<TGuess, TGuess> half, TGuess low, TGuess high, int iterations = 100)
            where TGuess : IComparable<TGuess>
            where TTarget : IComparable<TTarget>
        {
            if (low.CompareTo(high) >= 0)
            {
                throw new ArgumentException();
            }
            if (iterations < 1)
            {
                throw new ArgumentException("Number of iterations must be positive", nameof(iterations));
            }

            var ld = subTarget(func(low), target);
            var hd = subTarget(func(high), target);

            if (ld.CompareTo(hd) >= 0)
            {
                var temp = low;
                low = high;
                high = temp;

                var td = ld;
                ld = hd;
                hd = td;
            }

            Debug.Assert(ld.CompareTo(hd) < 0);

            error = default(TTarget);
            for (var i = 0; i < iterations; i++)
            {
                var guess = add(low, half(subGuess(high, low)));

                error = subTarget(func(guess), target);

                var e = error.CompareTo(default(TTarget));
                if (e == 0)
                {
                    return guess;
                }
                else if (e > 0)
                {
                    high = guess;
                }
                else if (e < 0)
                {
                    low = guess;
                }
            }

            return add(low, half(subGuess(high, low)));
        }
    }
}
