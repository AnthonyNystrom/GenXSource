using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenNoiseLib.LibNoise;
using System.IO;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream stream = null;
            NoiseTextureBuilder.BuildSphericalTexture(null, ref stream, ref stream);
            pictureBox1.Image = Bitmap.FromStream(stream);
            stream.Close();
        }
    }
}