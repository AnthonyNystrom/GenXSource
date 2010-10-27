namespace FacScan
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public partial class AboutForm : Form
    {
		public AboutForm()
		{
			this.InitializeComponent();
			this.textBox1.Select(0, 0);
		}
    }
}

