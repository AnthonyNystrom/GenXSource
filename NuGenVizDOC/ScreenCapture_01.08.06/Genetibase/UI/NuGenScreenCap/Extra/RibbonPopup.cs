using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    public class RibbonPopup : Form
    {
        public RibbonPopup()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.Text = "";
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;

            this.BackColor = Color.FromArgb(250, 250, 250);
        }
    }

    public delegate void RibbonPopupEventHandler(object sender);
}
