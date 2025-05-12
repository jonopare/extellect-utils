using System.Drawing;
using System.Numerics;
using System.Runtime.Versioning;

namespace Extellect.Drawing
{
    [SupportedOSPlatform("windows")]
    public class ColorExtensionsTests
    {
        [Fact]
        public void ToRgbVector()
        {
            var color = Color.FromArgb(1, 2, 3, 4);
            var rgb = color.ToRgbVector();
            Assert.Equal(new Vector3(0.007843138f, 0.011764707f, 0.015686275f), rgb);
        }

        [Fact]
        public void ToHslVector()
        {
            var color = Color.FromArgb(1, 2, 3, 4);
            var hsl = color.ToHslVector();
            Assert.Equal(Hsl.ToHslVector(color), hsl);
        }
    }
}