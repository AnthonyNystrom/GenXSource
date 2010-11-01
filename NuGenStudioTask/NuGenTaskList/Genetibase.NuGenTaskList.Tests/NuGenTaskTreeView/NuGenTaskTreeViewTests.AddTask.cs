/* -----------------------------------------------
 * NuGenTaskTreeViewTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskTreeViewTests
	{
		[Test]
		public void AddTaskTest()
		{
			this._TaskTreeView.AddTask();

			Assert.AreEqual(this._InitialCount + 1, this._TaskTreeView.Nodes.Count);
			Assert.IsTrue(this._TaskTreeView.Nodes[this._InitialCount].HasCheckBox);

			this._TaskTreeView.AddTask(new NuGenTaskTreeNode(this._ServiceProvider));
			Assert.AreEqual(this._InitialCount + 2, this._TaskTreeView.Nodes.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddTaskArgumentNullExceptionTest()
		{
			this._TaskTreeView.AddTask((NuGenTaskTreeNode)null);
		}

		[Test]
		public void AddTaskEventTest()
		{
			this._EventSink.ExpectedTaskAddedCallsCount = 1;
			this._TaskTreeView.AddTask();
			this._EventSink.Verify();
		}
	}
}
