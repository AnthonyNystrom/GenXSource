/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.ApplicationBlocks.Properties.Resources;

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
		private sealed class TypeCombo : NuGenComboBox
		{
			public NuGenImageType ImageType
			{
				get
				{
					string text = this.Text;

					if (text == res.ImageType_Color)
					{
						return NuGenImageType.Color;
					}
					else if (text == res.ImageType_Grayscale)
					{
						return NuGenImageType.Grayscale;
					}
					else if (text == res.ImageType_Monochrome)
					{
						return NuGenImageType.Monochrome;
					}

					return NuGenImageType.None;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="TypeCombo"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenImageListService"/></para></param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public TypeCombo(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.DropDownStyle = ComboBoxStyle.DropDownList;
				this.Items.AddRange(
					new object[]
					{
						res.ImageType_Color
						, res.ImageType_Grayscale
						, res.ImageType_Monochrome
					}
				);
				this.SelectedIndex = 0;
				this.Width = 100;
			}
		}
	}
}
