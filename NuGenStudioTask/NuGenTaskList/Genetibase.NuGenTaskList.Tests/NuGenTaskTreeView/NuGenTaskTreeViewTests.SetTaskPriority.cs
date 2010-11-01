/* -----------------------------------------------
 * NuGenTaskTreViewTests.cs
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
		public void SetTaskPrioritySimpleTest()
		{
			int wantedImageIndex = 1;
			int criticalImageIndex = 2;

			NuGenTaskTreeNode task = new NuGenTaskTreeNode(this._ServiceProvider, "task");

			task.SetPriorityImageIndex(NuGenTaskPriority.Wanted, wantedImageIndex);
			task.SetPriorityImageIndex(NuGenTaskPriority.Critical, criticalImageIndex);

			Assert.AreEqual(NuGenTaskPriority.Wanted, task.TaskPriority);
			Assert.AreEqual(wantedImageIndex, task.ImageIndex);
			Assert.AreEqual(wantedImageIndex, task.SelectedImageIndex);

			this._TaskTreeView.SetTaskPriority(task, NuGenTaskPriority.Critical);

			Assert.AreEqual(NuGenTaskPriority.Critical, task.TaskPriority);
			Assert.AreEqual(criticalImageIndex, task.ImageIndex);
			Assert.AreEqual(criticalImageIndex, task.SelectedImageIndex);
		}

		[Test]
		public void SetTaskPriorityRecursionTest()
		{
			NuGenFolderTreeNode parentFolder = new NuGenFolderTreeNode(this._ServiceProvider, "parentFolder");
			NuGenFolderTreeNode childFolder = new NuGenFolderTreeNode(this._ServiceProvider, "childFolder");

			for (int i = 0; i < 3; i++)
			{
				childFolder.Nodes.AddNode(new NuGenTaskTreeNode(this._ServiceProvider, i.ToString()));
				parentFolder.Nodes.AddNode(new NuGenTaskTreeNode(this._ServiceProvider, i.ToString()));
			}

			parentFolder.Nodes.AddNode(childFolder);

			this._TaskTreeView.SetTaskPriority(parentFolder, NuGenTaskPriority.Critical);

			foreach (NuGenTreeNode treeNode in parentFolder.Nodes)
			{
				if (treeNode is NuGenTaskTreeNode)
				{
					Assert.AreEqual(NuGenTaskPriority.Critical, ((NuGenTaskTreeNode)treeNode).TaskPriority);
				}
			}

			foreach (NuGenTreeNode treeNode in childFolder.Nodes)
			{
				if (treeNode is NuGenTaskTreeNode)
				{
					Assert.AreEqual(NuGenTaskPriority.Critical, ((NuGenTaskTreeNode)treeNode).TaskPriority);
				}
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetTaskPriorityArgumentNullExceptionTest()
		{
			this._TaskTreeView.SetTaskPriority(null, NuGenTaskPriority.Critical);
		}
	}
}
