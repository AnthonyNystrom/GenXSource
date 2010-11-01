/* -----------------------------------------------
 * NuGenAddTreeNodeEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// <para>Add: The handler should set the index the node will be added at.</para>
	/// -or-
	/// <para>Insert: Contains necessary data for Insert method.</para>
	/// </summary>
	public class NuGenAddTreeNodeEventArgs : EventArgs
	{
		#region Properties.Public

		/*
		 * TreeNodeToAdd
		 */

		private NuGenTreeNode _treeNodeToAdd = null;

		/// <summary>
		/// </summary>
		public NuGenTreeNode TreeNodeToAdd
		{
			get
			{
				return _treeNodeToAdd;
			}
		}

		/*
		 * TreeNodeIndex
		 */

		private int _treeNodeIndex = 0;

		/// <summary>
		/// </summary>
		public int TreeNodeIndex
		{
			get
			{
				return _treeNodeIndex;
			}
			set
			{
				_treeNodeIndex = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAddTreeNodeEventArgs"/> class.<para/>
		/// TreeNodeIndex = 0.
		/// </summary>
		public NuGenAddTreeNodeEventArgs(NuGenTreeNode treeNodeToAdd)
			: this(treeNodeToAdd, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAddTreeNodeEventArgs"/> class.
		/// </summary>
		public NuGenAddTreeNodeEventArgs(NuGenTreeNode treeNodeToAdd, int treeNodeIndex)
		{
			_treeNodeToAdd = treeNodeToAdd;
			_treeNodeIndex = treeNodeIndex;
		}

		#endregion
	}
}
