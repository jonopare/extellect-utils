namespace Extellect.Nuvo
{
    public class OnePageOptionsTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(-1, 10, 0)] // Negative page index should be treated as zero
        [InlineData(0, 10, 0)]
        [InlineData(1, 10, 10)]
        [InlineData(0, int.MaxValue, 0)]
        [InlineData(1, int.MaxValue, 0)] // Never skip anything if pageSize is negative
        [InlineData(0, -1, 0)]
        [InlineData(1, -1, 0)] // Never skip anything if pageSize is negative
        public void SkipCount(int pageIndex, int pageSize, int expected)
        {
            var sut = new OnePageOptions(pageIndex, pageSize);

            Assert.Equal(expected, sut.SkipCount);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(-1, 10, 10)] // Negative page index should treated as zero
        [InlineData(0, 10, 10)]
        [InlineData(1, 10, 10)]
        [InlineData(0, int.MaxValue, int.MaxValue)]
        [InlineData(1, int.MaxValue, int.MaxValue)] // Always take the full page even if the muppet asks for a second page
        [InlineData(0, -1, int.MaxValue)]
        [InlineData(1, -1, int.MaxValue)] // Always take the full page even if the muppet asks for a second page
        public void TakeCount(int pageIndex, int pageSize, int expected)
        {
            var sut = new OnePageOptions(pageIndex, pageSize);

            Assert.Equal(expected, sut.TakeCount);
        }

        [Theory]
        [InlineData(10, 1, 1)]
        [InlineData(10, 10, 1)]
        [InlineData(10, 11, 2)]
        public void PageCount_PositiveItemCount_ReturnsPositive(int pageSize, int itemCount, int expected)
        {
            var sut = new OnePageOptions(0, pageSize);
            var actual = sut.PageCount(itemCount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, 0, 0)]
        [InlineData(0, 0, 0)]
        [InlineData(10, 0, 0)]
        [InlineData(int.MaxValue, 0, 0)]
        [InlineData(-1, -1, -1)]
        [InlineData(0, -1, -1)]
        [InlineData(10, -1, -1)]
        [InlineData(int.MaxValue, -1, -1)]
        public void PageCount_ZeroItemCount_ReturnsZero(int pageSize, int itemCount, int expected)
        {
            var sut = new OnePageOptions(0, pageSize);
            var actual = sut.PageCount(itemCount);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1, -2, "itemCount cannot be negative.")]
        [InlineData(0, -2, "itemCount cannot be negative.")]
        [InlineData(10, -2, "itemCount cannot be negative.")]
        public void PageCount_NegativeItemCount_ThrowsArgumentException(int pageSize, int itemCount, string expectedMessage)
        {
            var sut = new OnePageOptions(0, pageSize);
            var caught = Assert.Throws<ArgumentException>(() => sut.PageCount(itemCount));
            Assert.Equal(expectedMessage, caught.Message);
        }
    }
}