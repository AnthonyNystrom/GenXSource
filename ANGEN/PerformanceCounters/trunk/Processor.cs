/* -----------------------------------------------
 * Processor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using P = Genetibase.PerformanceCounters.NuGenProcessorCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Processor category.
	/// </summary>
	public static class Processor
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
				case P.C1TransitionsPerSec:
					return new PerformanceCounter(
						"Processor",
						@"C1 Transitions/sec",
						"_Total",
						true
						);
				case P.C2TransitionsPerSec:
					return new PerformanceCounter(
						"Processor",
						@"C2 Transitions/sec",
						"_Total",
						true
						);
				case P.C3TransitionsPerSec:
					return new PerformanceCounter(
						"Processor",
						@"C3 Transitions/sec",
						"_Total",
						true
						);
				case P.DpcRate:
					return new PerformanceCounter(
						"Processor",
						"DPC Rate",
						"_Total",
						true
						);
				case P.DPCsQueuedPerSec:
					return new PerformanceCounter(
						"Processor",
						@"DPCs Queued/sec",
						"_Total",
						true
						);
				case P.InterruptsPerSec:
					return new PerformanceCounter(
						"Processor",
						@"Interrupts/sec",
						"_Total",
						true
						);
				case P.PercentC1Time:
					return new PerformanceCounter(
						"Processor",
						"% C1 Time",
						"_Total",
						true
						);
				case P.PercentC2Time:
					return new PerformanceCounter(
						"Processor",
						"% C2 Time",
						"_Total",
						true
						);
				case P.PercentC3Time:
					return new PerformanceCounter(
						"Processor",
						"% C3 Time",
						"_Total",
						true
						);
				case P.PercentDpcTime:
					return new PerformanceCounter(
						"Processor",
						"% DPC Time",
						"_Total",
						true
						);
				case P.PercentIdleTime:
					return new PerformanceCounter(
						"Processor",
						"% Idle Time",
						"_Total",
						true
						);
				case P.PercentInterruptTime:
					return new PerformanceCounter(
						"Processor",
						"% Interrupt Time",
						"_Total",
						true
						);
				case P.PercentPrivilegedTime:
					return new PerformanceCounter(
						"Processor",
						"% Privileged Time",
						"_Total",
						true
						);
				case P.PercentProcessorTime:
					return new PerformanceCounter(
						"Processor",
						"% Processor Time",
						"_Total",
						true
						);
				case P.PercentUserTime:
					return new PerformanceCounter(
						"Processor",
						"% User Time",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
