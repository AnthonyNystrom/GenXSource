/* -----------------------------------------------
 * INuGenItemRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public interface INuGenItemRenderer
	{
		/// <summary>
		/// </summary>
		void DrawItem(NuGenItemPaintParams paintParams);
	}
}
