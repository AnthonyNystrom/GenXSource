/* -----------------------------------------------
 * RectTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.WinApi.Tests
{
	[TestFixture]
	public class RectTests
	{
		[Test]
		public void RectEqualityTest()
		{
			RECT rect = new RECT(100, 100, 300, 300);
			RECT rect2 = new RECT(100, 100, 300, 300);
			Rectangle rect3 = new Rectangle(100, 100, 200, 200);

			Assert.AreEqual(rect, rect2);
			Assert.AreEqual(rect, rect3);

			Rectangle rect4 = new Rectangle(200, 200, 200, 200);
			RECT rect5 = new RECT(200, 300, 400, 500);

			Assert.AreNotEqual(rect, rect4);
			Assert.AreNotEqual(rect, rect5);

			Assert.IsTrue(rect == rect2);
			Assert.IsTrue(rect != rect5);
		}

		[Test]
		public void RectRegularTest()
		{
			RECT rect = new RECT(100, 100, 200, 200);

			Assert.AreEqual(100, rect.left);
			Assert.AreEqual(100, rect.top);
			Assert.AreEqual(200, rect.right);
			Assert.AreEqual(200, rect.bottom);

			Rectangle rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);

			Assert.AreEqual(rectangle.Location, rect.Location);
			Assert.AreEqual(rectangle.Size, rect.Size);

			rect.right = 100;
			rect.bottom = 100;

			rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);

			Assert.AreEqual(rectangle.Location, rect.Location);
			Assert.AreEqual(rectangle.Size, rect.Size);

			rect.right = 50;
			rect.bottom = 50;

			rectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);

			Assert.AreEqual(rectangle.Location, rect.Location);
			Assert.AreEqual(rectangle.Size, rect.Size);
		}
	}
}
