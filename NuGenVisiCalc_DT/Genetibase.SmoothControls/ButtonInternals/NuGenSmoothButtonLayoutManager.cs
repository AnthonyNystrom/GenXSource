/* -----------------------------------------------
 * NuGenSmoothButtonLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.ButtonInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothButtonLayoutManager : NuGenControlLayoutManager, INuGenButtonLayoutManager
	{
		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <returns></returns>
		public Rectangle GetContentRectangle(Rectangle clientRectangle)
		{
			return Rectangle.Inflate(clientRectangle, -6, -6);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothButtonLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothButtonLayoutManager()
		{
		}
	}
}
