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
