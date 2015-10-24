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
        /// Tries to solve a linear equation with matrices
        /// </summary>
        public static bool TrySolve(Matrix abcd, Matrix ef, out Matrix result)
        {
            result = null;

            Matrix inverse;
            if (!abcd.TryInvert(out inverse))
                return false;

            if (!inverse.TryMultiply(ef, out result))
                return false;

            return true;
        }
    }
}
