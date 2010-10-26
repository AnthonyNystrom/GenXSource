/* -----------------------------------------------
 * NbtConnection.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using N = Genetibase.PerformanceCounters.NuGenNbtConnectionCounter;

using System;
using System.Diagnostics;

namespace Genetibase.PerformanceCounters
{
	/// <summary>
	/// Defines performance counters for the NBT Connection category.
	/// </summary>
	public static class NbtConnection
	{
		/// <summary>
		/// Returns initialized performance counter according to the specified parameters.
		/// </summary>
		/// <param name="counter">Type of the counter.</param>
		/// <returns>Initialized performance counter.</returns>
		public static PerformanceCounter GetCounter(N counter)
		{
			switch (counter) 
			{
				case N.BytesReceivedPerSec:
					return new PerformanceCounter(
						"NBT Connection",
						"Bytes Sent/sec",
						"Total",
						true
						);
				case N.BytesSentPerSec:
					return new PerformanceCounter(
						"NBT Connection",
						"Bytes Sent/sec",
						"Total",
						true
						);
				case N.BytesTotalPerSec:
					return new PerformanceCounter(
						"NBT Connection",
						"Bytes Total/sec",
						"Total",
						true
						);
				default:
					return null;
			}
		}
	}
}
