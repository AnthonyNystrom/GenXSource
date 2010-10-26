using System;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenDEMVis.Controls;
using Genetibase.VisUI.Controls;
using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis
{
    partial class ScreenshotDlg : Form
    {
        private UI3DVisControl visControl;
        private ScreenshotSettings scrSettings;

        public ScreenshotDlg(UI3DVisControl visControl)
        {
            InitializeComponent();
            
            this.visControl = visControl;
            scrSettings.Destination = ScreenshotSettings.OutputDestination.File;
            textBox1.Text = string.Format("{0}screenshots\\{1}--{2}.jpg", visControl.Settings["Base.Path"],
                                          visControl.Title, DateTime.Now.ToString("DD-MM-YYYY[HH.mm]"));
            scrSettings.Format = ImageFileFormat.Jpg;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            // take a screenshot
            visControl.TakeScreenshot(scrSettings);
            pictureBox1.Image = Image.FromFile(scrSettings.File);
            MessageBox.Show(this, "Screenshot taken");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            scrSettings.File = textBox1.Text;
        }

        private void integerUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                scrSettings.Resolution.Width = integerUpDown1.Value;
        }

        private void integerUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                scrSettings.Resolution.Height = integerUpDown2.Value;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                scrSettings.Resolution.Width = integerUpDown1.Value;
                scrSettings.Resolution.Height = integerUpDown2.Value;
            }
            else
            {
                scrSettings.Resolution.Width = 0;
                scrSettings.Resolution.Height = 0;
            }
        }
    }
}