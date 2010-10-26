using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenImageWorks
{
    public partial class frmCA : Form
    {
        public ContentAlignment ContentAlignment
        {
            get
            {
                return this.contentAlignmentCtrl1.ContentAlignment;
            }
            set
            {
                contentAlignmentCtrl1.ContentAlignment = value;
            }
        }

        public frmCA()
        {
            InitializeComponent();
        }

        private void contentAlignmentCtrl1_Load(object sender, EventArgs e)
        {
            
        }

        private void ribbonButtonOp5OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void ribbonButtonOp5Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmCA_Load(object sender, EventArgs e)
        {
            this.contentAlignmentCtrl1.Invalidate(true);
        }
    }
}