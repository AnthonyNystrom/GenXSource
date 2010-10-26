/* -----------------------------------------------
 * NuGenAddTreeNodeRangeEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// The handler should add the specified <see cref="NuGenTreeNode"/> range to the collection.
	/// </summary>
	public class NuGenAddTreeNodeRangeEventArgs : EventArgs
	{
		#region Properties.Public

		private NuGenTreeNode[] _TreeNodeRangeToAdd = null;

		/// <summary>
		/// </summary>
		public NuGenTreeNode[] TreeNodeRangeToAdd
		{
			get
			{
				return _TreeNodeRangeToAdd;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAddTreeNodeRangeEventArgs"/> class.
		/// </summary>
		public NuGenAddTreeNodeRangeEventArgs(NuGenTreeNode[] treeNodeRangeToAdd)
		{
			_TreeNodeRangeToAdd = treeNodeRangeToAdd;
		}

		#endregion
	}
}
