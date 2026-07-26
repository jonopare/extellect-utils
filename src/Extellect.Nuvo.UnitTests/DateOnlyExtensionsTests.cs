namespace Extellect.Nuvo
{
    public class DateOnlyExtensionsTests
    {
        [Theory]
        [InlineData("2017-12-31", 20171231)]
        public void ToInt32(string dateText, int expected)
        {
            var actual = DateOnly.ParseExact(dateText, "yyyy-MM-dd", null).ToInt32();
            Assert.Equal(expected, actual);
        }
    }
}