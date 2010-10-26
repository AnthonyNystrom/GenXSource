/* -----------------------------------------------
 * NuGenTaskListAddInTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using EnvDTE;
using EnvDTE80;
using Extensibility;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskListAddInTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OnConnectionApplicationArgumentNullExceptionTest()
		{
			this.taskList.OnConnection(null, ext_ConnectMode.ext_cm_UISetup, null, ref this.custom);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void OnConnectionApplicationArgumentExceptionTest()
		{
			DynamicMock mockAddIn = new DynamicMock(typeof(AddIn), "AddIn");

			this.taskList.OnConnection(
				new object(),
				ext_ConnectMode.ext_cm_UISetup,
				(AddIn)mockAddIn.Object,
				ref this.custom
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void OnConnectionAddInArgumentNullExceptionTest()
		{
			this.taskList.OnConnection(new object(), ext_ConnectMode.ext_cm_UISetup, null, ref this.custom);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void OnConnectionAddInArgumentExceptionTest()
		{
			DynamicMock mockAddIn = new DynamicMock(typeof(DTE2), "AddIn");

			this.taskList.OnConnection(
				(DTE2)mockAddIn.Object,
				ext_ConnectMode.ext_cm_UISetup,
				new object(),
				ref this.custom
			);
		}
	}
}
