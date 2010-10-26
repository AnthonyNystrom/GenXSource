/* -----------------------------------------------
 * INuGenButtonLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.ButtonInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenButtonLayoutManager : INuGenControlLayoutManager
	{
		/// <summary>
		/// </summary>
		Rectangle GetContentRectangle(Rectangle clientRectangle);
	}
}
