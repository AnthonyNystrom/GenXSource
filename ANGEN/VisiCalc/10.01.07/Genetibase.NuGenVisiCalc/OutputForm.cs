/* -----------------------------------------------
 * OutputForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Genetibase.NuGenVisiCalc.Properties;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed partial class OutputForm : FloatingToolForm
	{
		public void ClearLog()
		{
			_logListBox.Items.Clear();
		}

		public void Log(String message)
		{
			_logListBox.Items.Insert(0, message);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			Settings.Default.OutputForm_Location = Location;
			Settings.Default.OutputForm_Size = Size;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Location = Settings.Default.OutputForm_Location;
			Size = Settings.Default.OutputForm_Size;
		}

		private void _clearButton_Click(object sender, EventArgs e)
		{
			ClearLog();
		}

		public OutputForm()
		{
			InitializeComponent();
		}
	}
}