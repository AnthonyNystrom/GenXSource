/* -----------------------------------------------
 * NuGenAZTreeNodeComparerTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Collections;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenAZTreeNodeComparerTests
	{
		private NuGenAZTreeNodeComparer comparer = null;

		[SetUp]
		public void SetUp()
		{
			this.comparer = new NuGenAZTreeNodeComparer();
		}

		[Test]
		public void CompareTest()
		{
			NuGenTreeNode mTreeNode = new NuGenTreeNode("m");
			NuGenTreeNode dTreeNode = new NuGenTreeNode("d");
			NuGenTreeNode cTreeNode = new NuGenTreeNode("c");
			NuGenTreeNode kTreeNode = new NuGenTreeNode("k");

			int mTodCompareResult = mTreeNode.Text.CompareTo(dTreeNode.Text);
			int kTomCompareResult = kTreeNode.Text.CompareTo(mTreeNode.Text);
			int dTocCompareResult = dTreeNode.Text.CompareTo(cTreeNode.Text);

			Assert.Greater(mTodCompareResult, 0);
			Assert.Less(kTomCompareResult, 0);
			Assert.Greater(dTocCompareResult, 0);

			Assert.AreEqual(mTodCompareResult, this.comparer.Compare(mTreeNode, dTreeNode));
			Assert.AreEqual(kTomCompareResult, this.comparer.Compare(kTreeNode, mTreeNode));
			Assert.AreEqual(dTocCompareResult, this.comparer.Compare(dTreeNode, cTreeNode));
		}
	}
}
