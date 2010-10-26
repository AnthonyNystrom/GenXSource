/* -----------------------------------------------
 * PagingFile.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using P = Genetibase.PerformanceCounters.NuGenPagingFileCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Paging File category.
	/// </summary>
	public static class PagingFile
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(P counter)
		{
			switch (counter) 
			{
				case P.PercentUsage:
					return new PerformanceCounter(
						"Paging File",
						"% Usage",
						"_Total",
						true
						);
				case P.PercentUsagePeak:
					return new PerformanceCounter(
						"Paging File",
						"% Usage Peak",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
