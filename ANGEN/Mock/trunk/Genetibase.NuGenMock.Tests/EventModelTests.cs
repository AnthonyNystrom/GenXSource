/* -----------------------------------------------
 * EventModelTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock.Tests
{
	[TestClass]
	public class EventModelTests
	{
		private class EventInitiator
		{
			public event EventHandler SimpleEvent;

			public void BubbleSimpleEvent()
			{
				if (this.SimpleEvent != null)
				{
					this.SimpleEvent(this, EventArgs.Empty);
				}
			}
		}

		private class EventSink : MockObject
		{
			private ExpectationCounter _simpleEventCalls = new ExpectationCounter("simpleEventCalls");

			public Int32 ExpectedSimpleEventCalls
			{
				set
				{
					_simpleEventCalls.Expected = value;
				}
			}

			public EventSink(EventInitiator initiator)
			{
				Assert.IsNotNull(initiator);
				initiator.SimpleEvent += delegate
				{
					_simpleEventCalls.Inc();
				};
			}
		}

		private EventSink _eventSink;
		private EventInitiator _initiator;

		[TestInitialize]
		public void SetUp()
		{
			_eventSink = new EventSink(_initiator = new EventInitiator());
		}

		[TestMethod]
		public void EventSinkTest()
		{
			_eventSink.ExpectedSimpleEventCalls = 1;
			_initiator.BubbleSimpleEvent();
			_eventSink.Verify();
		}
	}
}
