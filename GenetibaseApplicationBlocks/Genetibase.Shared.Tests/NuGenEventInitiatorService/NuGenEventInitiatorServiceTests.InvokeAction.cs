/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorServiceTests
	{
		[Test]
		public void InvokeActionTest()
		{
			_eventSink.ExpectedStartedCallsCount = 1;
			_eventSink.ExpectedSender = _ctrl;

			_ctrl.Start();
			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvokeActionInvalidOperationExceptionTest()
		{
			_ctrl.StartCustomHandler();
		}

		[Test]
		public void InvokeActionUnsubscribedTest()
		{
			_ctrl.StartUnsubscribed();
		}
	}
}
