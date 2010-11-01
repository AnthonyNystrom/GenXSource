/* -----------------------------------------------
 * NuGenSimpleControlStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Supports <see cref="NuGenControlState.Normal"/>, <see cref="NuGenControlState.Focused"/>, and
	/// <see cref="NuGenControlState.Disabled"/> states.
	/// </summary>
	public class NuGenSimpleControlStateTracker : NuGenControlStateTrackerBase
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSimpleControlStateTracker"/> class.
		/// </summary>
		public NuGenSimpleControlStateTracker()
		{
		}

		#endregion
	}
}
