/* -----------------------------------------------
 * NetClrLoading.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using NetClrL = Genetibase.PerformanceCounters.NuGenNetClrLoadingCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the .NET CLR Loading category.
	/// </summary>
	public static class NetClrLoading
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(NetClrL counter)
		{
			switch (counter) 
			{
				case NetClrL.AssemblySearchLength:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Assembly Search Length",
						"_Global_",
						true
						);
				case NetClrL.BytesInLoaderHeap:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Bytes in Loader Heap",
						"_Global_",
						true
						);
				case NetClrL.CurrentAppDomains:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Current appdomains",
						"_Global_",
						true
						);
				case NetClrL.CurrentAssemblies:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Current Assemblies",
						"_Global_",
						true
						);
				case NetClrL.CurrentClassesLoaded:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Current Classes Loaded",
						"_Global_",
						true
						);
				case NetClrL.PercentTimeLoading:
					return new PerformanceCounter(
						".NET CLR Loading",
						"% Time Loading",
						"_Global_",
						true
						);
				case NetClrL.RateOfAppDomains:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Rate of appdomains",
						"_Global_",
						true
						);
				case NetClrL.RateOfAppDomainsUnloaded:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Rate of appdomains unloaded",
						"_Global_",
						true
						);
				case NetClrL.RateOfAssemblies:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Rate of Assemblies",
						"_Global_",
						true
						);
				case NetClrL.RateOfClassesLoaded:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Rate of Classes Loaded",
						"_Global_",
						true
						);
				case NetClrL.RateOfLoadFailures:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Rate of Load Failures",
						"_Global_",
						true
						);
				case NetClrL.TotalAppDomains:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Total Appdomains",
						"_Global_",
						true
						);
				case NetClrL.TotalAssemblies:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Total Assemblies",
						"_Global_",
						true
						);
				case NetClrL.TotalClassesLoaded:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Total Classes Loaded",
						"_Global_",
						true
						);
				case NetClrL.TotalNumberOfLoadFailures:
					return new PerformanceCounter(
						".NET CLR Loading",
						"Total # of Load Failures",
						"_Global_",
						true
						);
				default:
					return null;
			}
		}
	}
}
