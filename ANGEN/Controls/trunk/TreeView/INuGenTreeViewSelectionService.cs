/* -----------------------------------------------
 * INuGenTreeViewSelectionService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Genetibase.Controls
{
	/// <summary>
	/// Indicates that this class provides functionality for multiple node selection.
	/// </summary>
	public interface INuGenTreeViewSelectionService
	{
		/// <summary>
		/// </summary>
		List<NuGenTreeNode> SelectedNodes
		{
			get;
		}

		/// <summary>
		/// </summary>
		void AddSelectedNode(NuGenTreeNode selectedNodeToAdd, Keys pressedKeys, MouseButtons pressedMouseButtons);

		/// <summary>
		/// </summary>
		void RemoveSelectedNode(NuGenTreeNode nodeToRemove);

		/// <summary>
		/// </summary>
		void SelectAllNodes(NuGenTreeNodeCollection nodesToSelect);
	}
}
