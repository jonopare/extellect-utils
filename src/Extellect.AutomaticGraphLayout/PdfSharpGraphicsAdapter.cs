using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.AutomaticGraphLayout
{
    public sealed class PdfSharpGraphicsAdapter : IGraphicsAdapter, IDisposable
    {
        private XGraphics _graphics;

        public PdfSharpGraphicsAdapter(XGraphics graphics)
        {
            _graphics = graphics;
        }

        public Matrix Transform
        {
            get
            {
                return D(_graphics.Transform);
            }
            set
            {
                _graphics.TranslateTransform(value.Elements[4], value.Elements[5]);
                _graphics.ScaleTransform(value.Elements[0], value.Elements[3]);
                //_graphics.ShearTransform(value.Elements[1], value.Elements[2]);
            }
        }

        public bool IsTopZero => false;

        public void Dispose()
        {
            if (_graphics != null)
            {
                _graphics.Dispose();
                _graphics = null;
            }
        }

        private Matrix D(XMatrix matrix) => new Matrix((float)matrix.M11, (float)matrix.M12, (float)matrix.M21, (float)matrix.M22, (float)matrix.OffsetX, (float)matrix.OffsetY);
        private XColor X(Color color) => XColor.FromArgb(color.ToArgb());
        private XPen X(Pen pen) => new XPen(X(pen.Color));
        private XBrush X(Brush brush) => new XSolidBrush(X(((SolidBrush)brush).Color));
        private XPoint X(PointF point) => new XPoint(point.X, point.Y);
        private XRect X(RectangleF rectangle) => new XRect(X(rectangle.Location), X(rectangle.Size));
        private XSize X(SizeF size) => new XSize(size.Width, size.Height);
        private XFont X(Font font) => new XFont(font.Name, font.Size, X(font.Style));
        private XFontStyle X(FontStyle fontStyle) => XFontStyle.Regular;
        private SizeF D(XSize size) => new SizeF((float)size.Width, (float)size.Height);

        public void DrawBezier(Pen pen, Point point1, Point point2, Point point3, Point point4)
        {
            _graphics.DrawBezier(X(pen), X(point1), X(point2), X(point3), X(point4));
        }

        public void DrawEllipse(Pen pen, RectangleF rectangleF)
        {
            _graphics.DrawEllipse(X(pen), X(rectangleF));
        }

        public void DrawLine(Pen pen, Point point1, Point point2)
        {
            _graphics.DrawLine(X(pen), X(point1), X(point2));
        }

        public void DrawPolygon(Pen pen, Point[] points)
        {
            _graphics.DrawPolygon(X(pen), points.Select(x => X(x)).ToArray());
        }

        public void DrawString(string text, Font font, Brush brush, PointF pointF)
        {
            var xFont = X(font);
            
            var offset = _graphics.MeasureString(text, xFont).Height * xFont.FontFamily.GetCellDescent(xFont.Style) / xFont.FontFamily.GetLineSpacing(xFont.Style);

            var xPoint = X(pointF);
            xPoint = new XPoint(xPoint.X, xPoint.Y - offset);

            _graphics.DrawString(text, xFont, X(brush), xPoint);
        }

        public void FillPolygon(Brush brush, PointF[] points)
        {
            _graphics.DrawPolygon(X(brush), points.Select(X).ToArray(), XFillMode.Alternate);
        }

        public SizeF MeasureString(string text, Font font)
        {
            var xFont = X(font);
            var size = D(_graphics.MeasureString(text, xFont));
            return size;
        }
    }
}
