/* -----------------------------------------------
 * Process.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using P = Genetibase.PerformanceCounters.NuGenProcessCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Process category.
	/// </summary>
	public static class Process
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
				case P.CreatingProcessId:
					return new PerformanceCounter(
						"Process",
						"Creating Process ID",
						"_Total",
						true
						);
				case P.ElapsedTime:
					return new PerformanceCounter(
						"Process",
						"Elapsed Time",
						"_Total",
						true
						);
				case P.HandleCount:
					return new PerformanceCounter(
						"Process",
						"Handle Count",
						"_Total",
						true
						);
				case P.IdProcess:
					return new PerformanceCounter(
						"Process",
						"ID Process",
						"_Total",
						true
						);
				case P.IoDataBytesPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Data Bytes/sec",
						"_Total",
						true
						);
				case P.IoDataOperationsPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Data Operations/sec",
						"_Total",
						true
						);
				case P.IoOtherBytesPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Other Bytes/sec",
						"_Total",
						true
						);
				case P.IoOtherOperationsPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO_Other_Operations/sec",
						"_Total",
						true
						);
				case P.IoReadBytesPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Read Bytes/sec",
						"_Total",
						true
						);
				case P.IoReadOperationsPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Read Operations/sec",
						"_Total",
						true
						);
				case P.IoWriteBytesPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Write Bytes/sec",
						"_Total",
						true
						);
				case P.IoWriteOperationsPerSec:
					return new PerformanceCounter(
						"Process",
						@"IO Write Operations/sec",
						"_Total",
						true
						);
				case P.PageFaultsPerSec:
					return new PerformanceCounter(
						"Process",
						@"Page Faults/sec",
						"_Total",
						true
						);
				case P.PageFileBytes:
					return new PerformanceCounter(
						"Process",
						"Page File Bytes",
						"_Total",
						true
						);
				case P.PageFileBytesPeak:
					return new PerformanceCounter(
						"Process",
						"Page File Bytes Peak",
						"_Total",
						true
						);
				case P.PercentPrivilegedTime:
					return new PerformanceCounter(
						"Process",
						"% Privileged Time",
						"_Total",
						true
						);
				case P.PercentProcessorTime:
					return new PerformanceCounter(
						"Process",
						"% Processor Time",
						"_Total",
						true
						);
				case P.PercentUserTime:
					return new PerformanceCounter(
						"Process",
						"% User Time",
						"_Total",
						true
						);
				case P.PoolNonpagedBytes:
					return new PerformanceCounter(
						"Process",
						"Pool Nonpaged Bytes",
						"_Total",
						true
						);
				case P.PoolPagedBytes:
					return new PerformanceCounter(
						"Process",
						"Pool Paged Bytes",
						"_Total",
						true
						);
				case P.PriorityBase:
					return new PerformanceCounter(
						"Process",
						"Priority Base",
						"_Total",
						true
						);
				case P.PrivateBytes:
					return new PerformanceCounter(
						"Process",
						"Private Bytes",
						"_Total",
						true
						);
				case P.ThreadCount:
					return new PerformanceCounter(
						"Process",
						"Thread Count",
						"_Total",
						true
						);
				case P.VirtualBytes:
					return new PerformanceCounter(
						"Process",
						"Virtual Bytes",
						"_Total",
						true
						);
				case P.VirtualBytesPeak:
					return new PerformanceCounter(
						"Process",
						"Virtual Bytes Peak",
						"_Total",
						true
						);
				case P.WorkingSet:
					return new PerformanceCounter(
						"Process",
						"Working Set",
						"_Total",
						true
						);
				case P.WorkingSetPeak:
					return new PerformanceCounter(
						"Process",
						"Working Set Peak",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
