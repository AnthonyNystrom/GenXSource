/* -----------------------------------------------
 * JobObject.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using J = Genetibase.PerformanceCounters.NuGenJobObjectCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Job Object category.
	/// </summary>
	public static class JobObject
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(J counter)
		{
			switch (counter) 
			{
				case J.CurrentPercentKernelModeTime:
					return new PerformanceCounter(
						"Job Object",
						"Current % Kernel Mode Time",
						"_Total",
						true
						);
				case J.CurrentPercentProcessorTime:
					return new PerformanceCounter(
						"Job Object",
						"Current % Processor Time",
						"_Total",
						true
						);
				case J.CurrentPercentUserModeTime:
					return new PerformanceCounter(
						"Job Object",
						"Current % User Mode Time",
						"_Total",
						true
						);
				case J.PagesPerSec:
					return new PerformanceCounter(
						"Job Object",
						"Pages/Sec",
						"_Total",
						true
						);
				case J.ProcessCountActive:
					return new PerformanceCounter(
						"Job Object",
						"Process Count - Active",
						"_Total",
						true
						);
				case J.ProcessCountTerminated:
					return new PerformanceCounter(
					"Job Object",
					"Process Count - Terminated",
					"_Total",
					true
					);
				case J.ProcessCountTotal:
					return new PerformanceCounter(
						"Job Object",
						"Process Count - Total",
						"_Total",
						true
						);
				case J.ThisPeriodMSecKernelMode:
					return new PerformanceCounter(
						"Job Object",
						"This Period mSec - Kernel Mode",
						"_Total",
						true
						);
				case J.ThisPeriodMSecProcessor:
					return new PerformanceCounter(
						"Job Object",
						"This Period mSec - Processor",
						"_Total",
						true
						);
				case J.ThisPeriodMSecUserMode:
					return new PerformanceCounter(
						"Job Object",
						"This Period mSec - User Mode",
						"_Total",
						true
						);
				case J.TotalMSecKernelMode:
					return new PerformanceCounter(
						"Job Object",
						"Total mSec - Kernel Mode",
						"_Total",
						true
						);
				case J.TotalMSecProcessor:
					return new PerformanceCounter(
						"Job Object",
						"Total mSec - Processor",
						"_Total",
						true
						);
				case J.TotalMSecUserMode:
					return new PerformanceCounter(
						"Job Object",
						"Total mSec - User Mode",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
