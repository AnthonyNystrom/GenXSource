using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IAP_App
{
    static class Program
    {
        public static PictureBox ImageBox = null;
        public static PictureBox HistogramBox = null;
        public static TextBox SelectionBox = null;
        public static TextBox RangeBox = null;
        public static Label SizeBox = null;
        public static Label ZoomBox = null;
        public static double ZoomScale = 1.0;

        public static void DrawRoundedRectangle(Graphics g, Brush brush, Pen pen, Rectangle rect, float radiusx, float radiusy)
        {
            float rx = Math.Min(radiusx * 2, rect.Width);
            float ry = Math.Min(radiusy * 2, rect.Height);

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rect.X, rect.Y, rx, ry, 180, 90);
            gp.AddArc(rect.X + rect.Width - rx, rect.Y, rx, ry, 270, 90);
            gp.AddArc(rect.X + rect.Width - rx, rect.Y + rect.Height - ry, rx, ry, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - ry, rx, ry, 90, 90);
            gp.CloseFigure();
            if (brush != null)
                g.FillPath(brush, gp);
            if (pen != null)
                g.DrawPath(pen, gp);
            gp.Dispose();
        }
        public static void DrawTopRoundedRectangle(Graphics g, Brush brush, Pen pen, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddLine(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, rectangle.X, rectangle.Y + rectangle.Height);
            gp.CloseFigure();
            if (brush != null)
                g.FillPath(brush, gp);
            if (pen != null)
                g.DrawPath(pen, gp);
            gp.Dispose();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }
    }
}