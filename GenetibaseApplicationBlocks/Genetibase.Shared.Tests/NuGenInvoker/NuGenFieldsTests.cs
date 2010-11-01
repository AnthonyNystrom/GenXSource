/* -----------------------------------------------
 * NuGenFieldsTests.cs
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
	public class NuGenFieldsTests
	{
		private NuGenFields _fields;

		[SetUp]
		public void SetUp()
		{
			_fields = new NuGenFields(new int());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			_fields = new NuGenFields(null);
		}

		[Test]
		[ExpectedException(typeof(NuGenFieldNotFoundException))]
		public void FieldNotFoundExceptionTest()
		{
			NuGenFieldInfo fieldInfo = _fields["MethodNotExist"];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentNullExceptionTest()
		{
			NuGenFieldInfo fieldInfo = _fields[null];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentNullException2Test()
		{
			NuGenFieldInfo fieldInfo = _fields[""];
		}
	}
}
