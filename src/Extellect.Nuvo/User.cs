using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Extellect.Nuvo
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class User
    {
        public Guid UserID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
