using Extellect.AutomaticGraphLayout;
using Extellect.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.AutomaticGraphLayout
{
    public static class BitmapExtensions
    {
        public static void Paint<T>(this Bitmap bitmap, Digraph<T> digraph, Func<T, string> nameSelector)
        {
            var font = new Font("Segoe UI", 12, FontStyle.Regular);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                var graphicsAdapter = new GdiPlusGraphicsAdapter(graphics);

                var painter = new Painter();
                painter.CreateAndLayoutGraph(graphicsAdapter, digraph, nameSelector);
                painter.DrawFromGraph(graphicsAdapter, new RectangleF(0, 0, bitmap.Width, bitmap.Height));
            }
        }
    }
}
