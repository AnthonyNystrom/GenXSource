/* -----------------------------------------------
 * AboutForm.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// </summary>
	public partial class AboutForm : Form
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AboutForm"/> class.
		/// </summary>
		public AboutForm()
		{
			this.InitializeComponent();

			_versionLabel.Text = string.Format(
				"Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()
			);
		}

		#endregion
	}
}