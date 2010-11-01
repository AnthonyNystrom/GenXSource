/* -----------------------------------------------
 * INuGenTreeViewDragDropService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Controls
{
	/// <summary>
	/// Indicates that this class provides service methods for drag-n-drop operations.
	/// </summary>
	public interface INuGenTreeViewDragDropService
	{
		/// <summary>
		/// Gets or sets the offset that is used to determine the drop position.
		/// </summary>
		int EdgeSensivity
		{
			get;
			set;
		}

		/// <summary>
		/// Determines the drop position.
		/// </summary>
		/// <param name="targetTreeNode"></param>
		/// <param name="cursorPosition"></param>
		/// <returns></returns>
		NuGenDropPosition GetDropPosition(NuGenTreeNode targetTreeNode, Point cursorPosition);

		/// <summary>
		/// Invokes service specific nodes merge operation.
		/// </summary>
		void DoDrop(NuGenTreeNode targetTreeNode, List<NuGenTreeNode> selectedNodes, NuGenDropPosition dropPosition);
	}
}
