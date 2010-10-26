/* -----------------------------------------------
 * NuGenTreeNodeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenTreeNodeTests
	{
		[Test]
		public void DefaultImageIndexTest()
		{
			int defaultImageIndex = 2;

			_treeNode.Collapse();
			_treeNode.DefaultImageIndex = defaultImageIndex;

			Assert.AreEqual(defaultImageIndex, _treeNode.ImageIndex);
			Assert.AreEqual(defaultImageIndex, _treeNode.SelectedImageIndex);
		}

		[Test]
		public void ExpandedImageIndexTest()
		{
			int expandedImageIndex = 3;

			_treeNode.Expand();
			_treeNode.ExpandedImageIndex = expandedImageIndex;

			Assert.AreEqual(expandedImageIndex, _treeNode.ImageIndex);
			Assert.AreEqual(expandedImageIndex, _treeNode.SelectedImageIndex);
		}
	}
}
