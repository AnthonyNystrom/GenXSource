/* -----------------------------------------------
 * NuGenTreeNodeSorter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// Sorts <see cref="T:NuGenTreeNodeCollection"/> using specified <see cref="T:IComparer`1"/>.
	/// </summary>
	public class NuGenTreeNodeSorter : INuGenTreeNodeSorter
	{
		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="collectionToSort"/> is <see langword="null"/>.
		/// <paramref name="comparerToUse"/> is <see langword="null"/>.
		/// </exception>
		public void Sort<T>(NuGenTreeNodeCollection collectionToSort, IComparer<T> comparerToUse) where T : NuGenTreeNode
		{
			if (collectionToSort == null)
			{
				throw new ArgumentNullException("collectionToSort");
			}

			if (comparerToUse == null)
			{
				throw new ArgumentNullException("comparerToUse");
			}

			int nodesCount = collectionToSort.Count;

			if (nodesCount == 0)
			{
				return;
			}

			T[] nodes = new T[nodesCount];

			for (int i = 0; i < nodesCount; i++)
			{
				nodes[i] = (T)collectionToSort[i];
			}

			Array.Sort<T>(nodes, comparerToUse);
			collectionToSort.Clear();
			collectionToSort.AddNodeRange(nodes);

			foreach (T treeNode in nodes)
			{
				this.Sort(treeNode.Nodes, comparerToUse);
			}
		}
	}
}
