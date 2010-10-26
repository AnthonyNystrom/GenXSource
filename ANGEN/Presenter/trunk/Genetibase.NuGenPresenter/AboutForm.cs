/* -----------------------------------------------
 * AboutForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.NuGenPresenter.Properties.Resources;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenPresenter
{
	internal sealed partial class AboutForm : Form
	{
		private void _siteLabel_Click(object sender, EventArgs e)
		{
			Process.Start(_siteLabel.Text);
		}

		public AboutForm()
		{
			this.InitializeComponent();
			this.SetStyle(ControlStyles.Opaque, true);

			this.Text = res.Text_AboutForm_Text;
		}
	}
}