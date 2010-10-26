using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenImageWorks.Undo;

namespace Genetibase.UI.NuGenImageWorks
{
    [DefaultEvent("ValueChanged")]
    public partial class RibbonTrackEn : UserControl
    { 
        private double value, defaultvalue, oldvalue;
        private Color startcolor, endcolor;
        private bool np;
        private bool SetValue = false;
        private bool SetViaDrag = false;
        private double displayvalue = 0.0;

        private int factor = 1;

        public int Factor
        {
            get { return factor; }
            set { factor = value; }
        }
               
        private bool hover;

        [Category("Action")]        
        public event ValueChangedEventHandler ValueChanged;

        public RibbonTrackEn()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            this.Title = "Track";
            this.startcolor = Color.Black;
            this.endcolor = Color.White;            
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle rect = new Rectangle(0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);

            RibbonControl.FillRoundRectangle(e.Graphics, new LinearGradientBrush(e.ClipRectangle, this.startcolor, this.endcolor, LinearGradientMode.Horizontal), rect, 3f);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(Math.Round(this.displayvalue, 0).ToString(), this.label1.Font, new SolidBrush(Color.FromArgb(218, 226, 226)), rect, sf);

            if (this.hover)
            {
                float x = this.np ? (float)((this.value + 1.0) / 2.0) * rect.Width : (float)this.value * rect.Width;
                e.Graphics.DrawLine(new Pen(new LinearGradientBrush(e.ClipRectangle, Color.FromArgb(255, 255, 247), Color.FromArgb(210, 192, 141), LinearGradientMode.Vertical), 3f), x, 0, x, rect.Height);
            }
            else
            {
                float x = this.np ? (float)((this.value + 1.0) / 2.0) * rect.Width : (float)this.value * rect.Width;
                e.Graphics.DrawLine(new Pen(new LinearGradientBrush(e.ClipRectangle, Color.FromArgb(223, 183, 136), Color.FromArgb(171, 161, 140), LinearGradientMode.Vertical), 3f), x, 0, x, rect.Height);
            }

            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(172, 172, 172)), rect, 3f);

            rect.Offset(1, 1);
            rect.Width -= 2;
            rect.Height -= 2;
            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(96, 0, 0, 0)), rect, 3f);
            rect.Offset(1, 1);
            rect.Width -= 2;
            rect.Height -= 2;
            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(64, 0, 0, 0)), rect, 3f);
            rect.Offset(1, 1);
            rect.Width -= 2;
            rect.Height -= 2;
            RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(Color.FromArgb(32, 0, 0, 0)), rect, 3f);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if( ! SetViaDrag )
                    this.oldvalue = this.value;

                SetViaDrag = false;

                if (this.np)
                {
                    this.value = -1.0 + (double)e.X / (double)this.pictureBox1.Width * 2.0;

                    /*if (this.value < -1.0)
                        this.value = -1.0;
                    else if (this.value > 1.0)
                        this.value = 1.0;*/
                }
                else
                {
                    this.value = (double)e.X / (double)this.pictureBox1.Width;

                    /*if (this.value < 0.0)
                        this.value = 0.0;
                    else if (this.value > 1.0)
                        this.value = 1.0;*/
                }                

                // Hack to make sure the valuechanged event is fired
                this.Value = this.value;
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.oldvalue = this.value;
                this.value = this.defaultvalue;


                // Hack to make sure the valuechanged event is fired
                this.Value = this.value;
            }

            this.displayvalue = this.value * factor;

            base.OnMouseClick(e);
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.oldvalue = this.value;
           
                ValueDlg vdlg = new ValueDlg(this.value, this.np);

                if (vdlg.ShowDialog() == DialogResult.OK)
                {
                    this.value = vdlg.Value;

                    // Hack to make sure the valuechanged event is fired
                    this.Value = this.value;
                }
            }
            this.displayvalue = this.value * factor;
            base.OnMouseDoubleClick(e);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.hover = true;

            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.hover = false;

            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {   
            if (e.Button == MouseButtons.Left)
            {
                SetViaDrag = true;

                if (SetValue)
                {
                    this.oldvalue = this.value;
                    SetValue = false;
                }

                if (this.np)
                {
                    this.value = -1.0 + ((double)e.X / (double)this.pictureBox1.Width) * 2.0;

                    if (this.value < -1.0)
                        this.value = -1.0;
                    else if (this.value > 1.0)
                        this.value = 1.0;
                }
                else
                {
                    this.value = (double)e.X / (double)this.pictureBox1.Width;

                    if (this.value < 0.0)
                        this.value = 0.0;
                    else if (this.value > 1.0)
                        this.value = 1.0;
                }
            }
            this.displayvalue = this.value * factor;
            base.OnMouseMove(e);
        }

        public string Title
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }
        public double Value
        {
            get { return this.value; }
            set
            {                
                this.value = value;
                this.pictureBox1.Invalidate();

                if (ValueChanged != null && Program.fireEvent)
                {
                    ValueChanged(this,new ValueChangedEventArgs(this.oldvalue,this.value) );
                }
            }
        }
        public double DefaultValue
        {
            get { return this.defaultvalue; }
            set { this.defaultvalue = value; }
        }
        public Color StartColor
        {
            get { return this.startcolor; }
            set { this.startcolor = value; }
        }
        public Color EndColor
        {
            get { return this.endcolor; }
            set { this.endcolor = value; }
        }
        public bool NP
        {
            get { return this.np; }
            set { this.np = value; }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            SetValue = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            SetValue = false;
        }

        private void RibbonTrack_EnabledChanged(object sender, EventArgs e)
        { }
    }
}
