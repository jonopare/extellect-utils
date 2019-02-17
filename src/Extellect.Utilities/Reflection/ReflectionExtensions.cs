using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets a property info given a property name expression. This is an attempt at a compile-time verified
        /// use of the property name, rather than resorting to magic strings.
        /// </summary>
        public static PropertyInfo GetProperty<TIn, TOut>(this Type type, Expression<Func<TIn, TOut>> propertyNameExpression)
        {
            var memberExpression = propertyNameExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("propertyNameExpression must be a MemberExpression");
            }
            return type.GetProperty(memberExpression.Member.Name);
        }

        /// <summary>
        /// If the type is nullable, return the underlying type, otherwise returns itself.
        /// </summary>
        public static Type GetNullableUnderlyingType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;
        }

        /// <summary>
        /// Gets the properties with the specified custom attribute defined.
        /// </summary>
        public static IEnumerable<PropertyCustomAttribute<TCustomAttribute>> GetPropertiesWithCustomAttribute<TCustomAttribute>(this Type type, bool inherit)
        {
            foreach (var propertyInfo in type.GetProperties())
            {
                var customAttribute = (TCustomAttribute)propertyInfo.GetCustomAttributes(typeof(TCustomAttribute), inherit).FirstOrDefault();
                if (customAttribute != null)
                    yield return new PropertyCustomAttribute<TCustomAttribute>(propertyInfo, customAttribute);
            }
        }

        /// <summary>
        /// Gets the properties with the specified custom attributes defined.
        /// </summary>
        public static IEnumerable<Tuple<PropertyInfo, IEnumerable<TCustomAttribute>>> GetPropertiesWithCustomAttributes<TCustomAttribute>(this Type type, bool inherit)
        {
            foreach (var propertyInfo in type.GetProperties())
            {
                var customAttributes = propertyInfo.GetCustomAttributes(typeof(TCustomAttribute), inherit).Cast<TCustomAttribute>();
                if (customAttributes.Any())
                    yield return new Tuple<PropertyInfo, IEnumerable<TCustomAttribute>>(propertyInfo, customAttributes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<TCustomAttribute> GetCustomAttributes<TCustomAttribute>(this Type type)
        {
            return type.GetCustomAttributes(typeof(TCustomAttribute), true).Cast<TCustomAttribute>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<TCustomAttribute> GetCustomAttributes<TCustomAttribute>(this MemberInfo memberInfo, bool inherit)
        {
            return memberInfo.GetCustomAttributes(typeof(TCustomAttribute), inherit).Cast<TCustomAttribute>();
        }
    }
}
