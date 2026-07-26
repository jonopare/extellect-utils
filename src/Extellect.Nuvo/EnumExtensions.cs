using System.Reflection;
using System.Text;

namespace Extellect.Nuvo
{
    public static class EnumExtensions
    {
        public static Dictionary<string, string> GetAltNames(Type enumType)
        {
            return enumType.GetFields()
                .SelectMany(x => ((AltNameAttribute[])x.GetCustomAttributes(typeof(AltNameAttribute), false)).SelectMany(y => y.AltNames.Select(z => new { AltName = z, x.Name })))
                .ToDictionary(x => x.AltName, x => x.Name);
        }

        /// <summary>
        /// Gets alternative names for an Enum value. This is useful when PIE uses a code that doesn't translate
        /// into a C# enum name.
        /// </summary>
        public static IEnumerable<string> GetAltNames<T>(this T value) where T : struct
        {
            var field = GetField(value);
            if (field != null)
            {
                foreach (AltNameAttribute altNameAttribute in field.GetCustomAttributes(typeof(AltNameAttribute), false))
                {
                    foreach (var altName in altNameAttribute.AltNames)
                    {
                        yield return altName;
                    }
                }
            }
        }

        public static bool TryParseWithAltNames<T>(this string value, out T result) where T : struct
        {
            if (!Enum.TryParse(value, out result))
            {
                if (GetAltNames(typeof(T)).TryGetValue(value, out var next))
                {
                    if (!Enum.TryParse(next, out result))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets an English language list of names for the specified enum.
        /// </summary>
        public static string GetNamesInEnglish<T>(this T _, bool quote = true) where T : struct
        {
            return GetNamesInEnglish(typeof(T));
        }

        public static string GetNamesInEnglish(this Type enumType, bool quote = true)
        {
            var names = Enum.GetNames(enumType);
            var text = new StringBuilder();
            for (var i = 0; i < names.Length; i++)
            {
                if (i > 0)
                {
                    if (i == names.Length - 1)
                    {
                        text.Append(" or ");
                    }
                    else
                    {
                        text.Append(", ");
                    }
                }
                if (quote)
                    text.Append('\'');
                text.Append(names[i]);
                if (quote)
                    text.Append('\'');
            }
            return text.ToString();
        }

        public static bool IsObsolete<T>(this T value) where T : struct
        {
            var field = GetField(value);
            if (field != null)
            {
                return field.GetCustomAttribute(typeof(ObsoleteAttribute), false) != null;
            }
            return false;
        }

        private static FieldInfo? GetField<T>(this T value) where T : struct
        {
            var name = value.ToString();
            if (name != null)
            {
                return typeof(T).GetField(name);
            }
            return null;
        }
    }
}
