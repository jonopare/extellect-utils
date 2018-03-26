#pragma warning disable 1591
using System;

namespace Extellect.Utilities.Math.Algebra
{
    public abstract class UnaryOperator : IEvaluable
    {
        private readonly IEvaluable _operand;

        public UnaryOperator(IEvaluable operand)
        {
            _operand = operand;
        }

        public IEvaluable Operand => _operand;

        public double Evaluate()
        {
            return DoEvaluation(_operand.Evaluate());
        }

        protected abstract double DoEvaluation(double value);
    }
}