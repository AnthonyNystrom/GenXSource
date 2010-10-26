using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class RibbonLabel : Label
    {
        
        public RibbonLabel()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Margin = new Padding(1);
            this.Padding = new Padding(2);
            this.TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.AutoSize = false;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;            
            base.OnPaint(e);
        }        
    }
}
