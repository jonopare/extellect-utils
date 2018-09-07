#pragma warning disable 1591
using System;

namespace Extellect.Utilities.Math
{
    [Obsolete]
    public class Gaussian
    {
        private readonly Random _random;


        /// <summary>
        /// Don't use this class. It only performs the inverse operation of the 
        /// normal distribution, it doesn't do the important step of turning it
        /// back into a probability distribution.
        /// </summary>
        /// <param name="random"></param>
        [Obsolete]
        public Gaussian(Random random)
        {
            _random = random;
        }

        [Obsolete]
        public double NextDouble()
        {
            var translated = Translate(_random.NextDouble());
            return (_random.Next() & 1) == 1 ? translated : (0 - translated);
        }

        [Obsolete]
        public static double Translate(double value)
        {
            // TODO: integrate this function below
            return System.Math.Sqrt(0 - System.Math.Log(value));
        }
    }
}
