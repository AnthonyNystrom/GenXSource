using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IAP_App
{
    public partial class CommandTabSection : UserControl
    {
        private string title;

        public CommandTabSection()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, ((Panel)sender).Width, ((Panel)sender).Height), Color.FromArgb(255, 105, 115, 127), Color.FromArgb(255, 168, 180, 206), LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(brush, 0, 0, ((Panel)sender).Width, ((Panel)sender).Height);

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Near;

            e.Graphics.DrawString(this.title, ((Panel)sender).Font, new SolidBrush(Color.FromArgb(64, 0, 0, 0)), new RectangleF(5, 1, ((Panel)sender).Width, ((Panel)sender).Height), format);
            e.Graphics.DrawString(this.title, ((Panel)sender).Font, new SolidBrush(Color.FromArgb(32, 0, 0, 0)), new RectangleF(6, 2, ((Panel)sender).Width, ((Panel)sender).Height), format);
            e.Graphics.DrawString(this.title, ((Panel)sender).Font, new SolidBrush(((Panel)sender).ForeColor), new RectangleF(4, 0, ((Panel)sender).Width, ((Panel)sender).Height), format);
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title= value;

                this.panel2.Invalidate();
            }
        }
    }
}
