/* -----------------------------------------------
 * NuGenEnumTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.Shared;
using Genetibase.Shared.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEnumTests
	{
		[TestMethod]
		public void ToUInt64Test()
		{
			DefaultEnum defaultEnum = DefaultEnum.DefaultElementOne | DefaultEnum.DefaultElementTwo;

			Assert.AreEqual(
				NuGenInvoker<Enum>.Methods["ToUInt64"].Invoke<UInt64>(defaultEnum),
				NuGenEnum.ToUInt64(defaultEnum)
				);

			defaultEnum |= DefaultEnum.DefaultElementThree;
		}

		[TestMethod]
		public void ToUInt64NegativeTest()
		{
			LongNegativeEnum longNegativeEnum = LongNegativeEnum.LongNegativeEnumOne;

			Assert.AreEqual(
				NuGenInvoker<Enum>.Methods["ToUInt64"].Invoke<UInt64>(longNegativeEnum),
				NuGenEnum.ToUInt64(longNegativeEnum)
			);

			longNegativeEnum |= LongNegativeEnum.LongNegativeEnumTwo;

			Assert.AreEqual(
				NuGenInvoker<Enum>.Methods["ToUInt64"].Invoke<UInt64>(longNegativeEnum),
				NuGenEnum.ToUInt64(longNegativeEnum)
			);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ToUInt64ArgumentExceptionTest()
		{
			NuGenEnum.ToUInt64(new Object());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToUInt64ArgumentNullExceptionTest()
		{
			NuGenEnum.ToUInt64(null);
		}
	}
}
