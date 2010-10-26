using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IAP_App
{
    public partial class CommandTabButton : Panel
    {
        private string title;
        private bool isselected, isfocused;

        public CommandTabButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int w = this.Width - 1;
            int h = this.Height;

            if (this.isfocused || this.isselected)
            {
                LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, w, h), Color.FromArgb(195, 203, 226), Color.FromArgb(175, 183, 206), LinearGradientMode.Vertical);
                LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, 0, w, h / 2), Color.FromArgb(216, 222, 234), Color.FromArgb(64, 216, 222, 234), LinearGradientMode.Vertical);

                Program.DrawTopRoundedRectangle(pe.Graphics, brush1, null, new Rectangle(0, 0, w, h), 5);
                Program.DrawRoundedRectangle(pe.Graphics, brush2, null, new Rectangle(0, 0, w, h / 2), 5, 5);

                if (this.isselected)
                {
                    GraphicsPath gp1 = new GraphicsPath();
                    gp1.AddEllipse(0, -(h / 2), w, h);
                    gp1.CloseFigure();

                    PathGradientBrush brush3 = new PathGradientBrush(gp1);
                    brush3.CenterColor = Color.AliceBlue;
                    brush3.SurroundColors = new Color[] { Color.Transparent };

                    GraphicsPath gp2 = new GraphicsPath();
                    gp2.AddEllipse(0, (h / 2), w, h);
                    gp2.CloseFigure();

                    PathGradientBrush brush4 = new PathGradientBrush(gp2);
                    brush4.CenterColor = Color.LightBlue;
                    brush4.SurroundColors = new Color[] { Color.Transparent };

                    pe.Graphics.FillEllipse(brush3, 0, -(h / 2), w, h);
                    pe.Graphics.FillEllipse(brush4, 0, (h / 2), w, h);
                }

                Program.DrawTopRoundedRectangle(pe.Graphics, null, new Pen(Color.FromArgb(32, 0, 0, 0), 1), new Rectangle(0, 0, w, h), 5);
                Program.DrawTopRoundedRectangle(pe.Graphics, null, new Pen(Color.FromArgb(128, 255, 255, 255), 1), new Rectangle(1, 1, w - 2, h - 2), 5);
            }

            StringFormat format = new StringFormat();
            format.LineAlignment = format.Alignment = StringAlignment.Center;

            pe.Graphics.DrawString(this.title, this.Font, new SolidBrush(this.ForeColor), new RectangleF(0, 0, this.Width, this.Height), format);
        }

        private void CommandTabButton_MouseEnter(object sender, EventArgs e)
        {
            this.isfocused = true;
            
            this.Invalidate();
        }

        private void CommandTabButton_MouseLeave(object sender, EventArgs e)
        {
            this.isfocused = false;
            
            this.Invalidate();
        }

        private void CommandTabButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Parent.Controls.Count; i++)
            {
                if (this.Parent.Controls[i].GetType() == typeof(CommandTabButton))
                    ((CommandTabButton)this.Parent.Controls[i]).IsSelected = false;
            }
            this.isselected = true;

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

        public bool IsSelected
        {
            get { return this.isselected; }
            set
            {
                this.isselected = value;

                this.Invalidate();
            }
        }
        public bool IsFocused
        {
            get { return this.isfocused; }
        }
    }
}
