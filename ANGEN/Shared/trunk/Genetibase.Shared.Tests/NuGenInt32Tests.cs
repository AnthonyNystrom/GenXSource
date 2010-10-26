/* -----------------------------------------------
 * NuGenInt32Tests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Genetibase.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public partial class NuGenInt32Tests
	{
		private NuGenInt32 _int;
		private EventSink _eventSink;
		private Int32 _min = 0;
		private Int32 _max = 100;

		[TestInitialize]
		public void SetUp()
		{
			_int = new NuGenInt32(_min, _max);
			_eventSink = new EventSink(_int);
		}

		[TestCleanup]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[TestMethod]
		public void BoundsTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;
			_eventSink.ExpectedMinimumChangedCount = 1;
			_eventSink.ExpectedMaximumChangedCount = 1;

			_int.Value = 10;
			_int.Minimum = 110;

			Assert.AreEqual(110, _int.Minimum);
			Assert.AreEqual(110, _int.Maximum);
			Assert.AreEqual(110, _int.Value);
		}

		[TestMethod]
		public void Bounds2Test()
		{
			_eventSink.ExpectedMinimumChangedCount = 2;
			_eventSink.ExpectedValueChangedCount = 3;
			_eventSink.ExpectedMaximumChangedCount = 1;

			_int.Minimum = 50;
			_int.Value = 80;
			_int.Maximum = 40;

			Assert.AreEqual(40, _int.Minimum);
			Assert.AreEqual(40, _int.Maximum);
			Assert.AreEqual(40, _int.Value);
		}

		[TestMethod]
		public void ConstructorTest()
		{
			NuGenInt32 int32 = new NuGenInt32(10, 100);

			Assert.AreEqual(10, int32.Minimum);
			Assert.AreEqual(10, int32.Value);
		}

		[TestMethod]
		public void EqualsTest()
		{
			NuGenInt32 compared = new NuGenInt32(0, 100);
			compared.Value = 50;

			_int.Value = 50;

			Assert.IsTrue(_int.Equals(compared));
			Assert.IsFalse(_int.Equals(null));

			compared.Value = 2;
			Assert.IsFalse(_int.Equals(compared));
		}

		[TestMethod]
		public void EmptyTest()
		{
			NuGenInt32 empty = NuGenInt32.Empty;

			Assert.AreEqual(0, empty.Maximum);
			Assert.AreEqual(0, empty.Minimum);
			Assert.AreEqual(0, empty.Value);
		}

		[TestMethod]
		public void MaximumTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;
			_eventSink.ExpectedMaximumChangedCount = 1;

			_int.Value = 50;
			_int.Maximum = 20;

			Assert.AreEqual(20, _int.Value);
		}

		[TestMethod]
		public void MinimumTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;
			_eventSink.ExpectedMinimumChangedCount = 1;

			_int.Value = 50;
			_int.Minimum = 80;

			Assert.AreEqual(80, _int.Value);
		}
		
		[TestMethod]
		public void ValueTest()
		{
			_eventSink.ExpectedValueChangedCount = 1;

			_int.Value = 50;
			Assert.AreEqual(50, _int.Value);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentExceptionTest()
		{
			_eventSink.ExpectedValueChangedCount = 0;
			_int.Value = _max + 1;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentException2Test()
		{
			_eventSink.ExpectedValueChangedCount = 0;
			_int.Value = _min - 1;
		}

		[TestMethod]
		public void NonNegativeInt32ToInt32CastTest()
		{
			NuGenNonNegativeInt32 nonNegativeInt32 = new NuGenNonNegativeInt32();
			nonNegativeInt32.Value = 1;

			NuGenInt32 int32 = nonNegativeInt32;
			Assert.AreEqual(1, int32.Value);
		}

		[TestMethod]
		public void PositiveInt32ToInt32CastTest()
		{
			NuGenPositiveInt32 positiveInt32 = new NuGenPositiveInt32();
			positiveInt32.Value = 1;

			NuGenInt32 int32 = positiveInt32;
			Assert.AreEqual(1, int32.Value);
		}
	}
}
