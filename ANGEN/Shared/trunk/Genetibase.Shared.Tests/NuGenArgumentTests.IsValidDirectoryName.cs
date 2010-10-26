/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[TestMethod]
		public void IsValidDirectoryNameTest()
		{
			String validDirectory = "directory";
			Assert.IsTrue(NuGenArgument.IsValidDirectoryName(validDirectory));

			String invalidDirectory = String.Concat(validDirectory, new String(Path.GetInvalidPathChars()[0], 1));
			Assert.IsFalse(NuGenArgument.IsValidDirectoryName(invalidDirectory));
		}

		[TestMethod]
		public void IsValidDirectoryNameEmptyStringTest()
		{
			Assert.IsFalse(NuGenArgument.IsValidDirectoryName(null));
			Assert.IsFalse(NuGenArgument.IsValidDirectoryName(""));
		}
	}
}
