/* -----------------------------------------------
 * NuGenBooleanTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public class NuGenBooleanTests
	{
		[TestMethod]
		public void TrueTest()
		{
			Assert.AreEqual(true, NuGenBoolean.TrueBox);
		}

		[TestMethod]
		public void FalseTest()
		{
			Assert.AreEqual(false, NuGenBoolean.FalseBox);
		}

		[TestMethod]
		public void BoxTest()
		{
			Assert.ReferenceEquals(NuGenBoolean.TrueBox, NuGenBoolean.Boxed(true));
			Assert.ReferenceEquals(NuGenBoolean.FalseBox, NuGenBoolean.Boxed(false));
		}
	}
}
