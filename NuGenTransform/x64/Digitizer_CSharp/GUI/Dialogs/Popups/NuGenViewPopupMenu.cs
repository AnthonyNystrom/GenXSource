using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI;

namespace Genetibase.NuGenTransform
{
    class NuGenViewPopupMenu : NuGenPopupMenu
    {

        public NuGenViewPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {
            InitializeComponent();
        }

        public override void InitializeDefaults()
        {
            this.ribbonButton1.Enabled = false;
            this.ribbonButton2.Enabled = false;
            this.ribbonButton3.Enabled = false;
            this.ribbonButton4.Enabled = false;
            this.ribbonButton5.Enabled = false;
            this.ribbonButton6.Enabled = false;
            this.ribbonButton7.Enabled = false;
            this.ribbonButton8.Enabled = false;
            this.ribbonButton9.Enabled = false;
            this.ribbonButton11.Enabled = false;
            this.ribbonButton12.Enabled = false;
        }

        public override void EnableControls()
        {

            this.ribbonButton1.Enabled = true;
            this.ribbonButton2.Enabled = true;
            this.ribbonButton3.Enabled = true;
            this.ribbonButton4.Enabled = true;
            this.ribbonButton5.Enabled = true;
            this.ribbonButton6.Enabled = true;
            this.ribbonButton7.Enabled = true;
            this.ribbonButton8.Enabled = true;
            this.ribbonButton9.Enabled = true;
            this.ribbonButton11.Enabled = true;
            this.ribbonButton12.Enabled = true;
        }
    
        private void InitializeComponent()
        {
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
            this.ribbonButton2 = new Genetibase.UI.RibbonButton();
            this.ribbonButton3 = new Genetibase.UI.RibbonButton();
            this.ribbonButton4 = new Genetibase.UI.RibbonButton();
            this.ribbonButton5 = new Genetibase.UI.RibbonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ribbonButton6 = new Genetibase.UI.RibbonButton();
            this.ribbonButton7 = new Genetibase.UI.RibbonButton();
            this.ribbonButton8 = new Genetibase.UI.RibbonButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.ribbonButton11 = new Genetibase.UI.RibbonButton();
            this.ribbonButton12 = new Genetibase.UI.RibbonButton();
            this.ribbonButton9 = new Genetibase.UI.RibbonButton();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.ribbonButton10 = new Genetibase.UI.RibbonButton();
            this.ribbonButton13 = new Genetibase.UI.RibbonButton();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.ribbonButton14 = new Genetibase.UI.RibbonButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitaxissmall;
            this.ribbonButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 5);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.ribbonButton1.Size = new System.Drawing.Size(155, 25);
            this.ribbonButton1.TabIndex = 2;
            this.ribbonButton1.Text = "Axis Points";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton1.Click += new System.EventHandler(this.ribbonButton1_Click);
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton2.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitscalelarge;
            this.ribbonButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton2.IsFlat = true;
            this.ribbonButton2.IsPressed = false;
            this.ribbonButton2.Location = new System.Drawing.Point(5, 30);
            this.ribbonButton2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.ribbonButton2.Size = new System.Drawing.Size(155, 36);
            this.ribbonButton2.TabIndex = 2;
            this.ribbonButton2.Text = "Scale Points";
            this.ribbonButton2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton2.Click += new System.EventHandler(this.ribbonButton2_Click);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton3.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitcurvelarge;
            this.ribbonButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton3.IsFlat = true;
            this.ribbonButton3.IsPressed = false;
            this.ribbonButton3.Location = new System.Drawing.Point(5, 66);
            this.ribbonButton3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.ribbonButton3.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton3.TabIndex = 2;
            this.ribbonButton3.Text = "Curve Points";
            this.ribbonButton3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton3.Click += new System.EventHandler(this.ribbonButton3_Click);
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton4.Image = global::Genetibase.NuGenTransform.Properties.Resources.digitmeasurelarge;
            this.ribbonButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton4.IsFlat = true;
            this.ribbonButton4.IsPressed = false;
            this.ribbonButton4.Location = new System.Drawing.Point(5, 96);
            this.ribbonButton4.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton4.Name = "ribbonButton4";
            this.ribbonButton4.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.ribbonButton4.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton4.TabIndex = 2;
            this.ribbonButton4.Text = "Measure Points";
            this.ribbonButton4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton4.Click += new System.EventHandler(this.ribbonButton4_Click);
            // 
            // ribbonButton5
            // 
            this.ribbonButton5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton5.IsFlat = true;
            this.ribbonButton5.IsPressed = false;
            this.ribbonButton5.Location = new System.Drawing.Point(5, 126);
            this.ribbonButton5.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton5.Name = "ribbonButton5";
            this.ribbonButton5.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton5.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton5.TabIndex = 2;
            this.ribbonButton5.Text = "All Points";
            this.ribbonButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton5.Click += new System.EventHandler(this.ribbonButton5_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(2, 159);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 1);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton6.IsFlat = true;
            this.ribbonButton6.IsPressed = false;
            this.ribbonButton6.Location = new System.Drawing.Point(5, 272);
            this.ribbonButton6.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton6.Name = "ribbonButton6";
            this.ribbonButton6.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton6.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton6.TabIndex = 2;
            this.ribbonButton6.Text = "View Gridlines";
            this.ribbonButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton6.Click += new System.EventHandler(this.ribbonButton6_Click);
            // 
            // ribbonButton7
            // 
            this.ribbonButton7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton7.IsFlat = true;
            this.ribbonButton7.IsPressed = false;
            this.ribbonButton7.Location = new System.Drawing.Point(5, 311);
            this.ribbonButton7.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton7.Name = "ribbonButton7";
            this.ribbonButton7.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton7.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton7.TabIndex = 2;
            this.ribbonButton7.Text = "Curve Geometry";
            this.ribbonButton7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton7.Click += new System.EventHandler(this.ribbonButton7_Click);
            // 
            // ribbonButton8
            // 
            this.ribbonButton8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton8.IsFlat = true;
            this.ribbonButton8.IsPressed = false;
            this.ribbonButton8.Location = new System.Drawing.Point(5, 339);
            this.ribbonButton8.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton8.Name = "ribbonButton8";
            this.ribbonButton8.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton8.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton8.TabIndex = 2;
            this.ribbonButton8.Text = "Measure Geometry";
            this.ribbonButton8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton8.Click += new System.EventHandler(this.ribbonButton8_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Silver;
            this.pictureBox2.Location = new System.Drawing.Point(0, 306);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(160, 1);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // ribbonButton11
            // 
            this.ribbonButton11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton11.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton11.IsFlat = true;
            this.ribbonButton11.IsPressed = false;
            this.ribbonButton11.Location = new System.Drawing.Point(5, 164);
            this.ribbonButton11.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton11.Name = "ribbonButton11";
            this.ribbonButton11.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton11.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton11.TabIndex = 2;
            this.ribbonButton11.Text = "Processed Background";
            this.ribbonButton11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton11.Click += new System.EventHandler(this.ribbonButton11_Click);
            // 
            // ribbonButton12
            // 
            this.ribbonButton12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton12.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton12.IsFlat = true;
            this.ribbonButton12.IsPressed = false;
            this.ribbonButton12.Location = new System.Drawing.Point(5, 194);
            this.ribbonButton12.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton12.Name = "ribbonButton12";
            this.ribbonButton12.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton12.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton12.TabIndex = 2;
            this.ribbonButton12.Text = "Original Background";
            this.ribbonButton12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton12.Click += new System.EventHandler(this.ribbonButton12_Click);
            // 
            // ribbonButton9
            // 
            this.ribbonButton9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton9.IsFlat = true;
            this.ribbonButton9.IsPressed = false;
            this.ribbonButton9.Location = new System.Drawing.Point(5, 228);
            this.ribbonButton9.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton9.Name = "ribbonButton9";
            this.ribbonButton9.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton9.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton9.TabIndex = 2;
            this.ribbonButton9.Text = "No Background";
            this.ribbonButton9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton9.Click += new System.EventHandler(this.ribbonButton9_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Silver;
            this.pictureBox3.Location = new System.Drawing.Point(0, 266);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(160, 1);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // ribbonButton10
            // 
            this.ribbonButton10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton10.IsFlat = true;
            this.ribbonButton10.IsPressed = false;
            this.ribbonButton10.Location = new System.Drawing.Point(5, 382);
            this.ribbonButton10.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton10.Name = "ribbonButton10";
            this.ribbonButton10.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton10.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton10.TabIndex = 2;
            this.ribbonButton10.Text = "Gray Color Scheme";
            this.ribbonButton10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton10.Click += new System.EventHandler(this.ribbonButton10_Click);
            // 
            // ribbonButton13
            // 
            this.ribbonButton13.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton13.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton13.IsFlat = true;
            this.ribbonButton13.IsPressed = false;
            this.ribbonButton13.Location = new System.Drawing.Point(5, 416);
            this.ribbonButton13.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton13.Name = "ribbonButton13";
            this.ribbonButton13.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton13.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton13.TabIndex = 2;
            this.ribbonButton13.Text = "Blue Color Scheme";
            this.ribbonButton13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton13.Click += new System.EventHandler(this.ribbonButton13_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Silver;
            this.pictureBox4.Location = new System.Drawing.Point(0, 377);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(160, 1);
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // ribbonButton14
            // 
            this.ribbonButton14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton14.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton14.IsFlat = true;
            this.ribbonButton14.IsPressed = false;
            this.ribbonButton14.Location = new System.Drawing.Point(5, 450);
            this.ribbonButton14.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton14.Name = "ribbonButton14";
            this.ribbonButton14.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton14.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton14.TabIndex = 2;
            this.ribbonButton14.Text = "Custom Color Scheme";
            this.ribbonButton14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton14.Click += new System.EventHandler(this.ribbonButton14_Click);
            // 
            // NuGenViewPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(165, 491);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ribbonButton9);
            this.Controls.Add(this.ribbonButton12);
            this.Controls.Add(this.ribbonButton11);
            this.Controls.Add(this.ribbonButton14);
            this.Controls.Add(this.ribbonButton13);
            this.Controls.Add(this.ribbonButton10);
            this.Controls.Add(this.ribbonButton8);
            this.Controls.Add(this.ribbonButton7);
            this.Controls.Add(this.ribbonButton6);
            this.Controls.Add(this.ribbonButton5);
            this.Controls.Add(this.ribbonButton4);
            this.Controls.Add(this.ribbonButton3);
            this.Controls.Add(this.ribbonButton2);
            this.Controls.Add(this.ribbonButton1);
            this.Name = "NuGenViewPopupMenu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        private Genetibase.UI.RibbonButton ribbonButton1;
        private Genetibase.UI.RibbonButton ribbonButton2;
        private Genetibase.UI.RibbonButton ribbonButton3;
        private Genetibase.UI.RibbonButton ribbonButton4;
        private Genetibase.UI.RibbonButton ribbonButton5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Genetibase.UI.RibbonButton ribbonButton6;
        private Genetibase.UI.RibbonButton ribbonButton7;
        private Genetibase.UI.RibbonButton ribbonButton8;
        private Genetibase.UI.RibbonButton ribbonButton11;
        private Genetibase.UI.RibbonButton ribbonButton12;
        private Genetibase.UI.RibbonButton ribbonButton9;
        private System.Windows.Forms.PictureBox pictureBox3;
        private Genetibase.UI.RibbonButton ribbonButton10;
        private Genetibase.UI.RibbonButton ribbonButton13;
        private System.Windows.Forms.PictureBox pictureBox4;
        private RibbonButton ribbonButton14;
        private System.Windows.Forms.PictureBox pictureBox2;

        private void ribbonButton10_Click(object sender, EventArgs e)
        {
            RibbonControl.ColorScheme = ColorScheme.Gray;
            Handler.Refresh();            
            Refresh();
        }

        private void ribbonButton13_Click(object sender, EventArgs e)
        {
            RibbonControl.ColorScheme = ColorScheme.Blue;
            Handler.Refresh();
            Refresh();
        }

        private bool colorDlgPop;

        public bool ColorDialogPopped
        {
            get
            {
                return colorDlgPop;
            }

            set
            {
                colorDlgPop = value;
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (ColorDialogPopped)
                return;

            base.OnDeactivate(e);
        }

        private void ribbonButton14_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            ColorDialogPopped = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                RibbonControl.ColorScheme = ColorScheme.Custom;
                RibbonControl.Color = dlg.Color;
            }

            ColorDialogPopped = false;

            Handler.Refresh();
            Refresh();
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            Handler.ScaleBarView_Click(sender, e);
            ribbonButton1.IsPressed = false;
            ribbonButton2.IsPressed = true;
            ribbonButton3.IsPressed = false;
            ribbonButton4.IsPressed = false;
            ribbonButton5.IsPressed = false;
            Refresh();
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            Handler.MeasurePointsView_Click(sender, e);
            ribbonButton1.IsPressed = false;
            ribbonButton2.IsPressed = false;
            ribbonButton3.IsPressed = false;
            ribbonButton4.IsPressed = true;
            ribbonButton5.IsPressed = false;
            Refresh();
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            Handler.CurvePointsView_Click(sender, e);
            ribbonButton1.IsPressed = false;
            ribbonButton2.IsPressed = false;
            ribbonButton3.IsPressed = true;
            ribbonButton4.IsPressed = false;
            ribbonButton5.IsPressed = false;
            Refresh();
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            Handler.AxesPointsView_Click(sender, e);
            ribbonButton1.IsPressed = true;
            ribbonButton2.IsPressed = false;
            ribbonButton3.IsPressed = false;
            ribbonButton4.IsPressed = false;
            ribbonButton5.IsPressed = false;
            Refresh();
        }

        private void ribbonButton5_Click(object sender, EventArgs e)
        {
            Handler.AllPointsView_Click(sender, e);
            ribbonButton1.IsPressed = false;
            ribbonButton2.IsPressed = false;
            ribbonButton3.IsPressed = false;
            ribbonButton4.IsPressed = false;
            ribbonButton5.IsPressed = true;
            Refresh();
        }

        private void ribbonButton6_Click(object sender, EventArgs e)
        {
            OnDeactivate(e);
            Handler.GridlinesDisplay_Click(sender, e);
            ribbonButton6.IsPressed = !ribbonButton6.IsPressed;
        }

        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            Handler.CurveGeometryInfo_Click(sender, e);
        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {
            Handler.MeasureGeometryInfo_Click(sender, e);
        }

        private void ribbonButton9_Click(object sender, EventArgs e)
        {
            Handler.NoBackground_Click(sender, e);
            ribbonButton9.IsPressed = true;
            ribbonButton11.IsPressed = false;
            ribbonButton12.IsPressed = false;

            Refresh();
        }

        private void ribbonButton11_Click(object sender, EventArgs e)
        {
            Handler.ProcessedImage_Click(sender, e);

            ribbonButton9.IsPressed = false;
            ribbonButton11.IsPressed = true;
            ribbonButton12.IsPressed = false;

            Refresh();
        }

        private void ribbonButton12_Click(object sender, EventArgs e)
        {
            Handler.OriginalBackground_Click(sender, e);

            ribbonButton9.IsPressed = false;
            ribbonButton11.IsPressed = false;
            ribbonButton12.IsPressed = true;

            Refresh();
        }

        public void SetPointView(ViewPointSelection sel)
        {
            ribbonButton1.IsPressed = false;
            ribbonButton2.IsPressed = false;
            ribbonButton3.IsPressed = false;
            ribbonButton4.IsPressed = false;
            ribbonButton5.IsPressed = false;
            
            switch (sel)
            {
                case ViewPointSelection.ViewAllPoints:
                    ribbonButton5.IsPressed = true; break;
                case ViewPointSelection.ViewAxesPoints:
                    ribbonButton1.IsPressed = true; break;
                case ViewPointSelection.ViewCurvePoints:
                    ribbonButton3.IsPressed = true; break;
                case ViewPointSelection.ViewMeasurePoints:
                    ribbonButton4.IsPressed = true; break;
                case ViewPointSelection.ViewScalePoints:
                    ribbonButton2.IsPressed = true; break;
            }

            Refresh();
        }

        public void SetBackgroundView(BackgroundSelection sel)
        {
            ribbonButton9.IsPressed = false;
            ribbonButton11.IsPressed = false;
            ribbonButton12.IsPressed = false;

            switch (sel)
            {
                case BackgroundSelection.BlankBackground:
                    ribbonButton9.IsPressed = true; break;
                case BackgroundSelection.OriginalImage:
                    ribbonButton12.IsPressed = true; break;
                case BackgroundSelection.ProcessedImage:
                    ribbonButton11.IsPressed = true; break;
            }

            Refresh();
        }
    }
}
