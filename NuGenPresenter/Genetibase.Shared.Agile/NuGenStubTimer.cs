/* -----------------------------------------------
 * NuGenStubTimer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Timers;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Agile
{
	/// <summary>
	/// Use <see cref="Interval"/> property to specify the amount of <see cref="Tick"/> events.
	/// <see cref="Start"/> method begins bubbling. <see cref="Stop"/> method does nothing.
	/// </summary>
	public class NuGenStubTimer : INuGenTimer
	{
		/*
		 * Tick
		 */

		public event EventHandler Tick;

		/*
		 * Interval
		 */

		private int _interval;

		/// <summary>
		/// Gets or sets the time, in milliseconds, between timer ticks.
		/// </summary>
		/// <value></value>
		public int Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				_interval = value;
			}
		}

		/*
		 * Start
		 */

		/// <summary>
		/// Starts the timer.
		/// </summary>
		public void Start()
		{
			for (int i = 0; i < this.Interval; i++)
			{
				if (this.Tick != null)
				{
					this.Tick(this, EventArgs.Empty);
				}
			}
		}

		/*
		 * Stop
		 */

		/// <summary>
		/// Stops the timer.
		/// </summary>
		public void Stop()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenStubTimer"/> class.
		/// </summary>
		public NuGenStubTimer()
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{	
		}
	}
}
