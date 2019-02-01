using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extellect.Utilities.Math.Algebra
{
    public class LogTests
    {
        private readonly Constant _base;
        private readonly Constant _exponent;
        private readonly Log _log;

        public LogTests()
        {
            _base = new Constant(2);
            _exponent = new Constant(8);
            _log = new Log(_base, _exponent);
        }

        [Fact]
        public void Ctor_SetsBaseAndExponent()
        {
            Assert.Equal(_base, _log.Base);
            Assert.Equal(_exponent, _log.Exponent);
        }

        [Fact]
        public void Evaluates()
        {
            Assert.Equal(3, _log.Evaluate());
        }

        [Fact]
        public void ToString_IsReadable()
        {
            Assert.Equal("log[2]8", _log.ToString());
        }
    }
}
