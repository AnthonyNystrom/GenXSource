/* -----------------------------------------------
 * NuGenBoundsParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;	

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenBoundsParams
	{
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
			set
			{
				_bounds = value;
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
			set
			{
				_imageAlign = value;
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
			set
			{
				_imageBounds = value;
			}
		}

		/*
		 * RightToLeft
		 */

		private RightToLeft _rightToLeft;

		/// <summary>
		/// </summary>
		public RightToLeft RightToLeft
		{
			get
			{
				return _rightToLeft;
			}
			set
			{
				_rightToLeft = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBoundsParams"/> class.
		/// </summary>
		public NuGenBoundsParams(
			Rectangle bounds
			, ContentAlignment imageAlign
			, Rectangle imageBounds
			, RightToLeft rightToLeft
			)
		{
			_bounds = bounds;
			_imageAlign = imageAlign;
			_imageBounds = imageBounds;
			_rightToLeft = rightToLeft;
		}
	}
}
