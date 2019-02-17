using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Extellect.AutomaticGraphLayout
{
    public sealed class GdiPlusGraphicsAdapter : IGraphicsAdapter, IDisposable
    {
        Graphics _graphics;

        public GdiPlusGraphicsAdapter(Graphics graphics)
        {
            _graphics = graphics;
        }

        public Matrix Transform
        {
            get
            {
                return _graphics.Transform;
            }
            set
            {
                _graphics.Transform = value;
            }
        }

        public bool IsTopZero => true;

        public void Dispose()
        {
            if (_graphics != null)
            {
                _graphics.Dispose();
                _graphics = null;
            }
        }

        public void DrawBezier(Pen pen, Point point1, Point point2, Point point3, Point point4)
        {
            _graphics.DrawBezier(pen, point1, point2, point3, point4);
        }

        public void DrawEllipse(Pen pen, RectangleF rectangleF)
        {
            _graphics.DrawEllipse(pen, rectangleF);
        }

        public void DrawLine(Pen pen, Point point1, Point point2)
        {
            _graphics.DrawLine(pen, point1, point2);
        }

        public void DrawPolygon(Pen pen, Point[] points)
        {
            _graphics.DrawPolygon(pen, points);
        }

        public void DrawString(string text, Font font, Brush black, PointF pointF)
        {
            _graphics.DrawString(text, font, black, pointF);
        }

        public void FillPolygon(Brush brush, PointF[] points)
        {
            _graphics.FillPolygon(brush, points);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return _graphics.MeasureString(text, font);
        }
    }
}
