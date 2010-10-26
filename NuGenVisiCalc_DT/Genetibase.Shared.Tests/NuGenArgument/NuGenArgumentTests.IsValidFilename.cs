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
		public void IsValidFilenameTest()
		{
			string validFilename = "filename";
			Assert.IsTrue(NuGenArgument.IsValidFileName(validFilename));

			string invalidFilename = string.Concat(validFilename, new string(Path.GetInvalidFileNameChars()[0], 1));
			Assert.IsFalse(NuGenArgument.IsValidFileName(invalidFilename));
		}

		[Test]
		public void IsValidFilenameEmptyStringTest()
		{
			Assert.IsFalse(NuGenArgument.IsValidFileName(null));
			Assert.IsFalse(NuGenArgument.IsValidFileName(""));
		}
	}
}
