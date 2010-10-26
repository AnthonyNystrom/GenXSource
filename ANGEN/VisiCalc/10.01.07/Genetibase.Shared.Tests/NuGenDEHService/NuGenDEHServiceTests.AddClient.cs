/* -----------------------------------------------
 * NuGenDEHServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenDEHServiceTests
	{
		[Test]
		public void AddClientTest()
		{
			_eventHandler.ExpectedHandleEventCallsCount = 1;
			_eventHandler.ExpectedTargetData = _invokeCount;

			_dehService.AddClient(_eventHandler);
			_dehService.AddClient(_eventInitiator);

			_eventInitiator.Run(_invokeCount);
			_eventHandler.Verify();
		}
	}
}
