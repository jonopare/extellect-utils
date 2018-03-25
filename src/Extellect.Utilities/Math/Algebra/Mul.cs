#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public class Mul : BinaryEvaluable
    {
        public Mul(IEvaluable left, IEvaluable right)
            : base(left, right)
        {
        }

        protected override double DoEvaluation(double leftValue, double rightValue)
        {
            return leftValue * rightValue;
        }

        public override string ToString()
        {
            return $"({Left} * {Right})";
        }
    }
}
