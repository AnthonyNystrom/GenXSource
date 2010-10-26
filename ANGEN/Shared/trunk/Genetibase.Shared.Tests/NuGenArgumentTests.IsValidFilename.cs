/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.IO;
using Genetibase.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[TestMethod]
		public void IsValidFilenameTest()
		{
			String validFilename = "filename";
			Assert.IsTrue(NuGenArgument.IsValidFileName(validFilename));

			String invalidFilename = String.Concat(validFilename, new String(Path.GetInvalidFileNameChars()[0], 1));
			Assert.IsFalse(NuGenArgument.IsValidFileName(invalidFilename));
		}

		[TestMethod]
		public void IsValidFilenameEmptyStringTest()
		{
			Assert.IsFalse(NuGenArgument.IsValidFileName(null));
			Assert.IsFalse(NuGenArgument.IsValidFileName(""));
		}
	}
}
