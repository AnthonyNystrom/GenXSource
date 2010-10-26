/* -----------------------------------------------
 * NuGenCountDownBlockTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Timers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.Tests
{
	partial class NuGenCountDownBlockTests
	{
		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _tickCount = new ExpectationCounter("tickCount");

			public int ExpectedTickCount
			{
				set
				{
					_tickCount.Expected = value;
				}
			}

			public EventSink(NuGenCountDownBlock eventInitiator)
			{
				Assert.IsNotNull(eventInitiator);

				eventInitiator.Tick += delegate
				{
					_tickCount.Inc();
				};
			}
		}

		private sealed class StubTimer : INuGenTimer
		{
			public event EventHandler Tick;

			private int _interval;

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

			public void Start()
			{
				_shouldStop = false;

				int i = 0;

				while (i < _interval)
				{
					if (_shouldStop)
					{
						break;
					}

					if (i == _interval - 1)
					{
						if (this.Tick != null)
						{
							this.Tick(this, EventArgs.Empty);
						}

						i = 0;
					}
				}
			}

			private bool _shouldStop;

			public void Stop()
			{
				_shouldStop = true;
			}

			public void Dispose()
			{
			}
		}
	}
}
