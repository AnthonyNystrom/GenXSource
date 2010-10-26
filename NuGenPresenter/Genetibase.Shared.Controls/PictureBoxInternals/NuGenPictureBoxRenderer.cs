/* -----------------------------------------------
 * NuGenPictureBoxRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Controls.PictureBoxInternals
{
	/// <summary>
	/// </summary>
	internal sealed class NuGenPictureBoxRenderer : INuGenPictureBoxRenderer
	{
		#region INuGenPictureBoxRenderer Members

		/*
		 * DrawImage
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="imagePaintParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public void DrawImage(NuGenImagePaintParams imagePaintParams)
		{
			if (imagePaintParams == null)
			{
				throw new ArgumentNullException("imagePaintParams");
			}

			imagePaintParams.Graphics.DrawImage(imagePaintParams.Image, imagePaintParams.Bounds);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBoxRenderer"/> class.
		/// </summary>
		public NuGenPictureBoxRenderer()
		{

		}

		#endregion
	}
}
