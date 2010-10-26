/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.ApplicationBlocks.Properties.Resources;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class FormatCombo : NuGenComboBox
		{
			public NuGenImageFileFormat FileFormat
			{
				get
				{
					string text = this.Text;

					if (text == res.FileFormat_JPEG)
					{
						return NuGenImageFileFormat.Jpeg;
					}
					else if (text == res.FileFormat_PNG)
					{
						return NuGenImageFileFormat.Png;
					}
					else if (text == res.FileFormat_TIFF)
					{
						return NuGenImageFileFormat.Tiff;
					}
					else if (text == res.FileFormat_BMP)
					{
						return NuGenImageFileFormat.Bmp;
					}

					return NuGenImageFileFormat.None;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="FormatCombo"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenImageListService"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public FormatCombo(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.DropDownStyle = ComboBoxStyle.DropDownList;
				this.Items.AddRange(
					new object[]
					{
						res.FileFormat_JPEG
						, res.FileFormat_PNG
						, res.FileFormat_TIFF
						, res.FileFormat_BMP
					}
				);
				this.SelectedIndex = 0;
				this.Width = 100;
			}
		}
	}
}
