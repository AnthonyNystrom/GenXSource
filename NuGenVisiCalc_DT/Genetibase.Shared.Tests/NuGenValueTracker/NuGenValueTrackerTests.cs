/* -----------------------------------------------
 * NuGenValueTrackerTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenValueTrackerTests
	{
		private NuGenValueTracker _valueTracker;
		private int _changeValue = 10;

		private Dictionary<MethodInvoker, MethodInvoker> _invokers;

		[SetUp]
		public void SetUp()
		{
			_valueTracker = new NuGenValueTracker();
			_invokers = new Dictionary<MethodInvoker,MethodInvoker>();

			_invokers.Add(_valueTracker.LargeChangeDown, _valueTracker.LargeChangeUp);
			_invokers.Add(_valueTracker.SmallChangeDown, _valueTracker.SmallChangeUp);
		}

		[Test]
		public void IndexerTest()
		{
			Assert.AreEqual(1, _valueTracker.LargeChange);
			Assert.AreEqual(1, _valueTracker.SmallChange);
			Assert.AreEqual(0, _valueTracker.Minimum);
			Assert.AreEqual(100, _valueTracker.Maximum);
			Assert.AreEqual(0, _valueTracker.Value);
		}

		[Test]
		public void LargeChangeAdvancedTest()
		{
			_valueTracker.Minimum = 20;
			_valueTracker.Maximum = 30;
			_valueTracker.LargeChange = 20;

			Assert.AreEqual(20, _valueTracker.Value);

			_valueTracker.LargeChangeDown();
			Assert.AreEqual(20, _valueTracker.Value);

			_valueTracker.LargeChangeUp();
			Assert.AreEqual(30, _valueTracker.Value);
		}

		[Test]
		public void LargeChangeArgumentExceptionTest()
		{
			try
			{
				_valueTracker.LargeChange = -1;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_valueTracker.LargeChange = 0;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void MinMaxTest()
		{
			_valueTracker.Minimum = 20;
			_valueTracker.Maximum = 10;

			Assert.AreEqual(10, _valueTracker.Minimum);
			Assert.AreEqual(10, _valueTracker.Maximum);
			Assert.AreEqual(10, _valueTracker.Value);

			_valueTracker.Minimum = 20;

			Assert.AreEqual(20, _valueTracker.Minimum);
			Assert.AreEqual(20, _valueTracker.Maximum);
			Assert.AreEqual(20, _valueTracker.Value);
		}

		[Test]
		public void SmallChangeArgumentExceptionTest()
		{
			try
			{
				_valueTracker.SmallChange = -1;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_valueTracker.SmallChange = 0;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void ValueArgumentExceptionTest()
		{
			_valueTracker.Minimum = 0;
			_valueTracker.Maximum = 10;

			try
			{
				_valueTracker.Value = 20;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void XChangeTest()
		{
			_valueTracker.LargeChange = _changeValue;
			_valueTracker.SmallChange = _changeValue;

			foreach (MethodInvoker downInvoker in _invokers.Keys)
			{
				_valueTracker.Value = 0;
				this.XChangeGenericTest(downInvoker, _invokers[downInvoker]);
			}
		}

		private void XChangeGenericTest(MethodInvoker down, MethodInvoker up)
		{
			Assert.IsNotNull(down);
			Assert.IsNotNull(up);

			Assert.AreEqual(0, _valueTracker.Value);
			down();

			Assert.AreEqual(0, _valueTracker.Value);
			up();
			Assert.AreEqual(_changeValue, _valueTracker.Value);

			for (int i = 0; i < 15; i++)
			{
				up();
			}

			Assert.AreEqual(_valueTracker.Maximum, _valueTracker.Value);
		}
	}
}
