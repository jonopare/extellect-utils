namespace Extellect.Nuvo
{
    public class DecimalExtensionsTests
    {
        [Theory]
        [InlineData("20171231", "2017-12-31")]
        public void ToInt32(string dateText, string expectedText)
        {
            var expected = DateTime.ParseExact(expectedText, "yyyy-MM-dd", null);
            var actual = decimal.Parse(dateText).ToDateTime();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("20171231.235901", "2017-12-31 23:59:01")]
        public void ToDecimal(string dateText, string expectedText)
        {
            var expected = DateTime.ParseExact(expectedText, "yyyy-MM-dd HH:mm:ss", null);
            var actual = decimal.Parse(dateText).ToDateTime();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1e-6, 0)]
        [InlineData(2e-6, 1e-6, 2e-6)]
        [InlineData(-1e-6, 1e-6, 0)]
        [InlineData(-2e-6, 1e-6, -2e-6)]
        public void SnapToZero(decimal value, decimal tolerance, decimal expected)
        {
            var actual = value.SnapToZero(tolerance);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(4.5, 5, 0.49, 4.5)]
        [InlineData(4.5, 5, 0.5, 5)]
        [InlineData(4.5, 5, 0.51, 5)]
        public void SnapTo(decimal value, decimal target, decimal tolerance, decimal expected)
        {
            var actual = value.SnapTo(target, tolerance);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(4.5, 5, 0.49, false)]
        [InlineData(4.5, 5, 0.5, true)]
        [InlineData(4.5, 5, 0.51, true)]
        public void IsWithinToleranceOf(decimal value, decimal target, decimal tolerance, bool expected)
        {
            var actual = value.IsWithinToleranceOf(target, tolerance);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(-1, false)]
        public void IsPositive(decimal value, bool expected)
        {
            var actual = value.IsPositive();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(-1, false)]
        public void IsZero(decimal value, bool expected)
        {
            var actual = value.IsZero();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(-1, true)]
        public void IsNegative(decimal value, bool expected)
        {
            var actual = value.IsNegative();
            Assert.Equal(expected, actual);
        }
    }
}