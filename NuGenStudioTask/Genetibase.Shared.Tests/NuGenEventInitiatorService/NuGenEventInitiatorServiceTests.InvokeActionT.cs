/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorServiceTests
	{
		[Test]
		public void InvokeActionTTest()
		{
			_eventSink.ExpectedStartedGenericCallsCount = 1;
			_eventSink.ExpectedSenderGeneric = _ctrl;

			_ctrl.StartGeneric();
			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvokeActionTInvalidOperationExceptionTest()
		{
			_ctrl.StartGenericCustomHandler();
		}
	}
}
