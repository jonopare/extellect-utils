using Extellect.Utilities.Sequencing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class CosDistributionFunctionTests
    {
        [Fact]
        public void NextDouble()
        {
            var helper = new DistributionFunctionTestHelper(11);
            
            var sut = DistributionFunction.Cos(helper.RandomMock.Object);

            var actual = Enumerable.Range(0, helper.Count)
                .Select(x => sut.NextDouble())
                .ToArray();

            var expected = new[]
            {
                0d,
                0.10016742116156,
                0.201357920790331,
                0.304692654015398,
                0.411516846067488,
                0.523598775598299,
                0.643501108793284,
                0.775397496610753,
                0.927295218001612,
                1.11976951499863,
                1.5707963267949
            };

            Assert.Equal(expected.Select(x => System.Math.Round(x, 10)), actual.Select(x => System.Math.Round(x, 10)));
        }

        [Fact]
        public void NextDouble_Many()
        {
            var helper = new DistributionFunctionTestHelper(100001);

            var sut = DistributionFunction.Cos(helper.RandomMock.Object);

            //sut.Interval

            var actual = Enumerable.Range(0, helper.Count - 1)
                .Select(x => System.Math.Floor(10 * sut.NextDouble() / System.Math.PI * 2d))
                .GroupBy(x => x)
                .Select(x => new { Bucket = x.Key, Count = x.Count() })
                .OrderBy(x => x.Bucket);

            var text = string.Join(Environment.NewLine, actual.Select(x => $"{x.Bucket}\t{x.Count}"));
        }

        [Theory]
        [InlineData(-0.5, -0.5, 0.5, 0)]
        [InlineData(0, -0.5, 0.5, 0.5)]
        [InlineData(0.5, -0.5, 0.5, 1)]
        [InlineData(1.5, 1.5, 2.5, 0)]
        [InlineData(2, 1.5, 2.5, 0.5)]
        [InlineData(2.5, 1.5, 2.5, 1)]
        public void Adjust(double input, double low, double high, double expected)
        {
            var randomMock = new Mock<IIterable<double>>();

            var sut = DistributionFunction.Cos(randomMock.Object, low * System.Math.PI, high * System.Math.PI);
            
            var actual = sut.Adjust(input * System.Math.PI);

            Assert.Equal(expected, actual, 15);
        }

        [Theory]
        [InlineData(0, -0.5, 0.5, -0.5)]
        [InlineData(0.5, -0.5, 0.5, 0)]
        [InlineData(1, -0.5, 0.5, 0.5)]
        //[InlineData(0.5, 1.5, 2.5, 2)]
        [InlineData(0, 0, 0.5, 0)]
        [InlineData(1, 0, 0.5, 0.5)]
        //[InlineData(0, 0.5, 1.5, 0.5)]
        //[InlineData(0.5, 0.5, 1.5, 1)]
        //[InlineData(1, 0.5, 1.5, 1.5)]
        public void NextDouble_Interval(double random, double low, double high, double expected)
        {
            Assert.True(random >= 0 && random <= 1);

            var randomMock = new Mock<IIterable<double>>();
            randomMock.Setup(x => x.Next()).Returns(random);

            var sut = DistributionFunction.Cos(randomMock.Object, low * System.Math.PI, high * System.Math.PI);

            var actual = sut.NextDouble();

            Assert.Equal(expected * System.Math.PI, actual, 15);
        }

        [Fact]
        public void NextDouble_Many_Interval()
        {
            var helper = new DistributionFunctionTestHelper(100001);

            //var sut = DistributionFunction.Cos(randomMock.Object, 1.5 * System.Math.PI, 2.5 * System.Math.PI);
            var sut = DistributionFunction.Cos(helper.RandomMock.Object, -0.5 * System.Math.PI, 0.5 * System.Math.PI);

            //sut.Interval

            var actual = Enumerable.Range(0, helper.Count - 1)
                .Select(x => System.Math.Floor(10 * sut.Adjust(sut.NextDouble())))
                .GroupBy(x => x)
                .Select(x => new { Bucket = x.Key, Count = x.Count() })
                .OrderBy(x => x.Bucket);

            var text = string.Join(Environment.NewLine, actual.Select(x => $"{x.Bucket + 0.5}\t{x.Count}"));
        }

        
    }
}
