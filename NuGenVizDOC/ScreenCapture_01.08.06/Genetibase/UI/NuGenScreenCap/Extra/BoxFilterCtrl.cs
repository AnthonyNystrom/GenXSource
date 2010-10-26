using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.Drawing;

namespace Genetibase.UI.NuGenImageWorks
{
    

    public partial class BoxFilterCtrl : UserControl
    {
        private BoxFilterProp bf;

        public BoxFilterProp BoxFilterProp
        {
            get 
            {
                return new BoxFilterProp((int)numericUpDown1.Value, (int)numericUpDown2.Value,label5.BackColor,label6.BackColor,label7.BackColor,checkBox1.Checked);
            }
            set
            {
                bf = new BoxFilterProp(value.BoxDepth,value.Angle,value.BoxStartColor,value.BoxEndColor,value.BackColor,value.DrawImageOnSides);
                numericUpDown2.Value = value.Angle;
                numericUpDown1.Value = value.BoxDepth;
                this.label5.BackColor = value.BoxStartColor;
                this.label6.BackColor = value.BoxEndColor;
                this.label7.BackColor = value.BackColor;
                this.checkBox1.Checked = value.DrawImageOnSides;
            }
        }

        public BoxFilterCtrl()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            DialogResult res = dlg.ShowDialog();

            if (res != DialogResult.OK)
                return;

            label5.BackColor = dlg.Color;
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            DialogResult res = dlg.ShowDialog();

            if (res != DialogResult.OK)
                return;

            label6.BackColor = dlg.Color;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            DialogResult res = dlg.ShowDialog();

            if (res != DialogResult.OK)
                return;

            label7.BackColor = dlg.Color;
        }

    }
}
