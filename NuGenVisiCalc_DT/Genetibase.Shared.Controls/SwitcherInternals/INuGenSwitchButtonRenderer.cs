/* -----------------------------------------------
 * INuGenSwitchButtonRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.SwitcherInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenSwitchButtonRenderer
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
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
