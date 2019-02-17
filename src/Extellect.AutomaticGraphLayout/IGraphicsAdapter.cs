using System.Drawing;
using System.Drawing.Drawing2D;

namespace Extellect.AutomaticGraphLayout
{
    public interface IGraphicsAdapter
    {
        bool IsTopZero { get; }
        Matrix Transform { get; set; }

        void DrawLine(Pen pen, Point point1, Point point2);
        void DrawPolygon(Pen pen, Point[] points);
        SizeF MeasureString(string text, Font font);
        void DrawEllipse(Pen pen, RectangleF rectangleF);
        void DrawString(string text, Font font, Brush black, PointF pointF);
        void FillPolygon(Brush brush, PointF[] points);
        void DrawBezier(Pen pen, Point point1, Point point2, Point point3, Point point4);
    }
}
