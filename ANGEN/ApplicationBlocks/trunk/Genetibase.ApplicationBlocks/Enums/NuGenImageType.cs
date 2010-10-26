/* -----------------------------------------------
 * NuGenImageType.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// </summary>
	[Flags]
	public enum NuGenImageType
	{
		/// <summary>
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// </summary>
		Color = 0x0001,

		/// <summary>
		/// </summary>
		Monochrome = 0x0002,

		/// <summary>
		/// </summary>
		Grayscale = 0x0004
	}
}
