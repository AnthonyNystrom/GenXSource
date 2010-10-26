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
		public void ClearCodeTasks()
		{
			_TaskTreeView.AddCodeTask(_TaskItem);
			NuGenCommentsFolderTreeNode folder = (NuGenCommentsFolderTreeNode)_TaskTreeView.Nodes[0];
			Assert.AreEqual(1, folder.Nodes.Count);

			_TaskTreeView.ClearCodeTasks();
			Assert.AreEqual(0, folder.Nodes.Count);
		}
	}
}
