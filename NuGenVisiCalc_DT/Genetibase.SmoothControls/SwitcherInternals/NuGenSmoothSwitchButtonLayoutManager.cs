/* -----------------------------------------------
 * NuGenSmoothSwitchButtonLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SwitcherInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.SwitcherInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothSwitchButtonLayoutManager : NuGenControlLayoutManager, INuGenSwitchButtonLayoutManager
	{
		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <returns></returns>
		public Rectangle GetContentRectangle(Rectangle clientRectangle)
		{
			return Rectangle.Inflate(clientRectangle, -3, -3);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSwitchButtonLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothSwitchButtonLayoutManager()
		{
		}
	}
}
