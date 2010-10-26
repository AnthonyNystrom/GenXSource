/* -----------------------------------------------
 * INuGenTabLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenTabLayoutManager : INuGenControlLayoutManager
	{
		/// <summary>
		/// </summary>
		NuGenTabLayoutBuilder RegisterLayoutBuilder(List<NuGenTabButton> tabButtons);

		/// <summary>
		/// </summary>
		Rectangle GetCloseButtonBounds(Rectangle bounds);

		/// <summary>
		/// </summary>
		Rectangle GetContentRectangle(Rectangle clientRectangle, Rectangle closeButtonRectangle);

		/// <summary>
		/// </summary>
		Rectangle GetTabStripBounds(Rectangle tabControlBounds);

		/// <summary>
		/// </summary>
		Rectangle GetTabPageBounds(Rectangle tabControlBounds, Rectangle tabStripBounds);
	}
}
