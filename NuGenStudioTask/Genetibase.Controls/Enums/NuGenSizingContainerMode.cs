/* -----------------------------------------------
 * NuGenSizingContainerMode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Controls
{
	/// <summary>
	/// Specifies the way the <see cref="NuGenSizingContainer"/> docks to its parent window.
	/// </summary>
	public enum NuGenSizingContainerMode
	{
		/// <summary>
		/// This <see cref="NuGenSizingContainer"/> docks to the left parent window border.
		/// </summary>
		Left,

		/// <summary>
		/// This <see cref="NuGenSizingContainer"/> docks to the right parent window border.
		/// </summary>
		Right
	}
}
