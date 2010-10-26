/* -----------------------------------------------
 * NuGenNonNegativeInt32Tests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenNonNegativeInt32Tests
	{
		private NuGenNonNegativeInt32 _int;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_int = new NuGenNonNegativeInt32();
			_eventSink = new EventSink(_int);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}
		
		[Test]
		public void EqualsTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			NuGenNonNegativeInt32 compared = new NuGenNonNegativeInt32();
			compared.Value = 20;

			_int.Value = 20;
			Assert.IsTrue(_int.Equals(compared));

			_int.Value = 30;
			Assert.IsFalse(_int.Equals(compared));
			Assert.IsFalse(_int.Equals(null));
		}

		[Test]
		public void ValueArgumentExceptionTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			try
			{
				_int.Value = 1;
			}
			catch (ArgumentException)
			{
				Assert.Fail();
			}

			try
			{
				_int.Value = 0;
			}
			catch (ArgumentException)
			{
				Assert.Fail();
			}

			try
			{
				_int.Value = -1;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}
	}
}
