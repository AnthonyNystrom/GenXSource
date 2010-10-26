using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class RibbonTab : TabPage
    {
        private NuGenMediImageCtrl ngMediImage;

        public NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set { ngMediImage = value; }
        }

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
            Color tpC1 = NuGenColorsStatic.TabPageColor1;
            Color tpC2 = NuGenColorsStatic.TabPageColor2;
            Color tpC3 = NuGenColorsStatic.TabPageColor3;
            Color tpC4 = NuGenColorsStatic.TabPageColor4;
            Color tpbC5 = NuGenColorsStatic.TabPageBorderColor;
            Color tpColor6 = NuGenColorsStatic.Color6;
            Color tpBackColor = NuGenColorsStatic.TabBarBackColor;

            if( ngMediImage != null )
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

            e.Graphics.Clear(tpBackColor);

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Color[] col = new Color[] { tpC1,tpC2 ,tpC3 ,tpC4  };
            float[] pos = new float[] { 0.0f, 0.2f, 0.2f, 1.0f };

            ColorBlend blend = new ColorBlend();
            blend.Colors = col;
            blend.Positions = pos;
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            brush.InterpolationColors = blend;

            RibbonControl.FillRoundRectangle(e.Graphics, brush, rect, 3f);

            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(tpColor6), rect, 3f);
            rect.Offset(1, 1);
            rect.Width -= 2;
            rect.Height -= 2;
            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect,tpbC5 , Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 3f);
        }
    }
}
