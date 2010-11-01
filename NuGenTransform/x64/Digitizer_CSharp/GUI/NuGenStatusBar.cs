using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenTransform
{
    public class NuGenStatusBar : UserControl
    {
        private List<StatusBarPanel> panels;

        private bool showPanels;

        public List<StatusBarPanel> Panels
        {
            get
            {
                return panels;
            }
        }

        public bool ShowPanels
        {
            get
            {
                return showPanels;
            }

            set
            {
                showPanels = value;
            }
        }

        public NuGenStatusBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            panels = new List<StatusBarPanel>();

            this.Font = new Font("Helvetica", 8.25F);            

            Height = 25;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            ScalePanelWidths();

            if (RibbonControl.ColorScheme == ColorScheme.Blue)
            {
                e.Graphics.Clear(Color.FromArgb(191, 219, 255)); //light blue

                Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 3);

                Color[] col = new Color[] { Color.FromArgb(225, 234, 245), Color.FromArgb(209, 223, 240), Color.FromArgb(199, 216, 237), Color.FromArgb(231, 242, 255) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                RibbonControl.FillRoundRectangle(e.Graphics, brush, rect, 3f);

                RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(141, 178, 227)), rect, 3f);
                rect.Offset(1, 1);
                rect.Width -= 2;
                rect.Height -= 2;
                RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect, Color.FromArgb(231, 239, 248), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 3f);

                Point currentPosition = new Point(5, 5);

                foreach (StatusBarPanel panel in panels)
                {
                    Rectangle rect2 = new Rectangle(currentPosition, new Size(panel.Width - adjustPerPanel, Height - 10));

                    RibbonControl.FillRoundRectangle(e.Graphics, new SolidBrush(Color.FromArgb(193, 217, 241)), rect2, 3.0f);
                    RibbonControl.DrawRoundRectangle(e.Graphics, Pens.DarkGray, rect2, 1.0f);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;

                    rect2.X += 5;
                    rect2.Width -= 5;
                    rect2.Y += 1;
                    rect2.Height -= 1;

                    e.Graphics.DrawString(panel.Text, this.Font, new SolidBrush(Color.FromArgb(21, 66, 139)), rect2, sf);

                    currentPosition.X += panel.Width - adjustPerPanel + 5;
                }
            }
            else if (RibbonControl.ColorScheme == ColorScheme.Gray)
            {
                e.Graphics.Clear(Color.FromArgb(83, 83, 83));

                Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 3);

                Color[] col = new Color[] { Color.FromArgb(210, 214, 221), Color.FromArgb(193, 198, 207), Color.FromArgb(180, 187, 197), Color.FromArgb(231, 240, 241) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

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

                Point currentPosition = new Point(5, 5);

                foreach (StatusBarPanel panel in panels)
                {
                    Rectangle rect2 = new Rectangle(currentPosition, new Size(panel.Width - adjustPerPanel, Height-10));

                    RibbonControl.FillRoundRectangle(e.Graphics, new LinearGradientBrush(rect2, Color.FromArgb(172, 174, 174), Color.FromArgb(157, 159, 159), LinearGradientMode.Vertical), rect2, 3.0f);
                    RibbonControl.DrawRoundRectangle(e.Graphics, Pens.Gray, rect2, 1.0f);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;

                    rect2.X += 5;
                    rect2.Width -= 5;
                    rect2.Y += 1;
                    rect2.Height -= 1;

                    e.Graphics.DrawString(panel.Text, this.Font, new SolidBrush(Color.White), rect2, sf);

                    currentPosition.X += panel.Width - adjustPerPanel + 5;
                }
            }
            else
            {
                e.Graphics.Clear(RibbonControl.Color);

                Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 3);

                Color[] col = new Color[] { RibbonControl.GetColor(1.025), RibbonControl.GetColor(1.0), RibbonControl.GetColor(0.975), RibbonControl.GetColor(1.075) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                RibbonControl.FillRoundRectangle(e.Graphics, brush, rect, 3f);

                RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(RibbonControl.GetColor(0.75)), rect, 3f);
                rect.Offset(1, 1);
                rect.Width -= 2;
                rect.Height -= 2;
                RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect, RibbonControl.GetColor(1.05), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 3f);

                Point currentPosition = new Point(5, 5);

                foreach (StatusBarPanel panel in panels)
                {
                    Rectangle rect2 = new Rectangle(currentPosition, new Size(panel.Width - adjustPerPanel, Height - 10));

                    RibbonControl.FillRoundRectangle(e.Graphics, new SolidBrush(RibbonControl.GetColor(0.975)), rect2, 3.0f);
                    RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(RibbonControl.GetColor(1.2)), rect2, 1.0f);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;

                    rect2.X += 5;
                    rect2.Width -= 5;
                    rect2.Y += 1;
                    rect2.Height -= 1;

                    e.Graphics.DrawString(panel.Text, this.Font, new SolidBrush(RibbonControl.TextColor), rect2, sf);

                    currentPosition.X += panel.Width - adjustPerPanel + 5;
                }
            }
        }

        private int adjustPerPanel = 0;

        private void ScalePanelWidths()
        {
            int bufferSize = panels.Count * 10;

            adjustPerPanel = (int)((double)bufferSize / (double)panels.Count + 0.5);
        }
    }
}
