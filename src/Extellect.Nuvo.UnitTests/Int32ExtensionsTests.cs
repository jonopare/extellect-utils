namespace Extellect.Nuvo
{
    public class Int32ExtensionsTests
    {
        [Theory]
        [InlineData(20190123, "2019-01-23")]
        [InlineData(17901231, "1790-12-31")]
        public void ToDate(int value, string expected)
        {
            var actual = value.ToDate().ToString("yyyy-MM-dd");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(201901, "20:19:01")]
        [InlineData(2, "00:00:02")]
        public void ToTime(int value, string expected)
        {
            var actual = value.ToTime().ToString("HH:mm:ss");
            Assert.Equal(expected, actual);
        }
    }
}