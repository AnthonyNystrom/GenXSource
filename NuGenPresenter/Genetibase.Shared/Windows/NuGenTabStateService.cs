/* -----------------------------------------------
 * NuGenTabStateService.cs
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
	public class NuGenTabStateService : INuGenTabStateService
	{
		#region INuGenTabStateService Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public INuGenTabStateTracker CreateStateTracker()
		{
			return new NuGenTabStateTracker();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabStateService"/> class.
		/// </summary>
		public NuGenTabStateService()
		{

		}

		#endregion
	}
}
