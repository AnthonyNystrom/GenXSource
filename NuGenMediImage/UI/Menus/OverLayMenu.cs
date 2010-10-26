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
    public partial class OverLayMenu : RibbonPopup
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

        public OverLayMenu(NuGenMediImageCtrl parent)
        {
            InitializeComponent();
            this.parent = parent;

            this.ribbonButton2.NgMediImage = this.parent;
            this.ribbonButton3.NgMediImage = this.parent;
        }        

        private void OverLayMenu_Paint(object sender, PaintEventArgs e)
        {
             e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }
        
        private void OverLayMenu_Deactivate(object sender, EventArgs e)
        {
            if (!this.cancelclose)
            {                
                this.Close();
            }
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.OverLayColor = Color.Black;

            this.Close();
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.OverLayColor = Color.White;

            Application.DoEvents();
            this.Close();
        }
    }
}
