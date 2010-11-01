/* -----------------------------------------------
 * NuGenTreeNodeCollectionTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Collections;

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTreeNodeCollectionTests
	{
		class TreeNodeCollectionEventSink : MockObject
		{
			#region Methods.Public.Expectations

			/*
			 * ClearRequestedCount
			 */

			private ExpectationCounter clearRequestedCount = new ExpectationCounter("clearRequestedCount");

			public int ExpectedClearRequestedCount
			{
				set
				{
					this.clearRequestedCount.Expected = value;
				}
			}

			/*
			 * ContainsNodeRequestedCount
			 */

			private ExpectationCounter containsNodeRequestedCount = new ExpectationCounter("containsNodeRequestedCount");

			public int ExpectedContainsNodeRequestedCount
			{
				set
				{
					this.containsNodeRequestedCount.Expected = value;
				}
			}

			/*
			 * ContainsNodeRequested
			 */

			private ExpectationArrayList containsNodeRequested = new ExpectationArrayList("containsNodeRequested");

			public void AddExpectedContainsNodeRequested(NuGenTreeNode treeNode)
			{
				this.containsNodeRequested.AddExpected(treeNode);
			}

			/*
			 * CountRequestedCount
			 */

			private ExpectationCounter countRequestedCount = new ExpectationCounter("countRequestedCount");

			public int ExpectedCountRequestedCount
			{
				set
				{
					this.countRequestedCount.Expected = value;
				}
			}

			/*
			 * EnumratorRequestsCount
			 */

			private ExpectationCounter enumeratorRequestsCount = new ExpectationCounter("enumeratorRequestsCount");

			public int ExpectedEnumeratorRequestsCount
			{
				set
				{
					this.enumeratorRequestsCount.Expected = value;
				}
			}

			/*
			 * NodeByIndexAdjustedCount
			 */

			private ExpectationCounter nodeByIndexAdjustedCount = new ExpectationCounter("nodeByIndexAdjustedCount");

			public int ExpectedNodeByIndexAdjustedCount
			{
				set
				{
					this.nodeByIndexAdjustedCount.Expected = value;
				}
			}

			/*
			 * NodeByIndexRequestedCount
			 */

			private ExpectationCounter nodeByIndexRequestedCount = new ExpectationCounter("nodeByIndexRequestedCount");

			public int ExpectedNodeByIndexRequestedCount
			{
				set
				{
					this.nodeByIndexRequestedCount.Expected = value;
				}
			}

			/*
			 * NodeRangeAdded
			 */

			private ExpectationArrayList nodeRangeAdded = new ExpectationArrayList("nodeRangeAdded");

			public void AddExpectedNodeRangeAdded(NuGenTreeNode[] treeNodes)
			{
				this.nodeRangeAdded.AddExpected(treeNodes);
			}

			/*
			 * NodeRangeAddedCount
			 */

			private ExpectationCounter nodeRangeAddedCount = new ExpectationCounter("nodeRangeAddedCount");

			public int ExpectedNodeRangeAddedCount
			{
				set
				{
					this.nodeRangeAddedCount.Expected = value;
				}
			}

			/*
			 * NodesAdded
			 */

			private ExpectationArrayList nodeAdded = new ExpectationArrayList("nodeAdded");

			public void AddExpectedNodeAdded(NuGenTreeNode treeNode)
			{
				this.nodeAdded.AddExpected(treeNode);
			}

			/*
			 * NodesAddedCount
			 */

			private ExpectationCounter nodeAddedCount = new ExpectationCounter("nodeAddedCount");

			public int ExpectedNodeAddedCount
			{
				set
				{
					this.nodeAddedCount.Expected = value;
				}
			}

			/*
			 * NodeInsertedCount
			 */

			private ExpectationCounter nodeInsertedCount = new ExpectationCounter("nodeInsertedCount");

			public int ExpectedNodeInsertedCount
			{
				set
				{
					this.nodeInsertedCount.Expected = value;
				}
			}
			
			/*
			 * NodeRemoved
			 */

			private ExpectationArrayList nodeRemoved = new ExpectationArrayList("nodeRemoved");

			public void AddExpectedNodeRemoved(NuGenTreeNode treeNode)
			{
				this.nodeRemoved.AddExpected(treeNode);
			}

			/*
			 * NodeRemovedCount
			 */

			private ExpectationCounter nodeRemovedCount = new ExpectationCounter("nodeRemovedCount");

			public int ExpectedNodeRemovedCount
			{
				set
				{
					this.nodeRemovedCount.Expected = value;
				}
			}

			#endregion

			#region Constructors

			public TreeNodeCollectionEventSink(NuGenTreeNodeCollection treeNodeCollection)
			{
				if (treeNodeCollection == null)
				{
					Assert.Fail("treeNodeCollection cannot be null.");
				}

				treeNodeCollection.ContainsNodeRequested += delegate(object sender, NuGenContainsItemRequestedEventArgs e)
				{
					this.containsNodeRequestedCount.Inc();
					this.containsNodeRequested.AddActual(e.NodeToCheck);
				};

				treeNodeCollection.EnumeratorRequested += delegate(object sender, NuGenEnumeratorRequestedEventArgs e)
				{
					this.enumeratorRequestsCount.Inc();
				};

				treeNodeCollection.NodeAdded += delegate(object sender, NuGenAddTreeNodeEventArgs e)
				{
					this.nodeAddedCount.Inc();
					this.nodeAdded.AddActual(e.TreeNodeToAdd);
				};

				treeNodeCollection.NodeRangeAdded += delegate(object sender, NuGenAddTreeNodeRangeEventArgs e)
				{
					this.nodeRangeAddedCount.Inc();
					this.nodeRangeAdded.AddActual(e.TreeNodeRangeToAdd);
				};

				treeNodeCollection.NodeInserted += delegate(object sender, NuGenAddTreeNodeEventArgs e)
				{
					this.nodeInsertedCount.Inc();
					this.nodeAdded.AddActual(e.TreeNodeToAdd);
				};

				treeNodeCollection.NodeRemoved += delegate(object sender, NuGenRemoveTreeNodeEventArgs e)
				{
					this.nodeRemovedCount.Inc();
					this.nodeRemoved.AddActual(e.TreeNodeToRemove);
				};

				treeNodeCollection.NodeByIndexAdjusted += delegate(object sender, NuGenIndexedTreeNodeEventArgs e)
				{
					this.nodeByIndexAdjustedCount.Inc();
				};

				treeNodeCollection.NodeByIndexRequested += delegate(object sender, NuGenIndexedTreeNodeEventArgs e)
				{
					this.nodeByIndexRequestedCount.Inc();
				};

				treeNodeCollection.NodeCountRequested += delegate(object sender, NuGenItemsCountRequestedEventArgs e)
				{
					this.countRequestedCount.Inc();
				};

				treeNodeCollection.ClearNodesRequested += delegate(object sender, NuGenItemsClearRequestedEventArgs e)
				{
					this.clearRequestedCount.Inc();
				};
			}

			#endregion
		}
	}
}
