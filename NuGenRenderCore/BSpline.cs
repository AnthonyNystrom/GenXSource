using System;
using System.Drawing;

namespace Genetibase.NuGenRenderCore.MathHelpers.Lines
{
    /// <summary>
    /// Encapsulates a set of B-Spline curves
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/B-spline</remarks>
    public class BSpline
    {
        readonly PointF[] points;

        public BSpline(PointF[] points)
        {
            this.points = points;
        }

        public void Draw(Bitmap b, int divisions, Color lineClr, int lineThickness)
        {
            Graphics g = Graphics.FromImage(b);
            Draw(g, divisions, new Pen(lineClr, lineThickness));
            g.Flush();
            g.Dispose();
        }

        public void Draw(Graphics g, int divisions, Color lineClr, int lineThickness)
        {
            Draw(g, divisions, new Pen(lineClr, lineThickness));
        }

        public void Draw(Graphics g, int divisions, Pen pen)
        {
            if (points.Length > 3)
            {
                PointF pt0;
                PointF pt1 = points[0];
                PointF pt2 = points[1];
                PointF pt3 = points[2];
                for (int point = 3; point < points.Length; point++)
                {
                    pt0 = pt1;
                    pt1 = pt2;
                    pt2 = pt3;
                    pt3 = points[point];

                    double temp = Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2F) + Math.Pow(pt2.Y - pt1.Y, 2F));
                    int interpol = (int)temp;

                    // calculate points
                    double[] a = new double[4];
                    double[] b = new double[4];
                    a[0] = (-pt0.X + 3 * pt1.X - 3 * pt2.X + pt3.X) / 6.0;
                    a[1] = (3 * pt0.X - 6 * pt1.X + 3 * pt2.X) / 6.0;
                    a[2] = (-3 * pt0.X + 3 * pt2.X) / 6.0;
                    a[3] = (pt0.X + 4 * pt1.X + pt2.X) / 6.0;
                    b[0] = (-pt0.Y + 3 * pt1.Y - 3 * pt2.Y + pt3.Y) / 6.0;
                    b[1] = (3 * pt0.Y - 6 * pt1.Y + 3 * pt2.Y) / 6.0;
                    b[2] = (-3 * pt0.Y + 3 * pt2.Y) / 6.0;
                    b[3] = (pt0.Y + 4 * pt1.Y + pt2.Y) / 6.0;

                    // draw lines
                    int x = (int)a[3];
                    int y = (int)b[3];

                    for (int i = 1; i <= interpol - 1; i++)
                    {
                        float t = Convert.ToSingle(i) / Convert.ToSingle(divisions);
                        int x2 = (int)((a[2] + t * (a[1] + t * a[0])) * t + a[3]);
                        int y2 = (int)((b[2] + t * (b[1] + t * b[0])) * t + b[3]);

                        g.DrawLine(pen, x, y, x2, y2);
                        x = x2;
                        y = y2;
                    }
                }
            }
        }
    }
}