/* -----------------------------------------------
 * NuGenValueTracker.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public sealed class NuGenValueTrackerService : INuGenValueTrackerService
	{
		#region INuGenValueTrackerService Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public NuGenValueTracker CreateValueTracker()
		{
			return new NuGenValueTracker();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenValueTrackerService"/> class.
		/// </summary>
		public NuGenValueTrackerService()
		{

		}

		#endregion
	}
}
