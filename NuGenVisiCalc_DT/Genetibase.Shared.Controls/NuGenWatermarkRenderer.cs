/* -----------------------------------------------
 * NuGenWatermarkRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Genetibase.Shared.Controls
{
	internal static class NuGenWatermarkRenderer
	{
		public static void DrawDesaturatedWatermark(Graphics g, Image watermark, Rectangle bounds, int transparency)
		{
			int width = watermark.Width;
			int height = watermark.Height;

			Image desaturatedImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			using (Graphics desaturatedGrfx = Graphics.FromImage(desaturatedImage))
			{
				desaturatedGrfx.DrawImage(
					watermark,
					new Rectangle(0, 0, width, height),
					0, 0, width, height,
					GraphicsUnit.Pixel,
					NuGenControlPaint.GetDesaturatedImageAttributes()
				);
			}

			NuGenWatermarkRenderer.DrawWatermark(g, desaturatedImage, bounds, transparency);
		}

		public static void DrawWatermark(Graphics g, Image watermark, Rectangle bounds, int transparency)
		{
			g.DrawImage(
				watermark
				, bounds
				, 0, 0, watermark.Width, watermark.Height
				, GraphicsUnit.Pixel
				, NuGenControlPaint.GetTransparentImageAttributes(transparency, false)
			);
 		}
	}
}
