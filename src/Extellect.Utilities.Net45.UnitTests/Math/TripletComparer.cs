using System;
using System.Collections.Generic;

namespace Extellect.Utilities.Math
{
    public class TripletComparer : IEqualityComparer<Triplet>
    {
        private int _precision;

        public TripletComparer(int precision)
        {
            _precision = precision;
        }
        

        public bool Equals(Triplet x, Triplet y)
        {
            foreach (var getter in Getters)
            {
                if (System.Math.Round(getter(x), _precision)
                    != System.Math.Round(getter(y), _precision))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(Triplet obj)
        {
            var hashCode = 0;
            foreach (var getter in Getters)
            {
                hashCode ^= System.Math.Round(getter(obj), _precision).GetHashCode();
            }
            return hashCode;
        }

        private IEnumerable<Func<Triplet, double>> Getters
        {
            get
            {
                yield return t => t.X;
                yield return t => t.Y;
                yield return t => t.Z;
            }
        }
    }
}
