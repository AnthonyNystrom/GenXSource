/* -----------------------------------------------
 * NuGenImagePaintParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenImagePaintParams : NuGenPaintParams
	{
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
			set
			{
				_image = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImagePaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenImagePaintParams(Graphics g)
			: base(g)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImagePaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="initializeFrom"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenImagePaintParams(NuGenPaintParams initializeFrom)
			: base(initializeFrom)
		{
		}
	}
}
