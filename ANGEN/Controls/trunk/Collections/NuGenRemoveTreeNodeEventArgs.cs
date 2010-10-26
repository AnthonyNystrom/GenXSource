/* -----------------------------------------------
 * NuGenRemoveTreeNodeEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// Contains necessary data for Remove method.
	/// </summary>
	public class NuGenRemoveTreeNodeEventArgs : EventArgs
	{
		#region Properties.Public

		private NuGenTreeNode _treeNodeToRemove = null;

		/// <summary>
		/// </summary>
		public NuGenTreeNode TreeNodeToRemove
		{
			get
			{
				return _treeNodeToRemove;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRemoveTreeNodeEventArgs"/> class.
		/// </summary>
		public NuGenRemoveTreeNodeEventArgs(NuGenTreeNode treeNodeToRemove)
		{
			_treeNodeToRemove = treeNodeToRemove;
		}

		#endregion
	}
}
