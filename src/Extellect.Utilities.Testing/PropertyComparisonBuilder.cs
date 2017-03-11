using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities
{
    public class PropertyComparisonBuilder<T>
    {
        private readonly List<PropertyInfo> _properties;

        public PropertyComparisonBuilder()
        {
            _properties = new List<PropertyInfo>();
        }

        public PropertyComparisonBuilder<T> WithPublicInstanceGetProperties()
        {
            foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                if (!_properties.Contains(property))
                {
                    _properties.Add(property);
                }
            }
            return this;
        }

        public PropertyComparisonBuilder<T> WithProperty(string name)
        {
            var property = typeof(T).GetProperty(name);
            
            if (!_properties.Contains(property))
            {
                _properties.Add(property);
            }

            return this;
        }

        public PropertyComparisonBuilder<T> WithProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return WithProperty(((MemberExpression)propertyExpression.Body).Member.Name);
        }

        public PropertyComparison<T> Build()
        {
            return new PropertyComparison<T>(_properties.ToArray());
        }
    }
}
