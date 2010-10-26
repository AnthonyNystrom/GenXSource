/* -----------------------------------------------
 * INuGenPictureBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using System;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Controls.PictureBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPictureBoxRenderer
	{
		/// <summary>
		/// </summary>
		void DrawImage(NuGenImagePaintParams imagePaintParams);
	}
}
