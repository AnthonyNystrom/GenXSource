/* -----------------------------------------------
 * NuGenControlPaint.Color.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * ColorFromArgb
		 */

		/// <summary>
		/// Creates a <see cref="T:System.Drawing.Color"/> structure from the specified transparency
		/// level and opaque color.
		/// </summary>
		/// <param name="transparencyLevel">Transparency level.</param>
		/// <param name="color">Opaque color.</param>
		/// <returns>A <see cref="T:System.Drawing.Color"/> structure with alpha channel.</returns>
		public static Color ColorFromArgb(int transparencyLevel, Color color)
		{
			return Color.FromArgb(GetAlphaChannel(transparencyLevel), color);
		}

		/*
		 * GetAlphaChannel
		 */

		/// <summary>
		/// Converts percent representation of transparency level to its real value.
		/// </summary>
		/// <param name="transparencyPercent">The transparency level in percent representation.</param>
		/// <returns>Alpha channel value.</returns>
		public static int GetAlphaChannel(int transparencyPercent)
		{
			return (int)(255f / 100f * (float)(100 - transparencyPercent));
		}

		/*
		 * GetTransparentImageAttributes
		 */

		/// <summary>
		/// Gets <c>System.Drawing.Imaging.ImageAttributes</c> for transparent image drawing.
		/// </summary>
		/// <param name="transparencyPercent">Percent of transparency.</param>
		/// <param name="shouldTile">Indicates if it is necessary to tile an image.</param>
		/// <returns><see cref="T:System.Drawing.Imaging.ImageAttributes"/> for transparent
		/// image drawing.</returns>
		public static ImageAttributes GetTransparentImageAttributes(int transparencyPercent, bool shouldTile)
		{
			ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
																		new float[] { 1, 0, 0, 0, 0 },
																		new float[] { 0, 1, 0, 0, 0 },
																		new float[] { 0, 0, 1, 0, 0 },
																		new float[] { 0, 0, 0, (100f - transparencyPercent) / 100f, 0 },
																		new float[] { 0, 0, 0, 0, 1 }
																	}
				);

			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(colorMatrix);

			if (shouldTile)
			{
				imageAttributes.SetWrapMode(WrapMode.Tile);
			}

			return imageAttributes;
		}
	}
}
