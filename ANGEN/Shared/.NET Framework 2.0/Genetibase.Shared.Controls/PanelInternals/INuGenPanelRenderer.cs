/* -----------------------------------------------
 * INuGenPanelRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.PanelInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPanelRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenBorderPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawExtendedBackground(NuGenPaintParams paintParams);
	}
}
