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
		public void InvokePropertyChangedTest()
		{
			_eventSink.ExpectedPropertyChangedCallsCount = 1;
			_ctrl.StartPropertyChanged();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void InvokePropertyChangedInvalidOperationExceptionTest()
		{
			_ctrl.StartPropertyChangedCustomHandler();
		}
	}
}
