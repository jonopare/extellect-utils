using Newtonsoft.Json.Converters;

namespace Extellect.Nuvo
{
    public class IsoDateConverter : IsoDateTimeConverter
    {
        public IsoDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
