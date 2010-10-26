/* -----------------------------------------------
 * ThumbnailSelectionService.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	internal sealed class ThumbnailSelectionService
	{
		private IList<Image> _selectedImages;

		public IList<Image> SelectedImages
		{
			get
			{
				if (_selectedImages == null)
				{
					_selectedImages = new List<Image>();
				}

				return _selectedImages;
			}
		}

		private Image _startImage;

		public void AddImage(Image imageToAdd, Keys pressedKeys, MouseButtons pressedMouseButtons)
		{
			if (imageToAdd == null)
			{
				throw new ArgumentNullException("imageToAdd");
			}

			if ((pressedMouseButtons & MouseButtons.Left) != MouseButtons.None)
			{
				if ((pressedKeys & Keys.Control) != Keys.None)
				{
					int selectedImageIndex;

					if ((selectedImageIndex = this.SelectedImages.IndexOf(imageToAdd)) > -1)
					{
						this.SelectedImages.Remove(imageToAdd);

						if (this.SelectedImages.Count > 0)
						{
							selectedImageIndex = Math.Max(0, Math.Min(selectedImageIndex, this.SelectedImages.Count - 1));
							_startImage = this.SelectedImages[selectedImageIndex];
						}
						else
						{
							_startImage = null;
						}
					}
					else
					{
						this.SelectedImages.Add(imageToAdd);
						_startImage = imageToAdd;
					}
				}
				else if ((pressedKeys & Keys.Shift) != Keys.None)
				{
					if (_startImage != null)
					{
						int startIndex = _imageCollection.IndexOf(_startImage);
						int endIndex = _imageCollection.IndexOf(imageToAdd);

						if (startIndex != endIndex)
						{
							this.SelectedImages.Clear();
						}

						if (startIndex < endIndex)
						{
							for (int i = startIndex; i <= endIndex; i++)
							{
								this.SelectedImages.Add(_imageCollection[i]);
							}
						}
						else if (startIndex > endIndex)
						{
							for (int i = endIndex; i <= startIndex; i++)
							{
								this.SelectedImages.Add(_imageCollection[i]);
							}
						}
					}
					else
					{
						this.SelectedImages.Add(imageToAdd);
						_startImage = imageToAdd;
					}
				}
				else
				{
					this.SelectedImages.Clear();
					this.SelectedImages.Add(imageToAdd);
					_startImage = imageToAdd;
				}
			}
		}

		public void ClearSelection()
		{
			_startImage = null;
			this.SelectedImages.Clear();
		}

		private NuGenThumbnailContainer.ImageCollection _imageCollection;

		public ThumbnailSelectionService(NuGenThumbnailContainer.ImageCollection imageCollection)
		{
			if (imageCollection == null)
			{
				throw new ArgumentNullException("imageCollection");
			}

			_imageCollection = imageCollection;
		}
	}
}
