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
		internal abstract class ImageEventArgs : EventArgs
		{
			private Image _image;

			public Image Image
			{
				get
				{
					return _image;
				}
			}

			protected ImageEventArgs(Image image)
			{
				if (image == null)
				{
					throw new ArgumentNullException("image");
				}

				_image = image;
			}
		}
	}
}
