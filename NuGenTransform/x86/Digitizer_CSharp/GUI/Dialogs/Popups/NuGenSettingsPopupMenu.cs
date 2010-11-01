using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    class NuGenSettingsPopupMenu : NuGenPopupMenu
    {
        private Genetibase.UI.RibbonButton ribbonButton5;
        private Genetibase.UI.RibbonButton ribbonButton2;
        private Genetibase.UI.RibbonButton ribbonButton3;
        private Genetibase.UI.RibbonButton ribbonButton4;
        private Genetibase.UI.RibbonButton ribbonButton6;
        private Genetibase.UI.RibbonButton ribbonButton7;
        private Genetibase.UI.RibbonButton ribbonButton8;
        private Genetibase.UI.RibbonButton ribbonButton9;
        private Genetibase.UI.RibbonButton ribbonButton10;
        private Genetibase.UI.RibbonButton ribbonButton11;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private Genetibase.UI.RibbonButton ribbonButton1;
    
        public NuGenSettingsPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {
            InitializeComponent();
        }

        public override void InitializeDefaults()
        {
            this.ribbonButton5.Enabled = false;
            this.ribbonButton1.Enabled = false;
            this.ribbonButton2.Enabled = false;
            this.ribbonButton3.Enabled = false;
            this.ribbonButton4.Enabled = false;
            this.ribbonButton6.Enabled = false;
            this.ribbonButton7.Enabled = false;
            this.ribbonButton8.Enabled = false;
            this.ribbonButton9.Enabled = false;
            this.ribbonButton10.Enabled = false;
            this.ribbonButton11.Enabled = false;
        }

        public override void EnableControls()
        {

            this.ribbonButton5.Enabled = true;
            this.ribbonButton1.Enabled = true;
            this.ribbonButton2.Enabled = true;
            this.ribbonButton3.Enabled = true;
            this.ribbonButton4.Enabled = true;
            this.ribbonButton6.Enabled = true;
            this.ribbonButton7.Enabled = true;
            this.ribbonButton8.Enabled = true;
            this.ribbonButton9.Enabled = true;
            this.ribbonButton10.Enabled = true;
            this.ribbonButton11.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.ribbonButton5 = new Genetibase.UI.RibbonButton();
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
            this.ribbonButton2 = new Genetibase.UI.RibbonButton();
            this.ribbonButton3 = new Genetibase.UI.RibbonButton();
            this.ribbonButton4 = new Genetibase.UI.RibbonButton();
            this.ribbonButton6 = new Genetibase.UI.RibbonButton();
            this.ribbonButton7 = new Genetibase.UI.RibbonButton();
            this.ribbonButton8 = new Genetibase.UI.RibbonButton();
            this.ribbonButton9 = new Genetibase.UI.RibbonButton();
            this.ribbonButton10 = new Genetibase.UI.RibbonButton();
            this.ribbonButton11 = new Genetibase.UI.RibbonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonButton5
            // 
            this.ribbonButton5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton5.IsFlat = true;
            this.ribbonButton5.IsPressed = false;
            this.ribbonButton5.Location = new System.Drawing.Point(5, 5);
            this.ribbonButton5.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton5.Name = "ribbonButton5";
            this.ribbonButton5.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton5.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton5.TabIndex = 3;
            this.ribbonButton5.Text = "Coordinates";
            this.ribbonButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton5.Click += new System.EventHandler(this.ribbonButton5_Click);
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 44);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton1.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton1.TabIndex = 3;
            this.ribbonButton1.Text = "Axes";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton1.Click += new System.EventHandler(this.ribbonButton1_Click);
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton2.IsFlat = true;
            this.ribbonButton2.IsPressed = false;
            this.ribbonButton2.Location = new System.Drawing.Point(5, 74);
            this.ribbonButton2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton2.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton2.TabIndex = 3;
            this.ribbonButton2.Text = "Scale Bar";
            this.ribbonButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton2.Click += new System.EventHandler(this.ribbonButton2_Click);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton3.IsFlat = true;
            this.ribbonButton3.IsPressed = false;
            this.ribbonButton3.Location = new System.Drawing.Point(5, 104);
            this.ribbonButton3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton3.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton3.TabIndex = 3;
            this.ribbonButton3.Text = "Curves";
            this.ribbonButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton3.Click += new System.EventHandler(this.ribbonButton3_Click);
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton4.IsFlat = true;
            this.ribbonButton4.IsPressed = false;
            this.ribbonButton4.Location = new System.Drawing.Point(5, 134);
            this.ribbonButton4.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton4.Name = "ribbonButton4";
            this.ribbonButton4.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton4.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton4.TabIndex = 3;
            this.ribbonButton4.Text = "Segments";
            this.ribbonButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton4.Click += new System.EventHandler(this.ribbonButton4_Click);
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton6.IsFlat = true;
            this.ribbonButton6.IsPressed = false;
            this.ribbonButton6.Location = new System.Drawing.Point(5, 164);
            this.ribbonButton6.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton6.Name = "ribbonButton6";
            this.ribbonButton6.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton6.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton6.TabIndex = 3;
            this.ribbonButton6.Text = "Point Match";
            this.ribbonButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton6.Click += new System.EventHandler(this.ribbonButton6_Click);
            // 
            // ribbonButton7
            // 
            this.ribbonButton7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton7.IsFlat = true;
            this.ribbonButton7.IsPressed = false;
            this.ribbonButton7.Location = new System.Drawing.Point(5, 194);
            this.ribbonButton7.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton7.Name = "ribbonButton7";
            this.ribbonButton7.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton7.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton7.TabIndex = 3;
            this.ribbonButton7.Text = "Measures";
            this.ribbonButton7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton7.Click += new System.EventHandler(this.ribbonButton7_Click);
            // 
            // ribbonButton8
            // 
            this.ribbonButton8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton8.IsFlat = true;
            this.ribbonButton8.IsPressed = false;
            this.ribbonButton8.Location = new System.Drawing.Point(5, 233);
            this.ribbonButton8.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton8.Name = "ribbonButton8";
            this.ribbonButton8.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton8.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton8.TabIndex = 3;
            this.ribbonButton8.Text = "Discretize";
            this.ribbonButton8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton8.Click += new System.EventHandler(this.ribbonButton8_Click);
            // 
            // ribbonButton9
            // 
            this.ribbonButton9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton9.IsFlat = true;
            this.ribbonButton9.IsPressed = false;
            this.ribbonButton9.Location = new System.Drawing.Point(5, 263);
            this.ribbonButton9.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton9.Name = "ribbonButton9";
            this.ribbonButton9.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton9.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton9.TabIndex = 3;
            this.ribbonButton9.Text = "Grid Removal";
            this.ribbonButton9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton9.Click += new System.EventHandler(this.ribbonButton9_Click);
            // 
            // ribbonButton10
            // 
            this.ribbonButton10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton10.IsFlat = true;
            this.ribbonButton10.IsPressed = false;
            this.ribbonButton10.Location = new System.Drawing.Point(5, 302);
            this.ribbonButton10.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton10.Name = "ribbonButton10";
            this.ribbonButton10.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton10.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton10.TabIndex = 3;
            this.ribbonButton10.Text = "Grid Display";
            this.ribbonButton10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton10.Click += new System.EventHandler(this.ribbonButton10_Click);
            // 
            // ribbonButton11
            // 
            this.ribbonButton11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton11.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton11.IsFlat = true;
            this.ribbonButton11.IsPressed = false;
            this.ribbonButton11.Location = new System.Drawing.Point(5, 341);
            this.ribbonButton11.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton11.Name = "ribbonButton11";
            this.ribbonButton11.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton11.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton11.TabIndex = 3;
            this.ribbonButton11.Text = "Export Setup";
            this.ribbonButton11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton11.Click += new System.EventHandler(this.ribbonButton11_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(3, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 1);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Silver;
            this.pictureBox2.Location = new System.Drawing.Point(3, 228);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(160, 1);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Silver;
            this.pictureBox3.Location = new System.Drawing.Point(2, 297);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(160, 1);
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Silver;
            this.pictureBox4.Location = new System.Drawing.Point(2, 336);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(160, 1);
            this.pictureBox4.TabIndex = 4;
            this.pictureBox4.TabStop = false;
            // 
            // NuGenSettingsPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(165, 377);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ribbonButton11);
            this.Controls.Add(this.ribbonButton10);
            this.Controls.Add(this.ribbonButton9);
            this.Controls.Add(this.ribbonButton8);
            this.Controls.Add(this.ribbonButton7);
            this.Controls.Add(this.ribbonButton6);
            this.Controls.Add(this.ribbonButton4);
            this.Controls.Add(this.ribbonButton3);
            this.Controls.Add(this.ribbonButton2);
            this.Controls.Add(this.ribbonButton1);
            this.Controls.Add(this.ribbonButton5);
            this.Name = "NuGenSettingsPopupMenu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        private void ribbonButton11_Click(object sender, EventArgs e)
        {
            Handler.Export_Settings_Click(sender, e);
        }

        private void ribbonButton10_Click(object sender, EventArgs e)
        {
            Handler.GridDisplay_Settings_Click(sender, e);
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            Handler.Segments_Settings_Click(sender, e);
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            Handler.Axes_Settings_Click(sender, e);
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            Handler.ScaleBar_Settings_Click(sender, e);
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            Handler.Curves_Settings_Click(sender, e);
        }

        private void ribbonButton5_Click(object sender, EventArgs e)
        {
            Handler.Coordinates_Click(sender, e);
        }

        private void ribbonButton6_Click(object sender, EventArgs e)
        {
            Handler.PointMatch_Settings_Click(sender, e);
        }

        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            Handler.Measures_Settings_Click(sender, e);
        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {
            Handler.Discretize_Settings_Click(sender, e);
        }

        private void ribbonButton9_Click(object sender, EventArgs e)
        {
            Handler.GridRemoval_Settings_Click(sender, e);
        }
    }
}
