/* -----------------------------------------------
 * LogicalDisk.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using L = Genetibase.PerformanceCounters.NuGenLogicalDiskCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the LogicalDisk category.
	/// </summary>
	public static class LogicalDisk
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(L counter)
		{
			switch (counter) 
			{
				case L.AvgDiskBytesPerRead:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk Bytes/Read",
						"_Total",
						true
						);
				case L.AvgDiskBytesPerTransfer:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk Bytes/Transfer",
						"_Total",
						true
						);
				case L.AvgDiskBytesPerWrite:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk Bytes/Write",
						"_Total",
						true
						);
				case L.AvgDiskQueueLength:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk Queue Length",
						"_Total",
						true
						);
				case L.AvgDiskReadQueueLength:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk Read Queue Length",
						"_Total",
						true
						);
				case L.AvgDiskSecPerRead:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk sec/Read",
						"_Total",
						true
						);
				case L.AvgDiskSecPerTransfer:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk sec/Transfer",
						"_Total",
						true
						);
				case L.AvgDiskSecPerWrite:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk sec/Write",
						"_Total",
						true
						);
				case L.AvgDiskWriteQueueLength:
					return new PerformanceCounter(
						"LogicalDisk",
						"Avg. Disk Write Queue Length",
						"_Total",
						true
						);
				case L.CurrentDiskQueueLength:
					return new PerformanceCounter(
						"LogicalDisk",
						"Current Disk Queue Length",
						"_Total",
						true
						);
				case L.DiskBytesPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Disk Bytes/sec",
						"_Total",
						true
						);
				case L.DiskReadBytesPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Disk Read Bytes/sec",
						"_Total",
						true
						);
				case L.DiskReadsPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Disk Reads/sec",
						"_Total",
						true
						);
				case L.DiskTransfersPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Disk Transfers/sec",
						"_Total",
						true
						);
				case L.DiskWriteBytesPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Disk Write Bytes/sec",
						"_Total",
						true
						);
				case L.DiskWritesPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Disk Writes/sec",
						"_Total",
						true
						);
				case L.FreeMegabytes:
					return new PerformanceCounter(
						"LogicalDisk",
						"Free Megabytes",
						"_Total",
						true
						);
				case L.PercentDiskReadTime:
					return new PerformanceCounter(
						"LogicalDisk",
						"% Disk Read Time",
						"_Total",
						true
						);
				case L.PercentDiskTime:
					return new PerformanceCounter(
						"LogicalDisk",
						"% Disk Time",
						"_Total",
						true
						);
				case L.PercentDiskWriteTime:
					return new PerformanceCounter(
						"LogicalDisk",
						"% Disk Write Time",
						"_Total",
						true
						);
				case L.PercentFreeSpace:
					return new PerformanceCounter(
						"LogicalDisk",
						"% Free Space",
						"_Total",
						true
						);
				case L.PercentIdleTime:
					return new PerformanceCounter(
						"LogicalDisk",
						"% Idle Time",
						"_Total",
						true
						);
				case L.SplitIoPerSec:
					return new PerformanceCounter(
						"LogicalDisk",
						"Split IO/Sec",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
