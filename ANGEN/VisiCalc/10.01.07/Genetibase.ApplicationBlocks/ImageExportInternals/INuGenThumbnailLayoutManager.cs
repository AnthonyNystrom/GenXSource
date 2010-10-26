/* -----------------------------------------------
 * INuGenThumbnailLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenThumbnailLayoutManager
	{
		/// <summary>
		/// </summary>
		Point GetCWRotateButtonLocation(Rectangle clientRectangle, Size cwRotateButtonSize);

		/// <summary>
		/// </summary>
		Size GetCWRotateButtonSize();

		/// <summary>
		/// </summary>
		Point GetCCWRotateButtonLocation(Rectangle clientRectangle, Size ccwRotateButtonSize);

		/// <summary>
		/// </summary>
		Size GetCCWRotateButtonSize();

		/// <summary>
		/// </summary>
		Rectangle GetImageBounds(Rectangle clientRectangle, Size imageSize);

		/// <summary>
		/// </summary>
		Rectangle GetTextBounds(Rectangle clientRectangle, float fontSize);

		/// <summary>
		/// </summary>
		Rectangle GetGridPanelBounds(Rectangle clientRectangle, RightToLeft rightToLeft);

		/// <summary>
		/// </summary>
		Rectangle GetLoupePanelBounds(Rectangle clientRectangle, RightToLeft rightToLeft);

		/// <summary>
		/// </summary>
		int GetToolbarHeight();

		/// <summary>
		/// </summary>
		Size GetToolbarButtonSize();
	}
}
