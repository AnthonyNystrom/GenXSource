/* -----------------------------------------------
 * NuGenMethodsTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenMethodsTests
	{
		private NuGenMethods _methods;

		[SetUp]
		public void SetUp()
		{
			_methods = new NuGenMethods(new NuGenInvokerTests.DummyClass());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			NuGenMethods methods = new NuGenMethods((object)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullException2Test()
		{
			NuGenMethods methods = new NuGenMethods((Type)null);
		}

		[Test]
		[ExpectedException(typeof(NuGenMethodNotFoundException))]
		public void InvalidMethodNameExceptionTest()
		{
			NuGenMethodInfo methodNotExist = _methods["MethodNotExist"];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InvalidMethodNameException2Test()
		{
			NuGenMethodInfo methodNull = _methods[null];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InvalidMethodNameException3Test()
		{
			NuGenMethodInfo methodEmpty = _methods[""];
		}
	}
}
