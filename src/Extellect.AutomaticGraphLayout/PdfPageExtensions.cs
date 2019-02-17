using Extellect.AutomaticGraphLayout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Drawing;

namespace Extellect.AutomaticGraphLayout
{
    public static class PdfPageExtensions
    {
        public static void Paint<T>(this PdfPage page, Digraph<T> digraph, Func<T, string> nameSelector)
        {
            using (var graphicsAdapter = new PdfSharpGraphicsAdapter(XGraphics.FromPdfPage(page, XGraphicsUnit.Millimeter)))
            {
                var painter = new Painter();
                painter.CreateAndLayoutGraph(graphicsAdapter, digraph, nameSelector);
                painter.DrawFromGraph(graphicsAdapter, new RectangleF(0, 0, (float)page.Width.Millimeter, (float)page.Height.Millimeter));
            }
        }
    }
}
