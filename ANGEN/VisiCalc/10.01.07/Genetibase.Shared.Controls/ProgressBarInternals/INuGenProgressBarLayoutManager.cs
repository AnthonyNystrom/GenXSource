/* -----------------------------------------------
 * INuGenProgressBarLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.ProgressBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenProgressBarLayoutManager
	{
		/// <summary>
		/// </summary>
		Rectangle[] GetBlocks(Rectangle continuousBounds, NuGenOrientationStyle orientation);

		/// <summary>
		/// </summary>
		Rectangle GetContinuousBounds(
			Rectangle containerBounds,
			int min,
			int max,
			int value,
			NuGenOrientationStyle orientation
		);
		
		/// <summary>
		/// </summary>
		Rectangle GetMarqueeBlockBounds(
			Rectangle containerBounds,
			int offset,
			NuGenOrientationStyle orientation
		);
	}
}
