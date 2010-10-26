using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    class RibbonTab : TabPage
    {
        public RibbonTab()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            e.Graphics.Clear(Color.FromArgb(83, 83, 83));

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Color[] col = new Color[] { Color.FromArgb(210, 214, 221), Color.FromArgb(193, 198, 207), Color.FromArgb(180, 187, 197), Color.FromArgb(231, 240, 241) };
            float[] pos = new float[] { 0.0f, 0.2f, 0.2f, 1.0f };

            ColorBlend blend = new ColorBlend();
            blend.Colors = col;
            blend.Positions = pos;
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            brush.InterpolationColors = blend;

            RibbonControl.FillRoundRectangle(e.Graphics, brush, rect, 3f);

            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(190, 190, 190)), rect, 3f);
            rect.Offset(1, 1);
            rect.Width -= 2;
            rect.Height -= 2;
            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect, Color.FromArgb(231, 233, 237), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 3f);
        }
    }
}
