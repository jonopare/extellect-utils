using System.Drawing;

namespace Extellect.Drawing
{
    public class TileTests
    {
        [Theory]
        [InlineData(3, 2, 3, 0)]
        [InlineData(3, 2, 0, 2)]
        [InlineData(3, 2, -1, 0)]
        [InlineData(3, 2, 0, -1)]
        public void GetPixel_OutOfRange_Throws(int width, int height, int x, int y)
        {
            var sut = new Tile(width, height);
            var caught = Assert.Throws<ArgumentOutOfRangeException>(() => sut.GetPixel(x, y));
        }

        [Fact]
        public void GetPixel_WithinRange_ReturnsColor()
        {
            const int x = 2;
            const int y = 1;
            var expected = Color.Blue;
            var sut = new Tile(3, 2);
            sut.Pixels[sut.IndexOf(x, y)] = expected;
            var pixel = sut.GetPixel(x, y);
            Assert.Equal(expected, pixel);
        }

        [Fact]
        public void SetPixel_WithinRange_SetsColor()
        {
            const int x = 2;
            const int y = 1;
            var expected = Color.Blue;
            var sut = new Tile(3, 2);
            sut.SetPixel(x, y, expected);
            var pixel = sut.Pixels[sut.IndexOf(x, y)];
            Assert.Equal(expected, pixel);
        }
    }
}
