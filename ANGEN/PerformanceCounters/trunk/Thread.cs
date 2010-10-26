/* -----------------------------------------------
 * Thread.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using T = Genetibase.PerformanceCounters.NuGenThreadCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Thread category.
	/// </summary>
	public static class Thread
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(T counter)
		{			
			switch (counter) 
			{
				case T.ContextSwitchesPerSec:
					return new PerformanceCounter(
						"Thread",
						"Context Switches/sec",
						"_Total",
						true
						);
				case T.ElapsedTime:
					return new PerformanceCounter(
						"Thread",
						"Elapsed Time",
						"_Total",
						true
						);
				case T.IdProcess:
					return new PerformanceCounter(
						"Thread",
						"ID Process",
						"_Total",
						true
						);
				case T.IdThread:
					return new PerformanceCounter(
						"Thread",
						"ID Thread",
						"_Total",
						true
						);
				case T.PercentPrivilegedTime:
					return new PerformanceCounter(
						"Thread",
						"% Privileged Time",
						"_Total",
						true
						);
				case T.PercentProcessorTime:
					return new PerformanceCounter(
						"Thread",
						"% Processor Time",
						"_Total",
						true
						);
				case T.PercentUserTime:
					return new PerformanceCounter(
						"Thread",
						"% User Time",
						"_Total",
						true
						);
				case T.PriorityBase:
					return new PerformanceCounter(
						"Thread",
						"Priority Base",
						"_Total",
						true
						);
				case T.PriorityCurrent:
					return new PerformanceCounter(
						"Thread",
						"Priority Current",
						"_Total",
						true
						);
				case T.StartAddress:
					return new PerformanceCounter(
						"Thread",
						"Start Address",
						"_Total",
						true
						);
				case T.ThreadState:
					return new PerformanceCounter(
						"Thread",
						"Thread State",
						"_Total",
						true
						);
				case T.ThreadWaitReason:
					return new PerformanceCounter(
						"Thread",
						"Thread Wait Reason",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
