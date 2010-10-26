/* -----------------------------------------------
 * NuGenSpinTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenSpinTests
	{
		private StubSpin _spin;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_spin = new StubSpin();
			_eventSink = new EventSink(_spin);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		public void UpButtonTest()
		{
			_eventSink.ExpectedValueChangedCount = 3;

			_spin.Value = 50;
			_spin.Increment = 1;
			_spin.OnUpButtonClick();

			Assert.AreEqual(51, _spin.Value);

			_spin.Value = 100;
			_spin.Maximum = 100;
			_spin.OnUpButtonClick();

			Assert.AreEqual(100, _spin.Value);
		}

		[Test]
		public void DownButtonTest()
		{
			_eventSink.ExpectedValueChangedCount = 3;

			_spin.Value = 50;
			_spin.Increment = 1;
			_spin.OnDownButtonClick();

			Assert.AreEqual(49, _spin.Value);

			_spin.Value = 0;
			_spin.Minimum = 0;
			_spin.OnDownButtonClick();

			Assert.AreEqual(0, _spin.Value);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void IncrementArgumentExceptionTest()
		{
			_spin.Increment = -1;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentExceptionTest()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_spin.Maximum = 100;
			_spin.Value = 120;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentException2Test()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_spin.Minimum = -100;
			_spin.Value = -120;
		}

		[Test]
		public void ValueRegularTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_spin.Value = 50;

			_spin.Maximum = 200;
			_spin.Value = 180;

			Assert.AreEqual(180, _spin.Value);
		}
		
		[Test]
		public void ValueRegular2Test()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_spin.Value = 50;

			_spin.Minimum = -50;
			_spin.Value = -20;

			Assert.AreEqual(-20, _spin.Value);
		}

		[Test]
		public void ValueEventTest()
		{
			_eventSink.ExpectedValueChangedCount = 1;
			_spin.TextBox.Text = "100";
			Assert.AreEqual(100, _spin.Value);
		}

		[Test]
		public void ValuePreviousValueTest()
		{
			_spin.Value = 20;
			Assert.AreEqual("20", _spin.TextBox.Text);

			_spin.TextBox.Text = "abc";
			Assert.AreEqual(20, _spin.Value);
		}
	}
}
