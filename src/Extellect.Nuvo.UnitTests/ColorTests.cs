namespace Extellect.Nuvo
{
    public class ColorTests
    {
        [Theory]
        [InlineData(1, 2, 3, "#010203")]
        [InlineData(13, 14, 15, "#0d0e0f")]
        [InlineData(16, 17, 18, "#101112")]
        [InlineData(253, 254, 255, "#fdfeff")]
        public void FromRgb_ToHex(byte r, byte g, byte b, string expected)
        {
            var sut = Color.FromRgb(r, g, b);
            var actual = sut.ToHex();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 3, "rgb(1,2,3)")]
        [InlineData(253, 254, 255, "rgb(253,254,255)")]
        public void FromRgb_ToRgb(byte r, byte g, byte b, string expected)
        {
            var sut = Color.FromRgb(r, g, b);
            var actual = sut.ToRgb();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 2, 3, 253, 254, 255, 0d, 1, 2, 3)]
        [InlineData(1, 2, 3, 253, 254, 255, 1d, 253, 254, 255)]
        [InlineData(1, 2, 3, 253, 254, 255, 0.5, 127, 128, 129)]
        [InlineData(1, 2, 3, 253, 254, 255, -0.001, 0, 1, 2)]
        [InlineData(1, 2, 3, 253, 254, 255, 1.005, 254, 255, 255)]
        public void Interpolate(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2, double factor, byte r3, byte g3, byte b3)
        {
            var sut = Color.FromRgb(r1, g1, b1);
            var other = Color.FromRgb(r2, g2, b2);
            var actual = sut.Interpolate(other, factor);
            var expected = Color.FromRgb(r3, g3, b3);
            Assert.Equal(expected, actual);
        }
    }
}
