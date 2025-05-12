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
    }
}
