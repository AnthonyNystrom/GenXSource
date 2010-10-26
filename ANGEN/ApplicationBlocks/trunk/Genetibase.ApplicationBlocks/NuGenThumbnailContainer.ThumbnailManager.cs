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
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class ThumbnailManager : IDisposable
		{
			public ImageCollection Images
			{
				get
				{
					return _imageTracker.Images;
				}
			}

			public NuGenThumbnail GetThumbnailFromImage(Image image)
			{
				return _imageThumbDictionary[image];
			}

			private void _imageTracker_ImageAdded(object sender, AddImageEventArgs e)
			{
				Image image = e.Image;
				NuGenThumbnail thumbnail = new NuGenThumbnail(_serviceProvider);
				thumbnail.Image = image;
				_imageThumbDictionary.Add(image, thumbnail);
				_ctrls.Add(thumbnail);
			}

			private void _imageTracker_ImageInserted(object sender, InsertImageEventArgs e)
			{
				Image image = e.Image;
				NuGenThumbnail thumbnail = new NuGenThumbnail(_serviceProvider);
				thumbnail.Image = image;
				_imageThumbDictionary.Add(image, thumbnail);
				_ctrls.Add(thumbnail);
				_ctrls.SetChildIndex(thumbnail, e.Index);
			}

			private void _imageTracker_ImageRemoved(object sender, RemoveImageEventArgs e)
			{
				if (_imageThumbDictionary.ContainsKey(e.Image))
				{
					NuGenThumbnail thumbnail = _imageThumbDictionary[e.Image];
					_imageThumbDictionary.Remove(e.Image);
					_ctrls.Remove(thumbnail);
				}
			}

			private INuGenServiceProvider _serviceProvider;
			private Dictionary<Image, NuGenThumbnail> _imageThumbDictionary;
			private ImageTracker _imageTracker;
			private ControlCollection _ctrls;

			public ThumbnailManager(
				INuGenServiceProvider serviceProvider
				, ControlCollection ctrls
				, ImageTracker imageTracker
				)
			{
				Debug.Assert(serviceProvider != null, "serviceProvider != null");
				_serviceProvider = serviceProvider;

				Debug.Assert(ctrls != null, "ctrls != null");
				_ctrls = ctrls;

				Debug.Assert(imageTracker != null, "imageTracker != null");
				_imageTracker = imageTracker;
				_imageTracker.ImageAdded += _imageTracker_ImageAdded;
				_imageTracker.ImageInserted += _imageTracker_ImageInserted;
				_imageTracker.ImageRemoved += _imageTracker_ImageRemoved;

				_imageThumbDictionary = new Dictionary<Image, NuGenThumbnail>();
			}

			public void Dispose()
			{
				if (_imageTracker != null)
				{
					_imageTracker.ImageAdded -= _imageTracker_ImageAdded;
					_imageTracker.ImageInserted -= _imageTracker_ImageInserted;
					_imageTracker.ImageRemoved -= _imageTracker_ImageRemoved;
				}
			}
		}
	}
}
