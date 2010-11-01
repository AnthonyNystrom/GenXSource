using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    class NuGenWindowPopupMenu : NuGenPopupMenu
    {
        private Genetibase.UI.RibbonButton ribbonButton5;
        private Genetibase.UI.RibbonButton ribbonButton1;
    
        public NuGenWindowPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {
            InitializeComponent();   
        }

        private void InitializeComponent()
        {
            this.ribbonButton5 = new Genetibase.UI.RibbonButton();
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
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
            this.ribbonButton5.TabIndex = 4;
            this.ribbonButton5.Text = "Tile";
            this.ribbonButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton5.Click += new EventHandler(ribbonButton5_Click);
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 35);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton1.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton1.TabIndex = 4;
            this.ribbonButton1.Text = "Cascade";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton1.Click += new EventHandler(ribbonButton1_Click);
            // 
            // NuGenWindowPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(165, 71);
            this.Controls.Add(this.ribbonButton1);
            this.Controls.Add(this.ribbonButton5);
            this.Name = "NuGenWindowPopupMenu";
            this.ResumeLayout(false);

        }

        void ribbonButton1_Click(object sender, EventArgs e)
        {
            Handler.Cascade_Click(sender, e);
        }

        void ribbonButton5_Click(object sender, EventArgs e)
        {
            Handler.Tile_Click(sender, e);
        }
    }
}
