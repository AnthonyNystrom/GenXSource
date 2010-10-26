using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    class RibbonGallery : FlowLayoutPanel
    {
        public RibbonGallery()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Margin = new Padding(1);
            this.Padding = new Padding(1);
            this.Size = new Size(100, 50);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            e.Graphics.Clear(Color.FromArgb(218, 226, 226));
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(172, 172, 172)), rect);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            this.Invalidate();

            base.OnScroll(se);
        }
    }
}
