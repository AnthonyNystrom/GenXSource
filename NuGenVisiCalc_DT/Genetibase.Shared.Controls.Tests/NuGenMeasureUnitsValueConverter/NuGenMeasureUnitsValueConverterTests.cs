/* -----------------------------------------------
 * NuGenMeasureUnitsValueConverterTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenMeasureUnitsValueConverterTests
	{
		private EventSink _eventSink;
		private NuGenMeasureUnitsValueConverter _muConverter;

		[SetUp]
		public void SetUp()
		{
			_muConverter = new NuGenMeasureUnitsValueConverter();
			_eventSink = new EventSink(_muConverter);
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

			_muConverter.Maximum = 100;
			_muConverter.Value = 120;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentException2Test()
		{
			_eventSink.ExpectedValueChangedCount = 0;

			_muConverter.Minimum = -100;
			_muConverter.Value = -120;
		}

		[Test]
		public void ValueRegularTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_muConverter.Value = 50;

			_muConverter.Maximum = 200;
			_muConverter.Value = 180;

			Assert.AreEqual(180, _muConverter.Value);
		}

		[Test]
		public void ValueRegular2Test()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			_muConverter.Value = 50;

			_muConverter.Minimum = -50;
			_muConverter.Value = -20;

			Assert.AreEqual(-20, _muConverter.Value);
		}

		[Test]
		public void ConvertToStringTest()
		{
			Int32 value = 25000;
			Assert.IsNull(_muConverter.MeasureUnits);

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("25000", text);
		}

		[Test]
		public void ConvertToString2Test()
		{
			Int32 value = 25000;
			_muConverter.MeasureUnits = new String[] { };

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("25000", text);
		}

		[Test]
		public void ConvertToString3Test()
		{
			Int32 value = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("1 kg", text);
		}

		[Test]
		public void ConvertToStringMUTest()
		{
			Int32 value = 25000;
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g" };

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("25000 g", text);
		}

		[Test]
		public void ConvertToStringMU2Test()
		{
			Int32 value = 25000;
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("25 kg", text);
		}

		[Test]
		public void ConvertToStringMU3Test()
		{
			Int32 value = 25000000;
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("25000 kg", text);
		}

		[Test]
		public void ConvertToStringFloatTest()
		{
			Int32 value = 25330;
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };

			String text = _muConverter.ConvertToString(value, CultureInfo.CurrentCulture);
			Assert.AreEqual("25,33 kg", text);
		}

		[Test]
		public void IncrementEmptyTest()
		{
			Assert.AreEqual(1, _muConverter.Increment);
		}

		[Test]
		public void IncrementEmpty2Test()
		{
			_muConverter.MeasureUnits = new String[] { };
			_muConverter.Factor = 1000;
			_muConverter.Maximum = 10000000;
			_muConverter.Value = 100000;
			Assert.AreEqual(1, _muConverter.Increment);
		}

		[Test]
		public void IncrementTest()
		{
			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };
			_muConverter.Factor = 1000;
			_muConverter.Maximum = 100000000;
			_muConverter.Value = 25000;
			Assert.AreEqual(1000, _muConverter.Increment);
		}

		[Test]
		public void IncrementExTest()
		{
			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };
			_muConverter.Factor = 1000;
			_muConverter.Maximum = 100000000;
			_muConverter.Value = 25000;
			Assert.AreEqual(1000, _muConverter.Increment);
			Assert.AreEqual("25 kg", _muConverter.Text);

			_muConverter.MeasureUnits = null;
			Assert.AreEqual(25000, _muConverter.Value);
			Assert.AreEqual(1, _muConverter.Increment);

			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };
			Assert.AreEqual(1000, _muConverter.Increment);
			Assert.AreEqual("25 kg", _muConverter.Text);
		}

		[Test]
		public void TextTest()
		{
			_muConverter.Maximum = 100000;
			_muConverter.Text = "25000";
			Assert.AreEqual(25000, _muConverter.Value);
		}

		[Test]
		public void TryParseNullTest()
		{
			Int32 result;
			Assert.IsFalse(_muConverter.TryParse(null, CultureInfo.CurrentCulture, out result));
		}

		[Test]
		public void TryParseSimpleTest()
		{
			Assert.IsNull(_muConverter.MeasureUnits);
			Int32 value;
			Assert.IsTrue(_muConverter.TryParse("25", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(25, value);
		}

		[Test]
		public void TryParseSimple2Test()
		{
			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };
			Int32 value;
			Assert.IsTrue(_muConverter.TryParse("25000", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(25000, value);
		}

		[Test]
		public void TryParseMUTest()
		{
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };
			Int32 value;
			Assert.IsTrue(_muConverter.TryParse("25 kg", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(25000, value);
		}

		[Test]
		public void TryParseMU2Test()
		{
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg", "T" };
			Int32 value;
			Assert.IsTrue(_muConverter.TryParse("25 T", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(25000000, value);
		}

		[Test]
		public void TryParseInvalidTest()
		{
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };
			Int32 value;
			Assert.IsFalse(_muConverter.TryParse("25 T", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(0, value);
		}

		[Test]
		public void TryParseInvalid2Test()
		{
			_muConverter.Factor = 1000;
			Assert.IsNull(_muConverter.MeasureUnits);
			Int32 value;
			Assert.IsFalse(_muConverter.TryParse("25 T", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(0, value);
		}

		[Test]
		public void TryParseFloatTest()
		{
		    _muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };
			Int32 value;
			Assert.IsTrue(_muConverter.TryParse("2500,5 kg", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(2500500, value);
		}

		[Test]
		public void TryParseFloat2Test()
		{
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };
			Int32 value;
			Assert.IsFalse(_muConverter.TryParse("2500.5 kg", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(0, value);
		}

		[Test]
		public void TryParseOverflowTest()
		{
			_muConverter.Factor = 1000;
			_muConverter.MeasureUnits = new String[] { "g", "kg" };
			Int32 value;
			Assert.IsTrue(_muConverter.TryParse("2500,5333 kg", CultureInfo.CurrentCulture, out value));
			Assert.AreEqual(2500533, value);
		}
	}
}
