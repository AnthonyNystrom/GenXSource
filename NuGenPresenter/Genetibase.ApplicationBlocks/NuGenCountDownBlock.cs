/* -----------------------------------------------
 * NuGenCountDownBlock.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Timers;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Represents a count down engine for timers.
	/// </summary>
	public partial class NuGenCountDownBlock : IDisposable
	{
		/// <summary>
		/// Occurs when the current time span changed.
		/// </summary>
		public event EventHandler Tick;

		/// <summary>
		/// Raises the <see cref="E:Genetibase.ApplicationBlocks.NuGenCountDownBlock.Tick"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnTick(EventArgs e)
		{
			if (this.Tick != null)
			{
				this.Tick(this, e);
			}
		}

		/// <summary>
		/// Gets the current span.
		/// </summary>
		public NuGenCountDownSpan CurrentSpan
		{
			get
			{
				return new NuGenCountDownSpan(_minutes, _seconds);
			}
		}

		/// <summary>
		/// Start counting down.
		/// </summary>
		public void Start()
		{
			_timer.Start();
		}

		/// <summary>
		/// Though the timer will be stopped automatically at the end of the initial span, you can stop
		/// it immediately at the desired time.
		/// </summary>
		public void Stop()
		{
			_timer.Stop();
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			if (_minutes == 0 && _seconds == 0)
			{
				this.Stop();
				return;
			}
			else if (_seconds == 0)
			{
				_seconds = 60;
				_minutes--;
			}

			_seconds--;
			this.OnTick(EventArgs.Empty);
		}

		private int _minutes;
		private int _seconds;
		private INuGenTimer _timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCountDownBlock"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="timer"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenCountDownBlock(NuGenCountDownSpan initialSpan, INuGenTimer timer)
		{
			if (timer == null)
			{
				throw new ArgumentNullException("timer");
			}

			_minutes = initialSpan.Minutes;
			_seconds = initialSpan.Seconds;
			_timer = timer;
			_timer.Tick += _timer_Tick;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources;
		/// <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);

				if (_timer != null)
				{
					_timer.Tick -= _timer_Tick;
					_timer.Dispose();
					_timer = null;
				}
			}
		}
	}
}
