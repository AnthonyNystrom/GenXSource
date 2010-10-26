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
		private string _stubString;

		[SetUp]
		public void SetUp()
		{
			_stubString = "";
			_properties = new NuGenProperties(_stubString);
		}

		[Test]
		public void FindPropertyTest()
		{
			NuGenPropertyInfo propertyInfo = _properties.FindProperty(null);
			Assert.IsNull(propertyInfo);

			propertyInfo = _properties.FindProperty("PropertyNotExist");
			Assert.IsNull(propertyInfo);

			propertyInfo = _properties.FindProperty("Length");
			Assert.IsNotNull(propertyInfo);
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
