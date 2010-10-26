/* -----------------------------------------------
 * PrintQueue.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using P = Genetibase.PerformanceCounters.NuGenPrintQueueCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the PrintQueue category.
	/// </summary>
	public static class PrintQueue
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
				case P.AddNetworkPrinterCalls:
					return new PerformanceCounter(
						"Print Queue",
						"Add Network Printer Calls",
						"_Total",
						true
						);
				case P.BytesPrintedPerSec:
					return new PerformanceCounter(
						"Print Queue",
						@"Bytes Printed/sec",
						"_Total",
						true
						);
				case P.EnumerateNetworkPrinterCalls:
					return new PerformanceCounter(
						"Print Queue",
						"Enumerate Network Printer Calls",
						"_Total",
						true
						);
				case P.JobErrors:
					return new PerformanceCounter(
						"Print Queue",
						"Job Errors",
						"_Total",
						true
						);
				case P.JobSpooling:
					return new PerformanceCounter(
						"Print Queue",
						"Jobs Spooling",
						"_Total",
						true
						);
				case P.MaxJobsSpooling:
					return new PerformanceCounter(
						"Print Queue",
						"Max Jobs Spooling",
						"_Total",
						true
						);
				case P.MaxReferences:
					return new PerformanceCounter(
						"Print Queue",
						"Max_References",
						"_Total",
						true
						);
				case P.NotReadyErrors:
					return new PerformanceCounter(
						"Print Queue",
						"Not Ready Errors",
						"_Total",
						true
						);
				case P.OutOfPaperErrors:
					return new PerformanceCounter(
						"Print Queue",
						"Out of Paper Errors",
						"_Total",
						true
						);
				case P.References:
					return new PerformanceCounter(
						"Print Queue",
						"References",
						"_Total",
						true
						);
				case P.TotalJobsPrinted:
					return new PerformanceCounter(
						"Print Queue",
						"Total Jobs Printed",
						"_Total",
						true
						);
				case P.TotalPagesPrinted:
					return new PerformanceCounter(
						"Print Queue",
						"Total Pages Printed",
						"_Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
