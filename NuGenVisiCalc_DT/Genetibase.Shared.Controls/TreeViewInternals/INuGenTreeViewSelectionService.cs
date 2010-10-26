/* -----------------------------------------------
 * INuGenTreeViewSelectionService.cs
 * Copyright © 2006-2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TreeViewInternals
{
	/// <summary>
	/// Indicates that this class provides functionality for multiple node selection.
	/// </summary>
	public interface INuGenTreeViewSelectionService
	{
		/// <summary>
		/// </summary>
		IList<TreeNode> SelectedNodes
		{
			get;
		}

		/// <summary>
		/// </summary>
		void AddSelectedNode(TreeNode selectedNodeToAdd, Keys pressedKeys, MouseButtons pressedMouseButtons);

		/// <summary>
		/// </summary>
		void RemoveSelectedNode(TreeNode nodeToRemove);

		/// <summary>
		/// </summary>
		void SelectAllNodes(TreeNodeCollection nodesToSelect);
	}
}
