/* -----------------------------------------------
 * NuGenTaskListAddInTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using Genetibase.NuGenTaskList;

using NUnit.Framework;

using System;
using System.Text.RegularExpressions;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public partial class NuGenTaskListAddInTests
	{
		private NuGenTaskListAddIn taskList = null;
		private Array custom = null;

		[SetUp]
		public void SetUp()
		{
			this.custom = Array.CreateInstance(typeof(int), 0);
			this.taskList = new NuGenTaskListAddIn();
		}
	}
}
