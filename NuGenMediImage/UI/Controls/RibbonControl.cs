using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class RibbonControl : TabControl
    {
        #region Global Ribbon Operations
        public static void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y + rectangle.Height - size, size, size, 0, 90);
            gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - size, size, size, 90, 90);
            gp.CloseFigure();
            g.DrawPath(pen, gp);
            gp.Dispose();
        }
        public static void FillRoundRectangle(Graphics g, Brush brush, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y + rectangle.Height - size, size, size, 0, 90);
            gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - size, size, size, 90, 90);
            gp.CloseFigure();
            g.FillPath(brush, gp);
            gp.Dispose();
        }
        public static void DrawTopRoundRectangle(Graphics g, Pen pen, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddLine(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, rectangle.X, rectangle.Y + rectangle.Height);
            gp.CloseFigure();
            g.DrawPath(pen, gp);
            gp.Dispose();
        }
        public static void FillTopRoundRectangle(Graphics g, Brush brush, Rectangle rectangle, float radius)
        {
            float size = radius * 2f;

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rectangle.X, rectangle.Y, size, size, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - size, rectangle.Y, size, size, 270, 90);
            gp.AddLine(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height, rectangle.X, rectangle.Y + rectangle.Height);
            gp.CloseFigure();
            g.FillPath(brush, gp);
            gp.Dispose();
        }

        public static void RenderSelection(Graphics g, Rectangle rectangle, float radius, bool pressed)
        {
            if (pressed)
            {
                Color[] col = new Color[] { Color.FromArgb(254, 216, 170), Color.FromArgb(251, 181, 101), Color.FromArgb(250, 157, 52), Color.FromArgb(253, 238, 170) };
                //Color[] col = new Color[] { Color.Red, Color.Yellow, Color.Green, Color.White };
                
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillRoundRectangle(g, brush, rectangle, 2f);

                DrawRoundRectangle(g, new Pen(Color.FromArgb(171, 161, 140)), rectangle, radius);
                rectangle.Offset(1, 1);
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                DrawRoundRectangle(g, new Pen(new LinearGradientBrush(rectangle, Color.FromArgb(223, 183, 136), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rectangle, radius);
            }
            else
            {
                Color[] col = new Color[] { Color.FromArgb(255, 254, 227), Color.FromArgb(255, 231, 151), Color.FromArgb(255, 215, 80), Color.FromArgb(255, 231, 150) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillRoundRectangle(g, brush, rectangle, 2f);

                DrawRoundRectangle(g, new Pen(Color.FromArgb(210, 192, 141)), rectangle, radius);
                rectangle.Offset(1, 1);
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                DrawRoundRectangle(g, new Pen(new LinearGradientBrush(rectangle, Color.FromArgb(255, 255, 247), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rectangle, 2f);
            }
        }
        public static void RenderTopSelection(Graphics g, Rectangle rectangle, float radius, bool pressed)
        {
            if (pressed)
            {
                Color[] col = new Color[] { Color.FromArgb(254, 216, 170), Color.FromArgb(251, 181, 101), Color.FromArgb(250, 157, 52), Color.FromArgb(253, 238, 170) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillTopRoundRectangle(g, brush, rectangle, 2f);

                DrawTopRoundRectangle(g, new Pen(Color.FromArgb(171, 161, 140)), rectangle, radius);
                rectangle.Offset(1, 1);
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                DrawTopRoundRectangle(g, new Pen(new LinearGradientBrush(rectangle, Color.FromArgb(223, 183, 136), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rectangle, radius);
            }
            else
            {
                Color[] col = new Color[] { Color.FromArgb(255, 254, 227), Color.FromArgb(255, 231, 151), Color.FromArgb(255, 215, 80), Color.FromArgb(255, 231, 150) };
                float[] pos = new float[] { 0.0f, 0.4f, 0.4f, 1.0f };

                ColorBlend blend = new ColorBlend();
                blend.Colors = col;
                blend.Positions = pos;
                LinearGradientBrush brush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
                brush.InterpolationColors = blend;

                FillTopRoundRectangle(g, brush, rectangle, 2f);

                DrawTopRoundRectangle(g, new Pen(Color.FromArgb(210, 192, 141)), rectangle, radius);
                rectangle.Offset(1, 1);
                rectangle.Width -= 2;
                rectangle.Height -= 2;
                DrawTopRoundRectangle(g, new Pen(new LinearGradientBrush(rectangle, Color.FromArgb(255, 255, 247), Color.Transparent, LinearGradientMode.ForwardDiagonal)), rectangle, 2f);
            }
        }
        #endregion

        public event RibbonPopupEventHandler OnPopup;

        private NuGenMediImageCtrl ngMediImage;

        public NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set { ngMediImage = value; }
        }

        private int hoverindex;
        private bool closed, pressed, _collapsed;

        private bool _showStartTab = true;
        private TabPage startTab = null;       

        public bool ShowStartTab
        {
            get { return _showStartTab; }
            set 
            {               
                if (_showStartTab == value)
                    return;

                _showStartTab = value;

                if (_showStartTab)
                {
                    if (this.startTab != null &&
                        this.TabPages[0] != null &&
                        this.TabPages[0] != this.startTab
                        )
                    {
                        this.TabPages.Insert(0, this.startTab);
                        this.SelectedIndex = this.SelectedIndex;
                    }
                }
                else
                {
                    if (this.TabPages[0] != null &&
                        this.TabPages[0].Name == "tbStart"
                        )
                    {
                        this.startTab = this.TabPages[0];
                        this.TabPages.Remove(this.startTab);
                        this.SelectedIndex = this.SelectedIndex;
                    }                   
                }

                this.Refresh();
            }
        }
        
        private int height;

        public RibbonControl()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.Size = new Size(116, 107);
            this.height = 107;
            this.Dock = DockStyle.Top;
        }

        public new bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value ;

                try
                {
                    this.Parent.Visible = value;
                }
                catch { }

            }
        }

        public bool Collapsed
        {
            get { return _collapsed; }
            set 
            { 
                _collapsed = value;

                if (_collapsed)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }


        public void Open()
        {            
            try
            {
                this.Parent.Size = new Size(this.Width, this.height);                
            }
            catch { }

            this.Size = new Size(this.Width, this.height);
            this.closed = false;
        }

        public void Close()
        {
            try
            {
                this.Parent.Size = new Size(this.Width, 26);
            }
            catch { }

            this.Size = new Size(this.Width, 26);

            this.closed = true;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.SelectedIndex = 1;
            this.hoverindex = -1;
        }

        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            base.OnSelecting(e);

            if (e.TabPage!=null && e.TabPage.Name == "tbStart")
            {
                e.Cancel = true;

                if (OnPopup != null)
                    OnPopup(this);
            }
            else if (this.closed)
                e.Cancel = true;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {            
            base.OnMouseClick(e);

            if (!this.closed)
                if (!this.GetTabRect(0).Contains(e.X, e.Y) && this.Height == 26)
                    this.Size = new Size(this.Width, this.height);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (!this.closed)
            {
                if (!this.GetTabRect(0).Contains(e.X, e.Y))
                {
                    this.height = this.Height;
                    this.Size = new Size(this.Width, 26);
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            for (int i = 0; i < this.TabCount; i++)
            {
                if (this.GetTabRect(i).Contains(e.X, e.Y))
                {
                    this.hoverindex = i;

                    break;
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.pressed = true;
            this.Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.pressed = false;
            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.hoverindex = -1;

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color tpBackColor = NuGenColorsStatic.TabBarBackColor;
            Color tpColor6 = NuGenColorsStatic.Color6;

            if (this.ngMediImage != null)
            {
                tpBackColor = ngMediImage.GetColorConfig().TabBarBackColor;
                tpColor6 = ngMediImage.GetColorConfig().Color6;
            }
                

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            e.Graphics.Clear(tpBackColor);

            e.Graphics.DrawLine(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width, 0);

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            if (this.Height > 26)
            {
                RibbonControl.FillRoundRectangle(e.Graphics, new SolidBrush(Color.FromArgb(32, Color.Black)), new Rectangle(5, this.Height - 20, this.Width - 10, 17), 3f);
                RibbonControl.FillRoundRectangle(e.Graphics, new SolidBrush(Color.FromArgb(32, Color.Black)), new Rectangle(5, this.Height - 20, this.Width - 10, 16), 3f);
            }

            int ti = 0;
            foreach (TabPage tab in this.TabPages)
            {
                Rectangle tabrect = this.GetTabRect(ti);
                tabrect.Height += 4;

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                if (this.SelectedIndex >= 0 && tab.Name != "tbStart" && this.Height > 26 && tab == this.SelectedTab)
                {
                    //Rectangle tabRectR = new Rectangle(tabrect.X + 3, tabrect.Y, tabrect.Width, tabrect.Height);
                    Rectangle tabRectR = tabrect;                     

                    RibbonControl.FillTopRoundRectangle(e.Graphics, new SolidBrush(Color.FromArgb(32, Color.Black)), new Rectangle(tabrect.X - 2, tabrect.Y, tabrect.Width + 4, tabrect.Height + 4), 3f);
                    RibbonControl.FillTopRoundRectangle(e.Graphics, new SolidBrush(Color.FromArgb(16, Color.Black)), new Rectangle(tabrect.X - 1, tabrect.Y, tabrect.Width + 2, tabrect.Height + 4), 3f);
                    RibbonControl.FillTopRoundRectangle(e.Graphics, new LinearGradientBrush(tabrect, Color.FromArgb(237, 238, 239), Color.FromArgb(206, 210, 217), LinearGradientMode.Vertical), tabRectR, 3f);

                    RibbonControl.DrawTopRoundRectangle(e.Graphics, new Pen(tpColor6), tabRectR, 3f);
                    tabrect.Offset(1, 1);
                    tabrect.Width -= 2;
                    tabrect.Height--;
                    RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(tabrect, Color.FromArgb(249, 249, 249), Color.Transparent, LinearGradientMode.ForwardDiagonal)), tabRectR, 3f);

                    Region region = new Region();
                    region.Exclude(new Rectangle(tabrect.X - 4, 0, tabrect.Width + 1, 2));
                    tab.Region = region;

                    tabrect.Height -= 2;
                    e.Graphics.DrawString(tab.Text, tab.Font, new SolidBrush(Color.Black), tabrect, sf);
                }
                else if (ti == this.hoverindex && (!this.closed || ti == 0))
                {
                    tabrect.Width--;

                    if (ti == 0)
                    {
                        tabrect.X += 2;
                        tabrect.Width -= 2;
                        tabrect.Height -= 3;
                        RibbonControl.RenderSelection(e.Graphics, tabrect, 3f, this.pressed);
                        tabrect.X -= 2;
                        tabrect.Width += 2;
                        tabrect.Height += 3;
                    }
                    else
                        RibbonControl.RenderTopSelection(e.Graphics, tabrect, 3f, this.pressed);

                    tabrect.Width++;

                    tabrect.Height -= 2;
                    e.Graphics.DrawString(tab.Text, tab.Font, new SolidBrush(Color.Black), tabrect, sf);
                }
                else
                {
                    tabrect.Height -= 2;
                    e.Graphics.DrawString(tab.Text, tab.Font, new SolidBrush(Color.White), tabrect, sf);
                }

                ti++;
            }
        }
    }
}
