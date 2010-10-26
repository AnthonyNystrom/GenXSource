using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IAP_App
{
    public partial class Window : Form
    {
        private bool dragmove;
        private int mpx, mpy;

        public Window()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, this.Width / 2, this.Height), Color.Silver, Color.White, LinearGradientMode.Horizontal);
            brush.WrapMode = WrapMode.TileFlipX;

            pe.Graphics.FillRectangle(brush, 0, 0, this.Width, this.Height);
        }

        private void Window_Resize(object sender, EventArgs e)
        {
            Rectangle wa = SystemInformation.WorkingArea;

            //if (this.Location.X + this.Width > wa.Width)
            //    this.Width = wa.Width;

            //if (this.Location.Y + this.Height > wa.Height)
            //    this.Height = wa.Height;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            float radiusx = 5;
            float radiusy = 5;

            float rx = Math.Min(radiusx * 2, rect.Width);
            float ry = Math.Min(radiusy * 2, rect.Height);

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rect.X, rect.Y, rx, ry, 180, 90);
            gp.AddArc(rect.X + rect.Width - rx, rect.Y, rx, ry, 270, 90);
            gp.AddArc(rect.X + rect.Width - rx, rect.Y + rect.Height - ry, rx, ry, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - ry, rx, ry, 90, 90);
            gp.CloseFigure();
            this.Region = new Region(gp);
            gp.Dispose();

            this.Invalidate();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //private void label4_Click(object sender, EventArgs e)
        //{
        //    if (((Label)sender).Text == "1")
        //    {
        //        this.WindowState = FormWindowState.Maximized;
        //        ((Label)sender).Text = "2";
        //    }
        //    else
        //    {
        //        this.WindowState = FormWindowState.Normal;
        //        ((Label)sender).Text = "1";
        //    }
        //}

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Load(object sender, EventArgs e)
        {
        }

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ((Panel)sender).Cursor = Cursors.NoMove2D;

                this.mpx = e.X - this.Location.X;
                this.mpy = e.Y - this.Location.Y;

                this.dragmove = true;
            }
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragmove)
                this.Location = new Point(e.X - mpx, e.Y - mpy);
        }

        private void panel5_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ((Panel)sender).Cursor = Cursors.Default;

                this.dragmove = false;
            }
        }
    }
}
