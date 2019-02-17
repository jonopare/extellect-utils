using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect
{
    public class PropertyComparisonResult
    {
        private PropertyComparisonResult(bool success, string propertyName, object expected, object actual)
        {
            Success = success;
            PropertyName = propertyName;
            Expected = expected;
            Actual = actual;
        }

        public static PropertyComparisonResult FromSuccess()
            => new PropertyComparisonResult(true, null, null, null);

        public bool Success { get; }

        public string PropertyName { get; }

        public object Expected { get; }

        public object Actual { get; }

        public static PropertyComparisonResult FromFailure(string propertyName, object expected, object actual) 
            => new PropertyComparisonResult(false, propertyName, expected, actual);
    }
}
