/* -----------------------------------------------
 * PhysicalDisk.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using P = Genetibase.PerformanceCounters.NuGenPhysicalDiskCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the Physical Disk category.
	/// </summary>
	public static class PhysicalDisk
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
				case P.AvgDiskBytesPerRead:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Avg. Disk Bytes/Read",
						"_Total",
						true
						);
				case P.AvgDiskBytesPerTransfer:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Avg. Disk Bytes/Transfer",
						"_Total",
						true
						);
				case P.AvgDiskBytesPerWrite:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Avg. Disk Bytes/Write",
						"_Total",
						true
						);
				case P.AvgDiskQueueLength:
					return new PerformanceCounter(
						"PhysicalDisk",
						"Avg. Disk Queue Length",
						"_Total",
						true
						);
				case P.AvgDiskSecPerRead:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Avg. Disk sec/Read",
						"_Total",
						true
						);
				case P.AvgDiskSecPerTransfer:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Avg. Disk sec/Transfer",
						"_Total",
						true
						);
				case P.AvgDiskSecPerWrite:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Avg. Disk sec/Write",
						"_Total",
						true
						);
				case P.AvgDiskWriteQueueLength:
					return new PerformanceCounter(
						"PhysicalDisk",
						"Avg. Disk Write Queue Length",
						"_Total",
						true
						);
				case P.CurrentDiskQueueLength:
					return new PerformanceCounter(
						"PhysicalDisk",
						"Current Disk Queue Length",
						"_Total",
						true
						);
				case P.DiskBytesPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Disk Bytes/sec",
						"_Total",
						true
						);
				case P.DiskReadBytesPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Disk Read Bytes/sec",
						"_Total",
						true
						);
				case P.DiskReadsPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Disk Reads/sec",
						"_Total",
						true
						);
				case P.DiskTransfersPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Disk Transfers/sec",
						"_Total",
						true
						);
				case P.DiskWriteBytesPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Disk Write Bytes/sec",
						"_Total",
						true
						);
				case P.DiskWritesPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Disk Writes/sec",
						"_Total",
						true
						);
				case P.PercentDiskReadTime:
					return new PerformanceCounter(
						"PhysicalDisk",
						"% Disk Read Time",
						"_Total",
						true
						);
				case P.PercentDiskTime:
					return new PerformanceCounter(
						"PhysicalDisk",
						"% Disk Time",
						"_Total",
						true
						);
				case P.PercentDiskWriteTime:
					return new PerformanceCounter(
						"PhysicalDisk",
						"% Disk Write Time",
						"_Total",
						true
						);
				case P.PercentIdleTime:
					return new PerformanceCounter(
						"PhysicalDisk",
						"% Idle Time",
						"_Total",
						true
						);
				case P.SplitIoPerSec:
					return new PerformanceCounter(
						"PhysicalDisk",
						@"Split IO/sec",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
