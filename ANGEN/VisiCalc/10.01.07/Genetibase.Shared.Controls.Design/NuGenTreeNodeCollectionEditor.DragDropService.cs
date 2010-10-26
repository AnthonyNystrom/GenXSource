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
			public override void DoDrop(TreeNode targetTreeNode, IList<TreeNode> selectedNodes, NuGenDropPosition dropPosition)
			{
				if (selectedNodes == null)
				{
					throw new ArgumentNullException("selectedNodes");
				}

				if (
					targetTreeNode == null
					|| DragDropService.CheckSelectedContainTarget(targetTreeNode, selectedNodes)
					|| !DragDropService.CheckParent(targetTreeNode, selectedNodes)
					)
				{
					return;
				}

				switch (dropPosition)
				{
					case NuGenDropPosition.After:
					case NuGenDropPosition.Before:
					{
						DragDropService.MoveNodes(targetTreeNode, selectedNodes, dropPosition);
						break;
					}
					case NuGenDropPosition.Inside:
					{
						DragDropService.InsertNodes(targetTreeNode, selectedNodes);
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
			private static bool CheckParent(TreeNode targetNode, IList<TreeNode> selected)
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
			private static bool CheckSelectedContainTarget(TreeNode targetNode, IList<TreeNode> selected)
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
			private static void InsertNodes(TreeNode targetNode, IList<TreeNode> selected)
			{
				DragDropService.RemoveNodesFromParents(selected);

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
			private static void MoveNodes(TreeNode targetNode, IList<TreeNode> selected, NuGenDropPosition dropPosition)
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
					DragDropService.RemoveNodesFromParents(selected);
				}
				else if (dropPosition == NuGenDropPosition.Before)
				{
					DragDropService.RemoveNodesFromParents(selected);
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
			private static void RemoveNodesFromParents(IList<TreeNode> nodesToRemove)
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
