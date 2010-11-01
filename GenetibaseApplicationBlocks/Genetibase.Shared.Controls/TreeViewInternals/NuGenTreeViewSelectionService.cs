/* -----------------------------------------------
 * NuGenTreeViewSelectionService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TreeViewInternals
{
	/// <summary>
	/// Provides functionality for multiple node selection.
	/// </summary>
	internal class NuGenTreeViewSelectionService : INuGenTreeViewSelectionService
	{
		#region Properties.Public

		/*
		 * SelectedNodes
		 */

		private List<TreeNode> selectedNodes = null;

		/// <summary>
		/// </summary>
		public List<TreeNode> SelectedNodes
		{
			get
			{
				if (this.selectedNodes == null)
				{
					this.selectedNodes = new List<TreeNode>();
				}

				return this.selectedNodes;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * AddSelectedNode
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="selectedNodeToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddSelectedNode(TreeNode selectedNodeToAdd, Keys pressedKeys, MouseButtons pressedMouseButtons)
		{
			if (selectedNodeToAdd == null)
			{
				throw new ArgumentNullException("selectedNodeToAdd");
			}

			if ((pressedMouseButtons & MouseButtons.Right) == MouseButtons.Right)
			{
				if (!this.SelectedNodes.Contains(selectedNodeToAdd))
				{
					this.SelectedNodes.Clear();
					this.SelectedNodes.Add(selectedNodeToAdd);
				}

				return;
			}

			if (pressedKeys == Keys.None)
			{
				this.SelectedNodes.Clear();
				this.SelectedNodes.Add(selectedNodeToAdd);
			}
			else if ((pressedKeys & Keys.Control) != 0)
			{
				if (this.SelectedNodes.Count == 0)
				{
					this.SelectedNodes.Add(selectedNodeToAdd);
				}
				else
				{
					if (this.SelectedNodes.Contains(selectedNodeToAdd))
					{
						this.SelectedNodes.Remove(selectedNodeToAdd);
					}
					else
					{
						if (this.SelectedNodes[0].Parent == selectedNodeToAdd.Parent)
						{
							this.SelectedNodes.Add(selectedNodeToAdd);
						}
					}
				}
			}
			else if ((pressedKeys & Keys.Shift) != 0)
			{
				if (this.SelectedNodes.Count == 0)
				{
					this.SelectedNodes.Add(selectedNodeToAdd);
				}
				else
				{
					if (this.SelectedNodes[0].Parent == selectedNodeToAdd.Parent)
					{
						int firstSelectedNodeIndex = this.SelectedNodes[0].Index;
						int lastSelectedNodeIndex = selectedNodeToAdd.Index;

						TreeNode currentNode = this.SelectedNodes[0];
						this.SelectedNodes.Clear();

						if (lastSelectedNodeIndex > firstSelectedNodeIndex)
						{
							lastSelectedNodeIndex++;
						}
						else if (lastSelectedNodeIndex < firstSelectedNodeIndex)
						{
							this.SelectedNodes.Add(currentNode);
							currentNode = selectedNodeToAdd;

							NuGenArgument.Exchange<int>(ref firstSelectedNodeIndex, ref lastSelectedNodeIndex);
						}

						for (
							int nodeIndex = firstSelectedNodeIndex;
							nodeIndex < lastSelectedNodeIndex;
							nodeIndex++
							)
						{
							if (currentNode != null)
							{
								if (!this.SelectedNodes.Contains(currentNode))
								{
									this.SelectedNodes.Add(currentNode);
								}

								currentNode = currentNode.NextVisibleNode;
							}
						}
					}
				}
			}
		}

		/*
		 * RemoveSelectedNode
		 */

		/// <summary>
		/// </summary>
		public void RemoveSelectedNode(TreeNode nodeToRemove)
		{
			if (this.SelectedNodes.Contains(nodeToRemove))
			{
				this.SelectedNodes.Remove(nodeToRemove);
			}
		}

		/*
		 * SelectAllNodes
		 */

		/// <summary>
		/// Selects all the nodes, including sub-nodes, in the specified collection.
		/// </summary>
		/// <param name="nodesToSelect"></param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="nodesToSelect"/> is <see langword="null"/>.</exception>
		public void SelectAllNodes(TreeNodeCollection nodesToSelect)
		{
			if (nodesToSelect == null)
			{
				throw new ArgumentNullException("ndoesToSelect");
			}

			if (nodesToSelect.Count == 0)
			{
				return;
			}

			foreach (TreeNode treeNode in nodesToSelect)
			{
				if (!this.SelectedNodes.Contains(treeNode))
				{
					this.SelectedNodes.Add(treeNode);
				}

				this.SelectAllNodes(treeNode.Nodes);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTreeViewSelectionService"/> class.
		/// </summary>
		public NuGenTreeViewSelectionService()
		{
		}

		#endregion
	}
}
