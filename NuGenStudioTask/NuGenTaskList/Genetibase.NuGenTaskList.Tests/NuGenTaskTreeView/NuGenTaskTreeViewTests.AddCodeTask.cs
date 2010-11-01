/* -----------------------------------------------
 * NuGenTaskTreeViewTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using EnvDTE;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskTreeViewTests
	{
		[Test]
		public void AddCodeTaskTest()
		{
			_TaskTreeView.AddCodeTask(_TaskItem);

			NuGenCommentsFolderTreeNode folder = (NuGenCommentsFolderTreeNode)_TaskTreeView.Nodes[0];
			Assert.AreEqual(1, folder.Nodes.Count);
			Assert.AreEqual(_TaskItemDescription, ((NuGenTaskTreeNodeBase)folder.Nodes[0]).Text);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddCodeTaskArgumentNullExceptionTest()
		{
			_TaskTreeView.AddCodeTask(null);
		}
	}
}
