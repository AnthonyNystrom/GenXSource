/* -----------------------------------------------
 * NuGenPropertiesTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenPropertiesTests
	{
		private NuGenProperties _properties;

		[SetUp]
		public void SetUp()
		{
			_properties = new NuGenProperties(new int());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			_properties = new NuGenProperties(null);
		}

		[Test]
		[ExpectedException(typeof(NuGenPropertyNotFoundException))]
		public void PropertyNotFoundExceptionTest()
		{
			NuGenPropertyInfo propertyInfo = _properties["PropertyNotExist"];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentNullExceptionTest()
		{
			NuGenPropertyInfo propertyInfo = _properties[null];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentNullException2Test()
		{
			NuGenPropertyInfo propertyInfo = _properties[""];
		}
	}
}
