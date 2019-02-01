using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class BezierTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0.1, 0.375964594300514, 0.36)]
        [InlineData(0.2, 0.710725635973, 0.64)]
        [InlineData(0.3, 1.014584013175, 0.84)]
        [InlineData(0.4, 1.297840614064, 0.96)]
        [InlineData(0.5, 1.5707963267949, 1)]
        [InlineData(0.6, 1.843752039526, 0.96)]
        [InlineData(0.7, 2.127008640414, 0.84)]
        [InlineData(0.8, 2.430867017616, 0.64)]
        [InlineData(0.9, 2.765628059289, 0.36)]
        [InlineData(1.0, 3.14159265358979, 0)]
        public void Position_Cubic(double t, double x, double y)
        {
            var g = 4d / 3d;
            var sut = new Bezier(new Triplet(0, 0, 0), new Triplet(g, g, 0), new Triplet(System.Math.PI - g, g, 0), new Triplet(System.Math.PI, 0, 0));

            var actual = sut.Position(t);

            Assert.Equal(x, actual.X, 12);
            Assert.Equal(y, actual.Y, 12);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0.5, 1.5707963267949, 1)]
        [InlineData(1.0, 3.14159265358979, 0)]
        public void Position_Quadratic(double t, double x, double y)
        {
            var sut = new Bezier(new Triplet(0, 0, 0), new Triplet(System.Math.PI / 2, 2, 0), new Triplet(System.Math.PI, 0, 0));

            var actual = sut.Position(t);

            Assert.Equal(x, actual.X, 12);
            Assert.Equal(y, actual.Y, 12);
        }

        /// <summary>
        /// Trace a quartic bezier curve inside a regular pentagon.
        /// </summary>
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0.25, 0.0403686271093947, 0.770407177574806)]
        [InlineData(0.5, 0.5, 1.052593921368)]
        [InlineData(0.75, 0.959631372890605, 0.770407177574806)]
        [InlineData(1.0, 1, 0)]
        public void Position_Quartic(double t, double x, double y)
        {
            var a = 2 * System.Math.PI / 5;
            var b = a / 2;
            var c = System.Math.PI - a - b;

            var sut = new Bezier(
                new Triplet(0, 0, 0),
                new Triplet(0 - System.Math.Cos(a), System.Math.Sin(a), 0),
                new Triplet(0.5, System.Math.Tan(c) / 2, 0),
                new Triplet(1 + System.Math.Cos(a), System.Math.Sin(a), 0),
                new Triplet(1, 0, 0));

            var actual = sut.Position(t);

            Assert.Equal(x, actual.X, 12);
            Assert.Equal(y, actual.Y, 12);
        }
    }
}
