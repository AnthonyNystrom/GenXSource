using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nuGenPSurface1.OpenSaveImageDialog();
        }

        private void nuGenPSurface1_DoubleClick(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = nuGenPSurface1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            nuGenPSurface1.XamlExport();
        }
    }
}