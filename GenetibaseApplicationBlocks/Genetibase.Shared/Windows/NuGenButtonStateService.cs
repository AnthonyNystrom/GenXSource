/* -----------------------------------------------
 * NuGenButtonStateService.cs
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
	public class NuGenButtonStateService : INuGenButtonStateService
	{
		#region INuGenButtonStateService Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public INuGenButtonStateTracker CreateStateTracker()
		{
			return new NuGenButtonStateTracker();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButtonStateService"/> class.
		/// </summary>
		public NuGenButtonStateService()
		{

		}

		#endregion
	}
}
