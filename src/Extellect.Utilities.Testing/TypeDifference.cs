using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Testing
{
    internal class TypeDifference : IDifference
    {
        private readonly Type _expected;
        private readonly Type _actual;

        public string Expected { get { return ToString(_expected); } }

        public string Actual { get { return ToString(_actual); } }

        public TypeDifference(Type expected, Type actual)
        {
            _expected = expected;
            _actual = actual;
        }

        private string ToString(Type value)
        {
            return $"Type={value}";
        }
    }
}
