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
        public void GoalSeek_Foo()
        {
            var actual = Solver.GoalSeek(x => 1d / x, 0.333, out double error, 0, 4);
            Assert.Equal(3.003003003003, actual, 12);
        }
    }
}
