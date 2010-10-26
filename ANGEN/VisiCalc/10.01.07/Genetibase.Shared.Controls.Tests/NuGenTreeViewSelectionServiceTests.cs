/* -----------------------------------------------
 * NuGenTreeViewSelectionServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.TreeViewInternals;

using NUnit.Framework;

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenTreeViewSelectionServiceTests
	{
		private NuGenTreeViewSelectionService selectionService = null;
		private NuGenTreeView treeView = null;

		[SetUp]
		public void SetUp()
		{
			this.selectionService = new NuGenTreeViewSelectionService();
			this.treeView = new NuGenTreeView();

			NuGenTreePopulateService.PopulateMultilevel(this.treeView, 10, 10);
		}

		[Test]
		public void AddSelectedNodeRightMouseButtonTest()
		{
			NuGenTreeNode node = (NuGenTreeNode)this.treeView.Nodes[0];
			NuGenTreeNode node2 = (NuGenTreeNode)this.treeView.Nodes[3];
			NuGenTreeNode node3 = (NuGenTreeNode)this.treeView.Nodes[4];

			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Right;

			this.selectionService.AddSelectedNode(node, Keys.None, MouseButtons.Left);
			this.selectionService.AddSelectedNode(node2, pressedKeys, MouseButtons.Left);
			this.selectionService.AddSelectedNode(node2, Keys.None, pressedMouseButtons);

			Assert.AreEqual(4, this.selectionService.SelectedNodes.Count);

			this.selectionService.AddSelectedNode(node3, Keys.None, pressedMouseButtons);

			Assert.AreEqual(1, this.selectionService.SelectedNodes.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddSelectedNodeArgumentNullExceptionTest()
		{
			this.selectionService.AddSelectedNode(null, Keys.None, MouseButtons.None);
		}

		[Test]
		public void SameNodeRemoveTest()
		{
			Keys pressedKeys = Keys.Control;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(0, this.selectionService.SelectedNodes.Count);

			this.selectionService.AddSelectedNode(this.treeView.Nodes[5], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[6], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[7], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(3, this.selectionService.SelectedNodes.Count);

			this.selectionService.AddSelectedNode(this.treeView.Nodes[6], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(2, this.selectionService.SelectedNodes.Count);
		}

		[Test]
		public void SimpleControlSelectionTest()
		{
			Keys pressedKeys = Keys.Control;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			NuGenTreeNode node = (NuGenTreeNode)this.treeView.Nodes[0];
			NuGenTreeNode node2 = (NuGenTreeNode)this.treeView.Nodes[5];

			this.selectionService.AddSelectedNode(node, pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(node2, pressedKeys, pressedMouseButtons);

			Assert.AreEqual(2, this.selectionService.SelectedNodes.Count);
			Assert.AreEqual(node, this.selectionService.SelectedNodes[0]);
			Assert.AreEqual(node2, this.selectionService.SelectedNodes[1]);
		}

		[Test]
		public void MultilevelControlSelectionTest()
		{
			Keys pressedKeys = Keys.Control;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			NuGenTreeNode node = (NuGenTreeNode)this.treeView.Nodes[0].Nodes[2];
			NuGenTreeNode node2 = (NuGenTreeNode)this.treeView.Nodes[1].Nodes[3];
			NuGenTreeNode node3 = (NuGenTreeNode)this.treeView.Nodes[0].Nodes[6];

			this.selectionService.AddSelectedNode(node, pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(node2, pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(node3, pressedKeys, pressedMouseButtons);

			Assert.AreEqual(2, this.selectionService.SelectedNodes.Count);
			Assert.AreEqual(node, this.selectionService.SelectedNodes[0]);
			Assert.AreEqual(node3, this.selectionService.SelectedNodes[1]);
		}

		[Test]
		public void SimpleShiftSelectionTest()
		{
			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			this.selectionService.AddSelectedNode(this.treeView.Nodes[0], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(4, this.selectionService.SelectedNodes.Count);

			for (int i = 0; i < 4; i++)
			{
				Assert.AreEqual(this.treeView.Nodes[i], this.selectionService.SelectedNodes[i]);
			}
		}

		[Test]
		public void SimpleReverseShiftSelectionTest()
		{
			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[0], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(4, this.selectionService.SelectedNodes.Count);

			Assert.AreEqual(this.treeView.Nodes[3], this.selectionService.SelectedNodes[0]);

			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(this.treeView.Nodes[i], this.selectionService.SelectedNodes[i + 1]);
			}
		}

		[Test]
		public void SimpleMergeShiftSelectionTest()
		{
			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[0], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[5], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(3, this.selectionService.SelectedNodes.Count);

			for (int i = 3; i < 6; i++)
			{
				Assert.AreEqual(this.treeView.Nodes[i], this.selectionService.SelectedNodes[i - 3]);
			}
		}

		[Test]
		public void SimpleReverseMergeShiftSelectionTest()
		{
			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			this.selectionService.AddSelectedNode(this.treeView.Nodes[5], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[8], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);

			Assert.AreEqual(3, this.selectionService.SelectedNodes.Count);
		}

		[Test]
		public void MultilevelShiftSelectionTest()
		{
			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			NuGenTreeNode node = (NuGenTreeNode)this.treeView.Nodes[0].Nodes[2];
			NuGenTreeNode node2 = (NuGenTreeNode)this.treeView.Nodes[1].Nodes[3];

			this.selectionService.AddSelectedNode(node, pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(node2, pressedKeys, pressedMouseButtons);

			Assert.AreEqual(1, this.selectionService.SelectedNodes.Count);
			Assert.AreEqual(node, this.selectionService.SelectedNodes[0]);
		}

		[Test]
		public void NoneKeysClearsSelection()
		{
			Keys pressedKeys = Keys.Shift;
			MouseButtons pressedMouseButtons = MouseButtons.Left;

			this.selectionService.AddSelectedNode(this.treeView.Nodes[0], pressedKeys, pressedMouseButtons);
			this.selectionService.AddSelectedNode(this.treeView.Nodes[3], pressedKeys, pressedMouseButtons);

			NuGenTreeNode node = (NuGenTreeNode)this.treeView.Nodes[4];

			this.selectionService.AddSelectedNode(node, Keys.None, pressedMouseButtons);

			Assert.AreEqual(1, this.selectionService.SelectedNodes.Count);
			Assert.AreEqual(node, this.selectionService.SelectedNodes[0]);
		}

		[Test]
		public void SelectAllTest()
		{
			int levelsCount = 10;
			int nodesCount = 10;

			for (int i = 0; i < 2; i++)
			{
				this.selectionService.SelectAllNodes(this.treeView.Nodes);
				Assert.AreEqual(levelsCount * nodesCount + levelsCount, this.selectionService.SelectedNodes.Count);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SelectAllArgumentNullExceptionTest()
		{
			this.selectionService.SelectAllNodes(null);
		}

		[Test]
		public void RemoveSelectedNodeTest()
		{
			NuGenTreeNode node = new NuGenTreeNode();
			this.selectionService.AddSelectedNode(node, Keys.None, MouseButtons.Left);
			this.selectionService.RemoveSelectedNode(node);

			Assert.AreEqual(0, this.selectionService.SelectedNodes.Count);

			this.selectionService.RemoveSelectedNode(null);
		}
	}
}
