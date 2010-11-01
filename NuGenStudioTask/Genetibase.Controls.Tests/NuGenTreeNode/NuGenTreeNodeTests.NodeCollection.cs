/* -----------------------------------------------
 * NuGenTreeNodeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTreeNodeTests
	{
		class TreeNodeCollectionHandler : MockObject
		{
			private NuGenTreeNode treeNode = null;

			private ExpectationCounter checkBoxFullCount = new ExpectationCounter("checkBoxFullCount");
			private ExpectationCounter checkBoxLessCount = new ExpectationCounter("checkBoxLessCount");
			private ExpectationValue nodesCount = new ExpectationValue("nodesCount");

			public int ExpectedCheckBoxFullCount
			{
				set
				{
					this.checkBoxFullCount.Expected = value;
				}
			}

			public int ExpectedCheckBoxLessCount
			{
				set
				{
					this.checkBoxLessCount.Expected = value;
				}
			}

			public int ExpectedNodesCount
			{
				set
				{
					this.nodesCount.Expected = value;
				}
			}

			public void Run(int nodeCount)
			{
				bool hasCheckBox = false;

				for (int i = 0; i < nodeCount; i++)
				{
					this.treeNode.Nodes.AddNode(new NuGenTreeNode("", hasCheckBox = !hasCheckBox));
				}

				this.nodesCount.Actual = this.treeNode.Nodes.Count;

				foreach (NuGenTreeNode node in this.treeNode.Nodes)
				{
					if (node.HasCheckBox)
					{
						this.checkBoxFullCount.Inc();
					}
					else
					{
						this.checkBoxLessCount.Inc();
					}
				}
			}

			public TreeNodeCollectionHandler(NuGenTreeNode treeNode)
			{
				if (treeNode == null)
				{
					Assert.Fail("treeNode cannot be null.");
				}

				this.treeNode = treeNode;
			}
		}

		[Test]
		public void PopulationTest()
		{
			TreeNodeCollectionHandler nodeCollectionHandler = new TreeNodeCollectionHandler(this.treeNode);

			int nodeCount = 10;

			nodeCollectionHandler.ExpectedCheckBoxFullCount = nodeCount / 2;
			nodeCollectionHandler.ExpectedCheckBoxLessCount = nodeCount / 2;
			nodeCollectionHandler.ExpectedNodesCount = nodeCount;

			nodeCollectionHandler.Run(nodeCount);

			nodeCollectionHandler.Verify();
		}

		[Test]
		public void AddRangeTest()
		{
			NuGenTreeNode node = new NuGenTreeNode();
			NuGenTreeNode node2 = new NuGenTreeNode();

			NuGenTreeNode[] treeNodeRange = new NuGenTreeNode[] { node, node2 };

			this.treeNode.Nodes.AddNodeRange(treeNodeRange);

			Assert.AreEqual(2, this.treeNode.Nodes.Count);
			Assert.AreEqual(node, this.treeNode.Nodes[0]);
			Assert.AreEqual(node2, this.treeNode.Nodes[1]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddRangeNullTest()
		{
			this.treeNode.Nodes.AddNodeRange(null);
		}

		[Test]
		public void ClearTest()
		{
			this.treeNode.Nodes.AddNode(new NuGenTreeNode());
			this.treeNode.Nodes.AddNode(new NuGenTreeNode());
			Assert.AreEqual(2, this.treeNode.Nodes.Count);

			this.treeNode.Nodes.Clear();
			Assert.AreEqual(0, this.treeNode.Nodes.Count);
		}

		[Test]
		public void ContainsTest()
		{
			NuGenTreeNode treeNodeToCheck = new NuGenTreeNode();
			this.treeNode.Nodes.AddNode(treeNodeToCheck);
			Assert.IsTrue(this.treeNode.Nodes.Contains(treeNodeToCheck));
			Assert.IsFalse(this.treeNode.Nodes.Contains(null));
		}

		[Test]
		public void IndexerTest()
		{
			NuGenTreeNode childNode = new NuGenTreeNode();
			NuGenTreeNode childNode2 = new NuGenTreeNode();

			this.treeNode.Nodes.AddNode(childNode);
			this.treeNode.Nodes.AddNode(childNode2);

			Assert.AreEqual(childNode, this.treeNode.Nodes[0]);
			Assert.AreEqual(childNode2, this.treeNode.Nodes[1]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddNodeArgumentNullExceptionTest()
		{
			this.treeNode.Nodes.AddNode(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InsertNodeArgumentNullExceptionTest()
		{
			this.treeNode.Nodes.InsertNode(0, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveNodeArgumentNullExceptionTest()
		{
			this.treeNode.Nodes.RemoveNode(null);
		}
	}
}
