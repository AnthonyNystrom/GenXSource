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
		public void ClearTest()
		{
			this._TaskTreeView.Nodes.AddNode(new NuGenFolderTreeNode(this._ServiceProvider));
			this._TaskTreeView.Nodes.AddNode(new NuGenCommentsFolderTreeNode(this._ServiceProvider));

			Assert.AreEqual(this._InitialCount + 2, this._TaskTreeView.Nodes.Count);

			this._TaskTreeView.Clear();

			Assert.AreEqual(this._InitialCount + 1, this._TaskTreeView.Nodes.Count);
			Assert.IsTrue(this._TaskTreeView.Nodes[this._InitialCount] is NuGenCommentsFolderTreeNode);
		}
	}
}
