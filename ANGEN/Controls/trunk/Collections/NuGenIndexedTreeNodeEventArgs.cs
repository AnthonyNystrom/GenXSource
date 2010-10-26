/* -----------------------------------------------
 * NuGenIndexedTreeNodeEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// Get by index: The handler should set the value of the <see cref="P:TreeNode"/> property according
	/// to the specified <see cref="P:TreeNodeIndex"/>.
	/// -or-
	/// Set by index: <see cref="P:TreeNode"/> specified the node to be set at the specified index at the 
	/// collection.
	/// </summary>
	public class NuGenIndexedTreeNodeEventArgs : EventArgs
	{
		#region Properties.Public

		/*
		 * TreeNode
		 */

		private NuGenTreeNode treeNode = null;

		/// <summary>
		/// </summary>
		public NuGenTreeNode TreeNode
		{
			get
			{
				return this.treeNode;
			}
			set
			{
				this.treeNode = value;
			}
		}

		/*
		 * TreeNodeIndex
		 */

		private int treeNodeIndex = 0;

		/// <summary>
		/// </summary>
		public int TreeNodeIndex
		{
			get
			{
				return this.treeNodeIndex;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenIndexedTreeNodeEventArgs"/> class.
		/// TreeNode = null.
		/// </summary>
		public NuGenIndexedTreeNodeEventArgs(int treeNodeIndex)
			: this(treeNodeIndex, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenIndexedTreeNodeEventArgs"/> class.
		/// </summary>
		public NuGenIndexedTreeNodeEventArgs(int treeNodeIndex, NuGenTreeNode treeNode)
		{
			this.treeNodeIndex = treeNodeIndex;
			this.treeNode = treeNode;
		}

		#endregion
	}
}
