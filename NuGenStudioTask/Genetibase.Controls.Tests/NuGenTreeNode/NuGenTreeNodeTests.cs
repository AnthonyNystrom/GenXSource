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
	[TestFixture]
	public partial class NuGenTreeNodeTests
	{
		private NuGenTreeView treeView = null;
		private NuGenTreeNode treeNode = null;

		[SetUp]
		public void SetUp()
		{
			this.treeView = new NuGenTreeView();
			this.treeNode = new NuGenTreeNode("Root", false);
		}

		[Test]
		public void ConstructorTest()
		{
			this.treeNode = new NuGenTreeNode();

			Assert.AreEqual(-1, this.treeNode.ImageIndex);
			Assert.AreEqual(-1, this.treeNode.SelectedImageIndex);
			Assert.AreEqual(-1, this.treeNode.ExpandedImageIndex);
			Assert.AreEqual("", this.treeNode.Text);
			Assert.IsTrue(this.treeNode.HasCheckBox);
			Assert.IsFalse(this.treeNode.Checked);

			string nodeText = "Node Text";
			
			int imageIndex = 1;
			int expandedImageIndex = 2;

			bool hasCheckBox = false;
			bool isChecked = true;

			this.treeNode = new NuGenTreeNode(
				nodeText,
				hasCheckBox,
				isChecked,
				imageIndex,
				expandedImageIndex
			);

			Assert.AreEqual(nodeText, this.treeNode.Text);
			Assert.AreEqual(imageIndex, this.treeNode.ImageIndex);
			Assert.AreEqual(imageIndex, this.treeNode.SelectedImageIndex);
			Assert.AreEqual(expandedImageIndex, this.treeNode.ExpandedImageIndex);
			Assert.AreEqual(imageIndex, this.treeNode.DefaultImageIndex);
			Assert.IsFalse(this.treeNode.HasCheckBox);
			Assert.IsTrue(this.treeNode.Checked);
		}
	}
}
