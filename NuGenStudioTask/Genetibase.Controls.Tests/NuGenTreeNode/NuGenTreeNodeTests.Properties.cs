/* -----------------------------------------------
 * NuGenTreeNodeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTreeNodeTests
	{
		[Test]
		public void DefaultImageIndexTest()
		{
			int defaultImageIndex = 2;

			this.treeNode.Collapse();
			this.treeNode.DefaultImageIndex = defaultImageIndex;

			Assert.AreEqual(defaultImageIndex, this.treeNode.ImageIndex);
			Assert.AreEqual(defaultImageIndex, this.treeNode.SelectedImageIndex);
		}

		[Test]
		public void ExpandedImageIndexTest()
		{
			int expandedImageIndex = 3;

			this.treeNode.Expand();
			this.treeNode.ExpandedImageIndex = expandedImageIndex;

			Assert.AreEqual(expandedImageIndex, this.treeNode.ImageIndex);
			Assert.AreEqual(expandedImageIndex, this.treeNode.SelectedImageIndex);
		}

		[Test]
		public void FirstNodeTest()
		{
			NuGenTreeNode firstNode = new NuGenTreeNode();
			NuGenTreeNode secondNode = new NuGenTreeNode();

			this.treeNode.Nodes.AddNode(firstNode);
			this.treeNode.Nodes.AddNode(secondNode);

			Assert.AreEqual(firstNode, this.treeNode.FirstNode);
		}

		[Test]
		[ExpectedException(typeof(InvalidCastException))]
		public void FirstNodeInvalidExceptionTest()
		{
			TreeNode firstNode = new TreeNode();
			NuGenTreeNode secondNode = new NuGenTreeNode();

			(this.treeNode as TreeNode).Nodes.Add(firstNode);
			this.treeNode.Nodes.AddNode(secondNode);

			Assert.IsNotNull(this.treeNode.FirstNode);
		}
	}
}
