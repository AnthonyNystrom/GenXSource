/* -----------------------------------------------
 * NuGenImageBoundsParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenImageBoundsParams
	{
		#region Properties.Public

		/*
		 * Bounds
		 */

		private Rectangle _bounds;

		/// <summary>
		/// </summary>
		public Rectangle Bounds
		{
			get
			{
				return _bounds;
			}
		}

		/*
		 * Image
		 */

		private Image _image;

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
		 * ImageAlign
		 */

		private ContentAlignment _imageAlign;

		/// <summary>
		/// </summary>
		public ContentAlignment ImageAlign
		{
			get
			{
				return _imageAlign;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageBoundsParams"/> class.
		/// </summary>
		/// <param name="bounds"></param>
		/// <param name="image">Can be <see langword="null"/>.</param>
		/// <param name="imageAlign"></param>
		public NuGenImageBoundsParams(Rectangle bounds, Image image, ContentAlignment imageAlign)
		{
			_bounds = bounds;
			_image = image;
			_imageAlign = imageAlign;
		}

		#endregion
	}
}
