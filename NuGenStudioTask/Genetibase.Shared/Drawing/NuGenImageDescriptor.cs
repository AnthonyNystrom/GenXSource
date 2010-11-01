/* -----------------------------------------------
 * NuGenImageDescriptor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Contains data associated with the specified <see cref="T:Image"/>.
	/// </summary>
	public class NuGenImageDescriptor
	{
		#region Properties.Public

		/*
		 * Image
		 */

		private Image _image = null;

		/// <summary>
		/// </summary>
		public Image Image
		{
			get
			{
				return _image;
			}
		}

		/*
		 * Key
		 */

		private string _key = "";

		/// <summary>
		/// </summary>
		public string Key
		{
			get
			{
				return _key;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageDescriptor"/> class.
		/// </summary>
		/// 
		/// <param name="image"></param>
		/// 
		/// <param name="key">
		/// Required by <see cref="M:System.Windows.Forms.ImageList.Images.Add"/> method.
		/// Key is not case-sensitive.
		/// </param>
		/// 
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="image"/> is <see langword="null"/>.
		/// </exception>
		public NuGenImageDescriptor(Image image, string key)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			_image = image;
			_key = key;
		}

		#endregion
	}
}
