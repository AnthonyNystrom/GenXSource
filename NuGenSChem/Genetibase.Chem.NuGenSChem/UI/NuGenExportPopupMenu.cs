using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Chem.NuGenSChem
{
    class NuGenExportPopupMenu : NuGenPopupMenu
    {
        private Genetibase.UI.RibbonButton exportMOLButton;
        private Genetibase.UI.RibbonButton ribbonButton1;
        private Genetibase.UI.RibbonButton exportCMLButton;

        public NuGenExportPopupMenu(NuGenEventHandler handler, NuGenPopupMenu parent)
            : base(handler, parent)
        {
            InitializeComponent();
        }
    
        private void InitializeComponent()
        {
            this.exportMOLButton = new Genetibase.UI.RibbonButton();
            this.exportCMLButton = new Genetibase.UI.RibbonButton();
            this.ribbonButton1 = new Genetibase.UI.RibbonButton();
            this.SuspendLayout();
            // 
            // exportMOLButton
            // 
            this.exportMOLButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.exportMOLButton.Command = null;
            this.exportMOLButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exportMOLButton.IsFlat = true;
            this.exportMOLButton.IsPressed = false;
            this.exportMOLButton.Location = new System.Drawing.Point(5, 5);
            this.exportMOLButton.Margin = new System.Windows.Forms.Padding(1);
            this.exportMOLButton.Name = "exportMOLButton";
            this.exportMOLButton.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.exportMOLButton.Size = new System.Drawing.Size(155, 36);
            this.exportMOLButton.TabIndex = 3;
            this.exportMOLButton.Text = "Export As MDL MOL";
            this.exportMOLButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.exportMOLButton.Click += new System.EventHandler(this.exportMOLButton_Click);
            // 
            // exportCMLButton
            // 
            this.exportCMLButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.exportCMLButton.Command = null;
            this.exportCMLButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.exportCMLButton.IsFlat = true;
            this.exportCMLButton.IsPressed = false;
            this.exportCMLButton.Location = new System.Drawing.Point(5, 41);
            this.exportCMLButton.Margin = new System.Windows.Forms.Padding(1);
            this.exportCMLButton.Name = "exportCMLButton";
            this.exportCMLButton.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.exportCMLButton.Size = new System.Drawing.Size(155, 36);
            this.exportCMLButton.TabIndex = 3;
            this.exportCMLButton.Text = "Export As CML XML";
            this.exportCMLButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.exportCMLButton.Click += new System.EventHandler(this.exportCMLButton_Click);
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ribbonButton1.Command = null;
            this.ribbonButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButton1.IsFlat = true;
            this.ribbonButton1.IsPressed = false;
            this.ribbonButton1.Location = new System.Drawing.Point(5, 77);
            this.ribbonButton1.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.Padding = new System.Windows.Forms.Padding(25, 2, 2, 2);
            this.ribbonButton1.Size = new System.Drawing.Size(155, 36);
            this.ribbonButton1.TabIndex = 5;
            this.ribbonButton1.Text = "Export as Bitmap";
            this.ribbonButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton1.Click += new System.EventHandler(this.ribbonButton1_Click);
            // 
            // NuGenExportPopupMenu
            // 
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(165, 117);
            this.Controls.Add(this.ribbonButton1);
            this.Controls.Add(this.exportCMLButton);
            this.Controls.Add(this.exportMOLButton);
            this.Name = "NuGenExportPopupMenu";
            this.Load += new System.EventHandler(this.NuGenExportPopupMenu_Load);
            this.ResumeLayout(false);

        }

        private void exportMOLButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.ExportMOL();
        }

        private void exportCMLButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.ExportCML();
        }

        private void NuGenExportPopupMenu_Load(object sender, EventArgs e)
        {

        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Handler.ExportBMP();
        }      
    }
}
