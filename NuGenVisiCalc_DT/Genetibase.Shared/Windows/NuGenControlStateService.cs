/* -----------------------------------------------
 * NuGenControlStateService.cs
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
	public class NuGenControlStateService : INuGenControlStateService
	{
		#region INuGenControlStateService Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public INuGenControlStateTracker CreateStateTracker()
		{
			return new NuGenControlStateTracker();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlStateService"/> class.
		/// </summary>
		public NuGenControlStateService()
		{

		}

		#endregion
	}
}
