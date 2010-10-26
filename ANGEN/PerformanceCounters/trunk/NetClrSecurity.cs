/* -----------------------------------------------
 * NetClrSecurity.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrSec = Genetibase.PerformanceCounters.NuGenNetClrSecurityCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Security category.
	/// </summary>
	public static class NetClrSecurity
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrSec counter)
		{
			switch (counter) 
			{
				case NetClrSec.NumberOfLinkTimeChecks:
					return new PerformanceCounter(
						".NET CLR Security",
						"# Link Time Checks",
						"_Global_",
						true
						);
				case NetClrSec.PercentTimeInRTChecks:
					return new PerformanceCounter(
						".NET CLR Security",
						"% Time in RT checks",
						"_Global_",
						true
						);
				case NetClrSec.PercentTimeInSigAuthenticating:
					return new PerformanceCounter(
						".NET CLR Security",
						"% Time Sig. Authenticating",
						"_Global_",
						true
						);
				case NetClrSec.StackWalkDepth:
					return new PerformanceCounter(
						".NET CLR Security",
						"Stack Walk Depth",
						"_Global_",
						true
						);
				case NetClrSec.TotalRuntimeChecks:
					return new PerformanceCounter(
						".NET CLR Security",
						"Total Runtime Checks",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
