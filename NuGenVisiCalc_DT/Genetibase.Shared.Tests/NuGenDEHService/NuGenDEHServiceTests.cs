/* -----------------------------------------------
 * NuGenDEHServiceTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Timers;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenDEHServiceTests
	{
		private int _invokeCount = 0;
		private NuGenDEHService _dehService = null;
		private MockEventHandler _eventHandler = null;
		private MockEventInitiator _eventInitiator = null;

		[SetUp]
		public void SetUp()
		{
			_invokeCount = 10;
			_dehService = new NuGenDEHService(new MockTimer(_invokeCount));
			_eventHandler = new MockEventHandler();
			_eventInitiator = new MockEventInitiator();
		}
	}
}
