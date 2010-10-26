/* -----------------------------------------------
 * NuGenTaskDragDropServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.NuGenTaskList.Tests
{
	partial class NuGenTaskDragDropServiceTests
	{
		[Test]
		public void InsideTaskDoDropTest()
		{
			NuGenTreeNode sourceNode = this._TaskTreeView.Nodes[1];

			List<NuGenTreeNode> selectedNodes = new List<NuGenTreeNode>();
			selectedNodes.Add(sourceNode);

			this._DropService.DoDrop(this._TaskTreeView.Nodes[13], selectedNodes, NuGenDropPosition.Inside);

			Assert.AreEqual(this._InitialCount + 20, this._TaskTreeView.Nodes.Count);
			Assert.AreEqual(10, sourceNode.Nodes.Count);
		}

		[Test]
		public void InsideFolderDoDropTest()
		{
			List<NuGenTreeNode> selectedNodes = new List<NuGenTreeNode>();

			NuGenTreeNode sourceParentNode = this._TaskTreeView.Nodes[this._InitialCount];

			for (int i = 0; i < 5; i++)
			{
				selectedNodes.Add(sourceParentNode.Nodes[i]);
			}

			NuGenTreeNode targetNode = this._TaskTreeView.Nodes[this._InitialCount + 1];

			this._DropService.DoDrop(targetNode, selectedNodes, NuGenDropPosition.Inside);

			Assert.AreEqual(5, sourceParentNode.Nodes.Count);
			Assert.AreEqual(15, targetNode.Nodes.Count);
		}

		[Test]
		public void ParentNullInsideFolderDoDropTest()
		{
			List<NuGenTreeNode> selectedNodes = new List<NuGenTreeNode>();

			for (int i = 10; i < 13; i++)
			{
				selectedNodes.Add(this._TaskTreeView.Nodes[i]);
			}

			NuGenTreeNode targetNode = this._TaskTreeView.Nodes[1];

			this._DropService.DoDrop(targetNode, selectedNodes, NuGenDropPosition.Inside);

			Assert.AreEqual(this._InitialCount + 17, this._TaskTreeView.Nodes.Count);
			Assert.AreEqual(13, targetNode.Nodes.Count);
		}
	}
}
