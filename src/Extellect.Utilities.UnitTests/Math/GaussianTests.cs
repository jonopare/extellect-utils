using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math
{
    public class GaussianTests
    {
        [Fact]
        public void NextDouble()
        {
            var sut = new Gaussian(new Random());

            var ts = Enumerable.Range(0, 10000)
                .Select(x => System.Math.Round(sut.NextDouble() * 10, MidpointRounding.AwayFromZero) / 10)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());
        }

        [Fact]
        public void Translate()
        {
            var count = 100d;
            var ts = Enumerable.Range(1, (int)count)
                .Select(x => x / count)
                .Select(x => new { x, t = Gaussian.Translate(x) })
                .ToArray();
        }

        [Fact]
        public void TranslateRandom()
        {
            var random = new Random();
            var count = 1000;
            var ts = Enumerable.Range(1, count)
                .Select(x => random.NextDouble())
                .Select(x => new { x, t = Gaussian.Translate(x) })
                .ToArray();
        }
    }
}
