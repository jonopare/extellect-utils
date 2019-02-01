#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math.Algebra
{
    public class Neg : UnaryOperator
    {
        private Neg(IEvaluable operand)
            : base(operand)
        {
        }

        public static IEvaluable Create(IEvaluable operand)
        {
            if (operand is Neg neg)
            {
                return neg.Operand;
            }
            else
            {
                return new Neg(operand);
            }
        }

        protected override double DoEvaluation(double value)
        {
            return 0 - value;
        }

        public override string ToString()
        {
            return $"-{Operand}";
        }
    }
}
