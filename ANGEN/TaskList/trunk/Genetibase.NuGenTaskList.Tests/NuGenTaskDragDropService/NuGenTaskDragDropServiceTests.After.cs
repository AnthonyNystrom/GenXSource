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
		public void AfterDoDropTest()
		{
			NuGenTreeNode sourceNode = this._TaskTreeView.Nodes[1];

			List<NuGenTreeNode> selectedNodes = new List<NuGenTreeNode>();
			selectedNodes.Add(sourceNode);

			NuGenTreeNode targetNode = this._TaskTreeView.Nodes[2];

			this._DropService.DoDrop(targetNode, selectedNodes, NuGenDropPosition.After);

			Assert.AreEqual(1, targetNode.Index);
			Assert.AreEqual(3, sourceNode.Index);
		}
	}
}
