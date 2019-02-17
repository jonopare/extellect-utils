#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Math.Algebra
{
    public class Add : BinaryOperator
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
            return $"({LeftOperand} + {RightOperand})";
        }
    }
}
