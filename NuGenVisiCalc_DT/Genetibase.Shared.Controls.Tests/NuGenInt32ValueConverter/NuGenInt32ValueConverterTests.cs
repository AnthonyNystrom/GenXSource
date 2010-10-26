/* -----------------------------------------------
 * NuGenInt32ValueConverterTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenInt32ValueConverterTests
	{
		private EventSink _eventSink;
		private NuGenInt32ValueConverter _valueConverter;

		[SetUp]
		public void SetUp()
		{
			_valueConverter = new NuGenInt32ValueConverter();
			_eventSink = new EventSink(_valueConverter);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentExceptionTest()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_valueConverter.Maximum = 100;
			_valueConverter.Value = 120;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentException2Test()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_valueConverter.Minimum = -100;
			_valueConverter.Value = -120;
		}

		[Test]
		public void ValueRegularTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_valueConverter.Value = 50;

			_valueConverter.Maximum = 200;
			_valueConverter.Value = 180;

			Assert.AreEqual(180, _valueConverter.Value);
		}

		[Test]
		public void ValueRegular2Test()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_valueConverter.Value = 50;

			_valueConverter.Minimum = -50;
			_valueConverter.Value = -20;

			Assert.AreEqual(-20, _valueConverter.Value);
		}

		[Test]
		public void ValueEventTest()
		{
			_eventSink.ExpectedValueChangedCount = 1;
			_valueConverter.Text = "100";
			Assert.AreEqual(100, _valueConverter.Value);
		}

		[Test]
		public void ValuePreviousValueTest()
		{
			_valueConverter.Value = 20;
			Assert.AreEqual("20", _valueConverter.Text);

			_valueConverter.Text = "abc";
			Assert.AreEqual(20, _valueConverter.Value);
		}
	}
}
