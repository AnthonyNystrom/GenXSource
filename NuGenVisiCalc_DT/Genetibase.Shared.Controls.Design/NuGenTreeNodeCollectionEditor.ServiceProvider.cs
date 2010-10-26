/* -----------------------------------------------
 * NuGenTreeNodeCollectionEditor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.TreeViewInternals;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.Design
{
	partial class NuGenTreeNodeCollectionEditor
	{
		private sealed class ServiceProvider : NuGenTreeViewServiceProvider
		{
			private INuGenTreeViewDragDropService _dragService;

			/// <summary>
			/// </summary>
			/// <value></value>
			protected override INuGenTreeViewDragDropService DragService
			{
				get
				{
					if (_dragService == null)
					{
						_dragService = new DragDropService();
					}

					return _dragService;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ServiceProvider"/> class.
			/// </summary>
			public ServiceProvider()
				: base()
			{
			}
		}
	}
}
