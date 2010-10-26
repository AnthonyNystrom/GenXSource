/* -----------------------------------------------
 * NuGenSmoothPinpointLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.PinpointInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.PinpointInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothPinpointLayoutManager : INuGenPinpointLayoutManager
	{
		/// <summary>
		/// </summary>
		public Rectangle GetSelectionFrameBounds(int desiredLeft, int desiredTop, int desiredWidth, int desiredHeight)
		{
			return new Rectangle(
				desiredLeft
				, desiredTop + 2
				, desiredWidth
				, desiredHeight
			);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothPinpointLayoutManager()
		{
		}
	}
}
