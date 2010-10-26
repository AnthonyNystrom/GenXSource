/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class ModeEventArgs : EventArgs
		{
			private NuGenThumbnailMode _mode;

			public NuGenThumbnailMode Mode
			{
				get
				{
					return _mode;
				}
			}

			public ModeEventArgs(NuGenThumbnailMode modeToActivate)
			{
				_mode = modeToActivate;
			}
		}
	}
}
