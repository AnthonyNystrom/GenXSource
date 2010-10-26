/* -----------------------------------------------
 * NuGenDEHServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Timers;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenDEHServiceTests
	{
		class MockTimer : INuGenTimer
		{
			#region Declarations

			private int _startCallsCount = 0;

			#endregion

			#region INuGenTimer Members

			/*
			 * Tick
			 */

			public event EventHandler Tick;

			protected virtual void OnTick(EventArgs e)
			{
				if (this.Tick != null)
				{
					this.Tick(this, e);
				}
			}

			/*
			 * Interval
			 */

			private int _interval = 0;

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

			public void Start()
			{
				_startCallsCount++;

				if (_startCallsCount == this.Interval)
				{
					this.OnTick(EventArgs.Empty);
					this.Stop();
				}
			}

			/*
			 * Stop
			 */

			public void Stop()
			{
				_startCallsCount = 0;
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Interval = 10.
			/// </summary>
			public MockTimer()
				: this(10)
			{
			}

			public MockTimer(int interval)
			{
				this.Interval = interval;
			}

			#endregion

			public void Dispose()
			{
			}
		}

		class MockEventInitiator : INuGenDEHClient
		{
			#region INuGenDEHClient Members

			public event NuGenDEHEventHandler EventToBeDelayed;

			protected virtual void OnEventToBeDelayed(NuGenDEHEventArgs e)
			{
				if (this.EventToBeDelayed != null)
				{
					this.EventToBeDelayed(this, e);
				}
			}

			public void HandleDelayedEvent(object sender, NuGenDEHEventArgs e)
			{
			}

			#endregion

			#region Methods.Public

			public void Run(int count)
			{
				for (int i = 0; i < count; i++)
				{
					this.OnEventToBeDelayed(new NuGenDEHEventArgs(this, count));
				}
			}

			#endregion
		}

		class MockEventHandler : MockObject, INuGenDEHClient
		{
			#region INuGenDEHClient Members

			public event NuGenDEHEventHandler EventToBeDelayed;

			protected virtual void OnEventToBeDelayed(NuGenDEHEventArgs e)
			{
				if (this.EventToBeDelayed != null)
				{
					this.EventToBeDelayed(this, e);
				}
			}

			public void HandleDelayedEvent(object sender, NuGenDEHEventArgs e)
			{
				if (e is NuGenDEHEventArgs)
				{
					this.handleEventCallsCount.Inc();
					this.targetData.Actual = ((NuGenDEHEventArgs)e).LParam;
				}
			}

			#endregion

			#region Properties.Public.Expectations

			private ExpectationCounter handleEventCallsCount = new ExpectationCounter("handleEventCallsCount");

			public int ExpectedHandleEventCallsCount
			{
				set
				{
					this.handleEventCallsCount.Expected = value;
				}
			}

			private ExpectationValue targetData = new ExpectationValue("targetData");

			public int ExpectedTargetData
			{
				set
				{
					this.targetData.Expected = value;
				}
			}

			#endregion
		}
	}
}
