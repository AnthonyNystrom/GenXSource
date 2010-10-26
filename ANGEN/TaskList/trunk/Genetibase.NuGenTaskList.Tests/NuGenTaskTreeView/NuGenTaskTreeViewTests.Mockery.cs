/* -----------------------------------------------
 * NuGenTaskTreeViewTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskTreeViewTests
	{
		class TaskTreeViewEventSink : MockObject
		{
			#region Properties.Public.Expectations

			/*
			 * FolderAddedCallsCount
			 */

			private ExpectationCounter folderAddedCallsCount = new ExpectationCounter("folderAddedCallsCount");

			public int ExpectedFolderAddedCallsCount
			{
				set
				{
					this.folderAddedCallsCount.Expected = value;
				}
			}

			/*
			 * TaskAddedCallsCount
			 */

			private ExpectationCounter taskAddedCallsCount = new ExpectationCounter("taskAddedCallsCount");

			public int ExpectedTaskAddedCallsCount
			{
				set
				{
					this.taskAddedCallsCount.Expected = value;
				}
			}

			#endregion

			#region Constructors

			public TaskTreeViewEventSink(NuGenTaskTreeView taskTreeView)
			{
				if (taskTreeView == null)
				{
					Assert.Fail("taskTreeView cannot be null.");
				}

				taskTreeView.FolderAdded += delegate
				{
					this.folderAddedCallsCount.Inc();
				};

				taskTreeView.TaskAdded += delegate
				{
					this.taskAddedCallsCount.Inc();
				};
			}

			#endregion
		}
	}
}
