/* -----------------------------------------------
 * NuGenTreePopulateService.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Controls.Tests
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
				treeViewToPopulate.Nodes.Add(treeNode);

				for (int node = 0; node < nodeCount; node++)
				{
					treeNode.Nodes.Add(new NuGenTreeNode());
				}
			}
		}
	}
}
