#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math.Algebra
{
    public class Log : BinaryOperator
    {
        public Log(IEvaluable leftOperand, IEvaluable rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        protected override double DoEvaluation(double leftValue, double rightValue)
        {
            return System.Math.Log(rightValue, leftValue);
        }

        public IEvaluable Base => LeftOperand;
        public IEvaluable Exponent => RightOperand;

        public override string ToString()
        {
            return $"log[{Base}]{Exponent}";
        }
    }
}
