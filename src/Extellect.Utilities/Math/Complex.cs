#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math
{
    public struct Complex : IEquatable<Complex>
    {
        public Complex(double real, double imaginary)
        {
            Re = real;
            Im = imaginary;
        }

        public double Re { get; }
        public double Im { get; }

        public bool Equals(Complex other)
        {
            return Re == other.Re && Im == other.Im;
        }
    }
}
