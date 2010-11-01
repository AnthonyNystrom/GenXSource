using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenTransform
{
    public partial class ProgressDialog : Form
    {
        private double progress = 0.0;

        public ProgressDialog(string title, string text)
        {
            InitializeComponent();

            this.Text = title;
            label1.Text = text;
            this.DoubleBuffered = true;
        }

        public double Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value;
                progressBar1.Value = (int)(progress * 100);
                Refresh();
            }
        }
    }
}