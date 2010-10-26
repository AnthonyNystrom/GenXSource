/* -----------------------------------------------
 * NetClrLocksAndThreads.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrLT = Genetibase.PerformanceCounters.NuGenNetClrLocksAndThreadsCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR LocksAndThreads category.
	/// </summary>
	public static class NetClrLocksAndThreads
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrLT counter)
		{
			switch (counter) 
			{
				case NetClrLT.ContentionRatePerSec:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"Contention Rate / sec",
						"_Global_",
						true
						);
				case NetClrLT.CurrentQueueLength:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"Current Queue Length",
						"_Global_",
						true
						);
				case NetClrLT.NumberOfCurrentLogicalThreads:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"# of current logical Threads",
						"_Global_",
						true
						);
				case NetClrLT.NumberOfCurrentPhysicalThreads:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"# of current physical Threads",
						"_Global_",
						true
						);
				case NetClrLT.NumberOfCurrentRecognizedThreads:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"# of current recognized Threads",
						"_Global_",
						true
						);
				case NetClrLT.QueueLengthPeak:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"Queue Length Peak",
						"_Global_",
						true
						);
				case NetClrLT.QueueLengthPerSec:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"Queue Length / sec",
						"_Global_",
						true
						);
				case NetClrLT.RateOfRecognizedThreadsPerSec:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"Rate of recognized threads / sec",
						"_Global_",
						true
						);
				case NetClrLT.TotalNumberOfContentions:
					return new PerformanceCounter(
						".NET CLR LocksAndThreads",
						"Total # of Contentions",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
