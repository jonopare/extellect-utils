namespace Extellect.Nuvo
{
    public static class Binary
    {
        public static bool TryParse(string? value, string trueValue, string falseValue, out bool result, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (value != null)
            {
                if (value.Equals(trueValue, comparisonType))
                {
                    result = true;
                    return true;
                }
                else if (value.Equals(falseValue, comparisonType))
                {
                    result = false;
                    return true;
                }
            }
            result = false;
            return false;
        }

        public static bool Parse(string? value, string trueValue, string falseValue, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            else if (!TryParse(value, trueValue, falseValue, out var result, comparisonType))
            {
                throw new FormatException($"Value must be either '{trueValue}' or '{falseValue}'");
            }
            else
            {
                return result;
            }
        }
    }
}
