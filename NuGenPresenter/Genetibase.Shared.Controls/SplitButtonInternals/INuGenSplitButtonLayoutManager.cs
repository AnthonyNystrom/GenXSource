/* -----------------------------------------------
 * INuGenSplitButtonLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ButtonInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.SplitButtonInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenSplitButtonLayoutManager : INuGenControlLayoutManager
	{
		/// <summary>
		/// </summary>
		Rectangle GetArrowRectangle(
			Rectangle clientRectangle
			, RightToLeft rightToLeft
		);

		/// <summary>
		/// </summary>
		Rectangle GetContentRectangle(
			Rectangle clientRectangle
			, Rectangle arrowRectangle
			, RightToLeft rightToLeft
		);

		/// <summary>
		/// </summary>
		Rectangle GetSplitLineRectangle(
			Rectangle clientRectangle
			, Rectangle arrowRectangle
			, RightToLeft rightToLeft
		);
	}
}
