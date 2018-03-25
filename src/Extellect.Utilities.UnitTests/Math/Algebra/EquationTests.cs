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
        [InlineData(false, false, "((m * x) + c)", "x = ((y - c) / m)")]
        [InlineData(false, true, "(c + (m * x))", "x = ((y - c) / m)")]
        [InlineData(true, false, "((x * m) + c)", "x = ((y - c) / m)")]
        [InlineData(true, true, "(c + (x * m))", "x = ((y - c) / m)")]
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
            Assert.Equal(sol.Left.Evaluate(), sol.Right.Evaluate());
        }

        [Theory]
        [InlineData("y", "y = ((x / m) + c)")]
        [InlineData("c", "c = ((x / m) - y)")]
        public void SolveFor_2(string targetName, string expectedSolution)
        {
            var y = new Variable("y");
            var m = new Variable("m");
            var x = new Variable("x");
            var c = new Variable("c");

            var variables = new[] { y, m, x, c }
                .ToDictionary(v => v.Name);

            var left = new Sub(y, c);
            var right = new Div(x, m);

            var eq = new Equation(left, right);
            Assert.Equal("(y - c) = (x / m)", eq.ToString());

            var sol = eq.SolveFor(variables[targetName]);
            Assert.Equal(expectedSolution, sol.ToString());

            //x.Assign(4);
            //m.Assign(3);
            //c.Assign(2);

            //y.Assign(right.Evaluate());

            //Assert.Equal(left.Evaluate(), right.Evaluate());
            //Assert.Equal(sol.Left.Evaluate(), sol.Right.Evaluate());
        }
    }
}
