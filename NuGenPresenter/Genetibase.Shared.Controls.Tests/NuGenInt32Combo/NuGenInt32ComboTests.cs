/* -----------------------------------------------
 * NuGenInt32ComboTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenInt32ComboTests
	{
		private StubInt32Combo _combo;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_combo = new StubInt32Combo();
			_eventSink = new EventSink(_combo);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentExceptionTest()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_combo.Maximum = 100;
			_combo.Value = 120;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentException2Test()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_combo.Minimum = -100;
			_combo.Value = -120;
		}

		[Test]
		public void ValueRegularTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_combo.Value = 50;

			_combo.Maximum = 200;
			_combo.Value = 180;

			Assert.AreEqual(180, _combo.Value);
		}

		[Test]
		public void ValueRegular2Test()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_combo.Value = 50;

			_combo.Minimum = -50;
			_combo.Value = -20;

			Assert.AreEqual(-20, _combo.Value);
		}

		[Test]
		public void ValueEventTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;
			
			_combo.Text = "100";
			_combo.OnKeyDown(new KeyEventArgs(Keys.Enter));
			Assert.AreEqual(100, _combo.Value);

			_combo.Text = "50";
			_combo.OnKeyDown(new KeyEventArgs(Keys.Return));
			Assert.AreEqual(50, _combo.Value);
		}

		[Test]
		public void ValuePreviousValueTest()
		{
			_combo.Value = 20;
			Assert.AreEqual("20", _combo.Text);

			_combo.Text = "abc";
			Assert.AreEqual(20, _combo.Value);
		}
	}
}
