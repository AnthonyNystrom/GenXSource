/* -----------------------------------------------
 * NuGenExportForm.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExport;
using Genetibase.Controls;
using Genetibase.Shared;

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Configures image export options.
	/// </summary>
	public partial class NuGenImageExportForm : Form
	{
		#region Declarations.Components

		private Container components = null;

		#endregion

		#region Methods.Public

		/*
		 * Show
		 */

		/// <summary>
		/// Shows the form to the user.
		/// </summary>
		/// <param name="imageToExport">Specifies the image to export.</param>
		public void Show(Image imageToExport)
		{
			if (imageToExport == null)
			{
				throw new ArgumentNullException("imageToExport");
			}

			Debug.Assert(this.pictureBox != null, "this.pictureBox != null");
			Debug.Assert(this.resolutionBlock != null, "this.resolutionBlock != null");

			this.pictureBox.Image = imageToExport;
			this.resolutionBlock.SetResolutionFromImage(imageToExport);

			base.Show();
		}

		/*
		 * ShowDialog
		 */

		/// <summary>
		/// Shows the form as a modal dialog.
		/// </summary>
		/// <param name="imageToExport">Specifies the <see cref="T:Image"/> to export.</param>
		public void ShowDialog(Image imageToExport)
		{
			if (imageToExport == null)
			{
				throw new ArgumentNullException("imageToExport");
			}

			Debug.Assert(this.pictureBox != null, "this.pictureBox != null");
			Debug.Assert(this.resolutionBlock != null, "this.resolutionBlock != null");

			this.pictureBox.Image = imageToExport;
			this.resolutionBlock.SetResolutionFromImage(imageToExport);
			
			base.ShowDialog();
		}

		#endregion

		#region EventHandlers

		private void goButton_Click(object sender, System.EventArgs e)
		{
			if (this.outputBlock.Verify() && this.resolutionBlock.Verify())
			{
				NuGenExportProgressForm exportProgressForm = new NuGenExportProgressForm();
				exportProgressForm.ShowDialog(
					this.pictureBox.Image,
					this.imageTypeBlock.ImageType,
					this.fileFormatBlock.FileFormat,
					this.resolutionBlock.Resolution,
					this.outputBlock.DirectoryName,
					this.outputBlock.Filename
				);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageExportForm"/> class.
		/// </summary>
		public NuGenImageExportForm()
		{
			this.InitializeComponent();
            frmNag Nag = new frmNag();
            Nag.ShowDialog();
            Nag.Dispose();
            Nag = null;

            
		}
		
		#endregion
	}
}
