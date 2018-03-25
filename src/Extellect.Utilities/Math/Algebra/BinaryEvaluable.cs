#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public abstract class BinaryEvaluable : IEvaluable
    {
        IEvaluable _left;
        IEvaluable _right;

        public BinaryEvaluable(IEvaluable left, IEvaluable right)
        {
            _left = left;
            _right = right;
        }

        public IEvaluable Left => _left;
        public IEvaluable Right => _right;

        public double Evaluate()
        {
            return DoEvaluation(_left.Evaluate(), _right.Evaluate());
        }

        protected abstract double DoEvaluation(double leftValue, double rightValue);
    }
}
