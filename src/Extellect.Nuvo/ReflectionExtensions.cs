using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Extellect.Nuvo
{
    public static class ReflectionExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullable(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsClass)
            {
                if (propertyInfo.GetCustomAttribute<RequiredAttribute>() == null)
                {
                    var context = new NullabilityInfoContext();
                    var nullability = context.Create(propertyInfo);
                    return nullability != null
                        && (nullability.ReadState == NullabilityState.Nullable
                            || nullability.WriteState == NullabilityState.Nullable);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return IsNullable(propertyInfo.PropertyType);
            }
        }

        public static Type GetNullableUnderlyingType(this Type type)
        {
            return type.IsNullable() ? Nullable.GetUnderlyingType(type)! : type;
        }

        public static Type GetNullableUnderlyingType(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsClass)
            {
                return propertyInfo.PropertyType;
            }
            else
            {
                return propertyInfo.PropertyType.IsNullable() ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)! : propertyInfo.PropertyType;
            }
        }

        public static bool IsClosedGenericOf(this Type type, Type openGenericType)
        {
            return type.IsGenericType
                && !type.IsGenericTypeDefinition
                && type.GetGenericTypeDefinition() == openGenericType;
        }

        /// <summary>
        /// Finds all concrete types of the given generic interface type that the class (or struct) implements.
        /// </summary>
        public static IEnumerable<Type> FindConcreteImplementations(this Type type, Type openGenericType)
        {
            if (type.IsClosedGenericOf(openGenericType))
            {
                yield return type;
            }
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsClosedGenericOf(openGenericType))
                {
                    yield return interfaceType;
                }
            }
        }
    }
}
