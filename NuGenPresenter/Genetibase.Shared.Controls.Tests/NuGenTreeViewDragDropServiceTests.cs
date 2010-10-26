/* -----------------------------------------------
 * NuGenTreeViewDragDropServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Controls.TreeViewInternals;

using NUnit.Framework;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenTreeViewDragDropServiceTests
	{
		private NuGenTreeViewDragDropService dragService = null;
		private NuGenTreeView treeView = null;
		private NuGenTreeNode treeNode = null;

		[SetUp]
		public void SetUp()
		{
			this.dragService = new NuGenTreeViewDragDropService();
			this.treeView = new NuGenTreeView();
			this.treeNode = new NuGenTreeNode();

			this.treeView.Nodes.Add(this.treeNode);
		}

		[Test]
		public void ConstructorTest()
		{
			Assert.AreEqual(2, dragService.EdgeSensivity);

			this.dragService.EdgeSensivity = 4;
			Assert.AreEqual(4, this.dragService.EdgeSensivity);

			this.dragService = new NuGenTreeViewDragDropService(3);
			Assert.AreEqual(3, dragService.EdgeSensivity);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ConstructorArgumentExceptionTest()
		{
			this.dragService = new NuGenTreeViewDragDropService(-1);
		}

		[Test]
		public void GetDropPositionTest()
		{
			Rectangle nodeBounds = this.treeNode.Bounds;

			Point cursorPosition = new Point(nodeBounds.Left, nodeBounds.Top + nodeBounds.Height / 2);
			NuGenDropPosition dropPosition = this.dragService.GetDropPosition(this.treeNode, cursorPosition);
			Assert.AreEqual(NuGenDropPosition.Inside, dropPosition);

			cursorPosition = new Point(nodeBounds.Left, nodeBounds.Top);
			dropPosition = this.dragService.GetDropPosition(this.treeNode, cursorPosition);
			Assert.AreEqual(NuGenDropPosition.Before, dropPosition);

			cursorPosition = new Point(nodeBounds.Left, nodeBounds.Bottom);
			dropPosition = this.dragService.GetDropPosition(this.treeNode, cursorPosition);
			Assert.AreEqual(NuGenDropPosition.After, dropPosition);

			cursorPosition = new Point(-10, -10);
			dropPosition = this.dragService.GetDropPosition(this.treeNode, cursorPosition);
			Assert.AreEqual(NuGenDropPosition.Nowhere, dropPosition);
			Assert.AreEqual(NuGenDropPosition.Nowhere, this.dragService.GetDropPosition(null, cursorPosition));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EdgeSensivityArgumentExceptionTest()
		{
			this.dragService.EdgeSensivity = -1;
		}
	}
}
