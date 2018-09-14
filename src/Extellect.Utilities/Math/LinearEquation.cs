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
                // only square invertible (e.g. non-zero determinant) matrices will reach here
                return inverse.TryMultiply(ef, out result);
            }
            else if (ef.N == 1 && abcd.M == abcd.N && abcd.TryAugment(ef, out Matrix augmented))
            {
                // presumably this branch will not execute until a singular matrix is passed as abcd
                // at which point we won't be able to perform a reduced row echelon anyway
                // because the rows would be linearly dependent. so i question the value of having this here...

                augmented.ReducedRowEchelon();

                result = augmented.Submatrix(0, augmented.N - 1, augmented.M, 1);

                return augmented.Submatrix(0, 0, augmented.M, augmented.N - 1)
                    .IsIdentity;
            }
            else
            {   
                return false;
            }
        }
    }
}
