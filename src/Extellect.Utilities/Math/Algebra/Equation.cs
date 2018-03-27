#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Math.Algebra
{
    public class Equation : BinaryOperator
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
        /// Solves a simple algebraic equation for one of its terms.
        /// </summary>
        public Equation SolveFor(Variable target)
        {
            var results = new List<IEvaluable>();            

            Func<BinaryOperator, IEvaluable, bool> isFoundLeft = (be, n) => (be.LeftOperand == n || be.LeftOperand == target);

            Func<BinaryOperator, IEvaluable, IEvaluable> ob = (be, n) => isFoundLeft(be, n) ? be.RightOperand : be.LeftOperand;

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
                            otherBranch = new Mul(otherBranch, Inv.Create(ob(mul, next)));
                        }
                        else if (e is Add add)
                        {
                            otherBranch = new Add(otherBranch, Neg.Create(ob(add, next)));
                        }
                        else if (e is Inv inv)
                        {
                            otherBranch = Inv.Create(otherBranch);
                        }
                        else if (e is Neg neg)
                        {
                            otherBranch = Neg.Create(otherBranch);
                        }
                        else if (e is Pow pow)
                        {
                            if (isFoundLeft(pow, next))
                            {
                                otherBranch = new Pow(otherBranch, Neg.Create(pow.RightOperand));
                            }
                            else
                            {
                                otherBranch = new Log(pow.LeftOperand, otherBranch);
                            }
                        }
                        else if (e is Log log)
                        {
                            if (isFoundLeft(log, next))
                            {
                                otherBranch = new Pow(log.RightOperand, Inv.Create(otherBranch));
                            }
                            else
                            {
                                otherBranch = new Pow(log.LeftOperand, otherBranch);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                    }
                    results.Add(otherBranch);
                };
            };

            Find(LeftOperand, target, new Stack<IEvaluable>(), createFound(RightOperand));

            Find(RightOperand, target, new Stack<IEvaluable>(), createFound(LeftOperand));

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
                    // terminal node - didn't find what we wanted
                }
            }
            else if (branch is Constant)
            {
                // terminal node - didn't find what we wanted
            }
            else if (branch is BinaryOperator binary)
            {
                breadcrumbs.Push(branch);
                Find(binary.LeftOperand, target, breadcrumbs, found);
                Find(binary.RightOperand, target, breadcrumbs, found);
                breadcrumbs.Pop();
            }
            else if (branch is UnaryOperator unary)
            {
                breadcrumbs.Push(branch);
                Find(unary.Operand, target, breadcrumbs, found);
                breadcrumbs.Pop();
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return $"{LeftOperand} = {RightOperand}";
        }
    }
}
