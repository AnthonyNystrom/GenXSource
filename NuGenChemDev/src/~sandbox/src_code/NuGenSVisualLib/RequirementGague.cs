using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace NuGenSVisualLib
{
    public partial class RequirementGague : UserControl
    {
        int minReqValue;
        int maxReqValue;
        int actualValue;

        bool setupOk = false;

        struct Bar
        {
            public int start, length;
            public bool padStart, padEnd;
        }

        Bar reqsBar;
        Bar outReqsBar;

        int rangeLow, rangeHigh;
        float scale;
        int range;

        bool showValues;
        bool drawTicks;
        bool showValuesAllways;

        Color gradTop, gradBottom, border, reqShade;

        public RequirementGague()
        {
            InitializeComponent();

            gradTop = Color.FromArgb(247, 246, 242);
            gradBottom = Color.FromArgb(197, 197, 175);
            border = Color.FromArgb(49, 106, 197);
            reqShade = Color.FromArgb(193, 210, 237);

            this.DoubleBuffered = true;
        }

        public int MinReqValue
        {
            get { return minReqValue; }
            set { minReqValue = value; setupOk = false; }
        }

        public int MaxReqValue
        {
            get { return maxReqValue; }
            set { maxReqValue = value; setupOk = false; }
        }

        public int ActualValue
        {
            get { return actualValue; }
            set { actualValue = value; setupOk = false; }
        }

        public bool DrawTicks
        {
            get { return drawTicks; }
            set { drawTicks = value; Invalidate(); }
        }

        public bool ShowValues
        {
            get { return showValuesAllways; }
            set { showValuesAllways = value; Invalidate(); }
        }

        public void UpdateLayout()
        {
            if (!setupOk)
            {
                // determine range & scale
                rangeLow = minReqValue;
                rangeHigh = maxReqValue;

                outReqsBar = new Bar();
                reqsBar = new Bar();

                int extraSpace = -20;

                if (actualValue < rangeLow)
                {
                    rangeLow = actualValue;
                    outReqsBar.padStart = true;
                    reqsBar.padEnd = true;
                    outReqsBar.start = actualValue;
                    outReqsBar.length = minReqValue - actualValue;
                }
                else if (actualValue > rangeHigh)
                {
                    rangeHigh = actualValue;
                    outReqsBar.padEnd = true;
                    reqsBar.padStart = true;
                    outReqsBar.start = maxReqValue;
                    outReqsBar.length = actualValue;
                }
                else
                {
                    reqsBar.padStart = true;
                    reqsBar.padEnd = true;
                }

                // set bars
                reqsBar.start = minReqValue;
                reqsBar.length = maxReqValue - minReqValue;

                if (reqsBar.length == 0)
                    extraSpace += -12;
                
                range = rangeHigh - rangeLow;
                scale = ((float)Width + (extraSpace)) / (float)range;

                

                setupOk = true;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(0, Height),
                                                    gradTop,
                                                    gradBottom),
                            e.ClipRectangle);

            g.DrawRectangle(new Pen(border), new Rectangle(0, 0, Width - 1, Height - 1));

            if (setupOk && Enabled)
            {
                // draw bars
                DrawBar(ref reqsBar, e.Graphics, true);
                DrawBar(ref outReqsBar, e.Graphics, false);

                int topGap = 3;
                if (showValues)
                    topGap = 12;

                int x = (int)((actualValue - rangeLow) * scale);
                if (actualValue == rangeLow)
                    x += 2;
                else if (actualValue == rangeHigh)
                    x -= 2;
                g.DrawLine(Pens.Black, x, topGap, x, Height - 3);
                g.DrawLine(Pens.Black, x + 1, topGap, x + 1, Height - 3);

                if (showValues || showValuesAllways)
                    DrawValues(g);

                if (actualValue < minReqValue)
                    g.DrawImage(Properties.Resources.redcross_32, new Rectangle(Width - 18, 3, 16, 16));
                else
                    g.DrawImage(Properties.Resources.greentick_32, new Rectangle(Width - 18, 3, 16, 16));
            }
            else
                g.DrawImage(Properties.Resources.greentick_32, new Rectangle(Width - 18, 3, 16, 16));
        }

        private void DrawValues(Graphics g)
        {
            Font font = new Font(this.Font.FontFamily, 6f);
            Font fontReq = new Font(font, FontStyle.Bold);

            DrawValue(actualValue, g, fontReq, false);
            DrawValue(minReqValue, g, font, (maxReqValue == minReqValue));
            if (maxReqValue != minReqValue)
                DrawValue(maxReqValue, g, font, false);
        }

        private void DrawValue(int value, Graphics g, Font font, bool push)
        {
            string valueStr = value.ToString();
            SizeF size = g.MeasureString(valueStr, font);
            
            int x = (int)((value - rangeLow) * scale);

            if (value == rangeLow)
            {
                if (push)
                    x += 5;
                x += 3;
            }
            else if (value == rangeHigh)
            {
                if (push)
                    x += 2;
                else
                    x -= (int)size.Width + 3;
            }
            else
                x -= (int)size.Width / 2;

            g.DrawString(valueStr, font, Brushes.Black, x, 1);
        }

        private void DrawBar(ref Bar bar, Graphics g, bool clr)
        {
            int startX = (int)((bar.start - rangeLow) * scale) + 1;
            int endX;
            if (bar.length != 0)
                endX = (int)(bar.length * scale) - 2;
            else
                endX = 10;

            if (bar.padStart)
            {
                startX += 2;
                endX -= 2;
            }
            if (bar.padEnd)
                endX -= 2;

            int topGap = 3;
            if (showValues)
                topGap = 12;

            Rectangle rect = new Rectangle(startX, topGap, endX, Height - topGap - 3);
            Brush brush;
            Pen pen;

            if (clr)
            {
                brush = new SolidBrush(reqShade);
                pen = new Pen(border);
            }
            else
            {
                brush = Brushes.LightGray;
                pen = Pens.Gray;
            }

            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);

            if (drawTicks)
            {
                // draw ticks
                for (int i = bar.start + 1; i < bar.start + bar.length; i++)
                {
                    int x = (int)((i - rangeLow) * scale);
                    g.DrawLine(pen, x, topGap + 2, x, Height - 5);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (Enabled)
            {
                showValues = true;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Enabled)
            {
                showValues = false;
                Invalidate();
            }
        }
    }
}