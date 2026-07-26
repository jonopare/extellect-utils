using Newtonsoft.Json;
using System.Globalization;

namespace Extellect.Nuvo
{
    public class IsoDateOnlyConverter : JsonConverter<DateOnly>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value is string stringValue && DateOnly.TryParseExact(stringValue, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly))
            {
                return dateOnly;
            }
            else if (reader.Value is DateTime dateTimeValue && dateTimeValue.TimeOfDay == TimeSpan.Zero)
            {
                return DateOnly.FromDateTime(dateTimeValue);
            }
            else
            {
                throw new FormatException();
            }
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
