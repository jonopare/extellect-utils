﻿using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Extellect.Drawing
{
    [SupportedOSPlatform("windows")]
    public class HslTests
    {
        [Fact(Skip = "This is only for visual confirmation")]
        public void Draw()
        {
            using (var bitmap = new Bitmap(640, 480))
            {
                var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                var buffer = new byte[data.Stride * data.Height];

                for (var y = 0; y < bitmap.Height; y++)
                {
                    var l = y / (float)bitmap.Height;
                    for (var x = 0; x < bitmap.Width; x++)
                    {
                        var h = x / (float)bitmap.Width;
                        var o = y * data.Stride + x * 4;
                        var c = Hsl.FromHsl(h, 1f, l);
                        buffer[o + 0] = c.B;
                        buffer[o + 1] = c.G;
                        buffer[o + 2] = c.R;
                        buffer[o + 3] = c.A;
                    }
                }

                Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);

                bitmap.UnlockBits(data);

                bitmap.Save(@"hsl.png", ImageFormat.Png);
            }
        }

        [Fact]
        public void Red_RoundTrip_Red()
        {
            var expected = Color.FromArgb(255, 0, 0);
            var hsl = expected.ToHslVector();
            Assert.Equal(expected.ToArgb(),
                Hsl.FromHsl(hsl.X, hsl.Y, hsl.Z).ToArgb());
        }

        [Fact]
        public void Green_RoundTrip_Green()
        {
            var expected = Color.FromArgb(0, 255, 0);
            var hsl = expected.ToHslVector();
            Assert.Equal(expected.ToArgb(),
                Hsl.FromHsl(hsl.X, hsl.Y, hsl.Z).ToArgb());
        }

        [Fact]
        public void Blue_RoundTrip_Blue()
        {
            var expected = Color.FromArgb(0, 0, 255);
            var hsl = expected.ToHslVector();
            Assert.Equal(expected.ToArgb(),
                Hsl.FromHsl(hsl.X, hsl.Y, hsl.Z).ToArgb());
        }

        [Fact]
        public void Red()
        {
            var hsl = Color.FromArgb(255, 0, 0).ToHslVector();
            Assert.Equal(new Vector3(0, 1, 0.5f), hsl);
        }

        [Fact]
        public void Yellow()
        {
            var hsl = Color.FromArgb(255, 255, 0).ToHslVector();
            Assert.Equal(new Vector3(1 / 6f, 1, 0.5f), hsl);
        }

        [Fact]
        public void Green()
        {
            var hsl = Color.FromArgb(0, 255, 0).ToHslVector();
            Assert.Equal(new Vector3(1 / 3f, 1, 0.5f), hsl);
        }

        [Fact]
        public void Cyan()
        {
            var hsl = Color.FromArgb(0, 255, 255).ToHslVector();
            Assert.Equal(new Vector3(1 / 2f, 1, 0.5f), hsl);
        }

        [Fact]
        public void Blue()
        {
            var hsl = Color.FromArgb(0, 0, 255).ToHslVector();
            Assert.Equal(new Vector3(2 / 3f, 1, 0.5f), hsl);
        }

        [Fact]
        public void Magenta()
        {
            var hsl = Color.FromArgb(255, 0, 255).ToHslVector();
            Assert.Equal(new Vector3(5 / 6f, 1, 0.5f), hsl);
        }

        [Fact]
        public void White()
        {
            var hsl = Color.FromArgb(255, 255, 255).ToHslVector();
            Assert.Equal(new Vector3(0, 0, 1), hsl);
        }

        [Fact]
        public void Black()
        {
            var hsl = Color.FromArgb(0, 0, 0).ToHslVector();
            Assert.Equal(new Vector3(0, 0, 0), hsl);
        }
    }
}