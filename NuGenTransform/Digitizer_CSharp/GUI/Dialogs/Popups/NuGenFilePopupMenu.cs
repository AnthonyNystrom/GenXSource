using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    class NuGenFilePopupMenu : NuGenPopupMenu
    {
        private Genetibase.UI.RibbonButton ribbonButton1; //Import
        private Genetibase.UI.RibbonButton ribbonButton2; //Export
        private Genetibase.UI.RibbonButton ribbonButton3; //ExportAs
        private Genetibase.UI.RibbonButton ribbonButton4; //Save
        private Genetibase.UI.RibbonButton ribbonButton5; //SaveAs
        private Genetibase.UI.RibbonButton ribbonButton6; //Open
    
        public NuGenFilePopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent):base(handler, parent)
        {
            InitializeComponent();   
        }

        public override void InitializeDefaults()
        {
            this.ribbonButton2.Enabled = false;
            this.ribbonButton3.Enabled = false;
            this.ribbonButton4.Enabled = false;
            this.ribbonButton5.Enabled = false;
        }

        public override void EnableControls()
        {

            this.ribbonButton1.Enabled = true;
            this.ribbonButton2.Enabled = true;
            this.ribbonButton3.Enabled = true;
            this.ribbonButton4.Enabled = true;
            this.ribbonButton5.Enabled = true;
            this.ribbonButton6.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
            this.ribbonButton2 = new Genetibase.UI.RibbonButton();
            this.ribbonButton3 = new Genetibase.UI.RibbonButton();
            this.ribbonButton4 = new Genetibase.UI.RibbonButton();
            this.ribbonButton5 = new Genetibase.UI.RibbonButton();
            this.ribbonButton6 = new Genetibase.UI.RibbonButton();
            this.SuspendLayout();
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 5);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton1.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton1.TabIndex = 4;
            this.ribbonButton1.Text = "Import";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton1.Click += new EventHandler(ActionClicked);
            this.ribbonButton1.Click += new EventHandler(Handler.Import_Click);
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton2.IsFlat = true;
            this.ribbonButton2.IsPressed = false;
            this.ribbonButton2.Location = new System.Drawing.Point(5, 35);
            this.ribbonButton2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton2.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton2.TabIndex = 4;
            this.ribbonButton2.Text = "Export";
            this.ribbonButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton2.Click += new EventHandler(ActionClicked);
            this.ribbonButton2.Click += new EventHandler(Handler.Export_Click);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton3.IsFlat = true;
            this.ribbonButton3.IsPressed = false;
            this.ribbonButton3.Location = new System.Drawing.Point(5, 65);
            this.ribbonButton3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton3.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton3.TabIndex = 4;
            this.ribbonButton3.Text = "Export As...";
            this.ribbonButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton3.Click += new EventHandler(ActionClicked);
            this.ribbonButton3.Click += new EventHandler(Handler.ExportAs_Click);
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton4.IsFlat = true;
            this.ribbonButton4.IsPressed = false;
            this.ribbonButton4.Location = new System.Drawing.Point(5, 95);
            this.ribbonButton4.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton4.Name = "ribbonButton4";
            this.ribbonButton4.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton4.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton4.TabIndex = 4;
            this.ribbonButton4.Text = "Save";
            this.ribbonButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton4.Click += new EventHandler(ActionClicked);
            this.ribbonButton4.Click += new EventHandler(Handler.Save_Click);
            // 
            // ribbonButton5
            // 
            this.ribbonButton5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton5.IsFlat = true;
            this.ribbonButton5.IsPressed = false;
            this.ribbonButton5.Location = new System.Drawing.Point(5, 125);
            this.ribbonButton5.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton5.Name = "ribbonButton5";
            this.ribbonButton5.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton5.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton5.TabIndex = 4;
            this.ribbonButton5.Text = "Save As...";
            this.ribbonButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton5.Click += new EventHandler(ActionClicked);
            this.ribbonButton5.Click += new EventHandler(Handler.SaveAs_Click);
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton6.IsFlat = true;
            this.ribbonButton6.IsPressed = false;
            this.ribbonButton6.Location = new System.Drawing.Point(5, 155);
            this.ribbonButton6.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton6.Name = "ribbonButton6";
            this.ribbonButton6.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton6.Size = new System.Drawing.Size(155, 30);
            this.ribbonButton6.TabIndex = 4;
            this.ribbonButton6.Text = "Open";
            this.ribbonButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton6.Click += new EventHandler(ActionClicked);
            this.ribbonButton6.Click += new EventHandler(Handler.Open_Click);
            // 
            // NuGenWindowPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(165, 190);
            this.Controls.Add(this.ribbonButton1);
            this.Controls.Add(this.ribbonButton2);
            this.Controls.Add(this.ribbonButton3);
            this.Controls.Add(this.ribbonButton4);
            this.Controls.Add(this.ribbonButton5);
            this.Controls.Add(this.ribbonButton6);
            this.Name = "NuGenWindowPopupMenu";
            this.ResumeLayout(false);

        }

        void ActionClicked(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
