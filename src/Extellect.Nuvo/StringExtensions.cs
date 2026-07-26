using System.Text;

namespace Extellect.Nuvo
{
    public static class StringExtensions
    {
        public static string[] FromPascalCase(this string value)
        {
            if (value.Length == 0)
            {
                return [value];
            }

            var result = new List<string>();
            var boundaries = new List<int>();
            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    boundaries.Add(i);
                }
                else if (char.IsDigit(value[i]) && (i == 0 || !char.IsDigit(value[i - 1])))
                {
                    boundaries.Add(i);
                }
            }

            if (!boundaries.Any() || boundaries[0] != 0)
            {
                boundaries.Insert(0, 0);
            }

            boundaries.Add(value.Length);

            for (var i = 0; i < boundaries.Count - 1; i++)
            {
                var startIndex = boundaries[i];
                var length = boundaries[i + 1] - startIndex;
                result.Add(value.Substring(startIndex, length));
            }

            return result.ToArray();
        }

        public static string ToPascalCase(this string value)
        {
            var result = new StringBuilder();
            foreach (var part in value.Split(new[] { '_', ' ' }).Select(x => x.Trim()).Where(x => x.Length > 0))
            {
                if (part.All(char.IsUpper))
                {
                    result.Append(InitialCap(part));
                }
                else if (part.All(char.IsDigit))
                {
                    result.Append(part);
                }
                else
                {
                    throw new ApplicationException($"Part was neither upper case nor digit: {part}");
                }
            }
            return result.ToString();
        }

        public static string InitialCap(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value[0] + value.Substring(1).ToLowerInvariant();
        }

        public static string? NullIf(this string? value, string? comparison)
        {
            if (value != null && value == comparison)
                return null;
            return value;
        }

        public static string? NullIfNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        public static string? Shuffle(this string? value)
        {
            if (value == null)
                return null;
            var result = value.ToCharArray();
            for (var i = 0; i < result.Length - 1; i += 2)
            {
                var temp = result[i];
                result[i] = result[i + 1];
                result[i + 1] = temp;
            }
            return new string(result);
        }

        public static string? Rot(this string? value, int n = 13)
        {
            if (value == null)
                return null;
            n %= 26;
            if (n < 0)
                n += 26;
            var result = value.ToCharArray();
            for (var c = 0; c < result.Length; c++)
            {
                if (result[c] >= 'A' && result[c] <= 'Z')
                {
                    result[c] = (char)(result[c] + n);
                    if (result[c] > 'Z')
                        result[c] = (char)(result[c] - 26);
                }
                else if (result[c] >= 'a' && result[c] <= 'z')
                {
                    result[c] = (char)(result[c] + n);
                    if (result[c] > 'z')
                        result[c] = (char)(result[c] - 26);
                }
            }
            return new string(result);
        }
    }
}
