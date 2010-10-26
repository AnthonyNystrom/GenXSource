/* -----------------------------------------------
 * NuGenStubTimerTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Timers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Agile.Tests
{
	[TestFixture]
	public partial class NuGenStubTimerTests
	{
		private INuGenTimer _timer;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_timer = new NuGenStubTimer();
			_eventSink = new EventSink(_timer);
		}
		
		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		public void StartStopTest()
		{
			_eventSink.ExpectedTickCount = 10;
			_timer.Interval = 10;
			_timer.Start();
		}
	}
}
