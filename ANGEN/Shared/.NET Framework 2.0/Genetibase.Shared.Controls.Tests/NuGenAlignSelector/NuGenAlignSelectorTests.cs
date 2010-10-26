/* -----------------------------------------------
 * NuGenAlignSelectorTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenAlignSelectorTests
	{
		private NuGenAlignSelector _selector;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_selector = new NuGenAlignSelector(new ServiceProvider());
			_eventSink = new EventSink(_selector);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		public void AlignmentChangedTest()
		{
			Assert.AreEqual(ContentAlignment.MiddleCenter, _selector.Alignment);

			_selector.Alignment = ContentAlignment.BottomCenter;
			Assert.AreEqual(ContentAlignment.BottomCenter, _selector.Alignment);
			Assert.IsFalse(_selector.Switchers[4].Checked);
			Assert.IsTrue(_selector.Switchers[7].Checked);

			_selector.Alignment = ContentAlignment.TopRight;
			Assert.AreEqual(ContentAlignment.TopRight, _selector.Alignment);
			Assert.IsTrue(_selector.Switchers[2].Checked);
			Assert.IsFalse(_selector.Switchers[7].Checked);
		}

		[Test]
		public void SwitcherEventsTest()
		{
			_eventSink.ExpectedAlignmentChangedCount = 3;

			_selector.Switchers[4].Checked = true;
			_selector.Switchers[5].Checked = true;
			_selector.Switchers[4].Checked = true;
			_selector.Switchers[3].Checked = true;
		}
	}
}
