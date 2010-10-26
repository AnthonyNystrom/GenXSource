/* -----------------------------------------------
 * Enums.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

namespace Genetibase.PerformanceCounters
{
	#region JobObject
	
	/// <summary>
	/// Defines performance counters for the Job Object category.
	/// </summary>
	public enum NuGenJobObjectCounter
	{
		/// <summary>
		/// Current % Kernel Mode Time
		/// </summary>
		CurrentPercentKernelModeTime,

		/// <summary>
		/// Current % Processor Time
		/// </summary>
		CurrentPercentProcessorTime,

		/// <summary>
		/// Current % User Mode Time
		/// </summary>
		CurrentPercentUserModeTime,

		/// <summary>
		/// Pages / sec
		/// </summary>
		PagesPerSec,

		/// <summary>
		/// Process Count - Active
		/// </summary>
		ProcessCountActive,

		/// <summary>
		/// Process Count - Terminated
		/// </summary>
		ProcessCountTerminated,

		/// <summary>
		/// Process Count - Total
		/// </summary>
		ProcessCountTotal,

		/// <summary>
		/// This Period mSec - Kernel Mode
		/// </summary>
		ThisPeriodMSecKernelMode,

		/// <summary>
		/// This Period mSec - Processor
		/// </summary>
		ThisPeriodMSecProcessor,

		/// <summary>
		/// This Period mSec - User Mode
		/// </summary>
		ThisPeriodMSecUserMode,

		/// <summary>
		/// Total mSec - Kernel Mode
		/// </summary>
		TotalMSecKernelMode,

		/// <summary>
		/// Total mSec - Processor
		/// </summary>
		TotalMSecProcessor,

		/// <summary>
		/// Total mSec - User Mode
		/// </summary>
		TotalMSecUserMode
	}

	#endregion

	#region JobObjectDetails

	/// <summary>
	/// Defines performance counters for the Job Oject Details category.
	/// </summary>
	public enum NuGenJobObjectDetailsCounter
	{
		/// <summary>
		/// % Privileged Time
		/// </summary>
		PercentPrivilegedTime,

		/// <summary>
		/// % Processor Time
		/// </summary>
		PercentProcessorTime,

		/// <summary>
		/// % User Time
		/// </summary>
		PercentUserTime,

		/// <summary>
		/// Creating Process ID
		/// </summary>
		CreatingProcessId,

		/// <summary>
		/// Elapsed Time
		/// </summary>
		ElapsedTime,

		/// <summary>
		/// Handle Count
		/// </summary>
		HandleCount,
		
		/// <summary>
		/// ID Process
		/// </summary>
		IdProcess,

		/// <summary>
		/// IO Data Bytes / sec
		/// </summary>
		IoDataBytesPerSec,

		/// <summary>
		/// IO Data Operations / sec
		/// </summary>
		IoDataOperationsPerSec,

		/// <summary>
		/// IO Other Bytes / sec
		/// </summary>
		IoOtherBytesPerSec,

		/// <summary>
		/// IO Other Operations / sec
		/// </summary>
		IoOtherOperationsPerSec,

		/// <summary>
		/// IO Read Bytes / sec
		/// </summary>
		IoReadBytesPerSec,

		/// <summary>
		/// IO Read Operations / sec
		/// </summary>
		IoReadOperationsPerSec,

		/// <summary>
		/// IO Write Bytes / sec
		/// </summary>
		IoWriteBytesPerSec,

		/// <summary>
		/// IO Write Operations / sec
		/// </summary>
		IoWriteOperationsPerSec,

		/// <summary>
		/// Page Faults / sec
		/// </summary>
		PageFaultsPerSec,

		/// <summary>
		/// Page File Bytes
		/// </summary>
		PageFileBytes,

		/// <summary>
		/// Page File Bytes Peak
		/// </summary>
		PageFileBytesPeak,

		/// <summary>
		/// Pool Nonpaged Bytes
		/// </summary>
		PoolNonpagedBytes,

		/// <summary>
		/// Pool Paged Bytes
		/// </summary>
		PoolPagedBytes,

		/// <summary>
		/// Priority Base
		/// </summary>
		PriorityBase,

		/// <summary>
		/// Private Bytes
		/// </summary>
		PrivateBytes,

		/// <summary>
		/// Thread Count
		/// </summary>
		ThreadCount,

		/// <summary>
		/// Virtual Bytes
		/// </summary>
		VirtualBytes,

		/// <summary>
		/// Virtual Bytes Peak
		/// </summary>
		VirtualBytesPeak,

		/// <summary>
		/// Working Set
		/// </summary>
		WorkingSet,

		/// <summary>
		/// Working Set Peak
		/// </summary>
		WorkingSetPeak
	}

	#endregion

	#region LogicalDisk

	/// <summary>
	/// Defines performance counters for the Logical Disk category.
	/// </summary>
	public enum NuGenLogicalDiskCounter
	{
		/// <summary>
		/// % Disk Read Time
		/// </summary>
		PercentDiskReadTime,

		/// <summary>
		/// % Disk Time
		/// </summary>
		PercentDiskTime,

		/// <summary>
		/// % Disk Write Time
		/// </summary>
		PercentDiskWriteTime,

		/// <summary>
		/// % Free Space
		/// </summary>
		PercentFreeSpace,

		/// <summary>
		/// % Idle Time
		/// </summary>
		PercentIdleTime,

		/// <summary>
		/// Avg. Disk Bytes / Read
		/// </summary>
		AvgDiskBytesPerRead,

		/// <summary>
		/// Avg. Disk Bytes / Transfer
		/// </summary>
		AvgDiskBytesPerTransfer,

		/// <summary>
		/// Avg. Disk Bytes / Write
		/// </summary>
		AvgDiskBytesPerWrite,

		/// <summary>
		/// Avg. Disk Queue Length
		/// </summary>
		AvgDiskQueueLength,

		/// <summary>
		/// Avg. Disk Read Queue Length
		/// </summary>
		AvgDiskReadQueueLength,

		/// <summary>
		/// Avg. Disk sec / Read
		/// </summary>
		AvgDiskSecPerRead,

		/// <summary>
		/// Avg. Disk sec / Transfer
		/// </summary>
		AvgDiskSecPerTransfer,

		/// <summary>
		/// Avg. Disk sec / Write
		/// </summary>
		AvgDiskSecPerWrite,

		/// <summary>
		/// Avg. Disk Write Queue Length
		/// </summary>
		AvgDiskWriteQueueLength,

		/// <summary>
		/// Current Disk Queue Length
		/// </summary>
		CurrentDiskQueueLength,

		/// <summary>
		/// Disk Bytes / sec
		/// </summary>
		DiskBytesPerSec,

		/// <summary>
		/// Disk Read Bytes / sec
		/// </summary>
		DiskReadBytesPerSec,

		/// <summary>
		/// Disk Reads / sec
		/// </summary>
		DiskReadsPerSec,

		/// <summary>
		/// Disk Transfers / sec
		/// </summary>
		DiskTransfersPerSec,

		/// <summary>
		/// Disk Write Bytes / sec
		/// </summary>
		DiskWriteBytesPerSec,

		/// <summary>
		/// Disk Writes / sec
		/// </summary>
		DiskWritesPerSec,
		
		/// <summary>
		/// Free Megabytes
		/// </summary>
		FreeMegabytes,

		/// <summary>
		/// Split IO / sec
		/// </summary>
		SplitIoPerSec
	}

	#endregion

	#region NbtConnection

	/// <summary>
	/// Defines performance counters for the NBT Connection category.
	/// </summary>
	public enum NuGenNbtConnectionCounter
	{
		/// <summary>
		/// Bytes Received / sec
		/// </summary>
		BytesReceivedPerSec,
		
		/// <summary>
		/// Bytes Sent / sec
		/// </summary>
		BytesSentPerSec,
		
		/// <summary>
		/// Bytes Total / sec
		/// </summary>
		BytesTotalPerSec
	}

	#endregion

	#region NetClrExceptions

	/// <summary>
	/// Defines performance counters for the .NET CLR Exceptions category.
	/// </summary>
	public enum NuGenNetClrExceptionsCounter
	{
		/// <summary>
		/// # of Exceps Thrown
		/// </summary>
		NumberOfExcepsThrown,
		
		/// <summary>
		/// # of Exceps Thrown/sec
		/// </summary>
		NumberOfExcepsThrownPerSec,
		
		/// <summary>
		/// # of Filters/sec
		/// </summary>
		NumberOfFiltersPerSec,
		
		/// <summary>
		/// # of Finallys/sec
		/// </summary>
		NumberOfFinallysPerSec,
		
		/// <summary>
		/// Throw To Catch Depth/sec
		/// </summary>
		ThrowToCatchDepth
	}

	#endregion

	#region NetClrInterop

	/// <summary>
	/// Defines Performance Counters for the .NET CLR Interop category.
	/// </summary>
	public enum NuGenNetClrInteropCounter
	{
		/// <summary>
		/// # of CCWs
		/// </summary>
		NumberOfCCWs,
		
		/// <summary>
		/// # of Marshalling
		/// </summary>
		NumberOfMarshalling,
		
		/// <summary>
		/// # of Stubs
		/// </summary>
		NumberOfStubs,
		
		/// <summary>
		/// # of TLB exports/sec
		/// </summary>
		NumberOfTlbExportsPerSec,
		
		/// <summary>
		/// # of TLB imports/sec
		/// </summary>
		NumberOfTlbImportsPerSec
	}

	#endregion

	#region NetClrJit

	/// <summary>
	/// Defines Performance Counters for the .NET CLR Jit category.
	/// </summary>
	public enum NuGenNetClrJitCounter
	{
		/// <summary>
		/// # of IL Bytes Jitted
		/// </summary>
		NumberOfILBytesJitted,
		
		/// <summary>
		/// # of Methods Jitted
		/// </summary>
		NumberOfMethodsJitted,
		
		/// <summary>
		/// % Time in Jit
		/// </summary>
		PercentTimeInJit,
		
		/// <summary>
		/// IL Bytes Jitted/sec
		/// </summary>
		ILBytesJittedPerSec,
		
		/// <summary>
		/// Standard Jit Failures
		/// </summary>
		StandardJitFailures,
		
		/// <summary>
		/// Total # of IL Bytes Jitted
		/// </summary>
		TotalNumberOfILBytesJitted
	}

	#endregion

	#region NetClrLoading

	/// <summary>
	/// Defines performance counters for the .NET CLR Loading category.
	/// </summary>
	public enum NuGenNetClrLoadingCounter
	{
		/// <summary>
		/// % Time Loading
		/// </summary>
		PercentTimeLoading,
		
		/// <summary>
		/// Assembly Search Length
		/// </summary>
		AssemblySearchLength,
		
		/// <summary>
		/// Bytes in Loader Heap
		/// </summary>
		BytesInLoaderHeap,
		
		/// <summary>
		/// Current AppDomains
		/// </summary>
		CurrentAppDomains,
		
		/// <summary>
		/// Current Assemblies
		/// </summary>
		CurrentAssemblies,
		
		/// <summary>
		/// Current Classes Loaded
		/// </summary>
		CurrentClassesLoaded,
		
		/// <summary>
		/// Rate of appdomains
		/// </summary>
		RateOfAppDomains,
		
		/// <summary>
		/// Rate of appdomains unloaded
		/// </summary>
		RateOfAppDomainsUnloaded,
		
		/// <summary>
		/// Rate of Assemblies
		/// </summary>
		RateOfAssemblies,
		
		/// <summary>
		/// Rate of Classes Loaded
		/// </summary>
		RateOfClassesLoaded,
		
		/// <summary>
		/// Rate of Load Failures
		/// </summary>
		RateOfLoadFailures,
		
		/// <summary>
		/// Total # of Load Failures
		/// </summary>
		TotalNumberOfLoadFailures,
		
		/// <summary>
		/// Total Appdomains
		/// </summary>
		TotalAppDomains,
		
		/// <summary>
		/// Total Assemblies
		/// </summary>
		TotalAssemblies,
		
		/// <summary>
		/// Total Classes Loaded
		/// </summary>
		TotalClassesLoaded
	}

	#endregion

	#region NetClrLocksAndThreads

	/// <summary>
	/// Defines performance counters for the .NET CLR LocksAndThreads category.
	/// </summary>
	public enum NuGenNetClrLocksAndThreadsCounter
	{
		/// <summary>
		/// # of current logical Threads
		/// </summary>
		NumberOfCurrentLogicalThreads,
		
		/// <summary>
		/// # of current physical Threads
		/// </summary>
		NumberOfCurrentPhysicalThreads,
		
		/// <summary>
		/// # of current recognized Threads
		/// </summary>
		NumberOfCurrentRecognizedThreads,
		
		/// <summary>
		/// Contention Rate/sec
		/// </summary>
		ContentionRatePerSec,
		
		/// <summary>
		/// Current Queue Length
		/// </summary>
		CurrentQueueLength,
		
		/// <summary>
		/// Queue Length/sec
		/// </summary>
		QueueLengthPerSec,
		
		/// <summary>
		/// Queue Length Peak
		/// </summary>
		QueueLengthPeak,
		
		/// <summary>
		/// Rate of recognized threads/sec
		/// </summary>
		RateOfRecognizedThreadsPerSec,
		
		/// <summary>
		/// Total # of Contentions
		/// </summary>
		TotalNumberOfContentions
	}

	#endregion

	#region NetClrMemory

	/// <summary>
	/// Defines performance counters for the .NET CLR Memory category.
	/// </summary>
	public enum NuGenNetClrMemoryCounter
	{
		/// <summary>
		/// # Bytes in all Heaps
		/// </summary>
		NumberOfBytesInAllHeaps,
		
		/// <summary>
		/// # GC Handles
		/// </summary>
		NumberOfGCHandles,
		
		/// <summary>
		/// # Gen 0 Collections
		/// </summary>
		NumberOfGen0Collections,
		
		/// <summary>
		/// # Gen 1 Collections
		/// </summary>
		NumberOfGen1Collections,
		
		/// <summary>
		/// # Gen 2 Collections
		/// </summary>
		NumberOfGen2Collections,
		
		/// <summary>
		/// # Induced GC
		/// </summary>
		NumberOfInducedGC,
		
		/// <summary>
		/// # of Pinned Objects
		/// </summary>
		NumberOfPinnedObjects,
		
		/// <summary>
		/// # of Sink Blocks in use
		/// </summary>
		NumberOfSinkBlocksInUse,
		
		/// <summary>
		/// # Total committed Bytes
		/// </summary>
		NumberOfTotalCommittedBytes,
		
		/// <summary>
		/// # Total reserved Bytes
		/// </summary>
		NumberOfTotalReservedBytes,
		
		/// <summary>
		/// % Time in GC
		/// </summary>
		PercentTimeInGC,
		
		/// <summary>
		/// Allocated Bytes/sec
		/// </summary>
		AllocatedBytesPerSec,
		
		/// <summary>
		/// Finalization Survivors
		/// </summary>
		FinalizationSurvivors,
		
		/// <summary>
		/// Gen 0 heap size
		/// </summary>
		Gen0HeapSize,
		
		/// <summary>
		/// Gen 0 Promoted Bytes/sec
		/// </summary>
		Gen0PromotedBytesPerSec,
		
		/// <summary>
		/// Gen 2 heap size
		/// </summary>
		Gen2HeapSize,
		
		/// <summary>
		/// Large Object Heap size
		/// </summary>
		LargeObjectHeapSize,
		
		/// <summary>
		/// Promoted Finalization-Memory from Gen 0
		/// </summary>
		PromotedFinalizationMemoryFromGen0,
		
		/// <summary>
		/// Promoted Finalization-Memory from Gen 1
		/// </summary>
		PromotedFinalizationMemoryFromGen1,
		
		/// <summary>
		/// Promoted Memory from Gen 0
		/// </summary>
		PromotedMemoryFromGen0,
		
		/// <summary>
		/// Promoted Memory from Gen 1
		/// </summary>
		PromotedMemoryFromGen1
	}

	#endregion

	#region NetClrRemoting

	/// <summary>
	/// Defines performance counters for the .NET CLR Remoting category.
	/// </summary>
	public enum NuGenNetClrRemotingCounter
	{
		/// <summary>
		/// Channels
		/// </summary>
		Channels,

		/// <summary>
		/// Context Proxies
		/// </summary>
		ContextProxies,

		/// <summary>
		/// Context-Bound Classes Loaded
		/// </summary>
		ContextBoundClassesLoaded,

		/// <summary>
		/// Context-Bound Objects Alloc/sec
		/// </summary>
		ContextBoundObjectsAllocPerSec,

		/// <summary>
		/// Contexts
		/// </summary>
		Contexts,

		/// <summary>
		/// Remote Calls/sec
		/// </summary>
		RemoteCallsPerSec,

		/// <summary>
		/// Total Remote Calls
		/// </summary>
		TotalRemoteCalls
	}

	#endregion

	#region NetClrSecurity

	/// <summary>
	/// Defines performance counters for the .NET CLR Security category.
	/// </summary>
	public enum NuGenNetClrSecurityCounter
	{
		/// <summary>
		/// # Link Time Checks
		/// </summary>
		NumberOfLinkTimeChecks,
		
		/// <summary>
		/// % Time in RT checks
		/// </summary>
		PercentTimeInRTChecks,
		
		/// <summary>
		/// % Time Sig. Authenticating
		/// </summary>
		PercentTimeInSigAuthenticating,
		
		/// <summary>
		/// Stack Walk Depth
		/// </summary>
		StackWalkDepth,
		
		/// <summary>
		/// Total Runtime Checks
		/// </summary>
		TotalRuntimeChecks
	}

	#endregion

	#region PagingFile

	/// <summary>
	/// Defines performance counters for the Paging File category.
	/// </summary>
	public enum NuGenPagingFileCounter
	{	
		/// <summary>
		/// % Usage
		/// </summary>
		PercentUsage,

		/// <summary>
		/// % Usage Peak
		/// </summary>
		PercentUsagePeak
	}

	#endregion

	#region PhysicalDisk

	/// <summary>
	/// Defines performance counters for the Physical Disk category.
	/// </summary>
	public enum NuGenPhysicalDiskCounter
	{
		/// <summary>
		/// % Disk Read Time
		/// </summary>
		PercentDiskReadTime,
		
		/// <summary>
		/// % Disk Time
		/// </summary>
		PercentDiskTime,
		
		/// <summary>
		/// % Disk Write Time
		/// </summary>
		PercentDiskWriteTime,
		
		/// <summary>
		/// % Idle Time
		/// </summary>
		PercentIdleTime,
		
		/// <summary>
		/// Avg. Disk Bytes/Read
		/// </summary>
		AvgDiskBytesPerRead,
		
		/// <summary>
		/// Avg. Disk Bytes/Transfer
		/// </summary>
		AvgDiskBytesPerTransfer,
		
		/// <summary>
		/// Avg. Disk Bytes/Write
		/// </summary>
		AvgDiskBytesPerWrite,
		
		/// <summary>
		/// Avg. Disk Queue Length
		/// </summary>
		AvgDiskQueueLength,
		
		/// <summary>
		/// Avg. Disk sec/Read
		/// </summary>
		AvgDiskSecPerRead,
		
		/// <summary>
		/// Avg. Disk sec/Transfer
		/// </summary>
		AvgDiskSecPerTransfer,
		
		/// <summary>
		/// Avg. Disk sec/Write
		/// </summary>
		AvgDiskSecPerWrite,
		
		/// <summary>
		/// Avg. Disk Write Queue Length
		/// </summary>
		AvgDiskWriteQueueLength,
		
		/// <summary>
		/// Current Disk Queue Length
		/// </summary>
		CurrentDiskQueueLength,
		
		/// <summary>
		/// Disk Bytes/sec
		/// </summary>
		DiskBytesPerSec,
		
		/// <summary>
		/// Disk Read Bytes/sec
		/// </summary>
		DiskReadBytesPerSec,
		
		/// <summary>
		/// Disk Reads/sec
		/// </summary>
		DiskReadsPerSec,
		
		/// <summary>
		/// Disk Transfers/sec
		/// </summary>
		DiskTransfersPerSec,
		
		/// <summary>
		/// Disk Write Bytes/sec
		/// </summary>
		DiskWriteBytesPerSec,
		
		/// <summary>
		/// Disk Writes/sec
		/// </summary>
		DiskWritesPerSec,
		
		/// <summary>
		/// Split IO/sec
		/// </summary>
		SplitIoPerSec
	}

	#endregion

	#region PrintQueue

	/// <summary>
	/// Defines performance counters for the Print Queue category.
	/// </summary>
	public enum NuGenPrintQueueCounter
	{
		/// <summary>
		/// Add Network Printer Calls
		/// </summary>
		AddNetworkPrinterCalls,

		/// <summary>
		/// Bytes Printed/sec
		/// </summary>
		BytesPrintedPerSec,

		/// <summary>
		/// Enumerate Network Printer Calls
		/// </summary>
		EnumerateNetworkPrinterCalls,

		/// <summary>
		/// Job Errors
		/// </summary>
		JobErrors,

		/// <summary>
		/// Jobs Spooling
		/// </summary>
		JobSpooling,

		/// <summary>
		/// Max Jobs Spooling
		/// </summary>
		MaxJobsSpooling,

		/// <summary>
		/// Max References
		/// </summary>
		MaxReferences,

		/// <summary>
		/// Not Ready Errors
		/// </summary>
		NotReadyErrors,

		/// <summary>
		/// Out of Paper Errors
		/// </summary>
		OutOfPaperErrors,

		/// <summary>
		/// References
		/// </summary>
		References,

		/// <summary>
		/// Total Jobs Printed
		/// </summary>
		TotalJobsPrinted,
		
		/// <summary>
		/// Total Pages Printed
		/// </summary>
		TotalPagesPrinted
	}

	#endregion

	#region Process

	/// <summary>
	/// Defines performance counters in the Process category.
	/// </summary>
	public enum NuGenProcessCounter
	{
		/// <summary>
		/// % Privileged Time
		/// </summary>
		PercentPrivilegedTime,

		/// <summary>
		/// % Processor Time
		/// </summary>
		PercentProcessorTime,

		/// <summary>
		/// % User Time
		/// </summary>
		PercentUserTime,

		/// <summary>
		/// Creating Process ID
		/// </summary>
		CreatingProcessId,

		/// <summary>
		/// Elapsed Time
		/// </summary>
		ElapsedTime,

		/// <summary>
		/// Handle Count
		/// </summary>
		HandleCount,

		/// <summary>
		/// ID Process
		/// </summary>
		IdProcess,

		/// <summary>
		/// IO Data Bytes/sec
		/// </summary>
		IoDataBytesPerSec,

		/// <summary>
		/// IO Data Operations/sec
		/// </summary>
		IoDataOperationsPerSec,

		/// <summary>
		/// IO Other Bytes/sec
		/// </summary>
		IoOtherBytesPerSec,

		/// <summary>
		/// IO Other Operations/sec
		/// </summary>
		IoOtherOperationsPerSec,

		/// <summary>
		/// IO Read Bytes/sec
		/// </summary>
		IoReadBytesPerSec,
		
		/// <summary>
		/// IO Read Operations/sec
		/// </summary>
		IoReadOperationsPerSec,

		/// <summary>
		/// IO Write Bytes/sec
		/// </summary>
		IoWriteBytesPerSec,

		/// <summary>
		/// IO Write Operations/sec
		/// </summary>
		IoWriteOperationsPerSec,

		/// <summary>
		/// Page Faults/sec
		/// </summary>
		PageFaultsPerSec,

		/// <summary>
		/// Page File Bytes
		/// </summary>
		PageFileBytes,

		/// <summary>
		/// Page File Bytes Peak
		/// </summary>
		PageFileBytesPeak,

		/// <summary>
		/// Pool Nonpaged Bytes
		/// </summary>
		PoolNonpagedBytes,

		/// <summary>
		/// Pool Paged Bytes
		/// </summary>
		PoolPagedBytes,

		/// <summary>
		/// Priority Base
		/// </summary>
		PriorityBase,

		/// <summary>
		/// Private Bytes
		/// </summary>
		PrivateBytes,

		/// <summary>
		/// Thread Count
		/// </summary>
		ThreadCount,

		/// <summary>
		/// Virtual Bytes
		/// </summary>
		VirtualBytes,

		/// <summary>
		/// Virtual Bytes Peak
		/// </summary>
		VirtualBytesPeak,

		/// <summary>
		/// Working Set
		/// </summary>
		WorkingSet,

		/// <summary>
		/// Working Set Peak
		/// </summary>
		WorkingSetPeak
	}

	#endregion

	#region Processor

	/// <summary>
	/// Defines performance counters for the Processor category.
	/// </summary>
	public enum NuGenProcessorCounter
	{
		/// <summary>
		/// % C1 Time
		/// </summary>
		PercentC1Time,

		/// <summary>
		/// % C2 Time
		/// </summary>
		PercentC2Time,

		/// <summary>
		/// % C3 Time
		/// </summary>
		PercentC3Time,

		/// <summary>
		/// % DPC Time
		/// </summary>
		PercentDpcTime,

		/// <summary>
		/// % Idle Time
		/// </summary>
		PercentIdleTime,

		/// <summary>
		/// % Interrupt Time
		/// </summary>
		PercentInterruptTime,

		/// <summary>
		/// % Privileged Time
		/// </summary>
		PercentPrivilegedTime,

		/// <summary>
		/// % Processor Time
		/// </summary>
		PercentProcessorTime,

		/// <summary>
		/// % User Time
		/// </summary>
		PercentUserTime,

		/// <summary>
		/// C1 Transitions/sec
		/// </summary>
		C1TransitionsPerSec,

		/// <summary>
		/// C2 Transitions/sec
		/// </summary>
		C2TransitionsPerSec,

		/// <summary>
		/// C3 Transitions/sec
		/// </summary>
		C3TransitionsPerSec,

		/// <summary>
		/// DPC Rate
		/// </summary>
		DpcRate,

		/// <summary>
		/// DPCs Queued/sec
		/// </summary>
		DPCsQueuedPerSec,

		/// <summary>
		/// Interrupts/sec
		/// </summary>
		InterruptsPerSec
	}

	#endregion

	#region Thread

	/// <summary>
	/// Defines performance counters for the Thread category.
	/// </summary>
	public enum NuGenThreadCounter
	{
		/// <summary>
		/// % Privileged Time
		/// </summary>
		PercentPrivilegedTime,
		
		/// <summary>
		/// % Processor Time
		/// </summary>
		PercentProcessorTime,
		
		/// <summary>
		/// % User Time
		/// </summary>
		PercentUserTime,
		
		/// <summary>
		/// Context Switches/sec
		/// </summary>
		ContextSwitchesPerSec,
		
		/// <summary>
		/// Elapsed Time
		/// </summary>
		ElapsedTime,
		
		/// <summary>
		/// ID Process
		/// </summary>
		IdProcess,
		
		/// <summary>
		/// ID Thread
		/// </summary>
		IdThread,
		
		/// <summary>
		/// Priority Base
		/// </summary>
		PriorityBase,
		
		/// <summary>
		/// Priority Current
		/// </summary>
		PriorityCurrent,
		
		/// <summary>
		/// Start Address
		/// </summary>
		StartAddress,
		
		/// <summary>
		/// Thread State
		/// </summary>
		ThreadState,
		
		/// <summary>
		/// Thread Wait Reason
		/// </summary>
		ThreadWaitReason
	}

	#endregion
}
