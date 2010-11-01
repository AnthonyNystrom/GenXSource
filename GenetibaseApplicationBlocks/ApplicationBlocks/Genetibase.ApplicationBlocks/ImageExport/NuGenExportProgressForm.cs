/* -----------------------------------------------
 * NuGenExportProgressForm.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// Displays export progress to the user.
	/// </summary>
	internal partial class NuGenExportProgressForm : Form
	{
		#region Declarations.Fields

		private bool _shouldCancel;

		#endregion

		#region Declarations.Delegates

		private delegate void ExportDelegate(
			Image image,
			NuGenImageType imageType,
			NuGenImageFileFormat fileFormat,
			Size resolution,
			string path,
			string filename
		);

		#endregion

		#region Methods.Public

		/// <summary>
		/// Shows the dialog to the user.
		/// </summary>
		/// <param name="image">Specifies the image to export.</param>
		/// <param name="imageType">Specifies image types (i.e. color, grayscale, monochrome).</param>
		/// <param name="resolution">Specifies the resolution for the image.</param>
		/// <param name="fileFormat">Specifies file formats (i.e. BMP, JPEG, etc).</param>
		/// <param name="path">Specifies destination directory.</param>
		/// <param name="filename">Specifies the string that is used in the filename pattern.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="image"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// <para>
		///		Width or height of the <paramref name="resolution"/> structure are not positive.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="path"/> is <see langword="null"/> or an empty string.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="filename"/> is <see langword="null"/> or an empty string.
		/// </para>
		/// </exception>
		public void ShowDialog(
			Image image,
			NuGenImageType imageType,
			NuGenImageFileFormat fileFormat,
			Size resolution,
			string path,
			string filename
			)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			this.SetProgressBarParams(_progressBar, imageType, fileFormat);

			NuGenImageExport export = new NuGenImageExport();
			export.Progress += this.exportProgress;

			ExportDelegate exportDelegate = new ExportDelegate(export.Export);
			exportDelegate.BeginInvoke(
				image,
				imageType,
				fileFormat,
				resolution,
				path,
				filename,
				new AsyncCallback(
					delegate
					{
						if (this.IsHandleCreated)
						{
							this.BeginInvoke(new MethodInvoker(
								delegate
								{
									this.Close();
								})
							);
						}
					}
				),
				null
			);

			base.ShowDialog();
		}

		#endregion

		#region Methods.Private

		private void SetProgressBarParams(ProgressBar progressBar, NuGenImageType imageType, NuGenImageFileFormat fileFormat)
		{
			Debug.Assert(progressBar != null, "progressBar != null");

			progressBar.Minimum = 0;
			progressBar.Maximum = NuGenEnum.FlagsSetOn(imageType) * NuGenEnum.FlagsSetOn(fileFormat);
		}

		#endregion

		#region EventHandlers

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			_shouldCancel = true;
		}

		private void exportProgress(object sender, CancelEventArgs e)
		{
			e.Cancel = _shouldCancel;

			if (_progressBar.IsHandleCreated)
			{
				_progressBar.BeginInvoke(new MethodInvoker(delegate
				{
					_progressBar.Value++;
				}));
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenExportProgressForm"/> class.
		/// </summary>
		public NuGenExportProgressForm()
		{
			this.InitializeComponent();
		}

		#endregion
	}
}
