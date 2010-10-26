/* -----------------------------------------------
 * INuGenDirectorySelectorRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.DirectorySelectorInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenDirectorySelectorRenderer
	{
		/// <summary>
		/// </summary>
		void DrawDropDownButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawText(NuGenTextPaintParams textPaintParams);
	}
}
