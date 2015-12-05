using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extellect.Utilities.Sequencing
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items, int count)
        {
            for (var i = 0; i < count; i++)
                foreach (var item in items)
                    yield return item;
        }

        /// <summary>
        /// 
        /// </summary>
        public static double StandardDeviationPopulation(this IEnumerable<double> items)
        {
            return System.Math.Sqrt(items.VariancePopulation());
        }

        /// <summary>
        /// 
        /// </summary>
        public static double StandardDeviationSample(this IEnumerable<double> items)
        {
            return System.Math.Sqrt(items.VarianceSample());
        }

        /// <summary>
        /// 
        /// </summary>
        public static double VariancePopulation(this IEnumerable<double> items)
        {
            var average = items.Average();

            return items.Select(x => x - average)
                .Select(x => x * x)
                .Average();
        }

        /// <summary>
        /// 
        /// </summary>
        public static double VarianceSample(this IEnumerable<double> items)
        {
            var average = items.Average();

            return items.Select(x => x - average)
                .Select(x => x * x)
                .AverageSample();
        }

        /// <summary>
        /// 
        /// </summary>
        public static double AverageSample(this IEnumerable<double> items)
        {
            double count = 0;
            return items.Aggregate(0d, (x, y) => { count++; return x + y; }, x => x / (count - 1));
        }

        /// <summary>
        /// Splits the values in an enumerable by position into batches of the specified size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int batchSize)
        {
            return items.Select((x, i) => new { Value = x, Batch = i / batchSize }).GroupBy(x => x.Batch, x => x.Value);
        }
    }
}
