using Extellect.Sequencing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Math
{
    public class SinDistributionFunctionTests
    {
        [Fact]
        public void NextDouble()
        {
            var randoms = Enumerable.Range(0, 11)
                .Select(x => x / 10d)
                .ToArray();

            var index = 0;

            var randomMock = new Mock<IIterable<double>>();
            randomMock.Setup(x => x.Next()).Returns(() => randoms[index++]);

            var sut = DistributionFunction.Sin(randomMock.Object);

            var actual = randoms.Select(x => sut.NextDouble())
                .ToArray();

            var expected = new[]
            {
                0d,
                0.643501108793284,
                0.927295218001612,
                1.159279480727410,
                1.369438406004570,
                1.570796326794900,
                1.77215424758523,
                1.98231317286238,
                2.21429743558818,
                2.49809154479651,
                3.14159265358979,
            };

            Assert.Equal(expected.Select(x => System.Math.Round(x, 10)), actual.Select(x => System.Math.Round(x, 10)));
        }

        [Fact]
        public void NextDouble_Many()
        {
            var helper = new DistributionFunctionTestHelper(100001);

            var sut = DistributionFunction.Sin(helper.RandomMock.Object);

            //sut.Interval

            var actual = Enumerable.Range(0, helper.Count - 1)
                .Select(x => System.Math.Floor(10 * sut.NextDouble() / System.Math.PI))
                .GroupBy(x => x)
                .Select(x => new { Bucket = x.Key, Count = x.Count() })
                .OrderBy(x => x.Bucket);

            var text = string.Join(Environment.NewLine, actual.Select(x => $"{x.Bucket}\t{x.Count}"));
        }
    }
}
