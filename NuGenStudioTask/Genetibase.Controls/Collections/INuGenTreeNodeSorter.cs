/* -----------------------------------------------
 * INuGenTreeNodeSorter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// Indicates that this class can sort <see cref="NuGenTreeNodeCollection"/> using specified
	/// <see cref="T:System.Collections.Generic.IComparer`1"></see>.
	/// </summary>
	public interface INuGenTreeNodeSorter
	{
		/// <summary>
		/// </summary>
		void Sort<T>(NuGenTreeNodeCollection collectionToSort, IComparer<T> comparerToUse) where T : NuGenTreeNode;
	}
}
