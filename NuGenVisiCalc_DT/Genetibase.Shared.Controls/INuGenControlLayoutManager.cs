/* -----------------------------------------------
 * INuGenControlLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
