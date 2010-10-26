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
	partial class NuGenTreeNodeTests
	{
		class TreeNodeCollectionHandler : MockObject
		{
			private NuGenTreeNode _treeNode = null;

			private ExpectationCounter _checkBoxFullCount = new ExpectationCounter("checkBoxFullCount");
			private ExpectationCounter _checkBoxLessCount = new ExpectationCounter("checkBoxLessCount");
			private ExpectationValue _nodesCount = new ExpectationValue("nodesCount");

			public int ExpectedCheckBoxFullCount
			{
				set
				{
					_checkBoxFullCount.Expected = value;
				}
			}

			public int ExpectedCheckBoxLessCount
			{
				set
				{
					_checkBoxLessCount.Expected = value;
				}
			}

			public int ExpectedNodesCount
			{
				set
				{
					_nodesCount.Expected = value;
				}
			}

			public void Run(int nodeCount)
			{
				bool hasCheckBox = false;

				for (int i = 0; i < nodeCount; i++)
				{
					_treeNode.Nodes.Add(new NuGenTreeNode("", hasCheckBox = !hasCheckBox));
				}

				_nodesCount.Actual = _treeNode.Nodes.Count;

				foreach (NuGenTreeNode node in _treeNode.Nodes)
				{
					if (node.HasCheckBox)
					{
						_checkBoxFullCount.Inc();
					}
					else
					{
						_checkBoxLessCount.Inc();
					}
				}
			}

			public TreeNodeCollectionHandler(NuGenTreeNode treeNode)
			{
				if (treeNode == null)
				{
					Assert.Fail("treeNode cannot be null.");
				}

				_treeNode = treeNode;
			}
		}

		[Test]
		public void PopulationTest()
		{
			TreeNodeCollectionHandler nodeCollectionHandler = new TreeNodeCollectionHandler(_treeNode);

			int nodeCount = 10;

			nodeCollectionHandler.ExpectedCheckBoxFullCount = nodeCount / 2;
			nodeCollectionHandler.ExpectedCheckBoxLessCount = nodeCount / 2;
			nodeCollectionHandler.ExpectedNodesCount = nodeCount;

			nodeCollectionHandler.Run(nodeCount);

			nodeCollectionHandler.Verify();
		}
	}
}
