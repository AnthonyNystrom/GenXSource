using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NuGenSurfacePlotterTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nuGenSurfacePlotter1.LoadModel("k:/rshape.3ds");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nuGenSurfacePlotter1.LoadSurface();
        }
    }
}