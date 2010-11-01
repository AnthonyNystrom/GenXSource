/* -----------------------------------------------
 * NuGenTaskListAddInTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using Genetibase.NuGenTaskList;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskListAddInTests
	{
		[Test]
		public void BuildCommandNameTest()
		{
			Assert.AreEqual(
				"Genetibase.NuGenTaskList.NuGenTaskListAddIn.NuGenTaskList",
				this.taskList.BuildCommandName("NuGenTaskList")
			);
		}
	}
}
