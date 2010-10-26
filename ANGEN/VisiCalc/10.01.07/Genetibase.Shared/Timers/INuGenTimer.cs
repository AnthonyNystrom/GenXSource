/* -----------------------------------------------
 * INuGenTimer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Timers
{
	/// <summary>
	/// Indicates that this class can bubble <see cref="T:Tick"/> event at the specified interval.
	/// </summary>
	public interface INuGenTimer : IDisposable
	{
		/// <summary>
		/// Occurs when the specified timer interval has elapsed.
		/// </summary>
		event EventHandler Tick;
		
		/// <summary>
		/// Gets or sets the time, in milliseconds, between timer ticks.
		/// </summary>
		int Interval
		{
			get;
			set;
		}
		
		/// <summary>
		/// Starts the timer.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops the timer.
		/// </summary>
		void Stop();
	}
}
