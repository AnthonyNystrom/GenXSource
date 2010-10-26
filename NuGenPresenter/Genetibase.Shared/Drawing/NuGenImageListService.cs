/* -----------------------------------------------
 * NuGenImageListService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Provides service methods to operate <see cref="T:System.Windows.Forms.ImageList"/>.
	/// </summary>
	public class NuGenImageListService : INuGenImageListService
	{
		#region INuGenImageListService.AddImage

		/// <summary>
		/// </summary>
		/// <param name="imageList"></param>
		/// <param name="imageToAdd"></param>
		/// <param name="keyToAssociateImageWith">May be <see langword="null"/> or an empty string.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="imageList"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="imageToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddImage(ImageList imageList, Image imageToAdd, string keyToAssociateImageWith)
		{
			if (imageList == null)
			{
				throw new ArgumentNullException("imageList");
			}

			if (imageToAdd == null)
			{
				throw new ArgumentNullException("imageToAdd");
			}

			imageList.Images.Add(keyToAssociateImageWith, imageToAdd);
		}

		/// <summary>
		/// Key = "".
		/// </summary>
		/// <param name="imageList"></param>
		/// <param name="imageToAdd"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="imageList"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="imageToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddImage(ImageList imageList, Image imageToAdd)
		{
			this.AddImage(imageList, imageToAdd, "");
		}

		#endregion

		#region INuGenImageListService.AddImages

		/// <summary>
		/// </summary>
		/// <param name="imageList"></param>
		/// <param name="imageDescriptors"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="imageList"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="imageDescriptors"/> is <see langword="null"/>.
		/// </exception>
		public void AddImages(ImageList imageList, NuGenImageDescriptor[] imageDescriptors)
		{
			if (imageList == null)
			{
				throw new ArgumentNullException("imageList");
			}

			if (imageDescriptors == null)
			{
				throw new ArgumentNullException("imageDescriptors");
			}

			foreach (NuGenImageDescriptor imageDesc in imageDescriptors)
			{
				imageList.Images.Add(imageDesc.Key, imageDesc.Image);
			}
		}


		#endregion

		#region INuGenImageListService.FindImageAtIndex

		/// <summary>
		/// Returns <see langword="null"/> if the image at the specified index was not found.
		/// </summary>
		/// <param name="imageList">Can be <see langword="null"/>.</param>
		/// <param name="index"></param>
		/// <returns></returns>
		public Image FindImageAtIndex(ImageList imageList, int index)
		{
			if (imageList != null && index > -1 && index < imageList.Images.Count)
			{
				return imageList.Images[index];
			}

			return null;
		}

		#endregion

		#region INuGenImageListService.GetImageIndex

		/// <summary>
		/// </summary>
		/// <param name="imageList"></param>
		/// <param name="key"></param>
		/// <returns>-1 if the specified <paramref name="key"/> does not exist.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="imageList"/> is <see langword="null"/>.
		/// </exception>
		public int GetImageIndex(ImageList imageList, string key)
		{
			if (imageList == null)
			{
				throw new ArgumentNullException("imageList");
			}
			
			return imageList.Images.IndexOfKey(key);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageListService"/> class.
		/// </summary>
		public NuGenImageListService()
		{
		}

		#endregion
	}
}
