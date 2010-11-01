/* -----------------------------------------------
 * LOGBRUSH.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines the style, color, and pattern of a physical brush. It is used by the CreateBrushIndirect 
	/// and ExtCreatePen functions.
	/// </summary>
	public struct LOGBRUSH
	{
		/// <summary>
		/// Specifies the brush style.
		/// </summary>
		public int lbStyle;

		/// <summary>
		/// Specifies the color in which the brush is to be drawn.
		/// </summary>
		public int lbColor;

		/// <summary>
		/// Specifies a hatch style.
		/// </summary>
		public int lbHatch;
	}
}
