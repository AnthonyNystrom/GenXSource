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
		[Test]
		public void AddFolderTest()
		{
			int oldNodesCount = this._TaskTreeView.Nodes.Count;

			this._TaskTreeView.AddFolder();
			
			Assert.AreEqual(oldNodesCount + 1, this._TaskTreeView.Nodes.Count);
			Assert.IsFalse(this._TaskTreeView.Nodes[0].HasCheckBox);

			this._TaskTreeView.AddFolder(new NuGenFolderTreeNode(this._ServiceProvider));

			Assert.AreEqual(oldNodesCount + 2, this._TaskTreeView.Nodes.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddFolderArgumentNullExceptionTest()
		{
			this._TaskTreeView.AddFolder((NuGenFolderTreeNode)null);
		}

		[Test]
		public void AddFolderEventTest()
		{
			this._EventSink.ExpectedFolderAddedCallsCount = 1;
			this._TaskTreeView.AddFolder();
			this._EventSink.Verify();
		}
	}
}
