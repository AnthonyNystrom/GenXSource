using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmoothyInterface.Forms
{
	public partial class UnhandledException : Form
	{
		public UnhandledException(Exception ex)
		{
			InitializeComponent();

			tbExceptionDetails.Text = ex.ToString();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}