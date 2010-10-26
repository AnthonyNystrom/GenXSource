/* -----------------------------------------------
 * INuGenTabStateService.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenTabStateService
	{
		/// <summary>
		/// </summary>
		/// <returns></returns>
		INuGenTabStateTracker CreateStateTracker();
	}
}
