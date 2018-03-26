using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math.Algebra
{
    public class EquationTests
    {
        [Theory]
        [InlineData(false, false, "((m * x) + c)", "x = ((y + -c) * 1/m)")]
        [InlineData(false, true, "(c + (m * x))", "x = ((y + -c) * 1/m)")]
        [InlineData(true, false, "((x * m) + c)", "x = ((y + -c) * 1/m)")]
        [InlineData(true, true, "(c + (x * m))", "x = ((y + -c) * 1/m)")]
        public void SolveFor(bool commuteMul, bool commuteAdd, string expRight, string expSol)
        {
            var y = new Variable("y");
            var m = new Variable("m");
            var x = new Variable("x");
            var c = new Variable("c");

            var left = y;
            var mul = commuteMul ? new Mul(x, m) : new Mul(m, x);
            var right = commuteAdd ? new Add(c, mul) : new Add(mul, c);
            Assert.Equal(expRight, right.ToString());

            var eq = new Equation(left, right);

            var sol = eq.SolveFor(x);
            Assert.Equal(expSol, sol.ToString());

            x.Assign(4);
            m.Assign(3);
            c.Assign(2);

            y.Assign(right.Evaluate());

            Assert.Equal(left.Evaluate(), right.Evaluate());
            Assert.Equal(sol.LeftOperand.Evaluate(), sol.RightOperand.Evaluate());
        }

        [Theory]
        [InlineData("y", "y = ((x * 1/m) + c)")]
        [InlineData("c", "c = -((x * 1/m) + -y)")]
        [InlineData("m", "m = 1/((y + -c) * 1/x)")]
        [InlineData("x", "x = ((y + -c) * m)")]
        public void SolveFor_2(string targetName, string expectedSolution)
        {
            var y = new Variable("y");
            var m = new Variable("m");
            var x = new Variable("x");
            var c = new Variable("c");

            var variables = new[] { y, m, x, c }
                .ToDictionary(v => v.Name);

            var left = new Add(y, Neg.Create(c));
            var right = new Mul(x, Inv.Create(m));

            var eq = new Equation(left, right);
            Assert.Equal("(y + -c) = (x * 1/m)", eq.ToString());

            var sol = eq.SolveFor(variables[targetName]);
            Assert.Equal(expectedSolution, sol.ToString());

            x.Assign(4d);
            m.Assign(3d);
            c.Assign(2d);
            y.Assign(10d / 3d);
            //// solve for y so that we can evaluate all variables - bit of cheating that allows us to modify the above expression a bit in testing
            //var sol_y = eq.SolveFor(y);
            //y.Assign(sol_y.RightOperand.Evaluate());

            Assert.Equal(left.Evaluate(), right.Evaluate(), 8);
            Assert.Equal(sol.LeftOperand.Evaluate(), sol.RightOperand.Evaluate(), 8);
        }
    }
}
