/* -----------------------------------------------
 * NuGenOutputBlock.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// Provides UI for file output parameters selection.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal partial class NuGenOutputBlock : UserControl
	{
		#region Properties.Public

		/*
		 * DirectoryName
		 */

		/// <summary>
		/// Gets or sets the directory name displayed to the user.
		/// </summary>
		public string DirectoryName
		{
			get
			{
				return this.directorySelector.Path;
			}
			set
			{
				this.directorySelector.Path = value;
			}
		}

		/*
		 * Filename
		 */

		/// <summary>
		/// Gets or sets the filename displayed to the user.
		/// </summary>
		public string Filename
		{
			get
			{
				return this.filenameTextBox.Text;
			}
			set
			{
				this.filenameTextBox.Text = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * Verify
		 */

		/// <summary>
		/// Verifies whether the data entered by the user is valid.
		/// </summary>
		/// <returns><see langword="true"/> if the entered data is valid; otherwise, <see langword="false"/>.</returns>
		public bool Verify()
		{
			if (!NuGenArgument.IsValidDirectoryName(this.directorySelector.Path))
			{
				MessageBox.Show(
					string.Format(Properties.Resources.Argument_InvalidDirectory, new string(Path.GetInvalidPathChars())),
					Properties.Resources.Message_Alert,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation
				);

				return false;
			}

			if (!NuGenArgument.IsValidFilename(this.filenameTextBox.Text))
			{
				MessageBox.Show(
					string.Format(Properties.Resources.Argument_InvalidFilename, new string(Path.GetInvalidFileNameChars())),
					Properties.Resources.Message_Alert,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation
				);

				this.filenameTextBox.SelectionStart = 0;
				this.filenameTextBox.SelectAll();
				
				return false;
			}

			return true;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOutputBlock"/> class.
		/// </summary>
		public NuGenOutputBlock()
		{
			this.InitializeComponent();
		}

		#endregion
	}
}
