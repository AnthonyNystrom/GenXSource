/* -----------------------------------------------
 * NuGenInvokerTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	public class EventSink : MockObject
	{
		private ExpectationCounter _someEventCount = new ExpectationCounter("someEventCount");

		public int ExpectedSomeEventCount
		{
			set
			{
				_someEventCount.Expected = value;
			}
		}

		public void EventsClass_SomeEvent(object sender, EventArgs e)
		{
			_someEventCount.Inc();
		}

		public EventSink()
		{
		}
	}
}
