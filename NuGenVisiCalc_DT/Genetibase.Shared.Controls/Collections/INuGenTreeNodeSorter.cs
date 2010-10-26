/* -----------------------------------------------
 * INuGenTreeNodeSorter.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Collections
{
	/// <summary>
	/// Indicates that this class can sort <see cref="TreeNodeCollection"/> using specified
	/// <see cref="T:System.Collections.Generic.IComparer`1"></see>.
	/// </summary>
	public interface INuGenTreeNodeSorter
	{
		/// <summary>
		/// </summary>
		void Sort<T>(TreeNodeCollection collectionToSort, IComparer<T> comparerToUse) where T : TreeNode;
	}
}
