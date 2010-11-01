/* -----------------------------------------------
 * Program.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.Demo
{
	/// <summary>
	/// Applicaton's main form.
	/// </summary>
	public partial class MainForm : Form
	{
		#region EventHandlers

		private void goButton_Click(object sender, EventArgs e)
		{
			NuGenImageExportForm exportForm = new NuGenImageExportForm();

			try
			{
				Image image = Image.FromFile(this.openFileSelector.GetPath);
				exportForm.ShowDialog(image);
			}
			catch (ArgumentException)
			{
				MessageBox.Show("Specify the path.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show("Specified file could not be found. Make sure the file exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		#endregion
	}
}