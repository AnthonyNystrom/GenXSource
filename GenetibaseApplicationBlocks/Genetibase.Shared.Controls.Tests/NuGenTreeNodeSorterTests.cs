/* -----------------------------------------------
 * NuGenTreeViewSorterTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using Genetibase.Shared.Controls.Collections;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
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

			this.treeView.Nodes.Add(mTreeNode);
			this.treeView.Nodes.Add(dTreeNode);
			this.treeView.Nodes.Add(cTreeNode);
			this.treeView.Nodes.Add(kTreeNode);

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
			this.sorter.Sort<NuGenTreeNode>(treeView.Nodes, null);
		}
	}
}
