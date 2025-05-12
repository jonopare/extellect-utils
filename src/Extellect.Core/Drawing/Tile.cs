using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Extellect.Drawing
{
    public class Tile
    {
        public int Width { get; }
        public int Height { get; }
        public Color[] Pixels { get; }

        public Tile(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new Color[width * height];
        }

        public Color GetPixel(int x, int y)
        {
            return Pixels[IndexOf(x, y)];
        }

        public void SetPixel(int x, int y, Color value)
        {
            Pixels[IndexOf(x, y)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y));
            return y * Width + x;
        }

        [SupportedOSPlatform("windows")]
        public static Tile FromBitmap(Bitmap bitmap)
        {
            var bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            return FromBitmap(bitmap, bounds);
        }

        [SupportedOSPlatform("windows")]
        public static Tile FromBitmap(Bitmap bitmap, Rectangle bounds)
        {
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                if (!BitConverter.IsLittleEndian)
                    throw new NotSupportedException();
                var data = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                try
                {
                    var raw = new byte[data.Stride * data.Height];
                    Marshal.Copy(data.Scan0, raw, 0, raw.Length);
                    var tile = new Tile(data.Width, data.Height);
                    for (var y = 0; y < data.Height; y++)
                    {
                        var m = y * data.Stride;
                        for (var x = 0; x < data.Width; x++)
                        {
                            var n = m + x * 4;
                            var pixel = Color.FromArgb(raw[n + 3], raw[n + 2], raw[n + 1], raw[n]); // Little endian
                            tile.SetPixel(x, y, pixel);
                        }
                    }
                    return tile;
                }
                finally
                {
                    bitmap.UnlockBits(data);
                }
            }
            else
            {
                using var copy = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
                var copyBounds = new Rectangle(0, 0, bounds.Width, bounds.Height);
                using (var graphics = Graphics.FromImage(copy))
                {
                    graphics.DrawImage(bitmap, copyBounds, bounds, GraphicsUnit.Pixel);
                }
                return FromBitmap(copy, copyBounds);
            }
        }

        [SupportedOSPlatform("windows")]
        public Bitmap ToBitmap()
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            if (!BitConverter.IsLittleEndian)
                throw new NotSupportedException();
            var data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            try
            {
                var raw = new byte[data.Stride * data.Height];
                var tile = new Tile(data.Width, data.Height);
                for (var y = 0; y < data.Height; y++)
                {
                    var m = y * data.Stride;
                    for (var x = 0; x < data.Width; x++)
                    {
                        var n = m + x * 4;
                        var pixel = GetPixel(x, y);
                        // Little endian
                        raw[n + 3] = pixel.A;
                        raw[n + 2] = pixel.R;
                        raw[n + 1] = pixel.G;
                        raw[n] = pixel.B;
                    }
                }
                Marshal.Copy(raw, 0, data.Scan0, raw.Length);
                return bitmap;
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        public Tile Crop(Rectangle bounds)
        {
            if (bounds.Left < 0 || bounds.Left + bounds.Width > Width)
                throw new ArgumentException();
            if (bounds.Top < 0 || bounds.Top + bounds.Height > Height)
                throw new ArgumentException();
            var tile = new Tile(bounds.Width, bounds.Height);
            for (var dy = 0; dy < bounds.Height; dy++)
            {
                var sy = bounds.Top + dy;
                for (var dx = 0; dx < bounds.Width; dx++)
                {
                    var sx = bounds.Left + dx;
                    tile[dx, dy] = this[sx, sy];
                }
            }
            return tile;
        }

        public Color this[int x, int y]
        {
            get
            {
                return GetPixel(x, y);
            }
            set
            {
                SetPixel(x, y, value);
            }
        }
    }
}
