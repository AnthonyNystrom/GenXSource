/* -----------------------------------------------
 * NuGenPropertyDescriptorTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenPropertyDescriptorTests
	{
		[Test]
		public void ConstructorArgumentNullException()
		{
			NuGenPropertyDescriptor descriptor;

			try
			{
				descriptor = new NuGenPropertyDescriptor(null, new object());
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				descriptor = new NuGenPropertyDescriptor(
					TypeDescriptor.GetProperties(new NuGenObjectDescriptorTests.DummyClass())["MyProperty"],
					null
				);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}
	}
}
