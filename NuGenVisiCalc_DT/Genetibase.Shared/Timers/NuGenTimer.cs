/* -----------------------------------------------
 * NuGenTimer.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Timers
{
	/// <summary>
	/// Represents a <see cref="T:System.Windows.Forms.Timer"/> aggregator to support <see cref="INuGenTimer"/>
	/// interface.
	/// </summary>
	public class NuGenTimer : INuGenTimer
	{
		#region INuGenTimer Members

		/*
		 * Tick
		 */

		/// <summary>
		/// Occurs when the specified timer interval has elapsed.
		/// </summary>
		public event EventHandler Tick
		{
			add
			{
				Debug.Assert(this.Timer != null, "this.Timer != null");
				this.Timer.Tick += value;
			}
			remove
			{
				Debug.Assert(this.Timer != null, "this.Timer != null");
				this.Timer.Tick -= value;
			}
		}

		/*
		 * Interval
		 */

		/// <summary>
		/// Gets or sets the time, in milliseconds, between timer ticks.
		/// </summary>
		public int Interval
		{
			get
			{
				Debug.Assert(this.Timer != null, "this.Timer != null");
				return this.Timer.Interval;
			}
			set
			{
				Debug.Assert(this.Timer != null, "this.Timer != null");
				this.Timer.Interval = value;
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
			Debug.Assert(this.Timer != null, "this.Timer != null");
			this.Timer.Start();
		}

		/*
		 * Stop
		 */

		/// <summary>
		/// Stops the timer.
		/// </summary>
		public void Stop()
		{
			Debug.Assert(this.Timer != null, "this.Timer != null");
			this.Timer.Stop();
		}

		#endregion

		#region Properties.Protected

		/*
		 * Timer
		 */

		private Timer _timer;

		/// <summary>
		/// Gets the <see cref="T:System.Windows.Forms.Timer"/> aggregated by this <see cref="NuGenTimer"/>.
		/// </summary>
		protected Timer Timer
		{
			get
			{
				if (_timer == null)
				{
					Debug.Assert(_components != null, "_components != null");
					_timer = new Timer(_components);
				}

				return _timer;
			}
		}

		#endregion

		private IContainer _components;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTimer"/> class.
		/// </summary>
		public NuGenTimer()
		{
			_components = new Container();
		}

		/// <summary>
		/// Cleans up any resources being used.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// Cleans up any resources being used.
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
		/// <see langword="false"/> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_components != null)
				{
					_components.Dispose();
				}
			}
		}
	}
}
