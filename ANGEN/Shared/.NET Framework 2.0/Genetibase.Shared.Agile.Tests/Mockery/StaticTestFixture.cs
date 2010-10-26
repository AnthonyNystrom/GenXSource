/* -----------------------------------------------
 * StaticTestFixture.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Genetibase.Shared.Agile.Tests.Mockery
{
	static class StaticTestFixture
	{
		[Test]
		public static void Test()
		{
		}

		[Test]
		private static void Test2()
		{
		}

		public static void Test3()
		{
			Assert.Fail("No associated TestAttribute.");
		}

		[Browsable(true)]
		public static void Test4()
		{
			Assert.Fail("No associated TestAttribute.");
		}
	}
}
