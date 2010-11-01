/* -----------------------------------------------
 * NuGenTimer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Timers
{
	/// <summary>
	/// Represents a <see cref="T:System.Windows.Forms.Timer"/> aggregator to support <see cref="T:INuGenTimer"/>
	/// interface.
	/// </summary>
	public class NuGenTimer : INuGenTimer, IDisposable
	{
		#region Declarations.Fields

		private IContainer components = null;

		#endregion

		#region IDisposable Members

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
				if (this.components != null)
				{
					this.components.Dispose();
				}
			}
		}

		#endregion

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

		private Timer timer = null;

		/// <summary>
		/// Gets the <see cref="T:System.Windows.Forms.Timer"/> aggregated by this <see cref="T:NuGenTimer"/>.
		/// </summary>
		protected Timer Timer
		{
			get
			{
				if (this.timer == null)
				{
					Debug.Assert(this.components != null, "this.components != null");
					this.timer = new Timer(this.components);
				}

				return this.timer;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTimer"/> class.
		/// </summary>
		public NuGenTimer()
		{
			this.components = new Container();
		}

		#endregion
	}
}
