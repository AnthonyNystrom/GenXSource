/* -----------------------------------------------
 * SizeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.WinApi.Tests
{
	[TestFixture]
	public class SizeTests
	{
		[Test]
		public void EqualityTest()
		{
			SIZE s = new SIZE(20, 30);
			SIZE s2 = new SIZE(20, 30);
			Size s3 = new Size(20, 30);

			Assert.AreEqual(s, s2);
			Assert.AreEqual(s, s3);

			Size s4 = new Size(30, 40);
			SIZE s5 = new SIZE(50, 60);

			Assert.AreNotEqual(s, s4);
			Assert.AreNotEqual(s, s5);

			Assert.IsTrue(s == s2);
			Assert.IsTrue(s != s5);
		}

		[Test]
		public void ConversionTest()
		{
			Size s = new Size(20, 30);
			SIZE s2 = s;

			Assert.AreEqual(20, s2.cx);
			Assert.AreEqual(30, s2.cy);
		}
	}
}
