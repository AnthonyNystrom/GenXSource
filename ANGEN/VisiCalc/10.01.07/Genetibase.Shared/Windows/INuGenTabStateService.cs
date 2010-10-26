/* -----------------------------------------------
 * INuGenTabStateService.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
