/* -----------------------------------------------
 * INuGenTaskBoxLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.TaskBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenTaskBoxLayoutManager
	{
		/// <summary>
		/// </summary>
		int GetCollapseButtonWidth();

		/// <summary>
		/// </summary>
		int GetHeaderHeight();
	}
}
