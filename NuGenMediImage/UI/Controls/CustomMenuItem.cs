using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public partial class CustomMenuItem : System.Windows.Forms.ToolStripMenuItem
    {
        private bool isflat;
        private bool ispressed, hover, pressed;
        private NuGenMediImageCtrl ngMediImage;

        public NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set { ngMediImage = value; }
        }


        public static void RenderSelection(Graphics g, Rectangle rectangle, float radius, bool pressed)
        {
            if (pressed)
            {
                Color[] col = new Color[] { Color.FromArgb(254, 216, 170), Color.FromArgb(251, 181, 101), Color.FromArgb(250, 157, 52), Color.FromArgb(253, 238, 170) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillRoundRectangle(g, brush, rectangle, 2f);

                DrawRoundRectangle(g, new Pen(Color.FromArgb(171, 161, 140)), rectangle, radius);
                rectangle.Offset(1, 1);
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                DrawRoundRectangle(g, new Pen(new LinearGradientBrush(rectangle, Color.FromArgb(223, 183, 136), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rectangle, radius);
            }
            else
            {
                Color[] col = new Color[] { Color.FromArgb(255, 254, 227), Color.FromArgb(255, 231, 151), Color.FromArgb(255, 215, 80), Color.FromArgb(255, 231, 150) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillRoundRectangle(g, brush, rectangle, 2f);

                DrawRoundRectangle(g, new Pen(Color.FromArgb(210, 192, 141)), rectangle, radius);
                rectangle.Offset(1, 1);
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                DrawRoundRectangle(g, new Pen(new LinearGradientBrush(rectangle, Color.FromArgb(255, 255, 247), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rectangle, 2f);
            }
        }

        public static void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y + rectangle.Height - size, size, size, 0, 90);
            gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - size, size, size, 90, 90);
            gp.CloseFigure();
            g.DrawPath(pen, gp);
            gp.Dispose();
        }

        public CustomMenuItem()
        {
            this.Margin = new Padding(1);
            this.Padding = new Padding(2);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.isflat = true;
            this.ispressed = false;

            InitializeComponent();
        }

        public static void FillRoundRectangle(Graphics g, Brush brush, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y + rectangle.Height - size, size, size, 0, 90);
            gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - size, size, size, 90, 90);
            gp.CloseFigure();
            g.FillPath(brush, gp);
            gp.Dispose();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.pressed = true;
            this.Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.pressed = false;
            this.Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this.hover = true;
            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.hover = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, this.Width - 6, this.Height - 1);

            if (this.hover)
            {
                RenderSelection(e.Graphics, rect, 2f, this.pressed);
            }
            else if (this.ispressed)
            {
                RenderSelection(e.Graphics, rect, 2f, true);
            }
            else if (!this.isflat)
            {
                Color[] col = new Color[] { ngMediImage.GetColorConfig().TabPageColor1, ngMediImage.GetColorConfig().TabPageColor2, ngMediImage.GetColorConfig().TabPageColor3, ngMediImage.GetColorConfig().TabPageColor4 };
                float[] pos = new float[] { 0.0f, 0.2f, 0.2f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillRoundRectangle(e.Graphics, brush, rect, 2f);

                DrawRoundRectangle(e.Graphics, new Pen(ngMediImage.GetColorConfig().Color6), rect, 2f);
                rect.Offset(1, 1);
                rect.Width -= 2;
                rect.Height -= 2;
                DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect, ngMediImage.GetColorConfig().TabPageBorderColor, Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 2f);
            }

            //base.OnPaint(e);
        }
    }
}
