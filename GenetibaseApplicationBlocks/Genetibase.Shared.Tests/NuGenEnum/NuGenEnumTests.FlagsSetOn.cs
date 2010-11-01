/* -----------------------------------------------
 * NuGenEnumTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEnumTests
	{
		[Test]
		public void EnumDefaultTest()
		{
			DefaultEnum defaultEnum = DefaultEnum.DefaultElementNone;
			Assert.AreEqual(0, NuGenEnum.FlagsSetOn(defaultEnum));

			defaultEnum |= DefaultEnum.DefaultElementOne | DefaultEnum.DefaultElementTwo | DefaultEnum.DefaultElementThree;
			Assert.AreEqual(3, NuGenEnum.FlagsSetOn(defaultEnum));

			defaultEnum &= ~DefaultEnum.DefaultElementTwo;
			Assert.AreEqual(2, NuGenEnum.FlagsSetOn(defaultEnum));

			defaultEnum &= ~DefaultEnum.DefaultElementOne;
			Assert.AreEqual(1, NuGenEnum.FlagsSetOn(defaultEnum));
		}

		[Test]
		public void EnumSimpleTest()
		{
			SimpleEnum simpleEnum = SimpleEnum.SimpleElementOne | SimpleEnum.SimpleElementTwo;
			Assert.AreEqual(2, NuGenEnum.FlagsSetOn(simpleEnum));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void EnumArgumentNullExceptionTest()
		{
			NuGenEnum.FlagsSetOn(null);
		}

		[Test]
		public void EnumLongTest()
		{
			LongEnum longEnum = LongEnum.LongElementOne;
			Assert.AreEqual(1, NuGenEnum.FlagsSetOn(longEnum));

			longEnum |= LongEnum.LongElementThree;
			Assert.AreEqual(2, NuGenEnum.FlagsSetOn(longEnum));
		}

		[Test]
		public void EnumLongNegativeTest()
		{
			LongNegativeEnum negativeEnum = LongNegativeEnum.LongNegativeEnumTwo;
			Assert.AreEqual(1, NuGenEnum.FlagsSetOn(negativeEnum));

			negativeEnum |= LongNegativeEnum.LongNegativeEnumThree;
			Assert.AreEqual(1, NuGenEnum.FlagsSetOn(negativeEnum));
		}
	}
}
