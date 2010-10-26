using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class frmFE : Form
    {
        public frmFE()
        {
            InitializeComponent();
        }

        public int Curvature
        {
            get
            {
                return (int)numericUpDown2.Value;
            }
            set
            {
                numericUpDown2.Value = value;
            }
        }

        private void frmFE_Load(object sender, EventArgs e)
        {
            this.Curvature = Effects2.curvature;
        }

        private void ribbonButtonOp5OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; 
            this.Close();
        }

        private void ribbonButtonOp5Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}