/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		internal sealed class RemoveImageEventArgs : ImageEventArgs
		{
			public RemoveImageEventArgs(Image imageToRemove)
				: base(imageToRemove)
			{
			}
		}
	}
}
