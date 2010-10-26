/* -----------------------------------------------
 * INuGenButtonStateService.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenButtonStateService
	{
		/// <summary>
		/// </summary>
		INuGenButtonStateTracker CreateStateTracker();
	}
}
