/* -----------------------------------------------
 * INuGenProgressBarRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Controls.ProgressBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenProgressBarRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawForeground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);
	}
}
