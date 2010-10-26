using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Genetibase.NuGenMediImage.UI.Controls;

namespace Genetibase.NuGenMediImage.UI.Menus
{
    public partial class ZoomSizeMenu : RibbonPopup
    {            

        private bool cancelclose = false;
        private NuGenMediImageCtrl parent = null;

        private string selectedLUT = null;

        public string SelectedLUT
        {
            get
            {
                return selectedLUT;
            }
        }

        public ZoomSizeMenu(NuGenMediImageCtrl parent)
        {
            InitializeComponent();
            this.parent = parent;

            this.ribbonButton1.NgMediImage = this.parent;
            this.ribbonButton2.NgMediImage = this.parent;
            this.ribbonButton3.NgMediImage = this.parent;
        }        

        private void ZoomSizeMenu_Paint(object sender, PaintEventArgs e)
        {
             e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }
        
        private void ZoomSizeMenu_Deactivate(object sender, EventArgs e)
        {
            if (!this.cancelclose)
            {                
                this.Close();
            }
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            int level = int.Parse(((RibbonButton)sender).Text);
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.ZoomBoxZoom = level;
            this.Close();            
        }
    }
}
