using System.Collections;
using System.Reflection;

namespace Extellect.Nuvo
{
    /// <summary>
    /// A DeepPropertyComparer compares the properties of two objects (of the same type) recursively.
    /// If any of the given ISimilarity has a matching ComparandType to the property type then
    /// that ISimilarity is used to compare the properties.
    /// Otherwise if the property is IEnumerable&lt;T&gt; then the two values will be compared for 
    /// sequence equality using this same recursive definition.
    /// Otherwise the property values are compared recursively.
    /// </summary>
    public class DeepPropertyComparer
    {
        private readonly static HashSet<Type> _primitiveTypes =
            [
                typeof(string),
                typeof(int),
                typeof(double),
                typeof(float),
                typeof(bool),
                typeof(char),
                typeof(DateTime),
                typeof(DateOnly),
                typeof(Guid),
            ];

        private readonly DeepPropertyComparisonOptions _options;
        private readonly Dictionary<Type, ISimilarity> _similarities;
        private readonly List<string> _differences;

        public string DifferenceText => string.Join(Environment.NewLine, _differences);
        public List<string> Differences => _differences;

        public DeepPropertyComparer(params ISimilarity[] similarities)
            : this(DeepPropertyComparisonOptions.None, similarities)
        {
        }

        public DeepPropertyComparer(DeepPropertyComparisonOptions options, params ISimilarity[] similarities)
        {
            _options = options;
            _similarities = similarities.ToDictionary(x => x.ComparandType);
            _differences = new List<string>();
        }

        public bool AreSimilar<TObject>(TObject x, TObject y)
        {
            return AreSimilar(typeof(TObject), x, y, new Stack<string>());
        }

        private bool AreSimilar(Type propertyType, object? x, object? y, Stack<string> context)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x != null && y != null)
            {
                if (_similarities.TryGetValue(propertyType, out var similarity))
                {
                    if (!similarity.AreSimilar(x, y))
                    {
                        _differences.Add($"{BuildContext(context)}: {x} != {y}");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (_options.HasFlag(DeepPropertyComparisonOptions.Primitives) && _primitiveTypes.Contains(propertyType))
                {
                    if (!Equals(x, y))
                    {
                        _differences.Add($"{BuildContext(context)}: {x} != {y}");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (_options.HasFlag(DeepPropertyComparisonOptions.Enumerables) && typeof(IEnumerable).IsAssignableFrom(propertyType))
                {
                    var elementType = GetSequenceElementType(propertyType) ?? typeof(object);
                    var enumerableX = (IEnumerable)x;
                    var enumerableY = (IEnumerable)y;
                    IEnumerator? ex = null;
                    IEnumerator? ey = null;
                    try
                    {
                        ex = enumerableX.GetEnumerator();
                        ey = enumerableY.GetEnumerator();
                        for (var index = 0; ; index++)
                        {
                            var moveX = ex.MoveNext();
                            var moveY = ey.MoveNext();
                            if (moveX != moveY)
                            {
                                _differences.Add($"{BuildContext(context)}: sequences differ in length (compared {index} item(s))");
                                return false;
                            }
                            if (!moveX)
                            {
                                return true;
                            }
                            context.Push($"[{index}]");
                            var itemMatch = AreSimilar(elementType, ex.Current, ey.Current, context);
                            context.Pop();
                            if (!itemMatch)
                            {
                                return false;
                            }
                        }
                    }
                    finally
                    {
                        (ex as IDisposable)?.Dispose();
                        (ey as IDisposable)?.Dispose();
                    }
                }
                else
                {
                    var result = true;
                    foreach (var property in propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(IsComparable))
                    {
                        var xp = property.GetValue(x);
                        var yp = property.GetValue(y);

                        context.Push(property.Name);
                        result &= AreSimilar(property.PropertyType, xp, yp, context);
                        context.Pop();
                    }
                    return result;
                }
            }
            else
            {
                _differences.Add($"{BuildContext(context)}: exactly one of X or Y is null");
                return false;
            }
        }

        private Type? GetSequenceElementType(Type propertyType)
        {
            if (propertyType.IsArray)
            {
                return propertyType.GetElementType();
            }
            return propertyType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();
        }

        private bool IsComparable(PropertyInfo property)
        {
            // Don't try and compare properties that are indexed
            return property.GetIndexParameters().Length == 0;
        }

        private static string BuildContext(Stack<string> context) => string.Join(".", context.Reverse());
    }
}
