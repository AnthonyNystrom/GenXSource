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
			PropertyInfo info = this.BuildPropertyInfo();
			NuGenPropertyInfo propertyInfo = new NuGenPropertyInfo(info, null);
		}

		[Test]
		public void GetUnderlyingPropertyInfoTest()
		{
			PropertyInfo info = this.BuildPropertyInfo();
			string stubString = "";
			NuGenPropertyInfo propertyInfo = new NuGenPropertyInfo(info, stubString);
			Assert.AreEqual(info, propertyInfo.GetUnderlyingPropertyInfo());
		}

		private PropertyInfo BuildPropertyInfo()
		{
			return typeof(string).GetProperty("Length", BindingFlags.Instance | BindingFlags.Public);
		}
	}
}
