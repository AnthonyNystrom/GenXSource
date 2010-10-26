/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class VisibilityEventArgs : EventArgs
		{
			private bool _visible;

			public bool Visible
			{
				get
				{
					return _visible;
				}
			}

			public VisibilityEventArgs(bool visible)
			{
				_visible = visible;
			}
		}
	}
}
