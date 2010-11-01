/* -----------------------------------------------
 * NuGenFileFormatBlock.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExport
{
	/// <summary>
	/// Provides UI to select file formats.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal partial class NuGenFileFormatBlock : UserControl
	{
		#region Properties.Public

		/*
		 * FileFormat
		 */

		/// <summary>
		/// Gets the file formats selected by user.
		/// </summary>
		public NuGenImageFileFormat FileFormat
		{
			get
			{
				NuGenImageFileFormat fileFormat = NuGenImageFileFormat.None;

				if (this.bmpCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Bmp;
				}

				if (this.emfCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Emf;
				}

				if (this.exifCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Exif;
				}

				if (this.gifCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Gif;
				}

				if (this.jpegCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Jpeg;
				}

				if (this.pngCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Png;
				}

				if (this.tiffCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Tiff;
				}

				if (this.wmfCheckBox.Checked)
				{
					fileFormat |= NuGenImageFileFormat.Wmf;
				}

				return fileFormat;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFileFormatBlock"/> class.
		/// </summary>
		public NuGenFileFormatBlock()
		{
			this.InitializeComponent();
		}

		#endregion
	}
}
