using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IAP_App
{
    public partial class StatusTab : UserControl
    {
        public StatusTab()
        {
            InitializeComponent();
        }

        private void StatusTab_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height), Color.FromArgb(195, 203, 226), Color.FromArgb(175, 183, 206), LinearGradientMode.Vertical);
                LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height / 2), Color.FromArgb(216, 222, 234), Color.FromArgb(64, 216, 222, 234), LinearGradientMode.Vertical);

                LinearGradientBrush brush3 = new LinearGradientBrush(new Rectangle(0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height), Color.FromArgb(32, 0, 0, 0), Color.Transparent, LinearGradientMode.Vertical);
                LinearGradientBrush brush4 = new LinearGradientBrush(new Rectangle(0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height), Color.FromArgb(64, 255, 255, 255), Color.Transparent, LinearGradientMode.Vertical);

                e.Graphics.FillRectangle(brush1, 0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height);
                e.Graphics.FillRectangle(brush2, 0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height / 2);

                e.Graphics.DrawRectangle(new Pen(brush3, 1), new Rectangle(0, 0, ((StatusTab)sender).Width, ((StatusTab)sender).Height));
                e.Graphics.DrawRectangle(new Pen(brush4, 1), new Rectangle(0, 1, ((StatusTab)sender).Width, ((StatusTab)sender).Height));
            }
            catch { }
        }

        private void label2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(64, Color.MidnightBlue)), 5, 5, ((Label)sender).Width - 10, ((Label)sender).Height - 10);
            e.Graphics.DrawEllipse(new Pen(Color.FromArgb(128, Color.White)), 5, 5, ((Label)sender).Width - 10, ((Label)sender).Height - 10);

            StringFormat format = new StringFormat();
            format.LineAlignment = format.Alignment = StringAlignment.Center;

            e.Graphics.DrawString(((Label)sender).Text, ((Label)sender).Font, new SolidBrush(((Label)sender).ForeColor), new RectangleF(0, 0, ((Label)sender).Width, ((Label)sender).Height), format);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Program.ZoomScale /= 2.0;

            Program.ImageBox.Width = (int)((double)Program.ImageBox.Image.Width * Program.ZoomScale);
            Program.ImageBox.Height = (int)((double)Program.ImageBox.Image.Height * Program.ZoomScale);

            Program.ImageBox.Parent.Invalidate();

            this.label4.Text = (100.0 * Program.ZoomScale).ToString() + "%";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Program.ZoomScale *= 2.0;

            Program.ImageBox.Width = (int)((double)Program.ImageBox.Image.Width * Program.ZoomScale);
            Program.ImageBox.Height = (int)((double)Program.ImageBox.Image.Height * Program.ZoomScale);

            Program.ImageBox.Parent.Invalidate();

            this.label4.Text = (100.0 * Program.ZoomScale).ToString() + "%";
        }

        private void StatusTab_Load(object sender, EventArgs e)
        {
            Program.SizeBox = this.label12;
            Program.ZoomBox = this.label4;
        }
    }
}
