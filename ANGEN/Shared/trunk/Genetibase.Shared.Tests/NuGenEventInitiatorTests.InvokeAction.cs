/* -----------------------------------------------
 * NuGenEventInitiatorTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorTests
	{
		[TestMethod]
		public void InvokeActionTest()
		{
			StubEventInitiator eventInititator = new StubEventInitiator();
			MockStubEventInitiatorSink eventSink = new MockStubEventInitiatorSink(eventInititator);

			eventSink.SetExpectedDummyEventCalls(1);
			eventInititator.Run();
			eventSink.Verify();
		}
	}
}
