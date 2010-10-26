using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.Drawing;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class frmBF : Form
    {
        public BoxFilterProp BoxFilterProp
        {
            get
            {
               return this.boxFilterCtrl1.BoxFilterProp;
            }

            set
            {
                this.boxFilterCtrl1.BoxFilterProp = value;
            }
        }
        
        public frmBF()
        {
            InitializeComponent();
        }

        private void ribbonButtonOp5OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Application.DoEvents();
        }

        private void ribbonButtonOp5Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Application.DoEvents();
        }
    }
}
