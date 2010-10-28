using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NormIllumMethods.Visual
{
    public partial class IsoAniParameter : Form
    {
        public IsoAniParameter()
        {
            InitializeComponent();
        }
        
        private double val;
        private bool ok = false;
        
        public double Value
        {
            get { return val;}
            set { val = value;}
        }

        public bool OK
        {
            get { return ok; }
            set { ok = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Value = Convert.ToDouble(txtParam.Text.Trim());
                if (Value <= 0)
                {
                    MessageBox.Show("Parameter value must be a number bigger than zero", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                OK = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parameter value must be a number", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}