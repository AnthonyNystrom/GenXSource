/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
