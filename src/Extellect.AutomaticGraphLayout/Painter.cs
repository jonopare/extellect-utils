using Microsoft.Glee;
using Microsoft.Glee.Splines;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DPoint = System.Drawing.Point;
using GPoint = Microsoft.Glee.Splines.Point;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Drawing.Text;
using PdfSharp;

namespace Extellect.AutomaticGraphLayout
{
    public class Painter
    {
        private readonly Font _font;
        private GleeGraph _gleeGraph;

        public Painter()
        {
            _font = new Font("Segoe UI", 12, FontStyle.Regular);
        }

        //public void Paint<T>(Digraph<T> dependencies, Func<T, string> nameSelector)
        //{
        //    //using (var bitmap = new Bitmap(800, 600))
        //    //{
                
        //    //    bitmap.Save(@"output.png", ImageFormat.Png);
        //    //}

        //    //using (var document = new PdfDocument())
        //    //{
        //    //    document.Info.Title = "GLEE2PDF";
        //    //    document.Info.Author = "Jonathan Pare";

        //    //    document.Info.Subject = "Demonstration of GLEE to PDF generation";
        //    //    document.Info.Keywords = "PDFsharp, GLEE";

        //    //    var page = document.AddPage();
        //    //    page.Orientation = PageOrientation.Landscape;

        //    //    page.Paint(dependencies, nameSelector);
                
        //    //    document.Save(@"output.pdf");
        //    //}
        //}

        //public void Paint<T>(IGraphicsAdapter graphicsAdapter, Digraph<T> dependencies, Func<T, string> nameSelector)
        //{
        //}

        internal void DrawFromGraph(IGraphicsAdapter graphics, RectangleF r)
        {
            SetGraphicsTransform(graphics, r);
            Pen pen = new Pen(Brushes.Black);
            DrawNodes(pen, graphics);
            DrawEdges(pen, graphics);
        }

        private void SetGraphicsTransform(IGraphicsAdapter graphics, RectangleF r)
        {
            var gr = _gleeGraph.BoundingBox;
            if (r.Height > 1 && r.Width > 1)
            {
                var sx = r.Width / (float)gr.Width;
                var sy = r.Height / (float)gr.Height;

                //var rotate = sx < sy ? 0 : (Math.PI / 2);
                //var scale = Math.Max(sx, sy);

                //var rotate = 0d;
                var scale = Math.Min(sx, sy);

                var g0 = (float)(gr.Left + gr.Right) / 2;
                var g1 = (float)(gr.Top + gr.Bottom) / 2;

                var c0 = (r.Left + r.Right) / 2;
                var c1 = (r.Top + r.Bottom) / 2;

                var dx = c0 - scale * g0;
                var dy = c1 - scale * g1;

                graphics.Transform = new Matrix(scale, 0, 0, scale, dx, dy);
                //graphics.Transform = new Matrix((float)Math.Cos(rotate) * scale, (float)Math.Sin(rotate) * scale, (float)Math.Sin(rotate) * -scale, (float)Math.Cos(rotate) * scale, dx, dy);
            }
        }

        private void DrawEdges(Pen pen, IGraphicsAdapter graphics)
        {
            foreach (Edge e in _gleeGraph.Edges)
            {
                DrawEdge(e, pen, graphics);
            }
        }

        private void DrawEdge(Edge e, Pen pen, IGraphicsAdapter graphics)
        {
            if (e.Curve is Curve c)
            {
                foreach (ICurve s in c.Segs)
                {
                    if (s is LineSeg l)
                    {
                        graphics.DrawLine(pen, GleePointToDrawingPoint(l.Start), GleePointToDrawingPoint(l.End));
                    }
                    else if (s is CubicBezierSeg cs)
                    {
                        graphics.DrawBezier(pen, GleePointToDrawingPoint(cs.B(0)), GleePointToDrawingPoint(cs.B(1)), GleePointToDrawingPoint(cs.B(2)), GleePointToDrawingPoint(cs.B(3)));
                    }
                }
                if (e.ArrowHeadAtSource)
                {
                    DrawArrow(e, pen, graphics, e.Curve.Start, e.ArrowHeadAtSourcePosition);
                }
                if (e.ArrowHeadAtTarget)
                {
                    DrawArrow(e, pen, graphics, e.Curve.End, e.ArrowHeadAtTargetPosition);
                }
            }
        }

        private void DrawArrow(Edge e, Pen pen, IGraphicsAdapter graphics, GPoint start, GPoint end)
        {
            PointF[] points;
            float arrowAngle = 30;

            GPoint dir = end - start;
            GPoint h = dir;
            dir /= dir.Length;

            GPoint s = new GPoint(-dir.Y, dir.X);

            s *= h.Length * ((float)Math.Tan(arrowAngle * 0.5f * (Math.PI / 180.0)));

            points = new PointF[] { GleePointToDrawingPoint(start + s), GleePointToDrawingPoint(end), GleePointToDrawingPoint(start - s) };

            graphics.FillPolygon(pen.Brush, points);
        }

        private void DrawNodes(Pen pen, IGraphicsAdapter graphics)
        {
            foreach (Node n in _gleeGraph.NodeMap.Values)
            {
                DrawNode(n, pen, graphics);
            }
        }

        private void DrawNode(Node n, Pen pen, IGraphicsAdapter graphics)
        {
            void drawText(string text, Microsoft.Glee.Splines.Rectangle bbox)
            {
                var textSize = graphics.MeasureString(text, _font);
                var textX = (float)(bbox.Left + bbox.Width / 2 - textSize.Width / 2);
                float textY;
                if (graphics.IsTopZero)
                {
                    textY = (float)(bbox.Top - bbox.Height / 2 - textSize.Height / 2);
                }
                else
                {
                    textY = (float)(bbox.Top - bbox.Height / 2 + textSize.Height / 2);
                }
                graphics.DrawString(text, _font, Brushes.Black, new PointF(textX, textY));
            }

            ICurve curve = n.MovedBoundaryCurve;
            if (curve is Ellipse el)
            {
                graphics.DrawEllipse(pen, new RectangleF((float)el.BBox.Left, (float)el.BBox.Bottom,
                    (float)el.BBox.Width, (float)el.BBox.Height));
                drawText(n.Id, el.BBox);
            }
            else if (curve is Curve c)
            {
                var points = c.Segs.Select(x => x.Start).ToList();
                points.Add(c.Segs.Last().End);

                graphics.DrawPolygon(pen, points.Select(GleePointToDrawingPoint).ToArray());

                drawText(n.Id, c.BBox);
            }
        }

        private DPoint GleePointToDrawingPoint(GPoint point)
        {
            return new DPoint((int)point.X, (int)point.Y);
        }

        internal void CreateAndLayoutGraph<T>(IGraphicsAdapter graphics, Digraph<T> dependencies, Func<T, string> nameSelector)
        {
            _gleeGraph = dependencies.ToGraph(graphics, _font, nameSelector);

            _gleeGraph.CalculateLayout();
        }
    }
}
