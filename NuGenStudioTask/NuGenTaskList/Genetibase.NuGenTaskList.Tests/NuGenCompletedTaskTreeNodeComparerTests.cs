/* -----------------------------------------------
 * NuGenCompletedTaskTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public partial class NuGenCompletedTaskTreeNodeComparerTests
	{
		private NuGenCompletedTaskTreeNodeComparer comparer = null;
		private INuGenServiceProvider serviceProvider = null;

		[SetUp]
		public void SetUp()
		{
			this.comparer = new NuGenCompletedTaskTreeNodeComparer();
			this.serviceProvider = new NuGenTaskServiceProvider();
		}

		[Test]
		public void CompareTest()
		{
			NuGenTaskTreeNode completedTask = new NuGenTaskTreeNode(this.serviceProvider);
			completedTask.Completed = true;

			NuGenTaskTreeNode unCompletedTask = new NuGenTaskTreeNode(this.serviceProvider);
			unCompletedTask.Completed = false;

			NuGenFolderTreeNode folder = new NuGenFolderTreeNode(this.serviceProvider);
			NuGenFolderTreeNode folder2 = new NuGenFolderTreeNode(this.serviceProvider);

			Assert.Less(this.comparer.Compare(folder, completedTask), 0);
			Assert.Less(this.comparer.Compare(folder, unCompletedTask), 0);
			Assert.Less(this.comparer.Compare(unCompletedTask, completedTask), 0);

			Assert.Greater(this.comparer.Compare(completedTask, unCompletedTask), 0);
			Assert.Greater(this.comparer.Compare(unCompletedTask, folder), 0);
			Assert.Greater(this.comparer.Compare(completedTask, folder), 0);
			
			Assert.AreEqual(0, this.comparer.Compare(folder, folder2));
			Assert.AreEqual(0, this.comparer.Compare(completedTask, completedTask));

			Assert.Greater(this.comparer.Compare(folder, null), 0);
			Assert.Less(this.comparer.Compare(null, unCompletedTask), 0);
		}
	}
}
