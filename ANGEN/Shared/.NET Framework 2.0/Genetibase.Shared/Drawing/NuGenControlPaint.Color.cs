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
		 * GetDesaturatedImageAttributes
		 */

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public static ImageAttributes GetDesaturatedImageAttributes()
		{
			float[][] matrixComponents = new float[5][];
			matrixComponents[0] = new float[] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f };
			matrixComponents[1] = new float[] { 0.2577f, 0.2577f, 0.2577f, 0f, 0f };
			matrixComponents[2] = new float[] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f };
			
			float[] transformComponents = new float[5];
			transformComponents[3] = 1f;
			matrixComponents[3] = transformComponents;
			matrixComponents[4] = new float[] { 0.38f, 0.38f, 0.38f, 0f, 1f };
			
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.ClearColorKey();
			imageAttributes.SetColorMatrix(new ColorMatrix(matrixComponents));

			return imageAttributes;
		}

		/*
		 * GetTransparentImageAttributes
		 */

		/// <summary>
		/// </summary>
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
