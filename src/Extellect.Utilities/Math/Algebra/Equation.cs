#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public class Equation : BinaryEvaluable
    {
        public Equation(IEvaluable left, IEvaluable right)
            : base(left, right)
        {
        }

        /// <summary>
        /// TODO: refactor so this class doesn't have to implement this method
        /// </summary>
        protected override double DoEvaluation(double leftValue, double rightValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Solves a simple algebraic equation for one of its terms. Ergh... I wouldn't trust the output of this method, it cannot handle solutions for negative terms (among other things)
        /// </summary>
        public Equation SolveFor(Variable target)
        {
            var results = new List<IEvaluable>();

            Func<BinaryEvaluable, IEvaluable, IEvaluable> ob = (be, n) => (be.Left == n || be.Left == target) ? be.Right : be.Left;

            Func<IEvaluable, Action<IList<IEvaluable>>> createFound = otherBranch =>
            {
                return es =>
                {
                    for (var i = es.Count - 1; i >= 0; i--)
                    {
                        var e = es[i];
                        IEvaluable next = i == 0 ? null : es[i - 1];
                        if (e is Mul mul)
                        {
                            otherBranch = new Div(otherBranch, ob(mul, next));
                        }
                        else if (e is Div div)
                        {
                            otherBranch = new Mul(otherBranch, ob(div, next));
                        }
                        else if (e is Add add)
                        {
                            otherBranch = new Sub(otherBranch, ob(add, next));
                        }
                        else if (e is Sub sub)
                        {
                            otherBranch = new Add(otherBranch, ob(sub, next));
                        }
                    }
                    results.Add(otherBranch);
                };
            };

            Find(Left, target, new Stack<IEvaluable>(), createFound(Right));

            Find(Right, target, new Stack<IEvaluable>(), createFound(Left));

            return new Equation(target, results.Single());
        }

        private void Find(IEvaluable branch, Variable target, Stack<IEvaluable> breadcrumbs, Action<IList<IEvaluable>> found)
        {
            if (branch is Variable variable)
            {
                if (variable == target)
                {
                    found(breadcrumbs.ToArray());
                }
                else
                {
                    // another variable
                }
            }
            else if (branch is BinaryEvaluable binary)
            {
                breadcrumbs.Push(branch);
                Find(binary.Left, target, breadcrumbs, found);
                Find(binary.Right, target, breadcrumbs, found);
                breadcrumbs.Pop();
            }
            else if (branch is Constant)
            {
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return $"{Left} = {Right}";
        }
    }
}
