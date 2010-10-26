/* -----------------------------------------------
 * NuGenFieldInfoTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenFieldInfoTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			NuGenFieldInfo fieldInfo = new NuGenFieldInfo(null, new object());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullException2Test()
		{
			FieldInfo info = typeof(int).GetField("MinValue");
			Assert.IsNotNull(info);

			NuGenFieldInfo fieldInfo = new NuGenFieldInfo(info, null);
		}
	}
}
