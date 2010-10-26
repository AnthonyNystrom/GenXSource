/* -----------------------------------------------
 * DynamicTestFixture.cs
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
	class DynamicTestFixture
	{
		[Test]
		public void Test()
		{
		}

		[Test]
		private void Test2()
		{
		}

		public void Test3()
		{
			Assert.Fail("No associated TestAttribute.");
		}

		[Browsable(true)]
		public void Test4()
		{
			Assert.Fail("No associated TestAttribute.");
		}

		[Test]
		public static void Test5()
		{
		}

		[Test]
		private static void Test6()
		{
		}

		public static void Test7()
		{
			Assert.Fail("No associated TestAttribute.");
		}
	}
}
