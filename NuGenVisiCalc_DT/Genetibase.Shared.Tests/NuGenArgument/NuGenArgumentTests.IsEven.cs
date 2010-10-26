/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[Test]
		public void IsEvenTest()
		{
			int zeroValue = 0;
			int positiveEvenValue = 10;
			int positiveOddValue = 1;
			int negativeEvenValue = -10;
			int negativeOddValue = -1;

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
