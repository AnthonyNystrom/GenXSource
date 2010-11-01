/* -----------------------------------------------
 * NuGenPropertyInfoTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenPropertyInfoTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			NuGenPropertyInfo propertyInfo = new NuGenPropertyInfo(null, new object());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullException2Test()
		{
			PropertyInfo info = typeof(string).GetProperty("Length", BindingFlags.Instance | BindingFlags.Public);
			Assert.IsNotNull(info);

			NuGenPropertyInfo propertyInfo = new NuGenPropertyInfo(info, null);
		}
	}
}
