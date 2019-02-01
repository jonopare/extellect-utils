using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class MathExpressionTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(System.Math.PI / 2, 0)]
        [InlineData(System.Math.PI, -1)]
        [InlineData(3 * System.Math.PI / 2, 0)]
        public void Cos(double arg, double expected)
        {
            var parameter = Expression.Parameter(typeof(double));
            var cos = MathExpression.Cos(parameter);
            var lambda = Expression.Lambda<Func<double, double>>(cos, parameter);
            var func = lambda.Compile();

            var actual = func(arg);

            Assert.Equal(expected, actual, 15);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(System.Math.PI / 2, 1)]
        [InlineData(System.Math.PI, 0)]
        [InlineData(3 * System.Math.PI / 2, -1)]
        public void Sin(double arg, double expected)
        {
            var parameter = Expression.Parameter(typeof(double));
            var sin = MathExpression.Sin(parameter);
            var lambda = Expression.Lambda<Func<double, double>>(sin, parameter);
            var func = lambda.Compile();

            var actual = func(arg);

            Assert.Equal(expected, actual, 15);
        }

        [Fact]
        public void Foo()
        {
            Expression<Func<double, double>> expr = x => System.Math.Cos(x);
            var sin = expr.Compile();
            var actual = sin(System.Math.PI / 2);
        }
    }
}
