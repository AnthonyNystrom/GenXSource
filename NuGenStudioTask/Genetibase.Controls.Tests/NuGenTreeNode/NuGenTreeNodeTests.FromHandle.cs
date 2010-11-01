/* -----------------------------------------------
 * NuGenTreeNodeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTreeNodeTests
	{
		[Test]
		public void FromHandleTest()
		{
			this.treeView.Nodes.AddNode(this.treeNode);
			Assert.AreEqual(this.treeNode, NuGenTreeNode.FromHandle(this.treeView, this.treeNode.Handle));
			Assert.AreNotEqual(new NuGenTreeNode(), NuGenTreeNode.FromHandle(this.treeView, this.treeNode.Handle));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FromHandleArgumentNullExceptionTest()
		{
			NuGenTreeNode.FromHandle(null, IntPtr.Zero);
		}
	}
}
