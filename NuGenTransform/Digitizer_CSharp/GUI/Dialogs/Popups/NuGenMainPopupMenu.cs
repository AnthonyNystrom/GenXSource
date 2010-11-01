using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.UI;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Genetibase.NuGenTransform
{
    class NuGenMainPopupMenu : NuGenPopupMenu
    {
        private RibbonButton ribbonButton1;
        private RibbonButton ribbonButton2;
        private RibbonButton ribbonButton3;
        private RibbonButton ribbonButton5;
        private RibbonButton ribbonButton6;
        private RibbonButton ribbonButton7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private RibbonButton ribbonButton4;
        private RibbonButton ribbonButton8;

        private NuGenViewPopupMenu viewMenu;
        private NuGenSettingsPopupMenu settingsMenu;
        private NuGenWindowPopupMenu windowMenu;
        private NuGenFilePopupMenu fileMenu;

        public NuGenMainPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {            
            InitializeComponent();

            viewMenu = new NuGenViewPopupMenu(handler, this);
            settingsMenu = new NuGenSettingsPopupMenu(handler, this);
            windowMenu = new NuGenWindowPopupMenu(handler, this);
            fileMenu = new NuGenFilePopupMenu(handler, this);

            AddChild(viewMenu);
            AddChild(settingsMenu);
            AddChild(windowMenu);
            AddChild(fileMenu);
        }

        public override void InitializeDefaults()
        {
            base.InitializeDefaults();

            this.ribbonButton1.Enabled = false;
            this.ribbonButton2.Enabled = false;
            this.ribbonButton3.Enabled = false;
        }

        public override void EnableControls()
        {
            base.EnableControls();

            this.ribbonButton1.Enabled = true;
            this.ribbonButton2.Enabled = true;
            this.ribbonButton3.Enabled = true;
        }    
        

        private void InitializeComponent()
        {
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
            this.ribbonButton2 = new Genetibase.UI.RibbonButton();
            this.ribbonButton3 = new Genetibase.UI.RibbonButton();
            this.ribbonButton4 = new Genetibase.UI.RibbonButton();
            this.ribbonButton5 = new Genetibase.UI.RibbonButton();
            this.ribbonButton6 = new Genetibase.UI.RibbonButton();
            this.ribbonButton7 = new Genetibase.UI.RibbonButton();
            this.ribbonButton8 = new Genetibase.UI.RibbonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 230);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton1.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton1.TabIndex = 1;
            this.ribbonButton1.Text = "Close";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton1.Click += new System.EventHandler(this.ribbonButton1_Click);
            this.ribbonButton1.MouseEnter += new EventHandler(MouseEnterDefault);            
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton2.IsFlat = true;
            this.ribbonButton2.IsPressed = false;
            this.ribbonButton2.Location = new System.Drawing.Point(5, 282);
            this.ribbonButton2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton2.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton2.TabIndex = 1;
            this.ribbonButton2.Text = "Export";
            this.ribbonButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton2.Click += new System.EventHandler(this.ribbonButton2_Click);
            this.ribbonButton2.MouseEnter += new EventHandler(MouseEnterDefault);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton3.IsFlat = true;
            this.ribbonButton3.IsPressed = false;
            this.ribbonButton3.Location = new System.Drawing.Point(5, 334);
            this.ribbonButton3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton3.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton3.TabIndex = 1;
            this.ribbonButton3.Text = "Print";
            this.ribbonButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton3.Click += new System.EventHandler(this.ribbonButton3_Click);
            this.ribbonButton3.MouseEnter += new EventHandler(MouseEnterDefault);
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton4.IsFlat = true;
            this.ribbonButton4.IsPressed = false;
            this.ribbonButton4.Location = new System.Drawing.Point(5, 386);
            this.ribbonButton4.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton4.Name = "ribbonButton4";
            this.ribbonButton4.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton4.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton4.TabIndex = 1;
            this.ribbonButton4.Text = "Exit";
            this.ribbonButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton4.Click += new System.EventHandler(this.ribbonButton4_Click);
            this.ribbonButton4.MouseEnter += new EventHandler(MouseEnterDefault);
            // 
            // ribbonButton5
            // 
            this.ribbonButton5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton5.Image = global::Genetibase.NuGenTransform.Properties.Resources.greenarrow;
            this.ribbonButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton5.IsFlat = true;
            this.ribbonButton5.IsPressed = false;
            this.ribbonButton5.Location = new System.Drawing.Point(5, 5);
            this.ribbonButton5.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton5.Name = "ribbonButton5";
            this.ribbonButton5.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton5.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton5.TabIndex = 1;
            this.ribbonButton5.Text = "View";
            this.ribbonButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton5.Click += new System.EventHandler(this.ribbonButton1_Click);
            this.ribbonButton5.MouseEnter += new EventHandler(ribbonButton5_MouseEnter);
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton6.Image = global::Genetibase.NuGenTransform.Properties.Resources.greenarrow;
            this.ribbonButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton6.IsFlat = true;
            this.ribbonButton6.IsPressed = false;
            this.ribbonButton6.Location = new System.Drawing.Point(5, 57);
            this.ribbonButton6.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton6.Name = "ribbonButton6";
            this.ribbonButton6.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton6.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton6.TabIndex = 1;
            this.ribbonButton6.Text = "Settings";
            this.ribbonButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton6.Click += new System.EventHandler(this.ribbonButton1_Click);
            this.ribbonButton6.MouseEnter += new EventHandler(ribbonButton6_MouseEnter);
            // 
            // ribbonButton7
            // 
            this.ribbonButton7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton7.Image = global::Genetibase.NuGenTransform.Properties.Resources.greenarrow;
            this.ribbonButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton7.IsFlat = true;
            this.ribbonButton7.IsPressed = false;
            this.ribbonButton7.Location = new System.Drawing.Point(5, 109);
            this.ribbonButton7.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton7.Name = "ribbonButton7";
            this.ribbonButton7.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton7.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton7.TabIndex = 1;
            this.ribbonButton7.Text = "Window";
            this.ribbonButton7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton7.Click += new System.EventHandler(this.ribbonButton1_Click);
            this.ribbonButton7.MouseEnter += new EventHandler(ribbonButton7_MouseEnter);
            //
            // ribbonButton8
            //
            this.ribbonButton8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton8.Image = global::Genetibase.NuGenTransform.Properties.Resources.greenarrow;
            this.ribbonButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton8.IsFlat = true;
            this.ribbonButton8.IsPressed = false;
            this.ribbonButton8.Location = new System.Drawing.Point(5, 161);
            this.ribbonButton8.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton8.Name = "ribbonButton8";
            this.ribbonButton8.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton8.Size = new System.Drawing.Size(144, 52);
            this.ribbonButton8.TabIndex = 1;
            this.ribbonButton8.Text = "File";
            this.ribbonButton8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton8.Click += new System.EventHandler(this.ribbonButton1_Click);
            this.ribbonButton8.MouseEnter += new EventHandler(ribbonButton8_MouseEnter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(0, 221);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 1);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // NuGenPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(155, 444);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ribbonButton4);
            this.Controls.Add(this.ribbonButton3);
            this.Controls.Add(this.ribbonButton2);
            this.Controls.Add(this.ribbonButton7);
            this.Controls.Add(this.ribbonButton6);
            this.Controls.Add(this.ribbonButton5);
            this.Controls.Add(this.ribbonButton1);
            this.Controls.Add(this.ribbonButton8);
            this.Name = "NuGenPopupMenu";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.NuGenPopupMenu_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        void ribbonButton8_MouseEnter(object sender, EventArgs e)
        {
            fileMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(ribbonButton8.Width, ribbonButton5.Location.Y + 5));
            fileMenu.PoppingUp = true;
            fileMenu.Show();
            windowMenu.Hide();
            viewMenu.Hide();
            settingsMenu.Hide();
        }

        public override void ChildDeactivated()
        {
            Rectangle clientArea = new Rectangle(0, 0, Width, Height);

            Point screenCoord = PointToClient(MousePosition);

            if(!clientArea.Contains(screenCoord) && ! Focused)            
                base.OnDeactivate(null);
        }

        void MouseEnterDefault(object sender, EventArgs e)
        {
            windowMenu.Hide();
            viewMenu.Hide();
            settingsMenu.Hide();
            fileMenu.Hide();
        }

        void ribbonButton7_MouseEnter(object sender, EventArgs e)
        {
            windowMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(ribbonButton7.Width, ribbonButton5.Location.Y + 5));
            windowMenu.PoppingUp = true;
            windowMenu.Show();
            viewMenu.Hide();
            settingsMenu.Hide();
            fileMenu.Hide();
        }

        void ribbonButton6_MouseEnter(object sender, EventArgs e)
        {
            settingsMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(ribbonButton6.Width, ribbonButton5.Location.Y + 5));
            settingsMenu.PoppingUp = true;
            settingsMenu.Show();
            viewMenu.Hide();
            windowMenu.Hide();
            fileMenu.Hide();
        }

        void ribbonButton5_MouseEnter(object sender, EventArgs e)
        {
            viewMenu.Location = ((RibbonButton)sender).PointToScreen(new Point(ribbonButton5.Width, ribbonButton5.Location.Y + 5));
            viewMenu.PoppingUp = true;
            viewMenu.Show();
            settingsMenu.Hide();
            windowMenu.Hide();
            fileMenu.Hide();
        }

        void NuGenPopupMenu_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Pen p = new Pen(Brushes.Gray);
            e.Graphics.DrawRectangle(p, new Rectangle(0,0,Width -1, Height -1));
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            } catch(Exception ex )
            {
                Application.Exit();
            }
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            Handler.Print_Click(sender, e);
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            Handler.Export_Click(sender, e);
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            Handler.Close_Click(sender, e);
            OnDeactivate(e);
        }

        public void CheckedPointViewOption(ViewPointSelection sel)
        {
            viewMenu.SetPointView(sel);
        }

        public void CheckedBackgroundOption(BackgroundSelection sel)
        {
            viewMenu.SetBackgroundView(sel);
        }
    }
}
