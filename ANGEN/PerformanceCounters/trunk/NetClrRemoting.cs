/* -----------------------------------------------
 * NetClrRemoting.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrR = Genetibase.PerformanceCounters.NuGenNetClrRemotingCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Remoting category.
	/// </summary>
	public static class NetClrRemoting
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrR counter)
		{
			switch (counter) 
			{
				case NetClrR.Channels:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Channels",
						"_Global_",
						true
						);
				case NetClrR.ContextBoundClassesLoaded:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Context-Bound Classes Loaded",
						"_Global_",
						true
						);
				case NetClrR.ContextBoundObjectsAllocPerSec:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Context-Bound Objects Alloc / sec",
						"_Global_",
						true
						);
				case NetClrR.ContextProxies:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Context Proxies",
						"_Global_",
						true
						);
				case NetClrR.Contexts:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Contexts",
						"_Global_",
						true
						);
				case NetClrR.RemoteCallsPerSec:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Remote Calls/sec",
						"_Global_",
						true
						);
				case NetClrR.TotalRemoteCalls:
					return new PerformanceCounter(
						".NET CLR Remoting",
						"Total Remote Calls",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
