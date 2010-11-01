/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
			Assert.IsTrue(NuGenArgument.IsValidFilename(validFilename));

			string invalidFilename = string.Concat(validFilename, new string(Path.GetInvalidFileNameChars()[0], 1));
			Assert.IsFalse(NuGenArgument.IsValidFilename(invalidFilename));
		}

		[Test]
		public void IsValidFilenameEmptyStringTest()
		{
			Assert.IsFalse(NuGenArgument.IsValidFilename(null));
			Assert.IsFalse(NuGenArgument.IsValidFilename(""));
		}
	}
}
