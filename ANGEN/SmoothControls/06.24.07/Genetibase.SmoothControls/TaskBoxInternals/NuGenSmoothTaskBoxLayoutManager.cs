/* -----------------------------------------------
 * NuGenSmoothTaskBoxLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.TaskBoxInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.TaskBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothTaskBoxLayoutManager : INuGenTaskBoxLayoutManager
	{
		/// <summary>
		/// </summary>
		public int GetCollapseButtonWidth()
		{
			return 21;
		}

		/// <summary>
		/// </summary>
		public int GetHeaderHeight()
		{
			return 21;
		}
	}
}
