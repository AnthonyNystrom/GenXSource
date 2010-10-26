/* -----------------------------------------------
 * NuGenPinpointWindowTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenPinpointWindowTests
	{
		private NuGenPinpointWindow _pinpointWindow;

		[SetUp]
		public void SetUp()
		{
			_pinpointWindow = new NuGenPinpointWindow(new ServiceProvider());
		}

		[Test]
		public void SelectedIndexTest()
		{
			this.PopulateItems(_pinpointWindow);
			Assert.AreEqual(0, _pinpointWindow.SelectedIndex);
			_pinpointWindow.SelectedIndex = 2;
			Assert.AreEqual(2, _pinpointWindow.SelectedIndex);
			Assert.AreEqual("SomeItem3", _pinpointWindow.SelectedItem);
		}

		[Test]
		public void SelectedIndexTest2()
		{
			this.PopulateItems(_pinpointWindow);
			_pinpointWindow.SelectedItem = "SomeItem2";
			Assert.AreEqual("SomeItem2", _pinpointWindow.SelectedItem);
			Assert.AreEqual(1, _pinpointWindow.SelectedIndex);
		}

		[Test]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void SelectedIndexOutOfRangeExceptionTest()
		{
			Assert.AreEqual(0, _pinpointWindow.SelectedIndex);
			_pinpointWindow.SelectedIndex = 3;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void SelectedValueArgumentExceptionTest()
		{
			_pinpointWindow.SelectedItem = "SomeValue";
		}

		private void PopulateItems(NuGenPinpointWindow pinpointWindow)
		{
			pinpointWindow.Items.AddRange(new object[] { "SomeItem", "SomeItem2", "SomeItem3" });
		}
	}
}
