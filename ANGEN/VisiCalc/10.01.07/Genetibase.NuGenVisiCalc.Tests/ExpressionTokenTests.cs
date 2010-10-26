/* -----------------------------------------------
 * ExpressionTokenTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Expression;
using Genetibase.Shared.ComponentModel;
using NUnit.Framework;

namespace Genetibase.NuGenVisiCalc.Tests
{
	[TestFixture]
	public class ExpressionTokenTests
	{
		private class DumbServiceProvider : NuGenServiceProvider
		{
		}

		private INuGenServiceProvider _serviceProvider;

		[SetUp]
		public void SetUp()
		{
			_serviceProvider = new DumbServiceProvider();
		}

		[Test]
		public void EqualsTest()
		{
			ExpressionToken token = new ExpressionToken(_serviceProvider, "a");
			ExpressionToken token2 = new ExpressionToken(_serviceProvider, "a");

			Assert.IsTrue(token.Equals(token2));
		}

		[Test]
		public void NotEqualsTest()
		{
			ExpressionToken token = new ExpressionToken(_serviceProvider, "a");
			ExpressionToken token2 = new ExpressionToken(_serviceProvider, "b");

			Assert.IsTrue(token != token2);
		}
	}
}
