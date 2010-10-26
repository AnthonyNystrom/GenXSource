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
    public partial class MeasurementUnitsMenu : RibbonPopup
    {   
        private NuGenMediImageCtrl parent = null;

        private MeasurementUnits selectedUnit;

        public MeasurementUnits SelectedUnit
        {
            get
            {
                return selectedUnit;
            }
        }

        public MeasurementUnitsMenu(NuGenMediImageCtrl parent)
        {
            InitializeComponent();
            this.parent = parent;

            this.ribbonButton1.NgMediImage = this.parent;
            this.ribbonButton2.NgMediImage = this.parent;
            this.ribbonButton3.NgMediImage = this.parent;
            this.ribbonButton4.NgMediImage = this.parent;
            
        }        

        private void ZoomUnitsMenu_Paint(object sender, PaintEventArgs e)
        {
             e.Graphics.DrawRectangle(new Pen(Color.FromArgb(115, 115, 115)), 0, 0, this.Width - 1, this.Height - 1);
        }
        
        private void ZoomUnitsMenu_Deactivate(object sender, EventArgs e)
        {                        
             this.Close();         
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            selectedUnit = (MeasurementUnits)Enum.Parse(typeof(MeasurementUnits),((RibbonButton)sender).Text,true);
            
            NuGenMediImageCtrl ngMediImage = (NuGenMediImageCtrl)parent;
            ngMediImage.MeasurementUnits = selectedUnit;
            Application.DoEvents();
            this.Close();            
        }
    }
}
