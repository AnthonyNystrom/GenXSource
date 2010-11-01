/* -----------------------------------------------
 * NuGenPropertyDescriptorCollectionTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenPropertyDescriptorCollectionTests
	{
		[Test]
		public void ConstructorArgumentNullExceptionTest()
		{
			NuGenPropertyDescriptorCollection collection;

			try
			{
				collection = new NuGenPropertyDescriptorCollection(
					null,
					new object()
				);

				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				collection = new NuGenPropertyDescriptorCollection(
					TypeDescriptor.GetProperties(new NuGenObjectDescriptorTests.DummyClass()),
					null
				);

				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}

		[Test]
		[ExpectedException(typeof(NuGenPropertyNotFoundException))]
		public void IndexerPropertyNotFoundExceptionTest()
		{
			int foo = 0;
			
			NuGenPropertyDescriptorCollection collection = new NuGenPropertyDescriptorCollection(
				TypeDescriptor.GetProperties(foo),
				foo
			);

			NuGenPropertyDescriptor descriptor = collection["SomePropertyThatDoesNotExist"];
		}
	}
}
