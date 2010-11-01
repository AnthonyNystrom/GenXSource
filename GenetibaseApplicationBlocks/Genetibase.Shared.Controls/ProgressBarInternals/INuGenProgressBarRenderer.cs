/* -----------------------------------------------
 * INuGenProgressBarRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.ProgressBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenProgressBarRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenProgressBarPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawForeground(NuGenProgressBarPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);
	}
}
