/* -----------------------------------------------
 * NuGenTaskDragDropService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Provides methods for <see cref="T:NuGenTaskTreeNodeBase"/> drag-n-drop operations.
	/// </summary>
	public class NuGenTaskDragDropService : NuGenTreeViewDragDropService
	{
		#region Methods.Public.Overriden

		/*
		 * DoDrop
		 */

		/// <summary>
		/// Invokes service specific nodes merge operation.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="selectedNodes"/> is <see langword="null"/>.</exception>
		public override void DoDrop(NuGenTreeNode targetTreeNode, List<NuGenTreeNode> selectedNodes, NuGenDropPosition dropPosition)
		{
			if (selectedNodes == null)
			{
				throw new ArgumentNullException("selectedNodes");
			}

			if (
				targetTreeNode == null
				|| this.CheckSelectedContainTarget(targetTreeNode, selectedNodes)
				|| !this.CheckParent(targetTreeNode, selectedNodes)
				)
			{
				return;
			}

			switch (dropPosition)
			{
				case NuGenDropPosition.After:
				case NuGenDropPosition.Before:
				{
					this.MoveNodes(targetTreeNode, selectedNodes, dropPosition);
					break;
				}
				case NuGenDropPosition.Inside:
				{
					this.InsertNodes(targetTreeNode, this.RetrieveInsertable(selectedNodes));
					break;
				}
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * CheckParent
		 */

		/// <summary>
		/// Returns <see langword="true"/> if the parent of the <paramref name="targetNode"/> is not equal
		/// to any of the selected nodes' parent.
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="selected"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="selected"/> is <see langword="null"/>.
		/// </exception>
		protected virtual bool CheckParent(NuGenTreeNode targetNode, List<NuGenTreeNode> selected)
		{
			if (selected == null)
			{
				throw new ArgumentNullException("selected");
			}

			foreach (NuGenTreeNode treeNode in selected)
			{
				if (targetNode.Parent == treeNode)
				{
					return false;
				}
			}

			return true;
		}

		/*
		 * CheckSelectedContainTarget
		 */
		
		/// <summary>
		/// Returns <see langword="true"/> if one of the selected nodes is equal to the specified <paramref name="targetNode"/>.
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="selected"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="selected"/> is <see langword="null"/>.
		/// </exception>
		protected virtual bool CheckSelectedContainTarget(NuGenTreeNode targetNode, List<NuGenTreeNode> selected)
		{
			if (selected == null)
			{
				throw new ArgumentNullException("selected");
			}

			foreach (NuGenTreeNode treeNode in selected)
			{
				if (treeNode == targetNode)
				{
					return true;
				}
			}

			return false;
		}

		/*
		 * InsertNodes
		 */

		/// <summary>
		/// Inserts selected nodes into the specified <paramref name="targetNode"/>.
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="selected"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="selected"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InsertNodes(NuGenTreeNode targetNode, List<NuGenTreeNode> selected)
		{
			if (
				targetNode is NuGenFolderTreeNode
				&& ((NuGenFolderTreeNode)targetNode).IsRemovable
				)
			{
				this.RemoveNodesFromParents(selected);

				foreach (NuGenTreeNode treeNode in selected)
				{
					targetNode.Nodes.AddNode(treeNode);
				}

				targetNode.Expand();
			}
		}

		/*
		 * MoveNodes
		 */

		/// <summary>
		/// Moves selected nodes to the new location specified by the <paramref name="targetNode"/> index.
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="selected"></param>
		/// <param name="dropPosition"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="targetNode"/> is <see langword="null"/>
		/// -or-
		/// <paramref name="selected"/> is <see langword="null"?/>
		/// </exception>
		protected virtual void MoveNodes(NuGenTreeNode targetNode, List<NuGenTreeNode> selected, NuGenDropPosition dropPosition)
		{
			if (targetNode == null)
			{
				throw new ArgumentNullException("targetNode");
			}

			if (selected == null)
			{
				throw new ArgumentNullException("selected");
			}

			int targetIndex = 0;

			if (dropPosition == NuGenDropPosition.After)
			{
				targetIndex = targetNode.Index + 1;
				this.RemoveNodesFromParents(selected);
			}
			else if (dropPosition == NuGenDropPosition.Before)
			{
				this.RemoveNodesFromParents(selected);
				targetIndex = targetNode.Index;
			}
			else
			{
				return;
			}

			foreach (NuGenTreeNode treeNode in selected)
			{
				if (targetNode.Parent != null)
				{
					targetNode.Parent.Nodes.InsertNode(targetIndex, treeNode);
				}
				else if (targetNode.TreeView != null)
				{
					targetNode.TreeView.Nodes.Insert(targetIndex, treeNode);
				}
				else
				{
					return;
				}
			}
		}

		/*
		 * RemoveNodesFromParents
		 */

		/// <summary>
		/// </summary>
		/// <param name="nodesToRemove"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodesToRemove"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void RemoveNodesFromParents(List<NuGenTreeNode> nodesToRemove)
		{
			if (nodesToRemove == null)
			{
				throw new ArgumentNullException("nodesToRemove");
			}

			foreach (NuGenTreeNode treeNode in nodesToRemove)
			{
				treeNode.Remove();
			}
		}

		/*
		 * RetrieveInsertable
		 */

		/// <summary>
		/// Deletes from the specified list of selected nodes those that cannot be inserted into other nodes
		/// and returns a new filtered collection.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="selected"/> is <see langword="null"/>.</exception>
		protected virtual List<NuGenTreeNode> RetrieveInsertable(List<NuGenTreeNode> selected)
		{
			if (selected == null)
			{
				throw new ArgumentNullException("selected");
			}

			List<NuGenTreeNode> filtered = new List<NuGenTreeNode>();

			foreach (NuGenTreeNode treeNode in selected)
			{
				if (treeNode is NuGenTaskTreeNodeBase)
				{
					if (((NuGenTaskTreeNodeBase)treeNode).IsRemovable)
					{
						filtered.Add(treeNode);
					}
				}
			}

			return filtered;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskDragDropService"/> class.
		/// </summary>
		public NuGenTaskDragDropService()
			: base()
		{
		}

		#endregion
	}
}
