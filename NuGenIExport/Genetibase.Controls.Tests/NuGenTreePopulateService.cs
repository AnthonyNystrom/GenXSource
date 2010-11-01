/* -----------------------------------------------
 * NuGenTreePopulateService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;

namespace Genetibase.Controls.Tests
{
	internal static class NuGenTreePopulateService
	{
		public static void PopulateMultilevel(NuGenTreeView treeViewToPopulate, int levelCount, int nodeCount)
		{
			if (treeViewToPopulate == null)
			{
				Assert.Fail("treeViewToPopulate cannot be null.");
			}

			for (int level = 0; level < levelCount; level++)
			{
				NuGenTreeNode treeNode = new NuGenTreeNode();
				treeViewToPopulate.Nodes.AddNode(treeNode);

				for (int node = 0; node < nodeCount; node++)
				{
					treeNode.Nodes.AddNode(new NuGenTreeNode());
				}
			}
		}
	}
}
