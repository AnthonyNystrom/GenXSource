/* -----------------------------------------------
 * NuGenTextBoundsParams.cs
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
	public class NuGenTextBoundsParams
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
		 * ImageBounds
		 */

		private Rectangle _imageBounds;

		/// <summary>
		/// </summary>
		public Rectangle ImageBounds
		{
			get
			{
				return _imageBounds;
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
		/// Initializes a new instance of the <see cref="NuGenTextBoundsParams"/> class.
		/// </summary>
		public NuGenTextBoundsParams(Rectangle bounds, Rectangle imageBounds, ContentAlignment imageAlign)
		{
			_bounds = bounds;
			_imageBounds = imageBounds;
			_imageAlign = imageAlign;
		}

		#endregion
	}
}
