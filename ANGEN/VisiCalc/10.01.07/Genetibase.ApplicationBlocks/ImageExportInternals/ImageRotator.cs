/* -----------------------------------------------
 * ImageRotator.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	internal static class ImageRotator
	{
		public static void RotateImage(Image imageToRotate, ImageRotationStyle style)
		{
			if (imageToRotate != null)
			{
				RotateFlipType flipType;

				switch (style)
				{
					case ImageRotationStyle.CCW:
					{
						flipType = RotateFlipType.Rotate90FlipXY;
						break;
					}
					default:
					{
						flipType = RotateFlipType.Rotate270FlipXY;
						break;
					}
				}

				imageToRotate.RotateFlip(flipType);
			}
		}
	}
}
