/* -----------------------------------------------
 * INuGenNavigationBarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.NavigationBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenNavigationBarRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawButtonBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawDropDownArrow(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawGrip(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawLargeButtonBody(NuGenItemPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawNavigationPaneBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawSmallButtonBody(NuGenImagePaintParams paintParams);
	}
}
