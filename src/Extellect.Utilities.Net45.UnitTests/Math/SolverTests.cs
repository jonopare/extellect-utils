using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class SolverTests
    {
        [Fact]
        public void GoalSeek_MathPower()
        {
            var actual = Solver.GoalSeek(x => System.Math.Pow(2, x), 16, out double error, 1, 20);
            Assert.Equal(4, actual);
        }

        [Fact]
        public void GoalSeek_Reciprocal()
        {
            var actual = Solver.GoalSeek(x => 1d / x, 0.333, out double error, 0, 4);
            Assert.Equal(3.003003003003, actual, 12);
        }

        [Fact]
        public void GoalSeek_AngleDouble()
        {
            Func<double, Angle> area = r => Angle.Pi * r * r;

            var actual = Solver.GoalSeek(area, Angle.Pi * 100, out Angle error, (x, y) => x - y, (x, y) => x + y, (x, y) => x - y, x => x / 2, 0, 25);

            Assert.Equal(10d, actual, 12);
        }

        [Fact]
        public void GoalSeek_DoubleAngle()
        {
            Func<Angle, double> area = pi => (pi * 100).Radians;

            var actual = Solver.GoalSeek(area, Angle.Pi.Radians * 100, out double error, (x, y) => x - y, (x, y) => x + y, (x, y) => x - y, x => x / 2, Angle.Zero, Angle.TwoPi);

            Assert.Equal(Angle.Pi.Radians, actual.Radians, 12);
        }
    }
}
