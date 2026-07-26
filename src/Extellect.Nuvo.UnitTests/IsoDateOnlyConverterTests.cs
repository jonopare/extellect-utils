using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Extellect.Nuvo
{
    public class IsoDateOnlyConverterTests
    {
        [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class Test
        {
            [JsonConverter(typeof(IsoDateOnlyConverter))]
            public DateOnly Date { get; set; }
        }

        [Theory]
        [InlineData("{ 'date': '2021-12-31' }", 20211231)]
        [InlineData("{ 'date': '2022-12-31T00:00:00' }", 20221231)]
        [InlineData("{ 'date': '2023-12-31T00:00:00Z' }", 20231231)]
        public void Deserialize(string json, int expected)
        {
            var obj = JsonHelper.DeserializeJson<Test>(json);
            Assert.Equal(expected.ToDate(), obj.Date);
        }
    }
}