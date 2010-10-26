/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[TestMethod]
		public void IsEvenTest()
		{
			Int32 zeroValue = 0;
			Int32 positiveEvenValue = 10;
			Int32 positiveOddValue = 1;
			Int32 negativeEvenValue = -10;
			Int32 negativeOddValue = -1;

			Assert.IsTrue(NuGenArgument.IsEven(zeroValue));
			Assert.AreNotEqual(NuGenArgument.IsEven(zeroValue), NuGenArgument.IsOdd(zeroValue));

			Assert.IsTrue(NuGenArgument.IsEven(positiveEvenValue));
			Assert.AreNotEqual(NuGenArgument.IsEven(positiveEvenValue), NuGenArgument.IsOdd(positiveEvenValue));

			Assert.IsTrue(NuGenArgument.IsEven(negativeEvenValue));
			Assert.AreNotEqual(NuGenArgument.IsEven(negativeEvenValue), NuGenArgument.IsOdd(negativeEvenValue));

			Assert.IsFalse(NuGenArgument.IsEven(positiveOddValue));
			Assert.AreNotEqual(NuGenArgument.IsEven(positiveOddValue), NuGenArgument.IsOdd(positiveOddValue));

			Assert.IsFalse(NuGenArgument.IsEven(negativeOddValue));
			Assert.AreNotEqual(NuGenArgument.IsEven(negativeOddValue), NuGenArgument.IsOdd(negativeOddValue));
		}
	}
}
