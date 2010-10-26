/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.IO;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[Test]
		public void IsValidDirectoryNameTest()
		{
			string validDirectory = "directory";
			Assert.IsTrue(NuGenArgument.IsValidDirectoryName(validDirectory));

			string invalidDirectory = string.Concat(validDirectory, new string(Path.GetInvalidPathChars()[0], 1));
			Assert.IsFalse(NuGenArgument.IsValidDirectoryName(invalidDirectory));
		}

		[Test]
		public void IsValidDirectoryNameEmptyStringTest()
		{
			Assert.IsFalse(NuGenArgument.IsValidDirectoryName(null));
			Assert.IsFalse(NuGenArgument.IsValidDirectoryName(""));
		}
	}
}
