/* -----------------------------------------------
 * JobObjectDetails.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using J = Genetibase.PerformanceCounters.NuGenJobObjectDetailsCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Job Object category.
	/// </summary>
	public static class JobObjectDetails
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
				case J.CreatingProcessId:
					return new PerformanceCounter(
						"Job Object Details",
						"Creating Process ID",
						"_Total",
						true
						);
				case J.ElapsedTime:
					return new PerformanceCounter(
						"Job Object Details",
						"Elapsed Time",
						"_Total",
						true
						);
				case J.HandleCount:
					return new PerformanceCounter(
						"Job Object Details",
						"Handle Count",
						"_Total",
						true
						);
				case J.IdProcess:
					return new PerformanceCounter(
						"Job Object Details",
						"ID Process",
						"_Total",
						true
						);
				case J.IoDataBytesPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Data Bytes/sec",
						"_Total",
						true
						);
				case J.IoDataOperationsPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Data Operatioins/sec",
						"_Total",
						true
						);
				case J.IoOtherBytesPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Other Bytes/sec",
						"_Total",
						true
						);
				case J.IoOtherOperationsPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Other Operations/sec",
						"_Total",
						true
						);
				case J.IoReadBytesPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Read Bytes/sec",
						"_Total",
						true
						);
				case J.IoReadOperationsPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Read Operations/sec",
						"_Total",
						true
						);
				case J.IoWriteBytesPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Write Bytes/sec",
						"_Total",
						true
						);
				case J.IoWriteOperationsPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"IO Write Operations/sec",
						"_Total",
						true
						);
				case J.PageFaultsPerSec:
					return new PerformanceCounter(
						"Job Object Details",
						"Page Faults/sec",
						"_Total",
						true
						);
				case J.PageFileBytes:
					return new PerformanceCounter(
						"Job Object Details",
						"",
						"_Total",
						true
						);
				case J.PageFileBytesPeak:
					return new PerformanceCounter(
						"Job Object Details",
						"Page File Bytes Peak",
						"_Total",
						true
						);
				case J.PercentPrivilegedTime:
					return new PerformanceCounter(
						"Job Object Details",
						"% Privileged Time",
						"_Total",
						true
						);
				case J.PercentProcessorTime:
					return new PerformanceCounter(
						"Job Object Details",
						"% Processor Time",
						"_Total",
						true
						);
				case J.PercentUserTime:
					return new PerformanceCounter(
						"Job Object Details",
						"% User Time",
						"_Total",
						true
						);
				case J.PoolNonpagedBytes:
					return new PerformanceCounter(
						"Job Object Details",
						"Pool Nonpaged Bytes",
						"_Total",
						true
						);
				case J.PoolPagedBytes:
					return new PerformanceCounter(
						"Job Object Details",
						"Pool Paged Bytes",
						"_Total",
						true
						);
				case J.PriorityBase:
					return new PerformanceCounter(
						"Job Object Details",
						"Priority Base",
						"_Total",
						true
						);
				case J.PrivateBytes:
					return new PerformanceCounter(
						"Job Object Details",
						"Private Bytes",
						"_Total",
						true
						);
				case J.ThreadCount:
					return new PerformanceCounter(
						"Job Object Details",
						"Thread Count",
						"_Total",
						true
						);
				case J.VirtualBytes:
					return new PerformanceCounter(
						"Job Object Details",
						"Virtual Bytes",
						"_Total",
						true
						);
				case J.VirtualBytesPeak:
					return new PerformanceCounter(
						"Job Object Details",
						"Virtual Bytes Peak",
						"_Total",
						true
						);
				case J.WorkingSet:
					return new PerformanceCounter(
						"Job Object Details",
						"Working Set",
						"_Total",
						true
						);
				case J.WorkingSetPeak:
					return new PerformanceCounter(
						"Job Object Details",
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
