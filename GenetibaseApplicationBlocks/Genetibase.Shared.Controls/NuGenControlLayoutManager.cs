/* -----------------------------------------------
 * NuGenControlLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a basic layout engine.
	/// </summary>
	public class NuGenControlLayoutManager : INuGenControlLayoutManager
	{
		#region INuGenControlLayoutManager Members

		/// <summary>
		/// </summary>
		/// <param name="imageBoundsParams"></param>
		/// <returns></returns>
		public Rectangle GetImageBounds(NuGenImageBoundsParams imageBoundsParams)
		{
			Image image = imageBoundsParams.Image;

			if (image != null)
			{
				return NuGenControlPaint.ImageBoundsFromContentAlignment(
					imageBoundsParams.Image.Size,
					imageBoundsParams.Bounds,
					imageBoundsParams.ImageAlign
				);
			}

			return Rectangle.Empty;
		}

		/// <summary>
		/// </summary>
		/// <param name="textBoundsParams"></param>
		/// <returns></returns>
		public Rectangle GetTextBounds(NuGenTextBoundsParams textBoundsParams)
		{
			return NuGenControlPaint.TextBoundsFromImageBounds(
				textBoundsParams.Bounds,
				textBoundsParams.ImageBounds,
				textBoundsParams.ImageAlign
			);
		}

		#endregion
	}
}
