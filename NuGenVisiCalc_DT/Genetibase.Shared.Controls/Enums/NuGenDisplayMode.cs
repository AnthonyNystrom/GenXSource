/* -----------------------------------------------
 * NuGenDisplayMode.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Defines the image display mode.
	/// </summary>
	public enum NuGenDisplayMode
	{
		/// <summary>
		/// Preserve aspect ratio of the image.
		/// </summary>
		ScaleToFit,

		/// <summary>
		/// Fill entire area with the image.
		/// </summary>
		StretchToFit,

		/// <summary>
		/// Draw only the visible portion of the image.
		/// </summary>
		ActualSize,

		/// <summary>
		/// Allows to set custom zoom factor.
		/// </summary>
		Zoom
	}
}
