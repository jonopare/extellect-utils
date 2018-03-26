#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math.Algebra
{
    public class Inv : UnaryOperator
    {
        private Inv(IEvaluable operand)
            : base(operand)
        {
        }

        public static IEvaluable Create(IEvaluable operand)
        {
            if (operand is Inv inv)
            {
                return inv.Operand;
            }
            else
            {
                return new Inv(operand);
            }
        }

        protected override double DoEvaluation(double value)
        {
            return 1 / value;
        }

        public override string ToString()
        {
            return $"1/{Operand}";
        }
    }
}
