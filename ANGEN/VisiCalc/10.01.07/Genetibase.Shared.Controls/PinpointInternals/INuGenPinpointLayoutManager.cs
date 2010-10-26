/* -----------------------------------------------
 * INuGenPinpointLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
