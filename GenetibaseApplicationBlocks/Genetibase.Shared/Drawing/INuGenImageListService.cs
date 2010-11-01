/* -----------------------------------------------
 * INuGenImageListService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Indicates that this class provides service methods to operate <see cref="T:System.Windows.Forms.ImageList"/>.
	/// </summary>
	public interface INuGenImageListService
	{
		/// <summary>
		/// </summary>
		void AddImage(ImageList imageList, Image imageToAdd, string keyToAssociateImageWith);

		/// <summary>
		/// </summary>
		void AddImages(ImageList imageList, NuGenImageDescriptor[] imageDescriptors);

		/// <summary>
		/// </summary>
		Image FindImageAtIndex(ImageList imageList, int index);

		/// <summary>
		/// </summary>
		int GetImageIndex(ImageList imageList, string key);
	}
}
