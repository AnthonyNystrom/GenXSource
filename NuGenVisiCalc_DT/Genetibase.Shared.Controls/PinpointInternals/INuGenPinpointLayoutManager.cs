/* -----------------------------------------------
 * INuGenPinpointLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.PinpointInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPinpointLayoutManager
	{
		/// <summary>
		/// </summary>
		Rectangle GetSelectionFrameBounds(
			int desiredLeft
			, int desiredTop
			, int desiredWidth
			, int desiredHeight
			);
	}
}
