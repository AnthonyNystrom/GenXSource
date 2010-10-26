/* -----------------------------------------------
 * INuGenTitleRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.TitleInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenTitleRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBody(NuGenItemPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);
	}
}
