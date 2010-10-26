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
    public partial class ZoomBoxMenu : RibbonPopup
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

        public ZoomBoxMenu(NuGenMediImageCtrl parent)
        {
            InitializeComponent();
            this.parent = parent;

            this.ribbonButton2.NgMediImage = this.parent;
            this.ribbonButton3.NgMediImage = this.parent;
            this.ribbonButton1.NgMediImage = this.parent;
        }        

        private void ZoomBoxMenu_Paint(object sender, PaintEventArgs e)
        {
             e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }
        
        private void ZoomBoxMenu_Deactivate(object sender, EventArgs e)
        {
            if (!this.cancelclose)
            {                
                this.Close();
            }
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.ZoomBoxSize = new Size(100, 100);
            this.Close();            
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.ZoomBoxSize = new Size(125, 125);
            this.Close();            
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.ZoomBoxSize = new Size(150, 150);
            this.Close();            
        }
    }
}
