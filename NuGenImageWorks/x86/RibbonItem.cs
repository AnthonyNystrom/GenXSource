using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    class RibbonItem : PictureBox
    {
        private bool ispressed, hover, pressed;

        public RibbonItem()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Margin = new Padding(0);
            this.Size = new Size(64, 48);
            this.ispressed = false;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.AutoSize = false;
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

                Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                if (rect.IntersectsWith(e.ClipRectangle))
                {
                    if (this.hover)
                        RibbonControl.RenderSelection(e.Graphics, rect, 2f, this.pressed);
                    else if (this.ispressed)
                        RibbonControl.RenderSelection(e.Graphics, rect, 2f, true);

                    if (this.Enabled && this.Image != null)
                        e.Graphics.DrawImage(this.Image, 4, 4, this.Width - 8, this.Height - 8);
                }
        }

        public bool IsPressed
        {
            get { return this.ispressed; }
            set
            {
                this.ispressed = value;

                this.Invalidate();
            }
        }
    }
}
