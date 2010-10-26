/* -----------------------------------------------
 * INuGenSmoothPictureBoxLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.PictureBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenSmoothPictureBoxLayoutManager
	{
		/// <summary>
		/// </summary>
		Padding GetInternalPictureBoxPadding();
	}
}
