/* -----------------------------------------------
 * NetClrMemory.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrMem = Genetibase.PerformanceCounters.NuGenNetClrMemoryCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Memory category.
	/// </summary>
	public static class NetClrMemory
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrMem counter)
		{
			switch (counter) 
			{
				case NetClrMem.AllocatedBytesPerSec:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Allocated Bytes/sec",
						"_Global_",
						true
						);
				case NetClrMem.FinalizationSurvivors:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Finalization Survivors",
						"_Global_",
						true
						);
				case NetClrMem.Gen0HeapSize:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Gen 0 heap size",
						"_Global_",
						true
						);
				case NetClrMem.Gen0PromotedBytesPerSec:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Gen 0 Promoted Bytes/Sec",
						"_Global_",
						true
						);
				case NetClrMem.Gen2HeapSize:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Gen 2 heap size",
						"_Global_",
						true
						);
				case NetClrMem.LargeObjectHeapSize:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Large Object Heap size",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfBytesInAllHeaps:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Bytes in all Heaps",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfGCHandles:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# GC Handles",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfGen0Collections:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Gen 0 Collections",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfGen1Collections:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Gen 1 Collections",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfGen2Collections:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Gen 2 Collections",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfInducedGC:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Induced GC",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfPinnedObjects:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# of Pinned Objects",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfSinkBlocksInUse:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# of Sink Blocks in use",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfTotalCommittedBytes:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Total committed Bytes",
						"_Global_",
						true
						);
				case NetClrMem.NumberOfTotalReservedBytes:
					return new PerformanceCounter(
						".NET CLR Memory",
						"# Total reserved Bytes",
						"_Global_",
						true
						);
				case NetClrMem.PercentTimeInGC:
					return new PerformanceCounter(
						".NET CLR Memory",
						"% Time in GC",
						"_Global_",
						true
						);
				case NetClrMem.PromotedFinalizationMemoryFromGen0:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Promoted Finalization-Memory from Gen 0",
						"_Global_",
						true
						);
				case NetClrMem.PromotedFinalizationMemoryFromGen1:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Promoted Finalization-Memory from Gen 1",
						"_Global_",
						true
						);
				case NetClrMem.PromotedMemoryFromGen0:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Promoted Memory from Gen 0",
						"_Global_",
						true
						);
				case NetClrMem.PromotedMemoryFromGen1:
					return new PerformanceCounter(
						".NET CLR Memory",
						"Promoted Memory from Gen 1",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
