/* -----------------------------------------------
 * NuGenTaskTreeViewTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskTreeViewTests
	{
		[Test]
		public void DeleteTaskTest()
		{
			NuGenTreeNode treeNode = new NuGenTreeNode();

			this._TaskTreeView.Nodes.AddNode(treeNode);
			Assert.AreEqual(this._InitialCount + 1, this._TaskTreeView.Nodes.Count);

			this._TaskTreeView.DeleteTask(treeNode);
			Assert.AreEqual(this._InitialCount, this._TaskTreeView.Nodes.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DeleteTaskArgumentNullExceptionTest()
		{
			this._TaskTreeView.DeleteTask(null);
		}
	}
}
