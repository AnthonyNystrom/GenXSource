using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class AxisPointDialog : Form
    {
        public AxisPointDialog()
        {
            InitializeComponent();

            this.MaximumSize = Size;
        }

        public double XTheta
        {
            get
            {
                double result = 0.0;
                try
                {
                    result = double.Parse(textBox1.Text);
                }
                catch (Exception e)
                {
                    return 0.0;
                }

                return result;
            }
        }

        public double YR
        {
            get
            {
                double result = 0.0;
                try
                {
                    result = double.Parse(textBox2.Text);
                }
                catch (Exception e)
                {
                    return 0.0;
                }

                return result;
            }
        }
    }    
}