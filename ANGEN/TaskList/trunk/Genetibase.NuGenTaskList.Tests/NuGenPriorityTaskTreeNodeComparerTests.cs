/* -----------------------------------------------
 * NuGenPriorityTaskTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Controls.Collections;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public partial class NuGenPriorityTaskTreeNodeComparerTests
	{
		private NuGenPriorityTaskTreeNodeComparer comparer = null;
		private INuGenServiceProvider serviceProvider = null;

		[SetUp]
		public void SetUp()
		{
			this.comparer = new NuGenPriorityTaskTreeNodeComparer();
			this.serviceProvider = new NuGenTaskServiceProvider();
		}

		[Test]
		public void CompareTest()
		{
			NuGenFolderTreeNode folder = new NuGenFolderTreeNode(this.serviceProvider);
			
			NuGenTaskTreeNode criticalTask = new NuGenTaskTreeNode(
				this.serviceProvider,
				"Critical",
				NuGenTaskPriority.Critical
			);
			
			NuGenTaskTreeNode requiredTask = new NuGenTaskTreeNode(
				this.serviceProvider, 
				"Required",
				NuGenTaskPriority.Required
				);
			
			NuGenTaskTreeNode wantedTask = new NuGenTaskTreeNode(
				this.serviceProvider,
				"Wanted",
				NuGenTaskPriority.Required
			);
			
			NuGenTaskTreeNode wouldBeNiceTask = new NuGenTaskTreeNode(
				this.serviceProvider,
				"WouldBeNice",
				NuGenTaskPriority.WouldBeNice
			);
			
			NuGenTaskTreeNode maybeTask = new NuGenTaskTreeNode(
				this.serviceProvider,
				"Maybe",
				NuGenTaskPriority.Maybe
			);

			NuGenTaskTreeNodeBase[] nodes = new NuGenTaskTreeNodeBase[]
			{
				criticalTask,
				requiredTask,
				wantedTask,
				wouldBeNiceTask,
				maybeTask
			};

			foreach (NuGenTaskTreeNodeBase node in nodes)
			{
				Assert.Less(this.comparer.Compare(folder, node), 0);
				Assert.Greater(this.comparer.Compare(node, folder), 0);
			}

			Assert.Less(this.comparer.Compare(criticalTask, maybeTask), 0);
			Assert.Less(this.comparer.Compare(wantedTask, wouldBeNiceTask), 0);
			Assert.Less(this.comparer.Compare(wouldBeNiceTask, maybeTask), 0);

			Assert.Greater(this.comparer.Compare(requiredTask, criticalTask), 0);
			Assert.Greater(this.comparer.Compare(wantedTask, criticalTask), 0);
			Assert.Greater(this.comparer.Compare(wouldBeNiceTask, requiredTask), 0);

			Assert.Greater(this.comparer.Compare(folder, null), 0);
			Assert.Less(this.comparer.Compare(null, folder), 0);
		}
	}
}
