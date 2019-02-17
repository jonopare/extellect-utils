using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extellect
{
    public class PropertyComparison<T>
    {
        private readonly PropertyInfo[] _properties;

        public PropertyComparison(PropertyInfo[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException();
            }

            if (!properties.Any())
            {
                throw new ArgumentException();
            }

            _properties = properties;
        }

        public PropertyComparisonResult AreEqual(T expected, T actual)
        {
            foreach (var property in _properties)
            {
                var expectedValue = property.GetValue(expected);
                var actualValue = property.GetValue(actual);

                if (expectedValue == null)
                {
                    if (actualValue != null)
                    {
                        return PropertyComparisonResult.FromFailure(property.Name, expectedValue, actualValue);
                    }
                }
                else
                {
                    if (actualValue == null)
                    {
                        return PropertyComparisonResult.FromFailure(property.Name, expectedValue, actualValue);
                    }
                    else if (!expectedValue.Equals(actualValue))
                    {
                        return PropertyComparisonResult.FromFailure(property.Name, expectedValue, actualValue);
                    }
                }
            }
            return PropertyComparisonResult.FromSuccess();
        }
    }
}
