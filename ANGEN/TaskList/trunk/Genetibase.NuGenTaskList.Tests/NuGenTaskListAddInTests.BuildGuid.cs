/* -----------------------------------------------
 * NuGenTaskListAddInTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:eisernWolf@gmail.com
 * --------------------------------------------- */

using Genetibase.NuGenTaskList;

using NUnit.Framework;

using System;
using System.Text.RegularExpressions;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskListAddInTests
	{
		[Test]
		public void BuildGuid()
		{
			Regex regex = new Regex(@"^\{\w{8,8}-(\w{4,4}-){3,3}\w{12,12}\}$");
			Assert.IsTrue(regex.IsMatch(this.taskList.BuildGuid()));
		}
	}
}
