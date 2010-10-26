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
		internal sealed class AddImageEventArgs : ImageEventArgs
		{
			public AddImageEventArgs(Image imageToAdd)
				: base(imageToAdd)
			{
			}
		}
	}
}
