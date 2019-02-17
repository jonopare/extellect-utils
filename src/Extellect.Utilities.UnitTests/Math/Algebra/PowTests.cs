using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Math.Algebra
{
    public class PowTests
    {
        private readonly Constant _base;
        private readonly Constant _exponent;
        private readonly Pow _power;

        public PowTests()
        {
            _base = new Constant(2);
            _exponent = new Constant(3);
            _power = new Pow(_base, _exponent);
        }

        [Fact]
        public void Ctor_SetsBaseAndExponent()
        {
            Assert.Equal(_base, _power.Base);
            Assert.Equal(_exponent, _power.Exponent);
        }

        [Fact]
        public void Evaluates()
        {
            Assert.Equal(8, _power.Evaluate());
        }

        [Fact]
        public void ToString_IsReadable()
        {
            Assert.Equal("(2 ^ 3)", _power.ToString());
        }
    }
}

