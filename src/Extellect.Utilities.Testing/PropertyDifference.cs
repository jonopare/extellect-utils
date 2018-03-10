using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Testing
{
    internal class PropertyDifference<T> : IDifference
    {
        private readonly string _propertyName;
        private readonly T _expected;
        private readonly T _actual;

        public string Expected { get { return ToString(_expected); } }

        public string Actual { get { return ToString(_actual); } }

        public PropertyDifference(string propertyName, T expected, T actual)
        {
            _propertyName = propertyName;
            _expected = expected;
            _actual = actual;
        }

        private string ToString(T value)
        {
            return $"{_propertyName}={AssertionHelper.ToString(value)}";
        }
    }
}
