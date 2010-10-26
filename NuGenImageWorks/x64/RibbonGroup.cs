using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    class RibbonGroup : GroupBox
    {
        public event RibbonPopupEventHandler OnPopup;

        private bool hoverplus, pressed;

        public RibbonGroup()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Margin = new Padding(1);
            this.Size = new Size(79, 79);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (OnPopup != null && this.hoverplus)
                OnPopup(this);
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
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (new Rectangle(this.Width - 20, this.Height - 20, 18, 18).Contains(e.X, e.Y))
                this.hoverplus = true;
            else
                this.hoverplus = false;

            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.hoverplus = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect1 = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            Rectangle rect2 = new Rectangle(0, this.Height - 18, this.Width - 2, 16);
            Rectangle rect3 = new Rectangle(this.Width - 18, this.Height - 18, 15, 15);

            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(231, 233, 237)), rect1, 3f);

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rect2.X, rect2.Y - 4f, 6f, 6f, 180, 90);
            gp.AddArc(rect2.X + rect2.Width - 6f, rect2.Y - 5f, 6f, 6f, 270, 90);
            gp.AddArc(rect2.X + rect2.Width - 6f, rect2.Y + rect2.Height - 6f, 6f, 6f, 0, 90);
            gp.AddArc(rect2.X, rect2.Y + rect2.Height - 6f, 6f, 6f, 90, 90);
            gp.CloseFigure();
            e.Graphics.SetClip(gp, CombineMode.Intersect);
            gp.Dispose();
            e.Graphics.FillRectangle(new LinearGradientBrush(rect2, Color.FromArgb(182, 184, 184), Color.FromArgb(157, 159, 159), LinearGradientMode.Vertical), rect2);
            e.Graphics.Clip = new Region();

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.White), rect2, sf);

            if (OnPopup != null)
            {
                if (this.hoverplus)
                {
                    RibbonControl.RenderSelection(e.Graphics, rect3, 2f, this.pressed);
                    e.Graphics.DrawString("+", this.Font, new SolidBrush(Color.Black), rect3, sf);
                }
                else
                    e.Graphics.DrawString("+", this.Font, new SolidBrush(Color.White), rect3, sf);
            }

            rect1.Offset(-1, -1);
            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(140, 141, 143)), rect1, 3f);
        }
    }
}
