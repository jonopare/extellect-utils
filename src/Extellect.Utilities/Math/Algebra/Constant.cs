using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    class Constant : IEvaluable
    {
        private readonly double _value;

        public Constant(double value)
        {
            _value = value;
        }

        public double Evaluate()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
