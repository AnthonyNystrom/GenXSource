using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NormIllumMethods.Visual
{
    public partial class RetinexParameter : Form
    {
        public RetinexParameter()
        {
            InitializeComponent();
        }

        private double[] sigmas;
        private double[] widths;
        private int size;
        private bool ok = false;

        public double[] Sigmas 
        {
            get { return sigmas; }
            set { sigmas = value; }
        }

        public double[] Widths
        {
            get { return widths; }
            set { widths = value; }
        }

        public int FilterSize
        {
            get { return size; }
            set { size = value; }
        }

        public bool OK
        {
            get { return ok; }
            set { ok = value; }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                double sig = Convert.ToDouble(txtSigma.Text.Trim());
                double wid = Convert.ToDouble(txtWidth.Text.Trim());
                if ((sig <= 0) || (wid <= 0))
                {
                    MessageBox.Show("Sigma and Width values must be numbers bigger than zero", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                listBoxSigma.Items.Add(sig);
                listBoxWidth.Items.Add(wid);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Sigma and Width values must be numbers.", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listBoxSigma.Items.RemoveAt(listBoxSigma.SelectedIndex);
            listBoxWidth.Items.RemoveAt(listBoxSigma.SelectedIndex);
            if (listBoxSigma.Items.Count == 0) btnRemove.Enabled = false;
        }

        private void listBoxSigma_SelectedValueChanged(object sender, EventArgs e)
        {
            listBoxWidth.SelectedIndex = listBoxSigma.SelectedIndex;
            btnRemove.Enabled = true;
        }

        private void listBoxWidth_SelectedValueChanged(object sender, EventArgs e)
        {
            listBoxSigma.SelectedIndex = listBoxWidth.SelectedIndex;
            btnRemove.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int count = listBoxSigma.Items.Count;
            if (count > 0)
            {
                object[] sig = new object[count];
                object[] wid = new object[count];
                double[] sigmas = new double[sig.Length];
                double[] widths = new double[wid.Length];
                listBoxSigma.Items.CopyTo(sig, 0);
                listBoxWidth.Items.CopyTo(wid, 0);
                sig.CopyTo(sigmas,0);
                wid.CopyTo(widths, 0);
                double sum = 0;
                for (int i = 0; i < count; i++) sum += widths[i];
                if (sum != 1)
                {
                    MessageBox.Show("The sum of widths must be equal to one.", "Atention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Sigmas = sigmas;
                Widths = widths;
                FilterSize = Convert.ToInt32(combBoxSize.SelectedItem);
                OK = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RetinexParameter_Load(object sender, EventArgs e)
        {
            combBoxSize.SelectedIndex = 0;
        }

       
       
    }
}



/*

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
        
        public double Value
        {
            get { return val;}
            set { val = value;}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Value = Convert.ToDouble(txtParam.Text.Trim());
            this.Close();
        }

        
    }
}
 
*/