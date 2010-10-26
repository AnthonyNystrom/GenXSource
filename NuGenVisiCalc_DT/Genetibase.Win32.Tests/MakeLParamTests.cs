/* -----------------------------------------------
 * MakeLParamTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.WinApi.Tests
{
	[TestFixture]
	public class MakeLParamTests
	{
		[Test]
		public void MakeLParamRegularTest()
		{
			int lParam = Common.MakeLParam(100, 200).ToInt32();
			
			Point pointBitConverter = Point.Empty;
			Point pointShift = Point.Empty;

			/* -------------------------------------------- */

			byte[] buffer = BitConverter.GetBytes(lParam);
			buffer[2] = 0;
			buffer[3] = 0;

			int x = BitConverter.ToInt32(buffer, 0);

			buffer = BitConverter.GetBytes(lParam);
			buffer[0] = buffer[2];
			buffer[1] = buffer[3];
			buffer[2] = 0;
			buffer[3] = 0;

			int y = BitConverter.ToInt32(buffer, 0);

			pointBitConverter = new Point(x, y);

			/* ------------------------------------------- */


			pointShift = new Point(
				lParam & 0xffff,
				(lParam >> 16) & 0xffff
			);

			/* ------------------------------------------- */

			Assert.AreEqual(pointBitConverter, pointShift);
		}
	}
}
