/* -----------------------------------------------
 * INuGenThumbnailRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenThumbnailRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawImage(NuGenImagePaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawCWRotateButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawCCWRotateButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawText(NuGenTextPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawToolBarSeparator(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		Font GetFont(Rectangle clientRectangle);

		/// <summary>
		/// </summary>
		Color GetForeColor(NuGenControlState state);

		/// <summary>
		/// </summary>
		Image GetGridModeImage();

		/// <summary>
		/// </summary>
		Image GetLoupeModeImage();

		/// <summary>
		/// </summary>
		Image GetRotateCWImage();

		/// <summary>
		/// </summary>
		Image GetRotateCCWImage();

		/// <summary>
		/// </summary>
		Image GetZoomInImage();

		/// <summary>
		/// </summary>
		Image GetZoomOutImage();
	}
}
