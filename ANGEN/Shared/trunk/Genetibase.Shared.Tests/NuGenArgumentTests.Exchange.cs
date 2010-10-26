/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Controls;
using Genetibase.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[TestMethod]
		public void ExchangeTest()
		{
			Int32 aValue = 5;
			Int32 bValue = 6;

			Int32 a = aValue;
			Int32 b = bValue;

			NuGenArgument.Exchange<Int32>(ref a, ref b);

			Assert.AreEqual(bValue, a);
			Assert.AreEqual(aValue, b);

			Button buttonAValue = new Button();
			Button buttonBValue = new Button();

			Button buttonA = buttonAValue;
			Button buttonB = buttonBValue;

			NuGenArgument.Exchange<Button>(ref buttonA, ref buttonB);

			Assert.AreEqual(buttonBValue, buttonA);
			Assert.AreEqual(buttonAValue, buttonB);
		}
	}
}
