/* -----------------------------------------------
 * NuGenControlLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a basic layout engine.
	/// </summary>
	public class NuGenControlLayoutManager : INuGenControlLayoutManager
	{
		/// <summary>
		/// </summary>
		/// <param name="imageBoundsParams"></param>
		/// <returns></returns>
		public Rectangle GetImageBounds(NuGenBoundsParams imageBoundsParams)
		{
			if (imageBoundsParams == null)
			{
				throw new ArgumentNullException("imageBoundsParams");
			}

			imageBoundsParams.ImageAlign = NuGenControlPaint.RTLContentAlignment(
				imageBoundsParams.ImageAlign
				, imageBoundsParams.RightToLeft
			);

			return NuGenControlPaint.ImageBoundsFromContentAlignment(
				imageBoundsParams.ImageBounds.Size
				, imageBoundsParams.Bounds
				, imageBoundsParams.ImageAlign
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="textBoundsParams"></param>
		/// <returns></returns>
		public Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams)
		{
			if (textBoundsParams == null)
			{
				throw new ArgumentNullException("textBoundsParams");
			}

			textBoundsParams.ImageAlign = NuGenControlPaint.RTLContentAlignment(
				textBoundsParams.ImageAlign
				, textBoundsParams.RightToLeft
			);

			return NuGenControlPaint.TextBoundsFromImageBounds(
				textBoundsParams.Bounds,
				textBoundsParams.ImageBounds,
				textBoundsParams.ImageAlign
			);
		}
	}
}
