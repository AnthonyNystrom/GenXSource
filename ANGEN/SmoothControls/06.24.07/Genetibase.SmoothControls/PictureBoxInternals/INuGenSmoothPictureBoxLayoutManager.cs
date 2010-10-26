/* -----------------------------------------------
 * INuGenSmoothPictureBoxLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
