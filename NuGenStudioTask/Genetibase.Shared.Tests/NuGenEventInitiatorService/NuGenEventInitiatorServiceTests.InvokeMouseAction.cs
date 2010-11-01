/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorServiceTests
	{
		[Test]
		public void InvokeMouseActionTest()
		{
			MouseEventArgs mouseEventArgs = new MouseEventArgs(MouseButtons.Left, 2, 640, 480, 0);

			_eventSink.ExpectedStartedMouseCallsCount = 1;
			_eventSink.ExpectedStartedMouseSender = _ctrl;
			_eventSink.ExpectedStartedMouseEventArgs = mouseEventArgs;

			_ctrl.StartMouseAction(mouseEventArgs);

			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvokeMouseActionInvalidOperationExceptionTest()
		{
			_ctrl.StartMouseActionCustomHandler();
		}
	}
}
