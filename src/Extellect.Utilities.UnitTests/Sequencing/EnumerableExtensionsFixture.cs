using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extellect.Testing;
using Xunit;

namespace Extellect.Sequencing
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumerableExtensionsFixture
    {
        private readonly int _precision = 14;

        private readonly double[] _statistics = new[]
                {
                    3.29,
                    2.70,
                    6.00,
                    7.05,
                    4.66,
                    2.60,
                    1.21,
                    2.70,
                    0.83,
                    9.08,
                };

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Repeat()
        {
            var input = new[] { "need", "more", "coffee" };
            var count = 4;

            var actual = input.Repeat(count).ToArray();

            Assert.Equal(count * input.Length, actual.Length);
            foreach (var batch in actual.Batch(input.Length))
                AssertionHelper.AreSequencesEqual(input, batch);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void StandardDeviationPopulation()
        {
            var actual = _statistics.StandardDeviationPopulation();
            Assert.Equal(2.51436194689627, actual, _precision);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void StandardDeviationSample()
        {
            var actual = _statistics.StandardDeviationSample();
            Assert.Equal(2.65037020474918, actual, _precision);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void VariancePopulation()
        {
            var actual = _statistics.VariancePopulation();
            Assert.Equal(6.322016, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void VarianceSample()
        {
            var actual = _statistics.VarianceSample();
            Assert.Equal(7.02446222222222, actual, _precision);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Batch()
        {
            var expected = new int[][]
            {
                new [] {  0,  1,  2,  3,  4,  5,  6,  7,  8,  9 },
                new [] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 },
                new [] { 20, 21, 22, 23, 24 },
            };

            var actual = Enumerable.Range(0, 25).Batch(10).Select(x => x.ToArray()).ToArray();

            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++ )
                AssertionHelper.AreSequencesEqual(expected[i], actual[i]);
        }
    }
}

