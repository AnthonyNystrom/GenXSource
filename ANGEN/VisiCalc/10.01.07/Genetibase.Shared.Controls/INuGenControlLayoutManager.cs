/* -----------------------------------------------
 * INuGenControlLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public interface INuGenControlLayoutManager
	{
		/// <summary>
		/// </summary>
		Rectangle GetImageBounds(NuGenBoundsParams imageBoundsParams);

		/// <summary>
		/// </summary>
		Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams);
	}
}
