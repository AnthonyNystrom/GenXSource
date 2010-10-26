/* -----------------------------------------------
 * NuGenEnumTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEnumTests
	{
		[Test]
		public void ToUInt64Test()
		{
			DefaultEnum defaultEnum = DefaultEnum.DefaultElementOne | DefaultEnum.DefaultElementTwo;

			Assert.AreEqual(
				NuGenInvoker<Enum>.Methods["ToUInt64"].Invoke<ulong>(defaultEnum),
				NuGenEnum.ToUInt64(defaultEnum)
				);

			defaultEnum |= DefaultEnum.DefaultElementThree;
		}

		[Test]
		public void ToUInt64NegativeTest()
		{
			LongNegativeEnum longNegativeEnum = LongNegativeEnum.LongNegativeEnumOne;

			Assert.AreEqual(
				NuGenInvoker<Enum>.Methods["ToUInt64"].Invoke<ulong>(longNegativeEnum),
				NuGenEnum.ToUInt64(longNegativeEnum)
			);

			longNegativeEnum |= LongNegativeEnum.LongNegativeEnumTwo;

			Assert.AreEqual(
				NuGenInvoker<Enum>.Methods["ToUInt64"].Invoke<ulong>(longNegativeEnum),
				NuGenEnum.ToUInt64(longNegativeEnum)
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ToUInt64ArgumentExceptionTest()
		{
			NuGenEnum.ToUInt64(new object());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToUInt64ArgumentNullExceptionTest()
		{
			NuGenEnum.ToUInt64(null);
		}
	}
}
