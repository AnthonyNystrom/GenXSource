/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		internal sealed class ImageTracker
		{
			public event EventHandler<AddImageEventArgs> ImageAdded;
			public event EventHandler<InsertImageEventArgs> ImageInserted;
			public event EventHandler<RemoveImageEventArgs> ImageRemoved;

			private ImageCollection _images;

			public ImageCollection Images
			{
				get
				{
					if (_images == null)
					{
						_images = new ImageCollection(this);
					}

					return _images;
				}
			}

			public void AddImage(Image imageToAdd)
			{
				Debug.Assert(imageToAdd != null, "imageToAdd != null");

				if (this.ImageAdded != null)
				{
					this.ImageAdded(this, new AddImageEventArgs(imageToAdd));
				}
			}

			public void InsertImage(int zeroBasedIndex, Image imageToInsert)
			{
				Debug.Assert(imageToInsert != null, "imageToInsert != null");

				if (this.ImageInserted != null)
				{
					this.ImageInserted(this, new InsertImageEventArgs(zeroBasedIndex, imageToInsert));
				}
			}

			public void RemoveImage(Image imageToRemove)
			{
				Debug.Assert(imageToRemove != null, "imageToRemove != null");

				if (this.ImageRemoved != null)
				{
					this.ImageRemoved(this, new RemoveImageEventArgs(imageToRemove));
				}
			}

			public ImageTracker()
			{
			}
		}
	}
}
