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
    public class LinearDistributionFunctionTests
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

            var sut = DistributionFunction.Linear(randomMock.Object);

            var actual = randoms.Select(x => sut.NextDouble())
                .ToArray();

            var expected = new[]
            {
                0,
                0.0725727758732213,
                0.149302498305743,
                0.230997605753172,
                0.318768447362763,
                0.414213562373095,
                0.519786371373179,
                0.639616893131612,
                0.78175803033942,
                0.966999966873138,
                1.4142135623730952,
            };

            Assert.Equal(expected.Select(x => System.Math.Round(x, 10)), actual.Select(x => System.Math.Round(x, 10)));
        }

        [Fact]
        public void NextDouble_Many()
        {
            var helper = new DistributionFunctionTestHelper(100001);

            var sut = DistributionFunction.Linear(helper.RandomMock.Object);

            //sut.Interval

            var actual = Enumerable.Range(0, helper.Count - 1)
                .Select(x => System.Math.Floor(10 * sut.NextDouble() / System.Math.Sqrt(2d)))
                .GroupBy(x => x)
                .Select(x => new { Bucket = x.Key, Count = x.Count() })
                .OrderBy(x => x.Bucket);

            var text = string.Join(Environment.NewLine, actual.Select(x => $"{x.Bucket}\t{x.Count}"));
        }
    }
}
