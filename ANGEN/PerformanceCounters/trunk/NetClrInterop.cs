/* -----------------------------------------------
 * NetClrInterop.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrIntrp = Genetibase.PerformanceCounters.NuGenNetClrInteropCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Interop category.
	/// </summary>
	public static class NetClrInterop
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrIntrp counter)
		{
			switch (counter) 
			{
				case NetClrIntrp.NumberOfCCWs:
					return new PerformanceCounter(
						".NET CLR Interop",
						@"# of CCWs",
						"_Global_",
						true
						);
				case NetClrIntrp.NumberOfMarshalling:
					return new PerformanceCounter(
						".NET CLR Interop",
						@"# of Marshalling",
						"_Global_",
						true
						);
				case NetClrIntrp.NumberOfStubs:
					return new PerformanceCounter(
						".NET CLR Interop",
						"# of Stubs",
						"_Global_",
						true
						);
				case NetClrIntrp.NumberOfTlbExportsPerSec:
					return new PerformanceCounter(
						".NET CLR Interop",
						@"# of TLB exports / sec",
						"_Global_",
						true
						);
				case NetClrIntrp.NumberOfTlbImportsPerSec:
					return new PerformanceCounter(
						".NET CLR Interop",
						@"# of TLB imports / sec",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
