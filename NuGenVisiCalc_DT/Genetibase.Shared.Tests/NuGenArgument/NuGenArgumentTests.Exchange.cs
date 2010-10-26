/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[Test]
		public void ExchangeTest()
		{
			int aValue = 5;
			int bValue = 6;

			int a = aValue;
			int b = bValue;

			NuGenArgument.Exchange<int>(ref a, ref b);

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
