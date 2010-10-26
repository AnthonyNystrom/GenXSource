/* -----------------------------------------------
 * AboutForm.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Genetibase.NuGenVisiCalc.Properties;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();

			SetStyle(ControlStyles.Opaque, true);

			_versionLabel.Text = String.Format(
				CultureInfo.CurrentUICulture
                , Resources.Text_Version
                , Assembly.GetExecutingAssembly().GetName().Version.ToString()
			);
		}
	}
}