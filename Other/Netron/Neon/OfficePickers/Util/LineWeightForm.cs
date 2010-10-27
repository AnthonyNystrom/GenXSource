using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Netron.Neon.OfficePickers
{
    public partial class LineWeightForm : Form
    {
        public LineWeightForm()
        {
            InitializeComponent();
        }

        
        /// <summary>
        /// Gets or sets the LineWeight
        /// </summary>
        public float LineWeight
        {
            get { return Convert.ToSingle(this.numericUpDown1.Value); }
            set {
                if(value>0F && value<= 10F)
                    numericUpDown1.Value = Convert.ToDecimal(value); 
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using(Pen pen = new Pen(Color.Black, Convert.ToSingle(numericUpDown1.Value)))
            {
                e.Graphics.DrawLine(pen, 12, 45, 148, 45);
            }
        }
    }
}