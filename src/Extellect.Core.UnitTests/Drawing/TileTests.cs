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

        [Fact]
        public void Crop()
        {
            var random = new Random(1_234_567_890);
            var sut = new Tile(10, 5);
            for (var y = 0; y < sut.Height; y++)
            {
                for (var x = 0; x < sut.Width; x++)
                {
                    sut[x, y] = Color.FromArgb(random.Next());
                }
            }
            var crop = sut.Crop(new Rectangle(2, 1, 6, 3));
            Assert.Equal(6, crop.Width);
            Assert.Equal(3, crop.Height);
            for (var y = 0; y < crop.Height; y++)
            {
                var ey = 1 + y;
                for (var x = 0; x < crop.Width; x++)
                {
                    var ex = 2 + x;
                    Assert.Equal(sut[ex, ey], crop[x, y]);
                }
            }
        }
    }
}
