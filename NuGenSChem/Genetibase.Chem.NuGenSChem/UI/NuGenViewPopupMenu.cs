using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI;

namespace Genetibase.Chem.NuGenSChem
{
    class NuGenViewPopupMenu : NuGenPopupMenu
    {

        public NuGenViewPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {
            InitializeComponent();
        }

        public override void InitializeDefaults()
        {
            this.showElementsButton.Enabled = false;
            this.showAllElementsButton.Enabled = false;
            this.showIndicesButton.Enabled = false;
            this.showRingIDButton.Enabled = false;
            this.showCIPPriority.Enabled = false;
            this.zoomOutButton.Enabled = false;
            this.zoomInButton.Enabled = false;
            this.zoomFullButton.Enabled = false;
        }

        public override void EnableControls()
        {

            this.showElementsButton.Enabled = true;
            this.showAllElementsButton.Enabled = true;
            this.showIndicesButton.Enabled = true;
            this.showRingIDButton.Enabled = true;
            this.showCIPPriority.Enabled = true;
            this.zoomOutButton.Enabled = true;
            this.zoomInButton.Enabled = true;
            this.zoomFullButton.Enabled = true;
        }
    
        private void InitializeComponent()
        {
            this.showElementsButton = new Genetibase.UI.RibbonButton();
            this.showAllElementsButton = new Genetibase.UI.RibbonButton();
            this.showIndicesButton = new Genetibase.UI.RibbonButton();
            this.showRingIDButton = new Genetibase.UI.RibbonButton();
            this.showCIPPriority = new Genetibase.UI.RibbonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ribbonButton10 = new Genetibase.UI.RibbonButton();
            this.ribbonButton13 = new Genetibase.UI.RibbonButton();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.ribbonButton14 = new Genetibase.UI.RibbonButton();
            this.zoomOutButton = new Genetibase.UI.RibbonButton();
            this.zoomInButton = new Genetibase.UI.RibbonButton();
            this.zoomFullButton = new Genetibase.UI.RibbonButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // showElementsButton
            // 
            this.showElementsButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.showElementsButton.Command = null;
            this.showElementsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showElementsButton.IsFlat = true;
            this.showElementsButton.IsPressed = true;
            this.showElementsButton.Location = new System.Drawing.Point(5, 5);
            this.showElementsButton.Margin = new System.Windows.Forms.Padding(1);
            this.showElementsButton.Name = "showElementsButton";
            this.showElementsButton.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.showElementsButton.Size = new System.Drawing.Size(155, 34);
            this.showElementsButton.TabIndex = 2;
            this.showElementsButton.Text = "Show Elements";
            this.showElementsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showElementsButton.Click += new System.EventHandler(this.showElementsButton_Click);
            // 
            // showAllElementsButton
            // 
            this.showAllElementsButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.showAllElementsButton.Command = null;
            this.showAllElementsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showAllElementsButton.IsFlat = true;
            this.showAllElementsButton.IsPressed = false;
            this.showAllElementsButton.Location = new System.Drawing.Point(5, 38);
            this.showAllElementsButton.Margin = new System.Windows.Forms.Padding(1);
            this.showAllElementsButton.Name = "showAllElementsButton";
            this.showAllElementsButton.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.showAllElementsButton.Size = new System.Drawing.Size(155, 34);
            this.showAllElementsButton.TabIndex = 2;
            this.showAllElementsButton.Text = "Show All Elements";
            this.showAllElementsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showAllElementsButton.Click += new System.EventHandler(this.showAllElementsButton_Click);
            // 
            // showIndicesButton
            // 
            this.showIndicesButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.showIndicesButton.Command = null;
            this.showIndicesButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showIndicesButton.IsFlat = true;
            this.showIndicesButton.IsPressed = false;
            this.showIndicesButton.Location = new System.Drawing.Point(5, 72);
            this.showIndicesButton.Margin = new System.Windows.Forms.Padding(1);
            this.showIndicesButton.Name = "showIndicesButton";
            this.showIndicesButton.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.showIndicesButton.Size = new System.Drawing.Size(155, 34);
            this.showIndicesButton.TabIndex = 2;
            this.showIndicesButton.Text = "       Show Indices";
            this.showIndicesButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.showIndicesButton.Click += new System.EventHandler(this.showIndicesButton_Click);
            // 
            // showRingIDButton
            // 
            this.showRingIDButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.showRingIDButton.Command = null;
            this.showRingIDButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showRingIDButton.IsFlat = true;
            this.showRingIDButton.IsPressed = false;
            this.showRingIDButton.Location = new System.Drawing.Point(5, 106);
            this.showRingIDButton.Margin = new System.Windows.Forms.Padding(1);
            this.showRingIDButton.Name = "showRingIDButton";
            this.showRingIDButton.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.showRingIDButton.Size = new System.Drawing.Size(155, 34);
            this.showRingIDButton.TabIndex = 2;
            this.showRingIDButton.Text = "       Show Ring ID";
            this.showRingIDButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.showRingIDButton.Click += new System.EventHandler(this.showRingIDButton_Click);
            // 
            // showCIPPriority
            // 
            this.showCIPPriority.BackColor = System.Drawing.Color.WhiteSmoke;
            this.showCIPPriority.Command = null;
            this.showCIPPriority.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showCIPPriority.IsFlat = true;
            this.showCIPPriority.IsPressed = false;
            this.showCIPPriority.Location = new System.Drawing.Point(5, 140);
            this.showCIPPriority.Margin = new System.Windows.Forms.Padding(1);
            this.showCIPPriority.Name = "showCIPPriority";
            this.showCIPPriority.Padding = new System.Windows.Forms.Padding(2);
            this.showCIPPriority.Size = new System.Drawing.Size(155, 34);
            this.showCIPPriority.TabIndex = 2;
            this.showCIPPriority.Text = "Show CIP Priority";
            this.showCIPPriority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showCIPPriority.Click += new System.EventHandler(this.showCIPPriority_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.Location = new System.Drawing.Point(2, 179);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 1);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // ribbonButton10
            // 
            this.ribbonButton10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton10.Command = null;
            this.ribbonButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton10.IsFlat = true;
            this.ribbonButton10.IsPressed = false;
            this.ribbonButton10.Location = new System.Drawing.Point(5, 296);
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
            this.ribbonButton13.Command = null;
            this.ribbonButton13.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton13.IsFlat = true;
            this.ribbonButton13.IsPressed = false;
            this.ribbonButton13.Location = new System.Drawing.Point(5, 330);
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
            this.pictureBox4.Location = new System.Drawing.Point(0, 291);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(160, 1);
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // ribbonButton14
            // 
            this.ribbonButton14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton14.Command = null;
            this.ribbonButton14.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton14.IsFlat = true;
            this.ribbonButton14.IsPressed = false;
            this.ribbonButton14.Location = new System.Drawing.Point(5, 364);
            this.ribbonButton14.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton14.Name = "ribbonButton14";
            this.ribbonButton14.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton14.Size = new System.Drawing.Size(155, 34);
            this.ribbonButton14.TabIndex = 2;
            this.ribbonButton14.Text = "Custom Color Scheme";
            this.ribbonButton14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton14.Click += new System.EventHandler(this.ribbonButton14_Click);
            // 
            // zoomOutButton
            // 
            this.zoomOutButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.zoomOutButton.Command = null;
            this.zoomOutButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.zoomOutButton.IsFlat = true;
            this.zoomOutButton.IsPressed = false;
            this.zoomOutButton.Location = new System.Drawing.Point(7, 253);
            this.zoomOutButton.Margin = new System.Windows.Forms.Padding(1);
            this.zoomOutButton.Name = "zoomOutButton";
            this.zoomOutButton.Padding = new System.Windows.Forms.Padding(2);
            this.zoomOutButton.Size = new System.Drawing.Size(155, 34);
            this.zoomOutButton.TabIndex = 6;
            this.zoomOutButton.Text = "Zoom Out";
            this.zoomOutButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
            // 
            // zoomInButton
            // 
            this.zoomInButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.zoomInButton.Command = null;
            this.zoomInButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.zoomInButton.IsFlat = true;
            this.zoomInButton.IsPressed = false;
            this.zoomInButton.Location = new System.Drawing.Point(7, 219);
            this.zoomInButton.Margin = new System.Windows.Forms.Padding(1);
            this.zoomInButton.Name = "zoomInButton";
            this.zoomInButton.Padding = new System.Windows.Forms.Padding(2);
            this.zoomInButton.Size = new System.Drawing.Size(155, 34);
            this.zoomInButton.TabIndex = 4;
            this.zoomInButton.Text = "Zoom In";
            this.zoomInButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
            // 
            // zoomFullButton
            // 
            this.zoomFullButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.zoomFullButton.Command = null;
            this.zoomFullButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.zoomFullButton.IsFlat = true;
            this.zoomFullButton.IsPressed = false;
            this.zoomFullButton.Location = new System.Drawing.Point(7, 185);
            this.zoomFullButton.Margin = new System.Windows.Forms.Padding(1);
            this.zoomFullButton.Name = "zoomFullButton";
            this.zoomFullButton.Padding = new System.Windows.Forms.Padding(2);
            this.zoomFullButton.Size = new System.Drawing.Size(155, 34);
            this.zoomFullButton.TabIndex = 5;
            this.zoomFullButton.Text = "Zoom Full";
            this.zoomFullButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.zoomFullButton.Click += new System.EventHandler(this.zoomFullButton_Click);
            // 
            // NuGenViewPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(165, 404);
            this.Controls.Add(this.zoomOutButton);
            this.Controls.Add(this.zoomInButton);
            this.Controls.Add(this.zoomFullButton);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ribbonButton14);
            this.Controls.Add(this.ribbonButton13);
            this.Controls.Add(this.ribbonButton10);
            this.Controls.Add(this.showCIPPriority);
            this.Controls.Add(this.showRingIDButton);
            this.Controls.Add(this.showIndicesButton);
            this.Controls.Add(this.showAllElementsButton);
            this.Controls.Add(this.showElementsButton);
            this.Name = "NuGenViewPopupMenu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        void zoomOutButton_Click(object sender, EventArgs e)
        {
            Handler.ZoomOut();
        }

        void zoomInButton_Click(object sender, EventArgs e)
        {
            Handler.ZoomIn();
        }

        void zoomFullButton_Click(object sender, EventArgs e)
        {
            Handler.ZoomFull();
            OnDeactivate(e);
        }

        private Genetibase.UI.RibbonButton showElementsButton;
        private Genetibase.UI.RibbonButton showAllElementsButton;
        private Genetibase.UI.RibbonButton showIndicesButton;
        private Genetibase.UI.RibbonButton showRingIDButton;
        private Genetibase.UI.RibbonButton showCIPPriority;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Genetibase.UI.RibbonButton ribbonButton10;
        private Genetibase.UI.RibbonButton ribbonButton13;
        private System.Windows.Forms.PictureBox pictureBox4;
        private RibbonButton zoomOutButton;
        private RibbonButton zoomInButton;
        private RibbonButton zoomFullButton;
        private RibbonButton ribbonButton14;

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

        private void showAllElementsButton_Click(object sender, EventArgs e)
        {
            Handler.ShowAllElements();
            showElementsButton.IsPressed = false;
            showAllElementsButton.IsPressed = true;
            showIndicesButton.IsPressed = false;
            showRingIDButton.IsPressed = false;
            showCIPPriority.IsPressed = false;
            Refresh();
        }

        private void showRingIDButton_Click(object sender, EventArgs e)
        {
            Handler.ShowRingID();
            showElementsButton.IsPressed = false;
            showAllElementsButton.IsPressed = false;
            showIndicesButton.IsPressed = false;
            showRingIDButton.IsPressed = true;
            showCIPPriority.IsPressed = false;
            Refresh();
        }

        private void showIndicesButton_Click(object sender, EventArgs e)
        {
            Handler.ShowIndices();
            showElementsButton.IsPressed = false;
            showAllElementsButton.IsPressed = false;
            showIndicesButton.IsPressed = true;
            showRingIDButton.IsPressed = false;
            showCIPPriority.IsPressed = false;
            Refresh();
        }

        private void showElementsButton_Click(object sender, EventArgs e)
        {
            Handler.ShowElements();
            showElementsButton.IsPressed = true;
            showAllElementsButton.IsPressed = false;
            showIndicesButton.IsPressed = false;
            showRingIDButton.IsPressed = false;
            showCIPPriority.IsPressed = false;
            Refresh();
        }

        private void showCIPPriority_Click(object sender, EventArgs e)
        {
            Handler.ShowCIPPriority();
            showElementsButton.IsPressed = false;
            showAllElementsButton.IsPressed = false;
            showIndicesButton.IsPressed = false;
            showRingIDButton.IsPressed = false;
            showCIPPriority.IsPressed = true;
            Refresh();
        }
    }
}
