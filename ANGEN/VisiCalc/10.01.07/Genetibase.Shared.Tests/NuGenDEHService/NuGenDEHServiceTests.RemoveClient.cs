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
		public void RemoveClientTest()
		{
			_eventHandler.ExpectedHandleEventCallsCount = 0;

			_dehService.AddClient(_eventHandler);
			_dehService.AddClient(_eventInitiator);
			_dehService.RemoveClient(_eventHandler);

			_eventInitiator.Run(_invokeCount);
			_eventHandler.Verify();
		}
	}
}
