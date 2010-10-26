/* -----------------------------------------------
 * NuGenTestMethodsProcessorTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Agile.Tests.Mockery;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Agile.Tests
{
	[TestFixture]
	public partial class NuGenTestMethodsProcessorTests
	{
		[Test]
		public void GetTestMethodsTest()
		{
			List<NuGenTestInvoker> methods = NuGenTestMethodsProcessor.GetTestMethods(typeof(StaticTestFixture));
			Assert.AreEqual(2, methods.Count);

			foreach (NuGenTestInvoker testInvoker in methods)
			{
				testInvoker.Invoke();
			}

			methods = NuGenTestMethodsProcessor.GetTestMethods(typeof(DynamicTestFixture));
			Assert.AreEqual(2, methods.Count);

			foreach (NuGenTestInvoker testInvoker in methods)
			{
				testInvoker.Invoke();
			}

			DynamicTestFixture dynamicTestFixture = new DynamicTestFixture();
			methods = NuGenTestMethodsProcessor.GetTestMethods(dynamicTestFixture);
			Assert.AreEqual(4, methods.Count);

			foreach (NuGenTestInvoker testInvoker in methods)
			{
				testInvoker.Invoke();
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetTestMethodsArgumentNullExceptionTest()
		{
			NuGenTestMethodsProcessor.GetTestMethods(null);
		}
	}
}
