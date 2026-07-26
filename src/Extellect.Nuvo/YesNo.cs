namespace Extellect.Nuvo
{
    public struct YesNo
    {
        public const string Yes = "YES";
        public const string No = "NO";

        public bool Value { get; }

        public YesNo(bool value)
        {
            Value = value;
        }

        public static implicit operator YesNo(bool value) => new YesNo(value);

        public static implicit operator bool(YesNo yesNo) => yesNo.Value;

        public static implicit operator bool?(YesNo? yesNo) => yesNo?.Value;

        public static YesNo Parse(string text)
        {
            return Binary.Parse(text, Yes, No);
        }

        public static bool TryParse(string text, out YesNo value)
        {
            var result = Binary.TryParse(text, Yes, No, out bool boolValue);
            value = boolValue;
            return result;
        }

        public override string ToString()
        {
            return Value ? Yes : No;
        }

        public static string ToString(bool value)
        {
            return value ? Yes : No;
        }

        public static string ToString(bool? value)
        {
            return value == null ? string.Empty : (value.Value ? Yes : No);
        }
    }
}
