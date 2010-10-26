using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IAP_App
{
    public partial class CommandTabSectionButton : Panel
    {
        private string title;
        private bool isfocused, ispressed;

        public CommandTabSectionButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int w = this.Width - 1;
            int h = this.Height - 1;

            LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, w, h), Color.FromArgb(213, 219, 233), Color.FromArgb(214, 218, 227), LinearGradientMode.Vertical);
            LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, 0, w, h / 2), Color.FromArgb(246, 249, 254), Color.FromArgb(64, 250, 251, 255), LinearGradientMode.Vertical);

            Program.DrawRoundedRectangle(pe.Graphics, brush1, null, new Rectangle(0, 0, w, h), 3, 3);
            Program.DrawRoundedRectangle(pe.Graphics, brush2, null, new Rectangle(0, 0, w, h / 2), 3, 3);

            if (this.isfocused)
            {
                GraphicsPath gp1 = new GraphicsPath();
                gp1.AddEllipse(0, -(h / 2), w, h);
                gp1.CloseFigure();

                PathGradientBrush brush3 = new PathGradientBrush(gp1);
                if (this.ispressed)
                    brush3.CenterColor = Color.LightSteelBlue;
                else
                    brush3.CenterColor = Color.AliceBlue;
                brush3.SurroundColors = new Color[] { Color.Transparent };

                GraphicsPath gp2 = new GraphicsPath();
                gp2.AddEllipse(0, (h / 2), w, h);
                gp2.CloseFigure();

                PathGradientBrush brush4 = new PathGradientBrush(gp2);
                if (this.ispressed)
                    brush4.CenterColor = Color.LightSteelBlue;
                else
                    brush4.CenterColor = Color.AliceBlue;
                brush4.SurroundColors = new Color[] { Color.Transparent };

                pe.Graphics.FillEllipse(brush3, 0, -(h / 2), w, h);
                pe.Graphics.FillEllipse(brush4, 0, (h / 2), w, h);
            }

            Program.DrawRoundedRectangle(pe.Graphics, null, new Pen(Color.FromArgb(32, 0, 0, 0), 1), new Rectangle(0, 0, w, h), 3, 3);
            Program.DrawRoundedRectangle(pe.Graphics, null, new Pen(Color.FromArgb(128, 255, 255, 255), 1), new Rectangle(1, 1, w - 2, h - 2), 3, 3);

            StringFormat format = new StringFormat();
            format.LineAlignment = format.Alignment = StringAlignment.Center;

            pe.Graphics.DrawString(this.title, this.Font, new SolidBrush(this.ForeColor), new RectangleF(0, 0, this.Width, this.Height), format);
        }

        private void CommandTabSectionButton_MouseEnter(object sender, EventArgs e)
        {
            this.isfocused = true;

            this.Invalidate();
        }

        private void CommandTabSectionButton_MouseLeave(object sender, EventArgs e)
        {
            this.isfocused = false;

            this.Invalidate();
        }

        private void CommandTabSectionButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.ispressed = true;

            this.Invalidate();
        }

        private void CommandTabSectionButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.ispressed = false;

            this.Invalidate();
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;

                this.Invalidate();
            }
        }

        public bool IsFocused
        {
            get { return this.isfocused; }
        }

        public bool IsPressed
        {
            get { return this.ispressed; }
        }
    }
}
