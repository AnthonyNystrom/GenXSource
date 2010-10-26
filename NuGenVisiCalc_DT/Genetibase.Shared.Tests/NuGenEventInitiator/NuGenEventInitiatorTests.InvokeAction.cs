/* -----------------------------------------------
 * NuGenEventInitiatorTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorTests
	{
		[Test]
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
