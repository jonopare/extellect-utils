namespace Extellect.Nuvo
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData("2017-12-31", 20171231)]
        public void ToInt32(string dateText, int expected)
        {
            var actual = DateTime.ParseExact(dateText, "yyyy-MM-dd", null).ToInt32();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("2017-12-31 23:59:01", "20171231.235901")]
        public void ToDecimal(string dateText, string expectedText)
        {
            var actual = DateTime.ParseExact(dateText, "yyyy-MM-dd HH:mm:ss", null).ToDecimal();
            var expected = decimal.Parse(expectedText);
            Assert.Equal(expected, actual);
        }
    }
}