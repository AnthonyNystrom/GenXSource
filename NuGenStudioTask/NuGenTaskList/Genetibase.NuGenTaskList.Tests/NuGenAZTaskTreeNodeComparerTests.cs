/* -----------------------------------------------
 * NuGenAZTaskTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public partial class NuGenAZTaskTreeNodeComparerTests
	{
		private NuGenAZTaskTreeNodeComparer comparer = null;
		private NuGenTreeView treeView = null;
		private INuGenServiceProvider serviceProvider = null;

		[SetUp]
		public void SetUp()
		{
			this.comparer = new NuGenAZTaskTreeNodeComparer();
			this.treeView = new NuGenTreeView();
			this.serviceProvider = new NuGenTaskServiceProvider();
		}

		[Test]
		public void CompareTest()
		{
			NuGenFolderTreeNode folder = new NuGenFolderTreeNode(this.serviceProvider);
			NuGenFolderTreeNode folder2 = new NuGenFolderTreeNode(this.serviceProvider);

			NuGenTaskTreeNode task = new NuGenTaskTreeNode(this.serviceProvider);
			NuGenTaskTreeNode task2 = new NuGenTaskTreeNode(this.serviceProvider);

			this.treeView.Nodes.AddNodeRange(new NuGenTreeNode[] { folder, folder2, task, task2 });

			folder.Text = "a";
			folder2.Text = "b";

			task.Text = "c";
			task2.Text = "d";

			Assert.Less(this.comparer.Compare(folder, folder2), 0);
			Assert.Greater(this.comparer.Compare(folder2, folder), 0);

			Assert.Less(this.comparer.Compare(task, task2), 0);
			Assert.Greater(this.comparer.Compare(task2, task), 0);

			Assert.Less(this.comparer.Compare(folder, task), 0);
			Assert.Less(this.comparer.Compare(folder2, task), 0);
			Assert.Less(this.comparer.Compare(folder, task2), 0);
			Assert.Less(this.comparer.Compare(folder2, task2), 0);

			Assert.Greater(this.comparer.Compare(folder, null), 0);
			Assert.Less(this.comparer.Compare(null, task2), 0);
		}
	}
}
