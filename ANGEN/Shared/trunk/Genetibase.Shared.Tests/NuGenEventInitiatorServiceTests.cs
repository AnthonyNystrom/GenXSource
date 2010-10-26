/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public partial class NuGenEventInitiatorServiceTests
	{
		private DummyControl _ctrl;
		private DummyControlEventSink _eventSink;

		[TestInitialize]
		public void SetUp()
		{
			_ctrl = new DummyControl();
			_eventSink = new DummyControlEventSink(_ctrl);
		}

		[TestMethod]
		public void InvokeHandlerTest()
		{
			_eventSink.ExpectedStartedCallsCount = 1;
			_eventSink.ExpectedSender = _ctrl;

			_ctrl.Start();
			_eventSink.Verify();
		}

		[TestMethod]
		public void InvokeHandlerInvalidOperationExceptionTest()
		{
			try
			{
				_ctrl.StartCustomHandler();
				Assert.Fail();
			}
			catch (InvalidOperationException)
			{
			}

			try
			{
				_ctrl.StartInvalidHandler();
				Assert.Fail();
			}
			catch (InvalidOperationException)
			{
			}

			try
			{
				_ctrl.StartInvalidParamCountHandler();
			}
			catch (InvalidOperationException)
			{
			}
		}
	}
}
