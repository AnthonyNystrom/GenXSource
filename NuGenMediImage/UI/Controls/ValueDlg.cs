using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public partial class ValueDlg : Form
    {
        public ValueDlg(double value, bool np)
        {
            InitializeComponent();

            if (np)
                this.numericUpDown1.Minimum = -1;
            else
                this.numericUpDown1.Minimum = 0;

            this.numericUpDown1.Value = (decimal)value;
        }

        public double Value
        {
            get { return (double)this.numericUpDown1.Value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
        }
    }
}
