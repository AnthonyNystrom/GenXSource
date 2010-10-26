/* -----------------------------------------------
 * NuGenCountDownSpanTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public class NuGenCountDownSpanTests
	{
		private NuGenCountDownSpan _span;

		[SetUp]
		public void SetUp()
		{
			_span = new NuGenCountDownSpan(10, 5);
		}

		[Test]
		public void ConstructorTest()
		{
			Assert.AreEqual(10, _span.Minutes);
			Assert.AreEqual(5, _span.Seconds);
		}

		[Test]
		public void ConstructorArgumentExceptionTest()
		{
			try
			{
				_span = new NuGenCountDownSpan(-10, 5);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_span = new NuGenCountDownSpan(10, -5);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_span = new NuGenCountDownSpan(10, 61);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void MinutesTest()
		{
			_span.Minutes = 0;

			try
			{
				_span.Minutes = -10;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}

		[Test]
		public void SecondsTest()
		{
			_span.Seconds = 0;
			_span.Seconds = 59;

			try
			{
				_span.Seconds = -5;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_span.Seconds = 60;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}
	}
}
