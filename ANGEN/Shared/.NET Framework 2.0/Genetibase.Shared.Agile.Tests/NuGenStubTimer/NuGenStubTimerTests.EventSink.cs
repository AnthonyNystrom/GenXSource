/* -----------------------------------------------
 * NuGenStubTimerTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;
using Genetibase.Shared.Timers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Agile.Tests
{
	partial class NuGenStubTimerTests
	{
		class EventSink : MockObject
		{
			#region Expectations

			private ExpectationCounter _tickCount = new ExpectationCounter("tickCount");

			public int ExpectedTickCount
			{
				set
				{
					_tickCount.Expected = value;
				}
			}

			#endregion

			#region Constructors

			public EventSink(INuGenTimer eventBubbler)
			{
				if (eventBubbler == null)
				{
					Assert.Fail("eventBubbler cannot not be null.");
				}

				eventBubbler.Tick += delegate
				{
					_tickCount.Inc();
				};
			}

			#endregion
		}
	}
}
