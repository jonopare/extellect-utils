using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Testing
{
    internal class ValueDifference : IDifference
    {
        private readonly object _expected;
        private readonly object _actual;

        public string Expected { get { return ToString(_expected); } }

        public string Actual { get { return ToString(_actual); } }

        public ValueDifference(object expected, object actual)
        {
            _expected = expected;
            _actual = actual;
        }
        private string ToString(object value)
        {
            return $"{AssertionHelper.ToString(value)}";
        }
    }
}
