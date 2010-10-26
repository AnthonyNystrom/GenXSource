/* -----------------------------------------------
 * NuGenCountDownBlockTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Timers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public partial class NuGenCountDownBlockTests
	{
		private NuGenCountDownBlock _countDownBlock;
		private NuGenCountDownSpan _span;
		private INuGenTimer _timer;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_span = new NuGenCountDownSpan(1, 3);
			_timer = new StubTimer();
			_timer.Interval = 1;
			_countDownBlock = new NuGenCountDownBlock(_span, _timer);
			_eventSink = new EventSink(_countDownBlock);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			_countDownBlock = new NuGenCountDownBlock(_span, null);
		}

		[Test]
		public void StartAutoStopTest()
		{
			_eventSink.ExpectedTickCount = 63;
			_countDownBlock.Start();
		}
	}
}
