/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenTreeNodeCollectionEditor
	{
		private sealed class NodesEditorTreeView : NuGenTreeView
		{
			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="NodesEditorTreeView"/> class.
			/// </summary>
			public NodesEditorTreeView()
				: base(new ServiceProvider())
			{
				this.AllowDrop = true;
			}

			#endregion
		}
	}
}
