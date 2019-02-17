using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Math
{
    /// <summary>
    /// See https://en.wikipedia.org/wiki/B%C3%A9zier_curve
    /// </summary>
    public class Bezier
    {
        private readonly Triplet[] _points;
        private readonly long[] _weights;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        public Bezier(params Triplet[] points)
        {
            if (points.Length < 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            _points = points;
            _weights = Enumerable.Range(0, points.Length)
                .Select(x => BinomialCoefficient.Combinations(points.Length - 1, x))
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public Triplet Position(double t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            return Enumerable.Range(0, _points.Length)
                .Select(i => Term(i, t))
                .Aggregate((x, y) => x + y);
        }

        /// <summary>
        /// Gets the ith term in the sequence of weighted positions.
        /// </summary>
        private Triplet Term(int i, double t)
        {
            return _weights[i] * System.Math.Pow(t, i) * System.Math.Pow(1 - t, _points.Length - i - 1) * _points[i];
        }
    }
}
