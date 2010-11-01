/* -----------------------------------------------
 * NuGenImagePaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
		#region Properties.Public

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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImagePaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="sender"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="image"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenImagePaintParams(
			object sender,
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			Image image
			)
			: base(sender, g, bounds, state)
		{
			_image = image;
		}

		#endregion
	}
}
