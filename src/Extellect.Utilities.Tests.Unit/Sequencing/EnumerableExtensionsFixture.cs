using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Extellect.Utilities.Testing;

namespace Extellect.Utilities.Sequencing
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class EnumerableExtensionsFixture
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Repeat()
        {
            var input = new[] { "need", "more", "coffee" };
            var count = 4;

            var actual = input.Repeat(count).ToArray();

            Assert.AreEqual(count * input.Length, actual.Length);
            foreach (var batch in actual.Batch(input.Length))
                AssertionHelper.AreSequencesEqual(input, batch);
        }

        private readonly double _tolerance = 1e-14;

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
        [TestMethod]
        public void StandardDeviationPopulation()
        {
            var actual = _statistics.StandardDeviationPopulation();
            Assert.AreEqual(2.51436194689627, actual, _tolerance);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void StandardDeviationSample()
        {
            var actual = _statistics.StandardDeviationSample();
            Assert.AreEqual(2.65037020474918, actual, _tolerance);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void VariancePopulation()
        {
            var actual = _statistics.VariancePopulation();
            Assert.AreEqual(6.322016, actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void VarianceSample()
        {
            var actual = _statistics.VarianceSample();
            Assert.AreEqual(7.02446222222222, actual, _tolerance);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Batch()
        {
            var expected = new int[][]
            {
                new [] {  0,  1,  2,  3,  4,  5,  6,  7,  8,  9 },
                new [] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 },
                new [] { 20, 21, 22, 23, 24 },
            };

            var actual = Enumerable.Range(0, 25).Batch(10).Select(x => x.ToArray()).ToArray();

            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++ )
                AssertionHelper.AreSequencesEqual(expected[i], actual[i]);
        }
    }
}

