using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Chem;

namespace ChemDevEnv
{
    public partial class ViewTab : Form
    {
        IChemControl chemControl;

        public IChemControl ChemControl
        {
            get { return chemControl; }
        }

        public ViewTab(IChemControl chemControl)
        {
            InitializeComponent();

            this.chemControl = chemControl;

            Controls.Add((Control)chemControl);
            ((Control)chemControl).Dock = DockStyle.Fill;
            chemControl.BackColor = Color.White;//SystemColors.Control;
            //this.Text = chemControl.Title;
        }

        private void buttonCommand1_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
        {
            if (panel1.Size.Height == 23)
                panel1.Size = new Size(panel1.Size.Width, 100);
            else
                panel1.Size = new Size(panel1.Size.Width, 23);
        }
    }
}