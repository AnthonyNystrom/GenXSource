using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class RibbonButton : Label
    {
        public event RibbonPopupEventHandler OnPopup;
        private NuGenMediImageCtrl ngMediImage;

        public NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set { ngMediImage = value; }
        }

        private bool isflat;
        private bool ispressed, hover, pressed;

        public RibbonButton()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Margin = new Padding(1);
            this.Padding = new Padding(2);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.isflat = true;
            this.ispressed = false;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.AutoSize = false;
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            this.Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (OnPopup != null)
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
            Color tpC1 = NuGenColorsStatic.TabPageColor1;
            Color tpC2 = NuGenColorsStatic.TabPageColor2;
            Color tpC3 = NuGenColorsStatic.TabPageColor3;
            Color tpC4 = NuGenColorsStatic.TabPageColor4;
            Color tpbC5 = NuGenColorsStatic.TabPageBorderColor;
            Color tpColor6 = NuGenColorsStatic.Color6;
            Color tpBackColor = NuGenColorsStatic.TabBarBackColor;

            if (ngMediImage != null)
            {
                tpC1 = ngMediImage.GetColorConfig().TabPageColor1;
                tpC2 = ngMediImage.GetColorConfig().TabPageColor2;
                tpC3 = ngMediImage.GetColorConfig().TabPageColor3;
                tpC4 = ngMediImage.GetColorConfig().TabPageColor4;
                tpbC5 = ngMediImage.GetColorConfig().TabPageBorderColor;
                tpColor6 = ngMediImage.GetColorConfig().Color6;
                tpBackColor = ngMediImage.GetColorConfig().TabBarBackColor;
            }

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            if (this.hover)
            {
                RibbonControl.RenderSelection(e.Graphics, rect, 2f, this.pressed);
            }
            else if (this.ispressed)
                RibbonControl.RenderSelection(e.Graphics, rect, 2f, true);
            else if (!this.isflat)
            {
                Color[] col = new Color[] { tpC1, tpC2, tpC3, tpC4 };
                float[] pos = new float[] { 0.0f, 0.2f, 0.2f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                RibbonControl.FillRoundRectangle(e.Graphics, brush, rect, 2f);

                RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(tpColor6), rect, 2f);
                rect.Offset(1, 1);
                rect.Width -= 2;
                rect.Height -= 2;
                RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect, tpBackColor, Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 2f);
            }

            base.OnPaint(e);
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public bool IsFlat
        {
            get { return this.isflat; }
            set { this.isflat = value; }
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
