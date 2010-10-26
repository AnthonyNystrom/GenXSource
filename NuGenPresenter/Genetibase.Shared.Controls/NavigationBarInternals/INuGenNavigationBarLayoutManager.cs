/* -----------------------------------------------
 * INuGenNavigationBarLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.NavigationBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenNavigationBarLayoutManager
	{
		/// <summary>
		/// </summary>
		int GetButtonHeight();

		/// <summary>
		/// </summary>
		int GetBottomContainerLeftMargin();

		/// <summary>
		/// </summary>
		int GetGripHeight();

		/// <summary>
		/// </summary>
		int GetSmallButtonWidth();

		/// <summary>
		/// </summary>
		int GetTitleHeight();
	}
}
