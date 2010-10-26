using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Capture
{
    public partial class frmCapture : Form
    {
        private Form parentForm;

        public Form ParentForm
        {
            get
            {
                return parentForm;
            }
            set
            {
                parentForm = value;
            }
        }

        public frmCapture()
        {
            InitializeComponent();
        }

        private void frmCapture_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void frmCapture_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.parentForm.Show();
        }
    }
}