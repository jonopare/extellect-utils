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
        private readonly Variable _y;
        private readonly Variable _m;
        private readonly Variable _x;
        private readonly Variable _c;
        private readonly Dictionary<string, Variable> _variablesByName;

        public EquationTests()
        {
            _y = new Variable("y");
            _m = new Variable("m");
            _x = new Variable("x");
            _c = new Variable("c");
            _variablesByName = new[] { _y, _m, _x, _c }
                .ToDictionary(v => v.Name);
        }

        [Theory]
        [InlineData(false, false, "((m * x) + c)", "x = ((y + -c) * 1/m)")]
        [InlineData(false, true, "(c + (m * x))", "x = ((y + -c) * 1/m)")]
        [InlineData(true, false, "((x * m) + c)", "x = ((y + -c) * 1/m)")]
        [InlineData(true, true, "(c + (x * m))", "x = ((y + -c) * 1/m)")]
        public void SolveFor_AddMul(bool commuteMul, bool commuteAdd, string expRight, string expSol)
        {
            var left = _y;
            var mul = commuteMul ? new Mul(_x, _m) : new Mul(_m, _x);
            var right = commuteAdd ? new Add(_c, mul) : new Add(mul, _c);
            Assert.Equal(expRight, right.ToString());

            var eq = new Equation(left, right);

            var sol = eq.SolveFor(_x);
            Assert.Equal(expSol, sol.ToString());

            _x.Assign(4);
            _m.Assign(3);
            _c.Assign(2);

            _y.Assign(right.Evaluate());

            Assert.Equal(left.Evaluate(), right.Evaluate());
            Assert.Equal(sol.LeftOperand.Evaluate(), sol.RightOperand.Evaluate());
        }

        [Theory]
        [InlineData("y", "y = ((x * 1/m) + c)")]
        [InlineData("c", "c = -((x * 1/m) + -y)")]
        [InlineData("m", "m = 1/((y + -c) * 1/x)")]
        [InlineData("x", "x = ((y + -c) * m)")]
        public void SolveFor_AddMulNegInv(string targetName, string expectedSolution)
        {
            var left = new Add(_y, Neg.Create(_c));
            var right = new Mul(_x, Inv.Create(_m));

            var eq = new Equation(left, right);
            Assert.Equal("(y + -c) = (x * 1/m)", eq.ToString());

            var sol = eq.SolveFor(_variablesByName[targetName]);
            Assert.Equal(expectedSolution, sol.ToString());

            _x.Assign(4d);
            _m.Assign(3d);
            _c.Assign(2d);
            _y.Assign(10d / 3d);

            Assert.Equal(left.Evaluate(), right.Evaluate(), 8);
            Assert.Equal(sol.LeftOperand.Evaluate(), sol.RightOperand.Evaluate(), 8);
        }

        [Fact]
        public void SolveFor_PowerToLog()
        {
            var left = _y;
            var right = new Add(new Pow(_m, _x), _c);
            var eq = new Equation(left, right);
            Assert.Equal("y = ((m ^ x) + c)", eq.ToString());

            var sol = eq.SolveFor(_x);
            Assert.Equal("x = log[m](y + -c)", sol.ToString());
        }

        [Fact]
        public void SolveFor_PowerToInversePower()
        {
            var left = _y;
            var right = new Add(new Pow(_m, _x), _c);
            var eq = new Equation(left, right);
            Assert.Equal("y = ((m ^ x) + c)", eq.ToString());

            var sol_m = eq.SolveFor(_m);
            Assert.Equal("m = ((y + -c) ^ 1/x)", sol_m.ToString());

            _y.Assign(9);
            _m.Assign(2);
            _x.Assign(3);
            _c.Assign(1);

            Assert.Equal(left.Evaluate(), right.Evaluate());
            Assert.Equal(sol_m.LeftOperand.Evaluate(), sol_m.RightOperand.Evaluate());
        }

        [Fact]
        public void SolveFor_LogToInversePower()
        {
            var left = _y;
            var right = new Add(new Log(_m, _x), _c);
            var eq = new Equation(left, right);
            Assert.Equal("y = (log[m]x + c)", eq.ToString());
            
            var sol_m = eq.SolveFor(_m);
            Assert.Equal("m = (x ^ 1/(y + -c))", sol_m.ToString());

            _y.Assign(4);
            _m.Assign(2);
            _x.Assign(8);
            _c.Assign(1);

            Assert.Equal(2, sol_m.LeftOperand.Evaluate());
            Assert.Equal(2, sol_m.RightOperand.Evaluate());
        }

        [Fact]
        public void SolveFor_LogToPower()
        {
            var left = _y;
            var right = new Add(new Log(_m, _x), _c);
            var eq = new Equation(left, right);
            Assert.Equal("y = (log[m]x + c)", eq.ToString());

            var sol_x = eq.SolveFor(_x);
            Assert.Equal("x = (m ^ (y + -c))", sol_x.ToString());
            
            _y.Assign(4);
            _m.Assign(2);
            _x.Assign(8);
            _c.Assign(1);

            Assert.Equal(8, sol_x.LeftOperand.Evaluate());
            Assert.Equal(8, sol_x.RightOperand.Evaluate());
        }

        [Fact]
        public void SolveFor_BiggerEquationThanNormal()
        {
            var left = _y;
            var right = new Log(new Constant(3), new Pow(new Mul(_m, Inv.Create(_x)), new Add(new Constant(3), Neg.Create(_c))));
            
            var eq = new Equation(left, right);
            var sol_x = eq.SolveFor(_x);

            _x.Assign(15);
            _m.Assign(7);
            _c.Assign(4);
            _y.Assign(right.Evaluate());

            //var r2 = new Mul(new Mul(_m, new Add(new Constant(3), Neg.Create(_c))), Inv.Create(new Pow(new Constant(3), _y)));

            Assert.Equal(sol_x.LeftOperand.Evaluate(), sol_x.RightOperand.Evaluate());


        }

        [Fact]
        public void SolveFor_BiggerEquationThanNormalButSmallerThanThat()
        {
            var left = _y;
            var right = new Log(new Constant(3), new Mul(_m, Inv.Create(_x)));

            var eq = new Equation(left, right);
            var sol_x = eq.SolveFor(_x);

            _x.Assign(15);
            _m.Assign(7);
            //_c.Assign(3);
            _y.Assign(right.Evaluate());

            Assert.Equal(_x.Evaluate(), sol_x.RightOperand.Evaluate());
        }

        [Fact]
        public void SolveFor_BiggerEquationThanNormalButSmallerThanThatBleh()
        {
            var left = _y;
            var right = new Log(new Constant(3), new Pow(_x, _m/*Inv.Create(_m)*/));

            var eq = new Equation(left, right);
            var sol_x = eq.SolveFor(_x);

            _x.Assign(15);
            _m.Assign(7);
            //_c.Assign(3);
            _y.Assign(right.Evaluate());

            Assert.Equal(sol_x.LeftOperand.Evaluate(), sol_x.RightOperand.Evaluate(), 10);
        }

        [Fact]
        public void SolveFor_Factor()
        {
            var left = _y;
            var right = new Add(_x, new Mul(_x, new Constant(3)));

            var eq = new Equation(left, right);
            var sol_x = eq.SolveFor(_x);

            // should be clever enough to factor this into
            // y = x + x         | y = 2 * x
            // y = x + 3 * x     | y = 4 * x
            // y = x * x         | y = x ^ 2
            // y = x * 3 * x     | y = (3 * x) ^ 2

            _x.Assign(15);
            _m.Assign(7);
            //_c.Assign(3);
            _y.Assign(right.Evaluate());

            Assert.Equal(sol_x.LeftOperand.Evaluate(), sol_x.RightOperand.Evaluate(), 10);
        }
    }
}
