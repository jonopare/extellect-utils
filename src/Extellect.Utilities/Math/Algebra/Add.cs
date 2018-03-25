#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math.Algebra
{
    public class Add : BinaryEvaluable
    {
        public Add(IEvaluable left, IEvaluable right)
            : base(left, right)
        {
        }

        protected override double DoEvaluation(double leftValue, double rightValue)
        {
            return leftValue + rightValue;
        }

        public override string ToString()
        {
            return $"({Left} + {Right})";
        }
    }
}
