/* -----------------------------------------------
 * NuGenPairTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenPairTests
	{
		private NuGenPair<int> _pair;

		[SetUp]
		public void SetUp()
		{
			_pair = new NuGenPair<int>(25, 32);
		}

		[Test]
		public void ConstructorTest()
		{
			NuGenPair<int> intPair = new NuGenPair<int>();

			Assert.AreEqual(0, intPair.GetValueOne());
			Assert.AreEqual(0, intPair.GetValueTwo());

			NuGenPair<Point> pointPair = new NuGenPair<Point>();

			Assert.AreEqual(Point.Empty, pointPair.GetValueOne());
			Assert.AreEqual(Point.Empty, pointPair.GetValueTwo());
		}

		[Test]
		public void GetValueTest()
		{
			Assert.AreEqual(25, _pair.GetValueOne());
			Assert.AreEqual(32, _pair.GetValueTwo());
		}

		[Test]
		public void GetValuesTest()
		{
			int valueOne;
			int valueTwo;

			_pair.GetValues(out valueOne, out valueTwo);

			Assert.AreEqual(25, valueOne);
			Assert.AreEqual(32, valueTwo);
		}

		[Test]
		public void SetValueTest()
		{
			_pair.SetValueOne(36);
			_pair.SetValueTwo(47);

			Assert.AreEqual(36, _pair.GetValueOne());
			Assert.AreEqual(47, _pair.GetValueTwo());
		}

		[Test]
		public void SetValuesTest()
		{
			_pair.SetValues(36, 47);

			Assert.AreEqual(36, _pair.GetValueOne());
			Assert.AreEqual(47, _pair.GetValueTwo());
		}
	}
}
