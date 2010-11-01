/* -----------------------------------------------
 * NuGenTreeNodeCollectionTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Collections;

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Genetibase.Controls.Tests
{
	[TestFixture]
	public partial class NuGenTreeNodeCollectionTests
	{
		private NuGenTreeNodeCollection treeNodeCollection = null;

		[SetUp]
		public void SetUp()
		{
			this.treeNodeCollection = new NuGenTreeNodeCollection();
		}

		[Test]
		public void EventTest()
		{
			TreeNodeCollectionEventSink eventSink = new TreeNodeCollectionEventSink(this.treeNodeCollection);

			NuGenTreeNode toAddNode = new NuGenTreeNode();
			NuGenTreeNode toInsertNode = new NuGenTreeNode();

			eventSink.ExpectedClearRequestedCount = 1;
			eventSink.ExpectedContainsNodeRequestedCount = 2;
			eventSink.ExpectedCountRequestedCount = 1;
			eventSink.ExpectedEnumeratorRequestsCount = 1;
			eventSink.ExpectedNodeByIndexAdjustedCount = 1;
			eventSink.ExpectedNodeByIndexRequestedCount = 1;
			eventSink.ExpectedNodeAddedCount = 2;
			eventSink.ExpectedNodeRangeAddedCount = 1;
			eventSink.ExpectedNodeInsertedCount = 1;
			eventSink.ExpectedNodeRemovedCount = 2;

			eventSink.AddExpectedContainsNodeRequested(toAddNode);
			eventSink.AddExpectedContainsNodeRequested(toInsertNode);
			eventSink.AddExpectedNodeAdded(toAddNode);
			eventSink.AddExpectedNodeAdded(toInsertNode);
			eventSink.AddExpectedNodeAdded(toAddNode);
			eventSink.AddExpectedNodeRangeAdded(new NuGenTreeNode[] { toAddNode, toInsertNode });

			eventSink.AddExpectedNodeRemoved(toAddNode);
			eventSink.AddExpectedNodeRemoved(toInsertNode);

			this.treeNodeCollection.AddNode(toAddNode);
			this.treeNodeCollection.InsertNode(0, toInsertNode);
			this.treeNodeCollection.AddNode(toAddNode);
			this.treeNodeCollection.AddNodeRange(new NuGenTreeNode[] { toAddNode, toInsertNode });
			this.treeNodeCollection.RemoveNode(toAddNode);
			this.treeNodeCollection.RemoveNode(toInsertNode);
			this.treeNodeCollection.GetEnumerator();
			this.treeNodeCollection.Contains(toAddNode);
			this.treeNodeCollection.Contains(toInsertNode);

			Assert.AreEqual(0, this.treeNodeCollection.Count);
			Assert.AreEqual(null, this.treeNodeCollection[0]);
			this.treeNodeCollection[0] = null;

			this.treeNodeCollection.Clear();

			eventSink.Verify();
		}
	}
}
