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
		internal sealed class InsertImageEventArgs : ImageEventArgs
		{
			private int _index;

			public int Index
			{
				get
				{
					return _index;
				}
			}

			public InsertImageEventArgs(int zeroBasedIndex, Image imageToInsert)
				: base(imageToInsert)
			{
				_index = zeroBasedIndex;
			}
		}
	}
}
