/* -----------------------------------------------
 * NuGenTreeViewSorterTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using Genetibase.Controls.Collections;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Controls.Tests
{
	[TestFixture]
	public partial class NuGenTreeNodeSorterTests
	{
		private NuGenTreeNodeSorter sorter = null;
		private NuGenTreeView treeView = null;

		[SetUp]
		public void SetUp()
		{
			this.sorter = new NuGenTreeNodeSorter();
			this.treeView = new NuGenTreeView();
		}

		[Test]
		public void AZSortTest()
		{
			NuGenTreeNode mTreeNode = new NuGenTreeNode("m");
			NuGenTreeNode dTreeNode = new NuGenTreeNode("d");
			NuGenTreeNode cTreeNode = new NuGenTreeNode("c");
			NuGenTreeNode kTreeNode = new NuGenTreeNode("k");

			this.treeView.Nodes.AddNode(mTreeNode);
			this.treeView.Nodes.AddNode(dTreeNode);
			this.treeView.Nodes.AddNode(cTreeNode);
			this.treeView.Nodes.AddNode(kTreeNode);

			Assert.AreEqual(mTreeNode, this.treeView.Nodes[0]);
			Assert.AreEqual(dTreeNode, this.treeView.Nodes[1]);
			Assert.AreEqual(cTreeNode, this.treeView.Nodes[2]);
			Assert.AreEqual(kTreeNode, this.treeView.Nodes[3]);

			this.sorter.Sort(this.treeView.Nodes, new NuGenAZTreeNodeComparer());

			Assert.AreEqual(cTreeNode, this.treeView.Nodes[0]);
			Assert.AreEqual(dTreeNode, this.treeView.Nodes[1]);
			Assert.AreEqual(kTreeNode, this.treeView.Nodes[2]);
			Assert.AreEqual(mTreeNode, this.treeView.Nodes[3]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AZSortArgumentNullExceptionOnCollectionTest()
		{
			DynamicMock comparerMock = new DynamicMock(typeof(IComparer<NuGenTreeNode>));
			this.sorter.Sort(null, (IComparer<NuGenTreeNode>)comparerMock.Object);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AZSortArgumentNullExceptionOnComparerTest()
		{
			this.sorter.Sort<NuGenTreeNode>(new NuGenTreeNodeCollection(), null);
		}
	}
}
