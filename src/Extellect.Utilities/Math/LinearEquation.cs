using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Math
{
    /// <summary>
    /// Linear equation helper class
    /// </summary>
    public static class LinearEquation
    {
        /// <summary>
        /// Tries to solve a linear equation with matrices.
        /// abcd * xy = ef
        /// => inverse(abcd) * ef = xy
        /// </summary>
        public static bool TrySolve(Matrix abcd, Matrix ef, out Matrix result)
        {
            result = null;

            // abcd must have the same # of rows as ef
            if (abcd.M != ef.M)
            {
                return false;
            }

            if (abcd.TryInvert(out Matrix inverse))
            {
                return inverse.TryMultiply(ef, out result);
            }
            else if (ef.N == 1 && abcd.TryAugment(ef, out Matrix augmented))
            {
                augmented.Echelon();

                var zeroes = Enumerable.Range(0, augmented.M)
                    .Where(x => augmented[x, x] == 0d);

                if (zeroes.Any())
                {
                    // we don't have a unique solution because two or more of the rows were the same line

                    // TODO: this next bit is not correct for all sitations

                    var firstNonZero = Enumerable.Range(0, augmented.M)
                        .Where(x => augmented[x, x] != 0d)
                        .First();

                    if (augmented[firstNonZero, augmented.N - 1] != 0)
                    {
                        throw new InvalidOperationException();
                    }

                    result = augmented.Submatrix(firstNonZero, 0, 1, augmented.N - 1);

                    if (result.N != 2)
                    {
                        throw new InvalidOperationException();
                    }

                    result = new Matrix(new[] { result[0, 1], 0 - result[0, 0]}, 2, 1, true);

                    return true;
                }
                else
                {
                    augmented.ReducedRowEchelon();

                    result = augmented.Submatrix(0, augmented.N - 1, augmented.M, 1);

                    return true;
                }
            }
            else
            {   
                return false;
            }
        }
    }
}
