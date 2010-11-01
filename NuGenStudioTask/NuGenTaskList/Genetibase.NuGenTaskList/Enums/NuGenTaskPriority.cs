/* -----------------------------------------------
 * NuGenTaskPriority.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// </summary>
	public enum NuGenTaskPriority : int
	{
		/// <summary>
		/// The highest priority.
		/// </summary>
		Critical = 1,

		/// <summary>
		/// High priority.
		/// </summary>
		Required = 2,

		/// <summary>
		/// Default priority.
		/// </summary>
		Wanted = 3,

		/// <summary>
		/// Low priority.
		/// </summary>
		WouldBeNice = 4,

		/// <summary>
		/// The lowest priority.
		/// </summary>
		Maybe = 5
	}
}
