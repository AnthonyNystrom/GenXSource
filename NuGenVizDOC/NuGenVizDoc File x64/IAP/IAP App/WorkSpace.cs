using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IAP_App
{
    public partial class WorkSpace : UserControl
    {
        private bool dragimage;
        private int mpx, mpy, idx, idy;

        public WorkSpace()
        {
            InitializeComponent();
        }

        private void WorkSpace_Load(object sender, EventArgs e)
        {
            IAP_Core.ImageOperations.Image = new IAP_Core.Image((Bitmap)this.pictureBox1.Image);
            IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height);

            this.pictureBox1.Image = IAP_Core.ImageOperations.Image.Bitmap;

            Program.ImageBox = this.pictureBox1;
        }

        private void WorkSpace_Paint(object sender, PaintEventArgs e)
        {
            if (!this.dragimage)
                this.pictureBox1.Location = new Point(this.idx + (this.Width / 2) - (this.pictureBox1.Width / 2), this.idy + (this.Height / 2) - (this.pictureBox1.Height / 2));

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush brush1 = new LinearGradientBrush(new Rectangle(0, 0, ((WorkSpace)sender).Width, ((WorkSpace)sender).Height / 2), Color.FromArgb(255, 155, 160, 179), Color.FromArgb(255, 114, 125, 153), LinearGradientMode.Vertical);
            brush1.WrapMode = WrapMode.TileFlipX;

            HatchBrush brush2 = new HatchBrush(HatchStyle.Percent75, Color.Transparent, Color.FromArgb(255, 169, 175, 199));

            e.Graphics.FillRectangle(brush1, 0, 0, ((WorkSpace)sender).Width, ((WorkSpace)sender).Height);
            e.Graphics.FillRectangle(brush2, 0, 0, ((WorkSpace)sender).Width, ((WorkSpace)sender).Height);

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(32, 0, 0, 0)), this.pictureBox1.Location.X - 3, this.pictureBox1.Location.Y - 3, this.pictureBox1.Width + 6, this.pictureBox1.Height + 6);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(32, 0, 0, 0)), this.pictureBox1.Location.X - 2, this.pictureBox1.Location.Y - 2, this.pictureBox1.Width + 4, this.pictureBox1.Height + 4);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(32, 0, 0, 0)), this.pictureBox1.Location.X - 1, this.pictureBox1.Location.Y - 1, this.pictureBox1.Width + 2, this.pictureBox1.Height + 2);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(16, 0, 0, 0)), this.pictureBox1.Location.X, this.pictureBox1.Location.Y, this.pictureBox1.Width + 4, this.pictureBox1.Height + 4);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(16, 0, 0, 0)), this.pictureBox1.Location.X, this.pictureBox1.Location.Y, this.pictureBox1.Width + 5, this.pictureBox1.Height + 5);

            if (this.dragimage)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.LightSteelBlue)), new Rectangle(new Point(this.idx + (this.Width / 2) - (this.pictureBox1.Width / 2), this.idy + (this.Height / 2) - (this.pictureBox1.Width / 2)), this.pictureBox1.Size));
                e.Graphics.DrawRectangle(Pens.LightSteelBlue, new Rectangle(new Point(this.idx + (this.Width / 2) - (this.pictureBox1.Width / 2), this.idy + (this.Height / 2) - (this.pictureBox1.Width / 2)), this.pictureBox1.Size));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.mpx = e.X;
                this.mpy = e.Y;

                IAP_Core.ImageOperations.Rect = new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height);

                Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();

                this.pictureBox1.Invalidate();
            }

            if (e.Button == MouseButtons.Right)
            {
                this.pictureBox1.Cursor = Cursors.NoMove2D;

                this.mpx = e.X - idx;
                this.mpy = e.Y - idy;

                this.dragimage = true;
             
                this.Invalidate();
           }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                IAP_Core.ImageOperations.Rect.X = mpx;
                IAP_Core.ImageOperations.Rect.Y = mpy;
                IAP_Core.ImageOperations.Rect.Width = Math.Min(e.X - IAP_Core.ImageOperations.Rect.X, IAP_Core.ImageOperations.Image.Width - IAP_Core.ImageOperations.Rect.X);
                IAP_Core.ImageOperations.Rect.Height = Math.Min(e.Y - IAP_Core.ImageOperations.Rect.Y, IAP_Core.ImageOperations.Image.Height - IAP_Core.ImageOperations.Rect.Y);

                if (IAP_Core.ImageOperations.Rect.Width < 0)
                    IAP_Core.ImageOperations.Rect.Width = 0;

                if (IAP_Core.ImageOperations.Rect.Height < 0)
                    IAP_Core.ImageOperations.Rect.Height = 0;

                Program.SelectionBox.Text = IAP_Core.ImageOperations.Rect.X.ToString() + "; " + IAP_Core.ImageOperations.Rect.Y.ToString() + "; " + IAP_Core.ImageOperations.Rect.Width.ToString() + "; " + IAP_Core.ImageOperations.Rect.Height.ToString();

                this.pictureBox1.Invalidate();
            }
            if (this.dragimage)
            {
                this.idx = e.X - mpx;
                this.idy = e.Y - mpy;

                this.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.pictureBox1.Cursor = Cursors.Default;

                this.dragimage = false;

                this.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (IAP_Core.ImageOperations.Image.GetType() != typeof(IAP_Core.ComplexImage) && IAP_Core.ImageOperations.Rect != new Rectangle(0, 0, IAP_Core.ImageOperations.Image.Width, IAP_Core.ImageOperations.Image.Height))
            {
                Rectangle rc = new Rectangle((int)((double)IAP_Core.ImageOperations.Rect.X * Program.ZoomScale), (int)((double)IAP_Core.ImageOperations.Rect.Y * Program.ZoomScale), (int)((double)(IAP_Core.ImageOperations.Rect.Width - 1) * Program.ZoomScale), (int)((double)(IAP_Core.ImageOperations.Rect.Height - 1) * Program.ZoomScale));

                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.LightSteelBlue)), rc);
                e.Graphics.DrawRectangle(Pens.LightSteelBlue, rc);
            }
            else if (IAP_Core.ImageOperations.Image.GetType() == typeof(IAP_Core.ComplexImage))
            {
                try
                {
                    string[] r = Program.RangeBox.Text.Split(';');

                    int min = (int)((double)int.Parse(r[0]) * Program.ZoomScale);
                    int max = (int)((double)int.Parse(r[1]) * Program.ZoomScale);

                    e.Graphics.DrawEllipse(Pens.LightSteelBlue, (int)((double)(IAP_Core.ImageOperations.Image.Width / 2) * Program.ZoomScale) - min, (int)((double)(IAP_Core.ImageOperations.Image.Height / 2) * Program.ZoomScale) - min, (min * 2) - 1, (min * 2) - 1);
                    e.Graphics.DrawEllipse(Pens.LightSteelBlue, (int)((double)(IAP_Core.ImageOperations.Image.Width / 2) * Program.ZoomScale) - max, (int)((double)(IAP_Core.ImageOperations.Image.Height / 2) * Program.ZoomScale) - max, (max * 2) - 1, (max * 2) - 1);
                }
                catch { }
            }
        }
    }
}
