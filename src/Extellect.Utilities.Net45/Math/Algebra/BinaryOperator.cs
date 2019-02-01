#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public abstract class BinaryOperator : IEvaluable
    {
        IEvaluable _leftOperand;
        IEvaluable _rightOperand;

        public BinaryOperator(IEvaluable leftOperand, IEvaluable rightOperand)
        {
            _leftOperand = leftOperand;
            _rightOperand = rightOperand;
        }

        public IEvaluable LeftOperand => _leftOperand;
        public IEvaluable RightOperand => _rightOperand;

        public double Evaluate()
        {
            return DoEvaluation(_leftOperand.Evaluate(), _rightOperand.Evaluate());
        }

        protected abstract double DoEvaluation(double leftValue, double rightValue);
    }
}
