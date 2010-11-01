/* -----------------------------------------------
 * PointTests.cs
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
	public class PointTests
	{
		[Test]
		public void EqualityTest()
		{
			POINT p = new POINT(20, 40);
			POINT p2 = new POINT(20, 40);
			Point p3 = new Point(20, 40);

			Assert.AreEqual(p, p2);
			Assert.AreEqual(p, p3);

			Point p4 = new Point(30, 30);
			POINT p5 = new POINT(50, 60);

			Assert.AreNotEqual(p, p4);
			Assert.AreNotEqual(p, p5);
		}
	}
}
