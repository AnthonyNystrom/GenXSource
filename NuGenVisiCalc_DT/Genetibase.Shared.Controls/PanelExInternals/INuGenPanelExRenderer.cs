/* -----------------------------------------------
 * INuGenPanelExRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.PanelExInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPanelExRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawShadow(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		Rectangle GetDisplayRectangle(Rectangle clientRectangle, bool drawShadow);
	}
}
