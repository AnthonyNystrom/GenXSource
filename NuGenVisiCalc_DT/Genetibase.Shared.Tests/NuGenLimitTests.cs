/* -----------------------------------------------
 * NuGenLimitTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenLimitTests
	{
		private NuGenLimit _limit;

		[SetUp]
		public void SetUp()
		{
			_limit = new NuGenLimit(10, 100);
		}

		[Test]
		public void ConstructorTest()
		{
			_limit = new NuGenLimit();
			Assert.AreEqual(0, _limit.Minimum);
			Assert.AreEqual(0, _limit.Maximum);

			_limit = new NuGenLimit(20, 30);
			Assert.AreEqual(20, _limit.Minimum);
			Assert.AreEqual(30, _limit.Maximum);
		}

		[Test]
		public void GetLimitedValueTest()
		{
			int value = 0;
			Assert.AreEqual(10, _limit.GetLimitedValue(value));

			value = 110;
			Assert.AreEqual(100, _limit.GetLimitedValue(value));

			value = 50;
			Assert.AreEqual(50, _limit.GetLimitedValue(value));
		}
	}
}
