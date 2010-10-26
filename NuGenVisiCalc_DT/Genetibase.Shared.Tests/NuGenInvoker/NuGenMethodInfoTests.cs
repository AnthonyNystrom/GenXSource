/* -----------------------------------------------
 * NuGenMethodInfoTests.cs
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
	public class NuGenMethodInfoTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			NuGenMethodInfo methodInfo = new NuGenMethodInfo(null);
		}
	}
}
