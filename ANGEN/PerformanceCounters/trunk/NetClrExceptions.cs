/* -----------------------------------------------
 * NetClrExceptions.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrEx = Genetibase.PerformanceCounters.NuGenNetClrExceptionsCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Exceptions category.
	/// </summary>
	public static class NetClrExceptions
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrEx counter)
		{
			switch (counter) 
			{
				case NetClrEx.NumberOfExcepsThrown:
					return new PerformanceCounter(
						".NET CLR Exceptions",
						@"# of Exceps Thrown",
						"_Global_",
						true
						);
				case NetClrEx.NumberOfExcepsThrownPerSec:
					return new PerformanceCounter(
						".NET CLR Exceptions",
						@"# of Exceps Thrown / sec",
						"_Global_",
						true
						);
				case NetClrEx.NumberOfFiltersPerSec:
					return new PerformanceCounter(
						".NET CLR Exceptions",
						@"# of Filters / sec",
						"_Global_",
						true
						);
				case NetClrEx.NumberOfFinallysPerSec:
					return new PerformanceCounter(
						".NET CLR Exceptions",
						"# of Finallys / sec",
						"_Global_",
						true
						);
				case NetClrEx.ThrowToCatchDepth:
					return new PerformanceCounter(
						".NET CLR Exceptions",
						@"Throw To Catch Depth / sec",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
