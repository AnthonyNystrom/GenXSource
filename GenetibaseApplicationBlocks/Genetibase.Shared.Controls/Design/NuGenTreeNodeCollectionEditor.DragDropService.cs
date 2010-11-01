/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.TreeViewInternals;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenTreeNodeCollectionEditor
	{
		private sealed class DragDropService : NuGenTreeViewDragDropService
		{
			#region Methods.Public.Overridden

			/*
			 * DoDrop
			 */

			/// <summary>
			/// Invokes service specific nodes merge operation.
			/// </summary>
			/// <exception cref="T:System.ArgumentNullException"><paramref name="selectedNodes"/> is <see langword="null"/>.</exception>
			public override void DoDrop(TreeNode targetTreeNode, List<TreeNode> selectedNodes, NuGenDropPosition dropPosition)
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
						this.InsertNodes(targetTreeNode, selectedNodes);
						break;
					}
				}
			}

			#endregion

			#region Methods.Private

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
			private bool CheckParent(TreeNode targetNode, List<TreeNode> selected)
			{
				if (selected == null)
				{
					throw new ArgumentNullException("selected");
				}

				foreach (TreeNode treeNode in selected)
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
			private bool CheckSelectedContainTarget(TreeNode targetNode, List<TreeNode> selected)
			{
				if (selected == null)
				{
					throw new ArgumentNullException("selected");
				}

				foreach (TreeNode treeNode in selected)
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
			private void InsertNodes(TreeNode targetNode, List<TreeNode> selected)
			{
				this.RemoveNodesFromParents(selected);

				foreach (TreeNode treeNode in selected)
				{
					targetNode.Nodes.Add(treeNode);
				}

				targetNode.Expand();
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
			/// <paramref name="selected"/> is <see langword="null"/>
			/// </exception>
			private void MoveNodes(TreeNode targetNode, List<TreeNode> selected, NuGenDropPosition dropPosition)
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

				foreach (TreeNode treeNode in selected)
				{
					if (targetNode.Parent != null)
					{
						targetNode.Parent.Nodes.Insert(targetIndex, treeNode);
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
			private void RemoveNodesFromParents(List<TreeNode> nodesToRemove)
			{
				if (nodesToRemove == null)
				{
					throw new ArgumentNullException("nodesToRemove");
				}

				foreach (TreeNode treeNode in nodesToRemove)
				{
					treeNode.Remove();
				}
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="DragDropService"/> class.
			/// </summary>
			public DragDropService()
				: base()
			{
			}

			#endregion
		}
	}
}
