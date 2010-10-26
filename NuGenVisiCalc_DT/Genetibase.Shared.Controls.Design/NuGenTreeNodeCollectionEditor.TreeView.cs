/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenTreeNodeCollectionEditor
	{
		private sealed class NodesEditorTreeView : NuGenCollectionEditorTreeView
		{
			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="NodesEditorTreeView"/> class.
			/// </summary>
			public NodesEditorTreeView()
				: base()
			{
				this.AllowDrop = true;
			}

			#endregion
		}
	}
}
