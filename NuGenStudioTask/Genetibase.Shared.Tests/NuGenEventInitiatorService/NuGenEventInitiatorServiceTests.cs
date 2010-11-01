/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenEventInitiatorServiceTests
	{
		private DummyControl _ctrl = null;
		private DummyControlEventSink _eventSink = null;

		[SetUp]
		public void SetUp()
		{
			_ctrl = new DummyControl();
			_eventSink = new DummyControlEventSink(_ctrl);
		}
	}
}
