namespace Extellect.Nuvo
{
    public class DoubleExtensionsTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1e-6, 0)]
        [InlineData(2e-6, 1e-6, 2e-6)]
        [InlineData(-1e-6, 1e-6, 0)]
        [InlineData(-2e-6, 1e-6, -2e-6)]
        public void SnapToZero(double value, double tolerance, double expected)
        {
            var actual = value.SnapToZero(tolerance);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(4.5, 5, 0.49, 4.5)]
        [InlineData(4.5, 5, 0.5, 5)]
        [InlineData(4.5, 5, 0.51, 5)]
        public void SnapTo(double value, double target, double tolerance, double expected)
        {
            var actual = value.SnapTo(target, tolerance);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(4.5, 5, 0.49, false)]
        [InlineData(4.5, 5, 0.5, true)]
        [InlineData(4.5, 5, 0.51, true)]
        public void IsWithinToleranceOf(double value, double target, double tolerance, bool expected)
        {
            var actual = value.IsWithinToleranceOf(target, tolerance);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(-1, false)]
        public void IsPositive(double value, bool expected)
        {
            var actual = value.IsPositive();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(-1, false)]
        public void IsZero(double value, bool expected)
        {
            var actual = value.IsZero();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(-1, true)]
        public void IsNegative(double value, bool expected)
        {
            var actual = value.IsNegative();
            Assert.Equal(expected, actual);
        }
    }
}