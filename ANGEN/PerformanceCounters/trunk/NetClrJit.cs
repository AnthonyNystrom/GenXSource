/* -----------------------------------------------
 * NetClrJit.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrJ = Genetibase.PerformanceCounters.NuGenNetClrJitCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Jit category.
	/// </summary>
	public static class NetClrJit
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrJ counter)
		{
			switch (counter) 
			{
				case NetClrJ.ILBytesJittedPerSec:
					return new PerformanceCounter(
						".NET CLR Jit",
						"# of IL Bytes Jitted",
						"_Global_",
						true
						);
				case NetClrJ.NumberOfILBytesJitted:
					return new PerformanceCounter(
						".NET CLR Jit",
						"# of IL Bytes Jitted",
						"_Global_",
						true
						);
				case NetClrJ.NumberOfMethodsJitted:
					return new PerformanceCounter(
						".NET CLR Jit",
						"# of Methods Jitted",
						"_Global_",
						true
						);
				case NetClrJ.PercentTimeInJit:
					return new PerformanceCounter(
						".NET CLR Jit",
						"% Time in Jit",
						"_Global_",
						true
						);
				case NetClrJ.StandardJitFailures:
					return new PerformanceCounter(
						".NET CLR Jit",
						"Standard Jit Failures",
						"_Global_",
						true
						);
				case NetClrJ.TotalNumberOfILBytesJitted:
					return new PerformanceCounter(
						".NET CLR Jit",
						"Total # of IL Bytes Jitted",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
