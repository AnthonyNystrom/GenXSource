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
    public partial class LutMenu : RibbonPopup
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

        public LutMenu(NuGenMediImageCtrl parent)
        {
            InitializeComponent();
            this.parent = parent;

            ribbonButton1.NgMediImage = this.parent;
            ribbonButton2.NgMediImage = this.parent;
            ribbonButton3.NgMediImage = this.parent;
            ribbonButton4.NgMediImage = this.parent;
            ribbonButton5.NgMediImage = this.parent;
            ribbonButton6.NgMediImage = this.parent;
            ribbonButton7.NgMediImage = this.parent;
            ribbonButton8.NgMediImage = this.parent;
            ribbonButton9.NgMediImage = this.parent;
            ribbonButton10.NgMediImage = this.parent;
            ribbonButton11.NgMediImage = this.parent;
            ribbonButton12.NgMediImage = this.parent;
            ribbonButton13.NgMediImage = this.parent;
            ribbonButton14.NgMediImage = this.parent;
            ribbonButton15.NgMediImage = this.parent;
            
        }        

        private void LutMenu_Paint(object sender, PaintEventArgs e)
        {
             e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }
        
        private void LutMenu_Deactivate(object sender, EventArgs e)
        {
            if (!this.cancelclose)
            {                
                this.Close();
            }
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            selectedLUT = ((RibbonButton)sender).Text;
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            //ngMediImage.ClearLUT();
            this.Hide();
            ngMediImage.ReadAndApplyLUT(selectedLUT);
            Application.DoEvents();
            this.Close();            
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
