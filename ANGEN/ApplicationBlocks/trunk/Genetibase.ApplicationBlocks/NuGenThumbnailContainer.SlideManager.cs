/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class SlideManager : IDisposable
		{
			public ImageCollection Images
			{
				get
				{
					return _imageTracker.Images;
				}
			}

			public NuGenSlide GetSlideFromImage(Image image)
			{
				return _imageSlideDictionary[image];
			}

			private void _imageTracker_ImageAdded(object sender, AddImageEventArgs e)
			{
				Image image = e.Image;
				NuGenSlide slide = new NuGenSlide();
				slide.Image = image;
				_imageSlideDictionary.Add(image, slide);
				_ctrls.Add(slide);
			}

			private void _imageTracker_ImageInserted(object sender, InsertImageEventArgs e)
			{
				Image image = e.Image;
				NuGenSlide slide = new NuGenSlide();
				slide.Image = image;
				_imageSlideDictionary.Add(image, slide);
				_ctrls.Add(slide);
				_ctrls.SetChildIndex(slide, e.Index);
			}

			private void _imageTracker_ImageRemoved(object sender, RemoveImageEventArgs e)
			{
				if (_imageSlideDictionary.ContainsKey(e.Image))
				{
					NuGenSlide slide = _imageSlideDictionary[e.Image];
					_imageSlideDictionary.Remove(e.Image);
					_ctrls.Remove(slide);
				}
			}

			private Dictionary<Image, NuGenSlide> _imageSlideDictionary;
			private ImageTracker _imageTracker;
			private ControlCollection _ctrls;

			/// <summary>
			/// Initializes a new instance of the <see cref="SlideManager"/> class.
			/// </summary>
			public SlideManager(ControlCollection ctrls, ImageTracker imageTracker)
			{
				Debug.Assert(ctrls != null, "ctrls != null");
				_ctrls = ctrls;

				Debug.Assert(imageTracker != null, "imageTracker != null");
				_imageTracker = imageTracker;
				_imageTracker.ImageAdded += _imageTracker_ImageAdded;
				_imageTracker.ImageInserted += _imageTracker_ImageInserted;
				_imageTracker.ImageRemoved += _imageTracker_ImageRemoved;

				_imageSlideDictionary = new Dictionary<Image, NuGenSlide>();
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
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
