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
		[Test]
		public void BuildCommandNameTest()
		{
			Assert.AreEqual(
				"Genetibase.Shared.Tests.NuGenExtensibilityTests+CommandHolderMock.Command",
				NuGenExtensibility.BuildCommandIdentifier(typeof(CommandHolderMock), "Command")
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuildCommandNameArgumentNullExceptionTest()
		{
			NuGenExtensibility.BuildCommandIdentifier(null, "");
		}
	}
}
