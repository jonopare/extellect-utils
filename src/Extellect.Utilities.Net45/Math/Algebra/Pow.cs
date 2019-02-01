#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math.Algebra
{
    public class Pow : BinaryOperator
    {
        public Pow(IEvaluable leftOperand, IEvaluable rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        protected override double DoEvaluation(double leftValue, double rightValue)
        {
            return System.Math.Pow(leftValue, rightValue);
        }

        public IEvaluable Base => LeftOperand;
        public IEvaluable Exponent => RightOperand;

        public override string ToString()
        {
            return $"({Base} ^ {Exponent})";
        }
    }
}
