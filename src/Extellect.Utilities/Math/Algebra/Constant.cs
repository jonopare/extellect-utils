#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Math.Algebra
{
    public class Constant : IEvaluable
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
