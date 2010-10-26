/* -----------------------------------------------
 * NuGenCollectionEditorTreeView.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCollectionEditorTreeView : NuGenTreeView
	{
		/// <summary>
		/// </summary>
		public TreeNode LastNode
		{
			get
			{
				TreeNode node = this.Nodes[this.Nodes.Count - 1];

				while (node.Nodes.Count > 0)
				{
					node = node.Nodes[node.Nodes.Count - 1];
				}

				return node;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCollectionEditorTreeView"/> class.
		/// </summary>
		public NuGenCollectionEditorTreeView()
		{
			this.HideSelection = false;
		}
	}
}
