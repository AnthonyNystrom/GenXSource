/* -----------------------------------------------
 * NuGenExtensibilityTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Extensibility;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenExtensibilityTests
	{
		private string _SolutionPath = @"C:\SolutionFolder\Solution.sln";

		[Test]
		public void BuildSlnAssociatedPathTest()
		{
			string associatedPath = @"C:\SolutionFolder\Solution.ext";
			string associatedFileExtension = "ext";

			string pathToCheck = NuGenExtensibility.BuildSlnAssociatedPath(
				_SolutionPath,
				associatedFileExtension
			);

			Assert.AreEqual(associatedPath, pathToCheck);
		}

		[Test]
		public void BuildSlnAssociatedPathEmptyExtensionTest()
		{
			string associatedPath = @"C:\SolutionFolder\Solution";

			string pathToCheck = NuGenExtensibility.BuildSlnAssociatedPath(_SolutionPath, "");
			Assert.AreEqual(associatedPath, pathToCheck);

			pathToCheck = NuGenExtensibility.BuildSlnAssociatedPath(_SolutionPath, null);
			Assert.AreEqual(associatedPath, pathToCheck);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildSlnAssociatedPathArgumentNullExceptionTest()
		{
			NuGenExtensibility.BuildSlnAssociatedPath(null, "");
		}
	}
}
