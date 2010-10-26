/* -----------------------------------------------
 * NuGenTreeNodeTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenTreeNodeTests
	{
		private NuGenTreeView _treeView = null;
		private NuGenTreeNode _treeNode = null;
		private string _nodeText = "Node Text";
		private NuGenTreeNode[] _nodes = null;
		private int _imageIndex = 1;
		private int _expandedImageIndex = 2;

		[SetUp]
		public void SetUp()
		{
			_treeView = new NuGenTreeView();
			_treeNode = new NuGenTreeNode("Root", false);

			_nodes = new NuGenTreeNode[] {
				new NuGenTreeNode(),
				new NuGenTreeNode(),
				new NuGenTreeNode()
			};
		}

		[Test]
		public void ConstructorTest()
		{
			_treeNode = new NuGenTreeNode();

			Assert.AreEqual(-1, _treeNode.ImageIndex);
			Assert.AreEqual(-1, _treeNode.SelectedImageIndex);
			Assert.AreEqual(-1, _treeNode.ExpandedImageIndex);
			Assert.AreEqual("", _treeNode.Text);
			Assert.IsTrue(_treeNode.HasCheckBox);
			Assert.IsFalse(_treeNode.Checked);

			bool hasCheckBox = false;
			bool isChecked = true;

			_treeNode = new NuGenTreeNode(
				_nodeText,
				hasCheckBox,
				isChecked,
				_imageIndex,
				_expandedImageIndex
			);

			Assert.AreEqual(_nodeText, _treeNode.Text);
			Assert.AreEqual(_imageIndex, _treeNode.ImageIndex);
			Assert.AreEqual(_imageIndex, _treeNode.SelectedImageIndex);
			Assert.AreEqual(_expandedImageIndex, _treeNode.ExpandedImageIndex);
			Assert.AreEqual(_imageIndex, _treeNode.DefaultImageIndex);
			Assert.IsFalse(_treeNode.HasCheckBox);
			Assert.IsTrue(_treeNode.Checked);
		}

		[Test]
		public void Constructor2Test()
		{
			NuGenTreeNode treeNode = new NuGenTreeNode(_nodeText, _nodes);
			Assert.AreEqual(3, treeNode.Nodes.Count);

			try
			{
				treeNode = new NuGenTreeNode(_nodeText, null);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}

		[Test]
		public void Constructor3Test()
		{
			NuGenTreeNode treeNode = new NuGenTreeNode(_nodeText, _imageIndex, _expandedImageIndex, _nodes);
			Assert.AreEqual(3, treeNode.Nodes.Count);

			try
			{
				treeNode = new NuGenTreeNode(_nodeText, _imageIndex, _expandedImageIndex, null);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}
	}
}
