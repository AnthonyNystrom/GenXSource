/* -----------------------------------------------
 * NuGenDisplayMode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Controls
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
		ActualSize
	}
}
