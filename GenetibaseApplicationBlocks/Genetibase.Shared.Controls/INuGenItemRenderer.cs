/* -----------------------------------------------
 * INuGenItemRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
