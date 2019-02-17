#pragma warning disable 1591
using Extellect.Sequencing;
using System;
using System.Collections.Generic;

namespace Extellect.Math
{
    /// <summary>
    /// Basic form implementation of the Box-Muller transform, used
    /// to generate independent standard normally distributed random
    /// numbers.
    /// </summary>
    public class BoxMuller : IDisposable
    {
        private IIterable<double> _uniform;
        private double[] _factors;

        private bool _flag;
        
        public BoxMuller(Random random)
            : this(random.ToIterable())
        {
        }

        public BoxMuller(IIterable<double> uniform)
        {
            _uniform = uniform;
            _factors = new double[2];
        }

        public double NextDouble()
        {
            if (!_flag)
            {
                _factors[0] = System.Math.Sqrt(-2d * System.Math.Log(NextUniform()));
                _factors[1] = 2d * System.Math.PI * NextUniform(); // fraction of a complete rotation
                _flag = true;
                return _factors[0] * System.Math.Cos(_factors[1]);
            }
            else
            {
                _flag = false;
                return _factors[0] * System.Math.Sin(_factors[1]);
            }
        }

        private double NextUniform()
        {
            // the first factor cannot take 0 as its random number because Log(0) is -Infinity.
            // it's ok to use 1 as Log(1) is 0, and so the Sqrt(...) is zero too.
            double u;
            while ((u = _uniform.Next()) <= 0d || u > 1d);
            return u;
        }

        public void Dispose()
        {
            if (_uniform is IDisposable disposable)
            {
                disposable.Dispose();
            }
            _uniform = null;
            _factors = null;
        }
    }
}
