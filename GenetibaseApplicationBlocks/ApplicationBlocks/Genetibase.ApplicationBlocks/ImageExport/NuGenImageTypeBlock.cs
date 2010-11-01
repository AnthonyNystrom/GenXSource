/* -----------------------------------------------
 * NuGenImageTypeBlock.cs
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
	/// Provides UI to select image types.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal partial class NuGenImageTypeBlock : UserControl
	{
		#region Properties.Public

		/*
		 * ImageType
		 */

		/// <summary>
		/// Gets the image types selected by user.
		/// </summary>
		public NuGenImageType ImageType
		{
			get
			{
				NuGenImageType imageType = NuGenImageType.None;

				if (this.colorCheckBox.Checked)
				{
					imageType |= NuGenImageType.Color;
				}

				if (this.grayscaleCheckBox.Checked)
				{
					imageType |= NuGenImageType.Grayscale;
				}

				if (this.monochromeCheckBox.Checked)
				{
					imageType |= NuGenImageType.Monochrome;
				}

				return imageType;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageTypeBlock"/> class.
		/// </summary>
		public NuGenImageTypeBlock()
		{
			this.InitializeComponent();
		}

		#endregion
	}
}
