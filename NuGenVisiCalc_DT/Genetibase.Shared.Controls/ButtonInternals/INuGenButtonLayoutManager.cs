/* -----------------------------------------------
 * INuGenButtonLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
